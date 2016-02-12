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
        public static void ResolvePathToExternalDependencies()
        {
            string NativeDllPath;
            if (!GetDllPathFromAppConfig(out NativeDllPath))
            {
               GetDllPathFromAssembly(out NativeDllPath);
            }
            if (!AddPathToNativeDllSearchPath(NativeDllPath))
            {
                AddPathStringToEnvironment(NativeDllPath);
            } 
        }

        public static bool GetDllPathFromAppConfig(out string aPath)
        {
            aPath = String.Empty;
            try
            {
                string pathFromAppSettings = ConfigurationManager.AppSettings["NativeDependenciesAbsolutePath"].ToString();
                if (String.IsNullOrEmpty(pathFromAppSettings)) return false;
                if (!Path.GetInvalidPathChars().All(c => pathFromAppSettings.Contains(c))) return false;

                aPath = pathFromAppSettings;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetAssemblyName()
        {
            string myPath = new Uri(System.Reflection.Assembly
                .GetExecutingAssembly().CodeBase).AbsolutePath;
            myPath = Uri.UnescapeDataString(myPath);
            return myPath;
        }

        public static void GetDllPathFromAssembly(out string aPath)
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

        public static void AddPathStringToEnvironment(string aPath)
        {
            try
            {
                string EnvPath = Environment.GetEnvironmentVariable("PATH");
                if (EnvPath.Contains(aPath)) return;
                
                Environment.SetEnvironmentVariable
                    ("PATH", String.Join(";", EnvPath, aPath));
            }
            catch(SecurityException) 
            { }
        }

        public static bool AddPathToNativeDllSearchPath(string aPath)
        {
            return SetDllDirectory(aPath);
        }

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);
    }
}
