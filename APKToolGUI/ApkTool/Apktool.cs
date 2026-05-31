using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using APKToolGUI.Properties;
using APKToolGUI.Utils;
using Java;

namespace APKToolGUI
{
    public class Apktool : JarProcess, IDisposable
    {
        private bool disposed = false;
        private static readonly Regex ApktoolVersionRegex = new Regex(@"v?(?<version>\d+\.\d+\.\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Version ParsedVersion { get; private set; }
        public string Version { get; private set; }

        public Apktool(string javaPath, string jarPath) : base(javaPath, jarPath)
        {
            Exited += Apktool_Exited;
            OutputDataReceived += Apktool_OutputDataReceived;
            ErrorDataReceived += Apktool_ErrorDataReceived;

            string apktoolVersion = GetVersion();
            string apktoolVersionOld = GetVersionOld();
            if (!String.IsNullOrWhiteSpace(apktoolVersion) && !Regex.IsMatch(apktoolVersion, @"\r\n?|\n"))
                Version = apktoolVersion;
            else if (!String.IsNullOrWhiteSpace(apktoolVersionOld) && !Regex.IsMatch(apktoolVersionOld, @"\r\n?|\n"))
                Version = apktoolVersionOld;

            ParsedVersion = ParseVersion(Version);

            Debug.WriteLine($"[Apktool] Parsed version: {ParsedVersion}");
        }

        static class DecompileKeys
        {
            //Do not decode sources.
            public const string NoSource = "-s";

            //Do not decode resources.
            public const string NoResource = "-r";

            //don't write out debug info (.local, .param, .line, etc.)
            //The -b flag has been removed from APKtool 3.0.1 and later versions,
            //but the --no-debug-info flag is supported in all versions.
            public const string NoDebugInfo = "--no-debug-info";

            //Skip changes detection and build all files.
            public const string Force = "-f";

            //Uses framework files located in <dir>.
            public const string FrameworkPath = "-p";

            //Use if there was an error and some resources were dropped
            public const string KeepBrokenResource = "--keep-broken-res";

            //Keeps files to closest to original as possible. Prevents rebuild.
            public const string MatchOriginal = "--match-original";

            //The name of folder that gets written. Default is apk.out
            public const string OutputDir = "-o";

            //Only disassemble the main dex classes (classes[0-9]*.dex) in the root.
            public const string OnlyMainClasses = "--only-main-classes";

            //The numeric api-level of the file to generate, e.g. 14 for ICS.
            public const string ApiLevel = "-api";

            // Sets the number of threads to use.
            public const string Jobs = "-j";
        }

        static class BuildKeys
        {
            //Skip changes detection and build all files.
            public const string ForceAll = "-f";

            //opies original AndroidManifest.xml and META-INF. See project page for more info.
            public const string CopyOriginal = "--copy-original";

            //Loads aapt from specified location.
            public const string Aapt = "-a";

            //Uses framework files located in <dir>.
            public const string FrameworkPath = "-p";

            // The name of apk that gets written. Default is dist/name.apk
            public const string OutputAppPath = "-o";

            // Disable crunching of resource files during the build step.
            public const string NoCrunch = "--no-crunch";

            //The numeric api-level of the file to generate, e.g. 14 for ICS.
            public const string ApiLevel = "-api";

            //Upgrades apktool to use experimental aapt2 binary.
            public const string UseAapt2 = "--use-aapt2";

            //Add a generic Network Security Configuration file in the output APK
            public const string NetSecConf = "--net-sec-conf";

            // Sets the number of threads to use.
            public const string Jobs = "-j";
        }

        static class InstallFrameworkKeys
        {
            //Stores framework files into <dir>.
            public const string FrameDir = "-p";

            //Tag frameworks using <tag>.
            public const string Tag = "-t";
        }

        static class EmptyFrameworkKeys
        {
            //Stores framework files into <dir>.
            public const string FrameDir = "-p";

            //Force delete destination directory.
            public const string ForceDelete = "-f";

            //Include all framework files regardless of tag. (3.0.1+)
            public const string All = "-a";
        }

        ApktoolDataReceivedEventHandler onApktoolOutputDataRecieved;
        ApktoolDataReceivedEventHandler onApktoolErrorDataRecieved;

        public event ApktoolDataReceivedEventHandler ApktoolOutputDataRecieved
        {
            add
            {
                onApktoolOutputDataRecieved += value;
            }
            remove
            {
                onApktoolOutputDataRecieved -= value;
            }
        }
        public event ApktoolDataReceivedEventHandler ApktoolErrorDataRecieved
        {
            add
            {
                onApktoolErrorDataRecieved += value;
            }
            remove
            {
                onApktoolErrorDataRecieved -= value;
            }
        }

        private void Apktool_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (onApktoolErrorDataRecieved != null && e.Data != null)
                onApktoolErrorDataRecieved(this, new ApktoolDataReceivedEventArgs(e.Data));
        }

        private void Apktool_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (onApktoolOutputDataRecieved != null && e.Data != null)
                onApktoolOutputDataRecieved(this, new ApktoolDataReceivedEventArgs(e.Data));
        }

        private void Apktool_Exited(object sender, EventArgs e)
        {
            CancelOutputRead();
            CancelErrorRead();
        }

        public int Decompile(string inputPath, string outputDir)
        {
            string keyNoSrc = null, keyNoRes = null, keyForce = null, keyFramePath = null, keyMatchOriginal = null, keyOutputDir = null, onlyMainClasses = null, noDebugInfo = null, keyKeepBrokenRes = null, apiLevel = null, jobs = null;

            if (Settings.Default.Decode_NoSrc)
                keyNoSrc = DecompileKeys.NoSource;
            if (Settings.Default.Decode_NoRes)
                keyNoRes = DecompileKeys.NoResource;
            if (Settings.Default.Decode_Force)
                keyForce = DecompileKeys.Force;
            if (Settings.Default.Decode_KeepBrokenRes)
                keyKeepBrokenRes = DecompileKeys.KeepBrokenResource;
            if (Settings.Default.Decode_MatchOriginal)
                keyMatchOriginal = DecompileKeys.MatchOriginal;
            if (Settings.Default.Decode_OnlyMainClasses && !Settings.Default.Decode_NoSrc && IsVersionAtMost("2.12.1"))
                onlyMainClasses = DecompileKeys.OnlyMainClasses;
            if (Settings.Default.Decode_NoDebugInfo)
                noDebugInfo = DecompileKeys.NoDebugInfo;
            if (Settings.Default.Decode_UseFramework)
                keyFramePath = String.Format("{0} \"{1}\"", DecompileKeys.FrameworkPath, Settings.Default.Framework_FrameDir);
            else
                keyFramePath = String.Format("{0} \"{1}\"", DecompileKeys.FrameworkPath, Program.STANDALONE_FRAMEWORK_DIR);
            if (Settings.Default.Decode_SetApiLevel)
                apiLevel = String.Format("{0} {1}", DecompileKeys.ApiLevel, Settings.Default.Decode_ApiLevel);
            if (Settings.Default.Decode_SetJobs)
                jobs = String.Format("{0} {1}", DecompileKeys.Jobs, Settings.Default.Decode_Jobs);
            keyOutputDir = String.Format("{0} \"{1}\"", DecompileKeys.OutputDir, outputDir);

            string args = String.Format($"d {keyNoSrc} {keyNoRes} {keyForce} {onlyMainClasses} {noDebugInfo} {keyMatchOriginal} {keyFramePath} {keyKeepBrokenRes} {apiLevel} {jobs} {keyOutputDir} \"{inputPath}\"");

            Log.d("Apktool CMD: " + JarPath + " " + args);

            Start(args);
            BeginOutputReadLine();
            BeginErrorReadLine();
            WaitForExit();
            return ExitCode;
        }

        public int Build(string inputFolder, string outputFile)
        {
            string keyForceAll = null, keyAapt = null, keyCopyOriginal = null, noCrunch = null, keyFramePath = null, keyOutputAppPath = null, apiLevel = null, jobs = null, useAapt2 = null, netSecConf = null;

            if (Settings.Default.Build_ForceAll)
                keyForceAll = BuildKeys.ForceAll;
            if (Settings.Default.Build_CopyOriginal)
                keyCopyOriginal = BuildKeys.CopyOriginal;
            if (Settings.Default.Build_NoCrunch)
                noCrunch = BuildKeys.NoCrunch;
            if (Settings.Default.Build_UseAapt)
                keyAapt = String.Format("{0} \"{1}\"", BuildKeys.Aapt, Settings.Default.Build_AaptPath);
            if (Settings.Default.Build_UseFramework)
                keyFramePath = String.Format("{0} \"{1}\"", BuildKeys.FrameworkPath, Settings.Default.Framework_FrameDir);
            else
                keyFramePath = String.Format("{0} \"{1}\"", BuildKeys.FrameworkPath, Program.STANDALONE_FRAMEWORK_DIR);
            if (Settings.Default.Build_SetApiLevel)
                apiLevel = String.Format("{0} {1}", BuildKeys.ApiLevel, Settings.Default.Build_ApiLevel);
            if (Settings.Default.Build_SetJobs)
                jobs = String.Format("{0} {1}", BuildKeys.Jobs, Settings.Default.Build_Jobs);
            if (Settings.Default.Build_UseAapt2)
                useAapt2 = BuildKeys.UseAapt2;
            if (Settings.Default.Build_NetSecConf)
                netSecConf = BuildKeys.NetSecConf;
            keyOutputAppPath = String.Format("{0} \"{1}\"", BuildKeys.OutputAppPath, outputFile);

            string args = String.Format($"b {keyForceAll} {keyAapt} {keyCopyOriginal} {noCrunch} {keyFramePath} {apiLevel} {jobs} {useAapt2} {netSecConf} {keyOutputAppPath} \"{inputFolder}\"");

            Log.d("Apktool CMD: " + JarPath + " " + args);

            Start(args);
            BeginOutputReadLine();
            BeginErrorReadLine();
            WaitForExit();
            return ExitCode;
        }

        public int InstallFramework()
        {
            string inputPath = Settings.Default.InstallFramework_InputFramePath;
            string keyFrameDir = null, keyTag = null;

            if (Settings.Default.Framework_UseFrameDir)
                keyFrameDir = String.Format("{0} \"{1}\"", InstallFrameworkKeys.FrameDir, Settings.Default.Framework_FrameDir);
            if (Settings.Default.InstallFramework_UseTag)
                keyTag = String.Format("{0} \"{1}\"", InstallFrameworkKeys.Tag, Settings.Default.InstallFramework_Tag);

            string args = String.Format($"if {keyFrameDir} {keyTag} \"{inputPath}\"");

            Log.d("Apktool CMD: " + JarPath + " " + args);

            Start(args);
            BeginOutputReadLine();
            BeginErrorReadLine();
            WaitForExit();
            return ExitCode;
        }

        public int ClearFramework()
        {
            string keyFramePath = null;
            if (Settings.Default.Decode_UseFramework)
                keyFramePath = String.Format("{0} \"{1}\"", InstallFrameworkKeys.FrameDir, Settings.Default.Framework_FrameDir);
            else
                keyFramePath = String.Format("{0} \"{1}\"", DecompileKeys.FrameworkPath, Program.STANDALONE_FRAMEWORK_DIR);

            string args = String.Format($"empty-framework-dir {EmptyFrameworkKeys.ForceDelete} {keyFramePath}");
            if (IsVersionAtLeast("3.0.1"))
                args = String.Format($"clean-frameworks {EmptyFrameworkKeys.All} {keyFramePath}");

            Log.d("Apktool CMD: " + JarPath + " " + args);

            Start(args);
            BeginOutputReadLine();
            BeginErrorReadLine();
            WaitForExit();
            return ExitCode;
        }

        public bool IsVersionAtLeast(string minimumVersion)
        {
            if (String.IsNullOrWhiteSpace(minimumVersion))
                throw new ArgumentException("Minimum version cannot be null or empty.", nameof(minimumVersion));

            return ParsedVersion.CompareTo(new Version(minimumVersion)) >= 0;
        }

        public bool IsVersionAtMost(string maximumVersion)
        {
            if (String.IsNullOrWhiteSpace(maximumVersion))
                throw new ArgumentException("Maximum version cannot be null or empty.", nameof(maximumVersion));

            return ParsedVersion.CompareTo(new Version(maximumVersion)) <= 0;
        }

        private static Version ParseVersion(string rawVersion)
        {
            if (String.IsNullOrWhiteSpace(rawVersion))
                return null;

            Match match = ApktoolVersionRegex.Match(rawVersion.Trim());
            if (!match.Success)
                return null;

            try
            {
                return new Version(match.Groups["version"].Value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetVersion()
        {
            using (JarProcess apktoolJar = new JarProcess(JavaPath, JarPath))
            {
                apktoolJar.EnableRaisingEvents = false;
                apktoolJar.Start("version");
                string version = apktoolJar.StandardOutput.ReadToEnd();
                apktoolJar.WaitForExit(3000);
                return version.Replace("\r\n", "");
            }
        }

        public string GetVersionOld()
        {
            using (JarProcess apktoolJar = new JarProcess(JavaPath, JarPath))
            {
                apktoolJar.EnableRaisingEvents = false;
                apktoolJar.Start("-version");
                string version = apktoolJar.StandardOutput.ReadToEnd();
                apktoolJar.WaitForExit(3000);
                return version.Replace("\r\n", "");
            }
        }

        public void Cancel()
        {
            try
            {
                foreach (var process in Process.GetProcessesByName("java"))
                {
                    using (process)
                    {
                        if (process.Id == Id)
                        {
                            ProcessUtils.KillAllProcessesSpawnedBy((uint)Id);
                            process.Kill();
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"[Apktool] Process already exited: {ex.Message}");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Debug.WriteLine($"[Apktool] Failed to access process: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Apktool] Failed to cancel process: {ex.Message}");
            }
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Cancel();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[Apktool] Error during disposal: {ex.Message}");
                    }
                    finally
                    {
                        base.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~Apktool()
        {
            Dispose(false);
        }
    }

    public delegate void ApktoolDataReceivedEventHandler(Object sender, ApktoolDataReceivedEventArgs e);

    public class ApktoolDataReceivedEventArgs : EventArgs
    {
        string data;
        string message;
        ApktoolEventType eventType;

        public ApktoolDataReceivedEventArgs(string _data)
        {
            data = _data;
            SetData();
        }
        public String Message
        {
            get
            {
                return message;
            }
        }
        public ApktoolEventType EventType
        {
            get
            {
                return eventType;
            }
        }

        private void SetData()
        {
            MatchCollection mCol = Regex.Matches(data, @"^(\w+):\s(.+)$");
            if (mCol.Count > 0)
            {
                switch (mCol[0].Groups[1].Value)
                {
                    case "W":
                        eventType = ApktoolEventType.Warning;
                        message = mCol[0].Groups[2].Value;
                        break;
                    case "Warning":
                        eventType = ApktoolEventType.Warning;
                        message = mCol[0].Groups[2].Value;
                        break;
                    case "I":
                        eventType = ApktoolEventType.None;
                        message = mCol[0].Groups[2].Value;
                        break;
                    case "Error":
                        eventType = ApktoolEventType.Error;
                        message = mCol[0].Groups[2].Value;
                        break;
                    case "E":
                        eventType = ApktoolEventType.Error;
                        message = mCol[0].Groups[2].Value;
                        break;
                    default:
                        eventType = ApktoolEventType.Unknown;
                        message = data;
                        break;
                }
            }
            else
            {
                eventType = ApktoolEventType.Unknown;
                message = data;
            }
        }
    }

    public enum ApktoolEventType
    {
        None,
        Success,
        Infomation,
        Warning,
        Error,
        Unknown
    }

    enum ApktoolActionType
    {
        Decompile,
        Build,
        InstallFramework,
        ClearFramework,
        Null
    }
}
