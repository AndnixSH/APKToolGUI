using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace APKToolGUI.Controls
{
    /// <summary>
    /// Small themed numeric up/down control (WPF has no built-in equivalent of the
    /// WinForms NumericUpDown). Templated in Themes/Controls.xaml.
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Up", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_Down", Type = typeof(RepeatButton))]
    public class NumericUpDown : Control
    {
        private TextBox _textBox;
        private bool _updating;

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown),
                new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, OnRangeChanged));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(int), typeof(NumericUpDown), new PropertyMetadata(100, OnRangeChanged));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public int Minimum { get { return (int)GetValue(MinimumProperty); } set { SetValue(MinimumProperty, value); } }
        public int Maximum { get { return (int)GetValue(MaximumProperty); } set { SetValue(MaximumProperty, value); } }
        public int Value { get { return (int)GetValue(ValueProperty); } set { SetValue(ValueProperty, value); } }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var n = (NumericUpDown)d;
            int clamped = n.Clamp((int)e.NewValue);
            if (clamped != (int)e.NewValue) { n.Value = clamped; return; }
            n.UpdateText();
        }

        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var n = (NumericUpDown)d;
            n.Value = n.Clamp(n.Value);
        }

        private int Clamp(int v)
        {
            if (v < Minimum) return Minimum;
            if (v > Maximum) return Maximum;
            return v;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_textBox != null) _textBox.TextChanged -= TextBox_TextChanged;
            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            if (_textBox != null) _textBox.TextChanged += TextBox_TextChanged;

            var up = GetTemplateChild("PART_Up") as RepeatButton;
            var down = GetTemplateChild("PART_Down") as RepeatButton;
            if (up != null) up.Click += (s, e) => Value = Clamp(Value + 1);
            if (down != null) down.Click += (s, e) => Value = Clamp(Value - 1);

            UpdateText();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating) return;
            int v;
            if (int.TryParse(_textBox.Text, out v))
                Value = Clamp(v);
        }

        private void UpdateText()
        {
            if (_textBox == null) return;
            _updating = true;
            _textBox.Text = Value.ToString();
            _updating = false;
        }
    }
}
