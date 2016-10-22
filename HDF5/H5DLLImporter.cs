/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by The HDF Group.                                               *
 * Copyright by the Board of Trustees of the University of Illinois.         *
 * All rights reserved.                                                      *
 *                                                                           *
 * This file is part of HDF5.  The full HDF5 copyright notice, including     *
 * terms governing use, modification, and redistribution, is contained in    *
 * the files COPYING and Copyright.html.  COPYING can be found at the root   *
 * of the source code distribution tree; Copyright.html can be found at the  *
 * root level of an installed copy of the electronic HDF5 document set and   *
 * is linked from the top-level documents page.  It can also be found at     *
 * http://hdfgroup.org/HDF5/doc/Copyright.html.  If you do not have          *
 * access to either file, you may request a copy from help@hdfgroup.org.     *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Runtime.InteropServices;
using System.Text;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    internal delegate T Converter<T>( IntPtr address );

    /// <summary>
    /// Helper class used to fetch public variables (e.g. native type values)
    /// exported by the HDF5 DLL
    /// </summary>
    internal abstract class H5DLLImporter
    {
        public static readonly H5DLLImporter Instance;

        static H5DLLImporter()
        {
            H5.open();

            switch (Environment.OSVersion.Platform)
            {
            case PlatformID.Win32NT:
            case PlatformID.Win32S:
            case PlatformID.Win32Windows:
            case PlatformID.WinCE:
                Instance = new H5WindowsDLLImporter(Constants.DLLFileName);
                break;
            case PlatformID.Xbox:
            case PlatformID.MacOSX:
            case PlatformID.Unix:
                Instance = new H5UnixDllImporter(Constants.DLLFileName);
                break;
            default:
                throw new NotImplementedException();;
            }
        }

        protected abstract IntPtr _GetAddress(string varName);

        public IntPtr GetAddress(string varName)
        {
            var address = _GetAddress(varName);
            if (address == IntPtr.Zero)
                throw new Exception(string.Format("The export with name \"{0}\" doesn't exist.", varName));
            return address;
        }

        public bool GetAddress(string varName, out IntPtr address)
        {
            address = _GetAddress(varName);
            return (address == IntPtr.Zero);
        }

        /*public bool GetValue<T>(
            string          varName,
            ref T           value,
            Func<IntPtr, T> converter
            )
        {
            IntPtr address;
            if (!this.GetAddress(varName, out address))
                return false;
            value = converter(address);
            return true;

            //return (T) Marshal.PtrToStructure(address,typeof(T));
        }*/

        public unsafe hid_t GetHid(string varName)
        {
            return *(hid_t*) this.GetAddress(varName);
        }
    }

    #region Windows Importer
    internal class H5WindowsDLLImporter : H5DLLImporter
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpszLib);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetProcAddress
            (IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string lpszLib);

        private IntPtr hLib;

        public H5WindowsDLLImporter(string libName)
        {
            hLib = GetModuleHandle(libName);
            if (hLib == IntPtr.Zero)  // the library hasn't been loaded
            {
                hLib = LoadLibrary(libName);
                if (hLib == IntPtr.Zero)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Couldn't load library \"{0}\"", libName), e);
                    }
                }
            }
        }

        protected override IntPtr _GetAddress(string varName)
        {
            return GetProcAddress(hLib, varName);
        }
    }
    #endregion

	internal class H5UnixDllImporter : H5DLLImporter{

		[DllImport("libdl.so")]
		protected static extern IntPtr dlopen(string filename, int flags);

		[DllImport("libdl.so")]
		protected static extern IntPtr dlsym(IntPtr handle, string symbol);

		[DllImport("libdl.so")]
		protected static extern IntPtr dlerror ();

        private IntPtr hLib;

        public H5UnixDllImporter(string libName)
        {
			if (libName == "hdf5.dll") {
				libName = "/usr/lib/libhdf5.so";

			}
			if (libName == "hdf5_hd.dll") {
				libName = "/usr/lib/libhdf5_hl.so";
			}
				


			hLib = dlopen(libName, RTLD_NOW);
			if (hLib==IntPtr.Zero)
			{
				throw new ArgumentException(
					String.Format(
						"Unable to load unmanaged module \"{0}\"",
						libName));
			}
        }

		const int RTLD_NOW = 2; // for dlopen's flags
		protected override IntPtr _GetAddress(string varName)
		{
			var address = dlsym(hLib, varName);
			var errPtr = dlerror();
			if(errPtr != IntPtr.Zero){
				throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
			}

			return address;
		}
	}
}
