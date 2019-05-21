using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace HDF.PInvoke
{
    public static class NativeDependencies
    {
        public const string NativePathSetting = "NativeDependenciesAbsolutePath";

        public static void ResolvePathToExternalDependencies()
        {
            bool updatePath;

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

            if (updatePath)
            {
                GetDllPathFromAssembly(out string NativeDllPath);
                AddPathStringToEnvironment(NativeDllPath);
            }
        }

        //----------------------------------------------------------------

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
            catch (SecurityException)
            {
                System.Diagnostics.Trace.WriteLine(
                    "Changing PATH not allowed");
            }
        }
    }
}