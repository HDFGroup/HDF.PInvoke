using System;
using System.Configuration;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;

namespace HDF.PInvoke
{    
    public static class NativeDependencies
    {
        public const string NativePathSetting = "NativeDependenciesAbsolutePath";

        public static void ResolvePathToExternalDependencies()
        {
            bool updatePath;
#if NET_STANDARD    
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
            {
                // In .NET Core on Linux native libraries for DllImport are always loaded 
                // from the library assembly directory and neither PATH nor LD_LIBRARY_PATH are 
                // taken into account. Thus modifying them makes no sense.
                updatePath = false;
            } 
            else
            {
                updatePath = true;
            }
#else
            updatePath = true;
#endif

            if (updatePath)
            {
                string NativeDllPath;
                if (!GetDllPathFromAppConfig(out NativeDllPath))
                {
                GetDllPathFromAssembly(out NativeDllPath);
                }               
                AddPathStringToEnvironment(NativeDllPath);
            }
        }

        //----------------------------------------------------------------

        private static bool GetDllPathFromAppConfig(out string aPath)
        {
            aPath = string.Empty;
#if NET_STANDARD            
            return false;
#else
            try
            {
                if (ConfigurationManager.AppSettings.Count <= 0) return false;
                string pathFromAppSettings = ConfigurationManager.
                    AppSettings[NativePathSetting].ToString();
                if (string.IsNullOrEmpty(pathFromAppSettings))
                    return false;

                foreach (var c in Path.GetInvalidPathChars())
                {
                    if (pathFromAppSettings.Contains( new string(c, 1) ))
                        return false;
                }

                aPath = pathFromAppSettings;
                return true;
            }
            catch
            {
                return false;
            }
#endif            
        }

        internal static string GetAssemblyName()
        {
            string myPath = new Uri(System.Reflection.Assembly
                .GetExecutingAssembly().CodeBase).AbsolutePath;
            myPath = Uri.UnescapeDataString(myPath);
            return myPath;
        }

        private static void GetDllPathFromAssembly(out string aPath)
        {
            switch (IntPtr.Size)
            {
            case 8:
                aPath = Path.Combine(Path.GetDirectoryName(GetAssemblyName()), Constants.DLL64bitPath);
                break;
            case 4:
                aPath = Path.Combine(Path.GetDirectoryName(GetAssemblyName()), Constants.DLL32bitPath);
                break;
            default:
                throw new NotImplementedException();
            }
        }

        private static void AddPathStringToEnvironment(string aPath)
        {
            try
            {
                string EnvPath = Environment.GetEnvironmentVariable("PATH");
                if (EnvPath.Contains(aPath)) return;
                
                Environment.SetEnvironmentVariable("PATH", aPath + ";" + EnvPath);


                System.Diagnostics.Trace.WriteLine(string.Format(
                    "{0} added to Path.", aPath));
            }
            catch(SecurityException) 
            { 
                System.Diagnostics.Trace.WriteLine(
                    "Changing PATH not allowed");
            }
        }
    }
}
