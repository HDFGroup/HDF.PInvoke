using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HDF.PInvoke
{    
    public static class NativeDependencies
    {
        public const string NativePathSetting = "NativeDependenciesAbsolutePath";

        public static void ResolvePathToExternalDependencies()
        {
            string NativeDllPath;
            if (!GetDllPathFromAppConfig(out NativeDllPath))
            {
               GetDllPathFromAssembly(out NativeDllPath);
            }
            
            AddPathStringToEnvironment(NativeDllPath);

            AssertVcppDependencies();
        }

        //----------------------------------------------------------------

        private static bool GetDllPathFromAppConfig(out string aPath)
        {
            aPath = string.Empty;
            try
            {
                if (ConfigurationManager.AppSettings.Count <= 0) return false;
                string pathFromAppSettings = ConfigurationManager.
                    AppSettings[NativePathSetting].ToString();
                if (string.IsNullOrEmpty(pathFromAppSettings))
                    return false;
                if (Path.GetInvalidPathChars().Any(
                    c => pathFromAppSettings.Contains(c))) return false;

                aPath = pathFromAppSettings;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetAssemblyName()
        {
            string myPath = new Uri(System.Reflection.Assembly
                .GetExecutingAssembly().CodeBase).AbsolutePath;
            myPath = Uri.UnescapeDataString(myPath);
            return myPath;
        }

        private static void GetDllPathFromAssembly(out string aPath)
        {
            if (Environment.Is64BitProcess)
            {
                aPath = Path.Combine(Path.GetDirectoryName(GetAssemblyName())
                    , Constants.DLL64bitPath);
            }
            else
            {
                aPath = Path.Combine(Path.GetDirectoryName(GetAssemblyName())
                    , Constants.DLL32bitPath);
            }
        }

        private static void AddPathStringToEnvironment(string aPath)
        {
            try
            {
                string EnvPath = Environment.GetEnvironmentVariable("PATH");
                if (EnvPath.Contains(aPath)) return;
                
                Environment.SetEnvironmentVariable
                    ("PATH", string.Join(";", aPath, EnvPath));
                    
                System.Diagnostics.Trace.WriteLine(string.Format(
                    "{0} added to Path.", aPath));
            }
            catch(SecurityException) 
            { 
                System.Diagnostics.Trace.WriteLine(
                    "Changing PATH not allowed");
            }
        }

        private static void AssertVcppDependencies()
        {
            var system32Path =
                Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
            var system64Path =
                Environment.GetFolderPath(Environment.SpecialFolder.System);

            string platform = Environment.Is64BitProcess ? "x64 and x86" : "x86";
            bool success = File.Exists(
                Path.Combine(system32Path, Constants.MSVCRDllName));

            if (Environment.Is64BitProcess)
            {
                success &= File.Exists(
                    Path.Combine(system64Path, Constants.MSVCRDllName));
            }

            if (!success)
            {
                throw new InvalidOperationException(
                    String.Format(Constants.MSVCRNotFoundErrorString, platform));
            }
        }
    }
}
