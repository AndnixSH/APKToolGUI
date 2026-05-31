using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace APKToolGUI.Controls
{
    /// <summary>
    /// A virtualized, monospace, console-style log view that replaces the old <see cref="RichTextBox"/>.
    ///
    /// Why it exists: RichTextBox renders a FlowDocument, which is NOT UI-virtualized — every line is a live
    /// layout element and any change re-measures the whole document on the UI thread (O(total lines)), which
    /// froze the GUI once the log held thousands of lines. This control keeps lines in a ring buffer and only
    /// draws the visual rows currently on screen (OnRender + FormattedText), so cost is O(visible).
    ///
    /// Monospace (Consolas) makes the geometry trivial: column = (x - padding) / charWidth, and the word-wrap
    /// column is just viewportWidth / charWidth. Word wrap turns one logical line into several "visual rows";
    /// selection is stored in logical (line, column) coordinates so it survives re-wrapping on resize.
    ///
    /// It implements <see cref="IScrollInfo"/> so a host <see cref="ScrollViewer"/> (CanContentScroll=True)
    /// drives scrolling and the scrollbars.
    /// </summary>
    public sealed class LogView : FrameworkElement, IScrollInfo
    {
        /// <summary>One coloured (optionally clickable) run of text within a line.</summary>
        public sealed class Segment
        {
            public string Text;
            public Brush Foreground;
            public bool Bold;
            public Action OnClick; // null => plain text; non-null => hyperlink

            public Segment(string text, Brush foreground = null, bool bold = false, Action onClick = null)
            {
                Text = text ?? string.Empty;
                Foreground = foreground;
                Bold = bold;
                OnClick = onClick;
            }
        }

        private sealed class Line
        {
            public readonly Segment[] Segments;
            public readonly int Length; // total character count across segments

            // Wrap cache: start column of each visual row, for the wrap width it was computed at.
            public int[] RowStarts;
            public int WrapCols = int.MinValue;

            public Line(Segment[] segments)
            {
                Segments = segments;
                int len = 0;
                for (int i = 0; i < segments.Length; i++)
                    len += segments[i].Text.Length;
                Length = len;
            }

            public string GetText()
            {
                if (Segments.Length == 1)
                    return Segments[0].Text;
                StringBuilder sb = new StringBuilder(Length);
                for (int i = 0; i < Segments.Length; i++)
                    sb.Append(Segments[i].Text);
                return sb.ToString();
            }
        }

        // Bounded scrollback. Lines are trimmed from the front once the buffer grows past MaxLines + TrimSlack,
        // in one bulk RemoveRange so trimming is amortised O(1) per line.
        public const int MaxLines = 32767;
        private const int TrimSlack = 4096;

        // Tab stop width in characters. Tabs are expanded to spaces on input so the buffer is truly fixed-width
        // (a raw '\t' renders as a wide gap that desyncs the charWidth-based geometry). 8 matches the tab stops
        // the existing log strings were authored for (e.g. AaptParser's "App name:\t\t" → column 24).
        private const int TabSize = 8;

        private const double LeftPadding = 3.0;
        private static readonly int[] SingleRow = new int[] { 0 };

        private readonly List<Line> _lines = new List<Line>();

        // Wrap/visual-row layout (rebuilt lazily; kept live incrementally on append).
        private bool _wrapText = true;
        private readonly List<int> _rowOffsets = new List<int>(); // _rowOffsets[i] = visual rows before line i
        private int _totalRows;
        private int _maxLineLen;
        private bool _layoutBuilt;
        private bool _layoutDirty;
        private int _layoutWrapCols = int.MinValue;
        private readonly List<int> _wrapScratch = new List<int>();

        // Cached font metrics (recomputed when font/dpi changes). Monospace => one char width fits all.
        private Typeface _typeface;
        private Typeface _boldTypeface;
        private double _charWidth;
        private double _lineHeight;
        private double _pixelsPerDip = 1.0;
        private bool _metricsValid;

        // Selection, expressed as (line, column) caret positions. Anchor is where the drag began.
        private struct Pos
        {
            public int Line;
            public int Col;
            public Pos(int line, int col) { Line = line; Col = col; }
            public int CompareTo(Pos o)
            {
                if (Line != o.Line) return Line < o.Line ? -1 : 1;
                if (Col != o.Col) return Col < o.Col ? -1 : 1;
                return 0;
            }
        }

        private Pos _selAnchor;
        private Pos _selCaret;
        private bool _hasSelection;
        private bool _selecting;       // mouse drag in progress
        private Point _mouseDownPoint;  // to distinguish click from drag (hyperlinks)

        // One active search highlight (logical line/col/length), or _searchLine < 0 for none.
        private int _searchLine = -1;
        private int _searchCol;
        private int _searchLen;

        // Auto-scroll: stay pinned to the bottom unless the user scrolls up.
        private bool _stickToBottom = true;

        private static readonly Brush SelectionBrush = new SolidColorBrush(Color.FromArgb(120, 51, 153, 255));
        private static readonly Brush SearchBrush = new SolidColorBrush(Color.FromRgb(38, 79, 120));
        private static readonly Brush LinkBrush = new SolidColorBrush(Color.FromRgb(60, 166, 255)); // old #FF3CA6FF

        static LogView()
        {
            SelectionBrush.Freeze();
            SearchBrush.Freeze();
            LinkBrush.Freeze();
        }

        public LogView()
        {
            Focusable = true;
            ClipToBounds = true;
            Cursor = Cursors.IBeam;
            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
            SnapsToDevicePixels = true;
            FocusVisualStyle = null;

            // Routed commands so Ctrl+C / Ctrl+A work via the command system (in addition to OnKeyDown),
            // and so the context menu items can bind to them and show their gestures.
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy,
                (s, e) => { CopySelection(); e.Handled = true; },
                (s, e) => { e.CanExecute = HasSelection; }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SelectAll,
                (s, e) => { SelectAll(); e.Handled = true; },
                (s, e) => { e.CanExecute = _lines.Count > 0; }));

            BuildContextMenu();
        }

        private void BuildContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            MenuItem copy = new MenuItem { Header = "Copy", Command = ApplicationCommands.Copy, CommandTarget = this };
            MenuItem copyAll = new MenuItem { Header = "Copy all" };
            copyAll.Click += (s, e) => CopyAll();
            MenuItem selectAll = new MenuItem { Header = "Select all", Command = ApplicationCommands.SelectAll, CommandTarget = this };

            menu.Items.Add(copy);
            menu.Items.Add(copyAll);
            menu.Items.Add(new Separator());
            menu.Items.Add(selectAll);
            ContextMenu = menu;
        }

        /// <summary>When true (default), long lines wrap to the viewport width; when false they're one row each.</summary>
        public bool WrapText
        {
            get { return _wrapText; }
            set
            {
                if (_wrapText == value) return;
                _wrapText = value;
                _canHScroll = !value;
                _layoutDirty = true;
                InvalidateMeasure();
                InvalidateVisual();
            }
        }

        #region Appearance dependency properties

        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
            nameof(FontFamily), typeof(FontFamily), typeof(LogView),
            new FrameworkPropertyMetadata(new FontFamily("Consolas"),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                OnFontChanged));

        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            nameof(FontSize), typeof(double), typeof(LogView),
            new FrameworkPropertyMetadata(13.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                OnFontChanged));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            nameof(Foreground), typeof(Brush), typeof(LogView),
            new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            nameof(Background), typeof(Brush), typeof(LogView),
            new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

        public FontFamily FontFamily { get { return (FontFamily)GetValue(FontFamilyProperty); } set { SetValue(FontFamilyProperty, value); } }
        public double FontSize { get { return (double)GetValue(FontSizeProperty); } set { SetValue(FontSizeProperty, value); } }
        public Brush Foreground { get { return (Brush)GetValue(ForegroundProperty); } set { SetValue(ForegroundProperty, value); } }
        public Brush Background { get { return (Brush)GetValue(BackgroundProperty); } set { SetValue(BackgroundProperty, value); } }

        private static void OnFontChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LogView v = (LogView)d;
            v._metricsValid = false;
            v._layoutDirty = true;
        }

        #endregion

        #region Metrics

        private void EnsureMetrics()
        {
            if (_metricsValid)
                return;

            FontFamily family = FontFamily ?? new FontFamily("Consolas");
            _typeface = new Typeface(family, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            _boldTypeface = new Typeface(family, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

            try { _pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip; }
            catch { _pixelsPerDip = 1.0; }

            FormattedText probe = MakeText("0", _typeface, Brushes.White);
            _charWidth = probe.WidthIncludingTrailingWhitespace;
            if (_charWidth <= 0) _charWidth = FontSize * 0.6;
            _lineHeight = Math.Ceiling(probe.Height);
            if (_lineHeight <= 0) _lineHeight = Math.Ceiling(FontSize * 1.4);

            _metricsValid = true;
        }

        private FormattedText MakeText(string text, Typeface tf, Brush brush)
        {
            return new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                tf, FontSize, brush ?? Foreground ?? Brushes.White, _pixelsPerDip);
        }

        #endregion

        #region Wrap / visual-row layout

        private int ComputeWrapCols()
        {
            if (!_wrapText || _charWidth <= 0)
                return int.MaxValue; // one row per line
            double avail = _viewport.Width - LeftPadding * 2;
            if (avail < _charWidth)
                return _viewport.Width <= 0 ? int.MaxValue : 1;
            return Math.Max(1, (int)Math.Floor(avail / _charWidth));
        }

        private int[] WrapLine(Line line, int wrapCols)
        {
            if (wrapCols <= 0 || line.Length <= wrapCols)
                return SingleRow;

            string t = line.GetText();
            int n = t.Length;
            List<int> rows = _wrapScratch;
            rows.Clear();
            rows.Add(0);
            int pos = 0;
            while (pos + wrapCols < n)
            {
                int limit = pos + wrapCols;
                int br = -1;
                for (int k = limit; k > pos; k--)
                    if (t[k] == ' ') { br = k; break; }

                int next = (br > pos) ? br + 1 : limit; // break after the space, else hard break
                if (next <= pos) next = pos + 1;
                rows.Add(next);
                pos = next;
            }
            return rows.ToArray();
        }

        private void EnsureWrapped(Line line, int wrapCols)
        {
            if (line.RowStarts != null && line.WrapCols == wrapCols)
                return;
            line.RowStarts = WrapLine(line, wrapCols);
            line.WrapCols = wrapCols;
        }

        private void EnsureLayout()
        {
            EnsureMetrics();
            int wrapCols = ComputeWrapCols();
            if (_layoutBuilt && !_layoutDirty && wrapCols == _layoutWrapCols)
                return;

            _rowOffsets.Clear();
            int total = 0;
            int maxLen = 0;
            for (int i = 0; i < _lines.Count; i++)
            {
                _rowOffsets.Add(total);
                Line ln = _lines[i];
                EnsureWrapped(ln, wrapCols);
                total += ln.RowStarts.Length;
                if (ln.Length > maxLen) maxLen = ln.Length;
            }
            _totalRows = total;
            _maxLineLen = maxLen;
            _layoutWrapCols = wrapCols;
            _layoutDirty = false;
            _layoutBuilt = true;
        }

        private void RowToLine(int visualRow, out int line, out int rowInLine)
        {
            int count = _lines.Count;
            int lo = 0, hi = count - 1, ans = 0;
            while (lo <= hi)
            {
                int mid = (lo + hi) >> 1;
                if (_rowOffsets[mid] <= visualRow) { ans = mid; lo = mid + 1; }
                else hi = mid - 1;
            }
            line = ans;
            rowInLine = visualRow - _rowOffsets[ans];
        }

        private static void RowRange(Line line, int rowInLine, out int start, out int end)
        {
            start = line.RowStarts[rowInLine];
            end = (rowInLine + 1 < line.RowStarts.Length) ? line.RowStarts[rowInLine + 1] : line.Length;
        }

        private double ExtentWidthValue
        {
            get { return _wrapText ? _viewport.Width : (LeftPadding * 2 + _maxLineLen * _charWidth); }
        }

        #endregion

        #region Public log API

        public void AppendLine(string text, Brush color = null, bool bold = false)
        {
            AddSegments(new[] { new Segment(text ?? string.Empty, color, bold) });
        }

        public void AppendSegments(IList<Segment> segments)
        {
            if (segments == null || segments.Count == 0) { AddSegments(new[] { new Segment(string.Empty) }); return; }
            Segment[] arr = new Segment[segments.Count];
            for (int i = 0; i < segments.Count; i++) arr[i] = segments[i];
            AddSegments(arr);
        }

        public void SetText(string text)
        {
            Clear();
            AppendLine(text);
        }

        public void Clear()
        {
            _lines.Clear();
            _rowOffsets.Clear();
            _totalRows = 0;
            _maxLineLen = 0;
            _layoutBuilt = false;
            _layoutDirty = true;
            _hasSelection = false;
            _selecting = false;
            _searchLine = -1;
            _stickToBottom = true;
            _offset = new Vector(0, 0);
            InvalidateMeasure();
            InvalidateVisual();
            if (ScrollOwner != null) ScrollOwner.InvalidateScrollInfo();
        }

        public string GetText()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _lines.Count; i++)
            {
                if (i > 0) sb.Append("\r\n");
                sb.Append(_lines[i].GetText());
            }
            return sb.ToString();
        }

        public int LineCount { get { return _lines.Count; } }

        public string GetLineText(int index)
        {
            return (index >= 0 && index < _lines.Count) ? _lines[index].GetText() : string.Empty;
        }

        private static string StripCr(string s)
        {
            return s.IndexOf('\r') >= 0 ? s.Replace("\r", string.Empty) : s;
        }

        // Single normalization point for ALL appends (plain text, bold, and hyperlink lines, whether they
        // arrive via the batched RichBox pipeline or directly). Splits embedded '\n' into separate visual
        // lines (otherwise FormattedText would draw several lines stacked inside one row's height — the
        // "overlapping text" bug), strips '\r', and expands tabs.
        private void AddSegments(Segment[] segments)
        {
            bool hasNl = false;
            for (int i = 0; i < segments.Length; i++)
                if (segments[i].Text.IndexOf('\n') >= 0) { hasNl = true; break; }

            if (!hasNl)
            {
                AddLine(new Line(ExpandTabs(StripCrSegments(segments))));
                return;
            }

            List<Segment> cur = new List<Segment>();
            for (int i = 0; i < segments.Length; i++)
            {
                Segment s = segments[i];
                string[] parts = s.Text.Split('\n');
                for (int p = 0; p < parts.Length; p++)
                {
                    if (p > 0)
                    {
                        AddLine(new Line(ExpandTabs(cur.ToArray())));
                        cur.Clear();
                    }
                    string piece = StripCr(parts[p]);
                    if (piece.Length > 0)
                        cur.Add(new Segment(piece, s.Foreground, s.Bold, s.OnClick));
                }
            }
            AddLine(new Line(ExpandTabs(cur.ToArray())));
        }

        private static Segment[] StripCrSegments(Segment[] segments)
        {
            bool hasCr = false;
            for (int i = 0; i < segments.Length; i++)
                if (segments[i].Text.IndexOf('\r') >= 0) { hasCr = true; break; }
            if (!hasCr) return segments;

            Segment[] result = new Segment[segments.Length];
            for (int i = 0; i < segments.Length; i++)
            {
                Segment s = segments[i];
                result[i] = s.Text.IndexOf('\r') >= 0
                    ? new Segment(StripCr(s.Text), s.Foreground, s.Bold, s.OnClick)
                    : s;
            }
            return result;
        }

        private static Segment[] ExpandTabs(Segment[] segments)
        {
            bool hasTab = false;
            for (int i = 0; i < segments.Length; i++)
                if (segments[i].Text.IndexOf('\t') >= 0) { hasTab = true; break; }
            if (!hasTab) return segments;

            Segment[] result = new Segment[segments.Length];
            int col = 0;
            for (int i = 0; i < segments.Length; i++)
            {
                Segment s = segments[i];
                if (s.Text.IndexOf('\t') < 0) { result[i] = s; col += s.Text.Length; continue; }

                StringBuilder sb = new StringBuilder(s.Text.Length + 8);
                for (int c = 0; c < s.Text.Length; c++)
                {
                    char ch = s.Text[c];
                    if (ch == '\t')
                    {
                        int spaces = TabSize - (col % TabSize);
                        sb.Append(' ', spaces);
                        col += spaces;
                    }
                    else { sb.Append(ch); col++; }
                }
                result[i] = new Segment(sb.ToString(), s.Foreground, s.Bold, s.OnClick);
            }
            return result;
        }

        private void AddLine(Line line)
        {
            _lines.Add(line);

            if (_lines.Count > MaxLines + TrimSlack)
            {
                int remove = _lines.Count - MaxLines;
                _lines.RemoveRange(0, remove);
                ShiftAfterTrim(remove);
                _layoutDirty = true; // front removed: rebuild prefix table
            }

            if (line.Length > _maxLineLen) _maxLineLen = line.Length;

            // Keep the visual-row layout live incrementally when it's valid, so autoscroll and the scrollbar
            // stay correct without an O(n) rebuild per line. Otherwise mark it for a lazy rebuild.
            if (_layoutBuilt && !_layoutDirty)
            {
                _rowOffsets.Add(_totalRows);
                EnsureWrapped(line, _layoutWrapCols);
                _totalRows += line.RowStarts.Length;
            }
            else
            {
                _layoutDirty = true;
            }

            InvalidateMeasure();
            InvalidateVisual();
            if (ScrollOwner != null) ScrollOwner.InvalidateScrollInfo();

            // Pin to the bottom immediately using the live row count.
            if (_stickToBottom && !HasSelection && _viewport.Height > 0 && _lineHeight > 0)
            {
                EnsureLayout();
                double maxY = Math.Max(0, _totalRows * _lineHeight - _viewport.Height);
                if (Math.Abs(maxY - _offset.Y) > 0.001)
                {
                    _offset.Y = maxY;
                    if (ScrollOwner != null) ScrollOwner.InvalidateScrollInfo();
                    InvalidateVisual();
                }
            }
        }

        private void ShiftAfterTrim(int removedLines)
        {
            if (_searchLine >= 0)
            {
                _searchLine -= removedLines;
                if (_searchLine < 0) _searchLine = -1;
            }
            if (_hasSelection)
            {
                _selAnchor.Line -= removedLines;
                _selCaret.Line -= removedLines;
                if (_selAnchor.Line < 0 || _selCaret.Line < 0)
                    _hasSelection = false;
            }
        }

        #endregion

        #region Search support

        public void SetSearchHighlight(int line, int col, int length)
        {
            _searchLine = line;
            _searchCol = col;
            _searchLen = length;
            InvalidateVisual();
        }

        public void ClearSearchHighlight()
        {
            _searchLine = -1;
            InvalidateVisual();
        }

        /// <summary>Scroll so the given logical line sits roughly in the middle of the viewport.</summary>
        public void ScrollLineIntoView(int line)
        {
            if (line < 0) return;
            EnsureLayout();
            if (line >= _rowOffsets.Count) return;
            double target = _rowOffsets[line] * _lineHeight - _viewport.Height / 2;
            SetVerticalOffset(target);
        }

        #endregion

        #region Rendering

        protected override void OnRender(DrawingContext dc)
        {
            EnsureLayout();

            double w = RenderSize.Width, h = RenderSize.Height;
            dc.DrawRectangle(Background ?? Brushes.Transparent, null, new Rect(0, 0, w, h));

            if (_lines.Count == 0 || _lineHeight <= 0 || _totalRows == 0)
                return;

            int firstRow = (int)Math.Floor(_offset.Y / _lineHeight);
            int lastRow = (int)Math.Ceiling((_offset.Y + h) / _lineHeight);
            if (firstRow < 0) firstRow = 0;
            if (lastRow > _totalRows) lastRow = _totalRows;

            Pos selLo = default(Pos), selHi = default(Pos);
            bool hasSel = _hasSelection && _selAnchor.CompareTo(_selCaret) != 0;
            if (hasSel)
            {
                if (_selAnchor.CompareTo(_selCaret) <= 0) { selLo = _selAnchor; selHi = _selCaret; }
                else { selLo = _selCaret; selHi = _selAnchor; }
            }

            double baseX = LeftPadding - _offset.X;

            int line, rowInLine;
            RowToLine(firstRow, out line, out rowInLine);

            for (int vr = firstRow; vr < lastRow; vr++)
            {
                // Advance to the line owning this visual row.
                while (line < _lines.Count - 1 && vr >= _rowOffsets[line] + _lines[line].RowStarts.Length)
                {
                    line++;
                    rowInLine = 0;
                }
                rowInLine = vr - _rowOffsets[line];

                Line ln = _lines[line];
                int rs, re;
                RowRange(ln, rowInLine, out rs, out re);
                double y = vr * _lineHeight - _offset.Y;

                // Selection highlight (logical columns intersected with this row).
                if (hasSel && line >= selLo.Line && line <= selHi.Line)
                {
                    int from = (line == selLo.Line) ? selLo.Col : 0;
                    int to = (line == selHi.Line) ? selHi.Col : ln.Length;
                    int a = Math.Max(from, rs), b = Math.Min(to, re);
                    if (b > a)
                        dc.DrawRectangle(SelectionBrush, null,
                            new Rect(baseX + (a - rs) * _charWidth, y, (b - a) * _charWidth, _lineHeight));
                }

                // Search highlight.
                if (_searchLine == line && _searchLen > 0)
                {
                    int a = Math.Max(_searchCol, rs), b = Math.Min(_searchCol + _searchLen, re);
                    if (b > a)
                        dc.DrawRectangle(SearchBrush, null,
                            new Rect(baseX + (a - rs) * _charWidth, y, (b - a) * _charWidth, _lineHeight));
                }

                // Text: draw each segment piece that overlaps this row's [rs, re).
                Segment[] segs = ln.Segments;
                int segCol = 0;
                for (int s = 0; s < segs.Length; s++)
                {
                    Segment seg = segs[s];
                    int segStart = segCol;
                    int segEnd = segCol + seg.Text.Length;
                    segCol = segEnd;

                    int a = Math.Max(segStart, rs), b = Math.Min(segEnd, re);
                    if (b <= a) continue;

                    string sub = seg.Text.Substring(a - segStart, b - a);
                    bool isLink = seg.OnClick != null;
                    Brush brush = isLink ? LinkBrush : (seg.Foreground ?? Foreground ?? Brushes.White);
                    Typeface tf = seg.Bold ? _boldTypeface : _typeface;

                    double x = baseX + (a - rs) * _charWidth;
                    dc.DrawText(MakeText(sub, tf, brush), new Point(x, y));

                    if (isLink)
                    {
                        double uy = y + _lineHeight - 1.5;
                        dc.DrawLine(new Pen(brush, 1.0), new Point(x, uy), new Point(x + (b - a) * _charWidth, uy));
                    }
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            EnsureMetrics();
            double vw = double.IsInfinity(availableSize.Width) ? 0 : availableSize.Width;
            double vh = double.IsInfinity(availableSize.Height) ? 0 : availableSize.Height;
            _viewport = new Size(vw, vh);
            EnsureLayout();
            VerifyScrollData(_viewport, new Size(ExtentWidthValue, _totalRows * _lineHeight));
            return new Size(vw, vh);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            EnsureMetrics();
            _viewport = finalSize;
            EnsureLayout();
            VerifyScrollData(finalSize, new Size(ExtentWidthValue, _totalRows * _lineHeight));
            return finalSize;
        }

        #endregion

        #region Mouse / keyboard interaction

        private bool PointToLineCol(Point p, out int line, out int col)
        {
            line = 0; col = 0;
            EnsureLayout();
            if (_lines.Count == 0 || _lineHeight <= 0 || _totalRows == 0)
                return false;

            int vr = (int)Math.Floor((p.Y + _offset.Y) / _lineHeight);
            if (vr < 0) vr = 0;
            if (vr >= _totalRows) vr = _totalRows - 1;

            int rowInLine;
            RowToLine(vr, out line, out rowInLine);
            int rs, re;
            RowRange(_lines[line], rowInLine, out rs, out re);

            int colInRow = (int)Math.Round((p.X - LeftPadding + _offset.X) / _charWidth);
            if (colInRow < 0) colInRow = 0;
            col = rs + colInRow;
            if (col > re) col = re;
            if (col > _lines[line].Length) col = _lines[line].Length;
            return true;
        }

        private Pos PointToPos(Point p)
        {
            int line, col;
            PointToLineCol(p, out line, out col);
            return new Pos(line, col);
        }

        private Action HitTestLink(Point p)
        {
            EnsureLayout();
            if (_lines.Count == 0 || _lineHeight <= 0 || _totalRows == 0)
                return null;

            int vr = (int)Math.Floor((p.Y + _offset.Y) / _lineHeight);
            if (vr < 0 || vr >= _totalRows) return null;

            int line, rowInLine;
            RowToLine(vr, out line, out rowInLine);
            int rs, re;
            RowRange(_lines[line], rowInLine, out rs, out re);

            int colInRow = (int)Math.Floor((p.X - LeftPadding + _offset.X) / _charWidth);
            if (colInRow < 0) return null;
            int col = rs + colInRow;
            if (col >= re || col >= _lines[line].Length) return null;

            int segCol = 0;
            Segment[] segs = _lines[line].Segments;
            for (int s = 0; s < segs.Length; s++)
            {
                int len = segs[s].Text.Length;
                if (col >= segCol && col < segCol + len)
                    return segs[s].OnClick;
                segCol += len;
            }
            return null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point p = e.GetPosition(this);

            if (_selecting && e.LeftButton == MouseButtonState.Pressed)
            {
                _selCaret = PointToPos(p);
                _hasSelection = true;
                if (p.Y < 0) SetVerticalOffset(_offset.Y - _lineHeight);
                else if (p.Y > RenderSize.Height) SetVerticalOffset(_offset.Y + _lineHeight);
                InvalidateVisual();
                return;
            }

            Cursor = HitTestLink(p) != null ? Cursors.Hand : Cursors.IBeam;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            Focus();
            _mouseDownPoint = e.GetPosition(this);
            _selAnchor = _selCaret = PointToPos(_mouseDownPoint);
            _hasSelection = false;
            _selecting = true;
            CaptureMouse();
            InvalidateVisual();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            _selecting = false;
            ReleaseMouseCapture();

            Point p = e.GetPosition(this);
            double moved = Math.Abs(p.X - _mouseDownPoint.X) + Math.Abs(p.Y - _mouseDownPoint.Y);
            if (moved < 3)
            {
                Action onClick = HitTestLink(p);
                if (onClick != null)
                {
                    try { onClick(); }
                    catch (Exception ex) { Debug.WriteLine("LogView link click error: " + ex); }
                }
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            double lines = e.Delta / 120.0 * 3.0;
            SetVerticalOffset(_offset.Y - lines * _lineHeight);
            e.Handled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            bool ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            // Ctrl+C / Ctrl+A are handled by the ApplicationCommands bindings set up in the constructor.
            if (e.Key == Key.PageDown) { PageDown(); e.Handled = true; }
            else if (e.Key == Key.PageUp) { PageUp(); e.Handled = true; }
            else if (e.Key == Key.Home && ctrl) { ScrollToHome(); e.Handled = true; }
            else if (e.Key == Key.End && ctrl) { ScrollToEnd(); e.Handled = true; }
        }

        public void SelectAll()
        {
            if (_lines.Count == 0) return;
            _selAnchor = new Pos(0, 0);
            _selCaret = new Pos(_lines.Count - 1, _lines[_lines.Count - 1].Length);
            _hasSelection = true;
            InvalidateVisual();
        }

        public bool HasSelection { get { return _hasSelection && _selAnchor.CompareTo(_selCaret) != 0; } }

        public void CopySelection()
        {
            string text = GetSelectedText();
            if (string.IsNullOrEmpty(text)) return;
            try { Clipboard.SetText(text); }
            catch (Exception ex) { Debug.WriteLine("LogView copy error: " + ex); }
        }

        /// <summary>Copy the entire log to the clipboard (regardless of selection).</summary>
        public void CopyAll()
        {
            string text = GetText();
            if (string.IsNullOrEmpty(text)) return;
            try { Clipboard.SetText(text); }
            catch (Exception ex) { Debug.WriteLine("LogView copy-all error: " + ex); }
        }

        private string GetSelectedText()
        {
            if (!HasSelection) return string.Empty;
            Pos lo, hi;
            if (_selAnchor.CompareTo(_selCaret) <= 0) { lo = _selAnchor; hi = _selCaret; }
            else { lo = _selCaret; hi = _selAnchor; }

            StringBuilder sb = new StringBuilder();
            for (int i = lo.Line; i <= hi.Line && i < _lines.Count; i++)
            {
                string t = _lines[i].GetText();
                int from = (i == lo.Line) ? Math.Min(lo.Col, t.Length) : 0;
                int to = (i == hi.Line) ? Math.Min(hi.Col, t.Length) : t.Length;
                if (to > from) sb.Append(t.Substring(from, to - from));
                if (i != hi.Line) sb.Append("\r\n");
            }
            return sb.ToString();
        }

        #endregion

        #region IScrollInfo

        private Vector _offset;
        private Size _viewport;
        private Size _extent;
        private bool _canHScroll;
        private bool _canVScroll = true;

        public bool CanVerticallyScroll { get { return _canVScroll; } set { _canVScroll = value; } }
        public bool CanHorizontallyScroll { get { return _canHScroll; } set { _canHScroll = value; } }

        public double ExtentWidth { get { return _extent.Width; } }
        public double ExtentHeight { get { return _extent.Height; } }
        public double ViewportWidth { get { return _viewport.Width; } }
        public double ViewportHeight { get { return _viewport.Height; } }
        public double HorizontalOffset { get { return _offset.X; } }
        public double VerticalOffset { get { return _offset.Y; } }
        public ScrollViewer ScrollOwner { get; set; }

        private void VerifyScrollData(Size viewport, Size extent)
        {
            bool changed = viewport != _viewport || extent != _extent;
            _viewport = viewport;
            _extent = extent;

            double maxY = Math.Max(0, _extent.Height - _viewport.Height);
            double maxX = Math.Max(0, _extent.Width - _viewport.Width);
            // Stay pinned to the bottom as new rows extend the document — unless the user scrolled up or is
            // holding a selection (don't yank content out from under a drag-select).
            bool pin = _stickToBottom && !HasSelection;
            double cy = pin ? maxY : Math.Min(Math.Max(0, _offset.Y), maxY);
            double cx = Math.Min(Math.Max(0, _offset.X), maxX);
            if (cy != _offset.Y || cx != _offset.X) { _offset.X = cx; _offset.Y = cy; changed = true; }

            if (changed && ScrollOwner != null)
                ScrollOwner.InvalidateScrollInfo();
        }

        public void SetVerticalOffset(double offset)
        {
            double maxY = Math.Max(0, _extent.Height - _viewport.Height);
            if (offset > maxY) offset = maxY;
            if (offset < 0) offset = 0;
            _stickToBottom = offset >= maxY - 0.5;
            if (Math.Abs(offset - _offset.Y) < 0.001) return;
            _offset.Y = offset;
            if (ScrollOwner != null) ScrollOwner.InvalidateScrollInfo();
            InvalidateVisual();
        }

        public void SetHorizontalOffset(double offset)
        {
            double maxX = Math.Max(0, _extent.Width - _viewport.Width);
            if (offset > maxX) offset = maxX;
            if (offset < 0) offset = 0;
            if (Math.Abs(offset - _offset.X) < 0.001) return;
            _offset.X = offset;
            if (ScrollOwner != null) ScrollOwner.InvalidateScrollInfo();
            InvalidateVisual();
        }

        public void LineUp() { SetVerticalOffset(_offset.Y - _lineHeight); }
        public void LineDown() { SetVerticalOffset(_offset.Y + _lineHeight); }
        public void LineLeft() { SetHorizontalOffset(_offset.X - _charWidth); }
        public void LineRight() { SetHorizontalOffset(_offset.X + _charWidth); }
        public void PageUp() { SetVerticalOffset(_offset.Y - _viewport.Height); }
        public void PageDown() { SetVerticalOffset(_offset.Y + _viewport.Height); }
        public void PageLeft() { SetHorizontalOffset(_offset.X - _viewport.Width); }
        public void PageRight() { SetHorizontalOffset(_offset.X + _viewport.Width); }
        public void MouseWheelUp() { SetVerticalOffset(_offset.Y - 3 * _lineHeight); }
        public void MouseWheelDown() { SetVerticalOffset(_offset.Y + 3 * _lineHeight); }
        public void MouseWheelLeft() { SetHorizontalOffset(_offset.X - 3 * _charWidth); }
        public void MouseWheelRight() { SetHorizontalOffset(_offset.X + 3 * _charWidth); }

        public Rect MakeVisible(Visual visual, Rect rectangle) { return rectangle; }

        #endregion

        #region Scroll helpers (RichTextBox-compatible names)

        public void ScrollToEnd() { SetVerticalOffset(double.MaxValue); }
        public void ScrollToHome() { SetVerticalOffset(0); }
        public void ScrollToVerticalOffset(double offset) { SetVerticalOffset(offset); }

        #endregion
    }
}
