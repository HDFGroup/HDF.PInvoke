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

namespace HDF.PInvoke
{
    /// <summary>
    /// Helper class used to fetch public variables (e.g. native type values)
    /// exported by the HDF5 DLL
    /// </summary>
    internal abstract class H5DLLImporter
    {

        internal abstract bool GetValue<T>
            (
            string          libname,
            string          varName,
            ref T           value,
            Func<IntPtr, T> converter
            );

        internal static H5DLLImporter Create()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return new H5WindowsDLLImporter();
			case PlatformID.Xbox:
			case PlatformID.MacOSX:
			case PlatformID.Unix:
				return new H5UnixDllImporter ();
                default:
                    break;
            }
            throw new NotImplementedException();
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

        internal override bool GetValue<T>
            (
            string          libname,
            string          varName,
            ref T           value,
            Func<IntPtr, T> converter)
        {
            try
            {
                IntPtr hdl = GetModuleHandle(libname);
                if (hdl == IntPtr.Zero)  // the library hasn't been loaded
                {
                    hdl = LoadLibrary(libname);
                }

                if (hdl != IntPtr.Zero)
                {
                    IntPtr addr = GetProcAddress(hdl, varName);
                    if (addr != IntPtr.Zero)
                    {
                        value = converter(addr);  // Marshal.ReadInt32(addr);
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Trace.TraceError(
                    String.Format(
                    "Error fetching imported variable '{0}' from DLL '{1}':",
                    varName, libname));
                System.Diagnostics.Trace.TraceError(exc.ToString());
            }
            return false;
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

		const int RTLD_NOW = 2; // for dlopen's flags
		internal override bool GetValue<T>(string libname, string varName, ref T value, Func<IntPtr, T> converter)
		{
			if (libname == "hdf5.dll") {
				libname = "/usr/lib/libhdf5.so";

			}
			if (libname == "hdf5_hd.dll") {
				libname = "/usr/lib/libhdf5_hl.so";
			}
				


			try
			{
				IntPtr handle = dlopen(libname, RTLD_NOW);
				if (handle==IntPtr.Zero)
				{
					throw new ArgumentException(
						String.Format(
							"Unable to load unmanaged module \"{0}\"",
							libname));
				}

				if (handle != IntPtr.Zero)
				{
					IntPtr address = dlsym(handle, varName);
					IntPtr errPtr = dlerror();
					if(errPtr != IntPtr.Zero){
						throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
					}

					if (address != IntPtr.Zero)
					{
						value = converter(address);
						return true;
					}
				}
			}
			catch (Exception ex) 
			{
				System.Diagnostics.Trace.TraceError (
					String.Format (
						"Error fetching imported variable '{0}' from DLL '{1}':",
						varName, libname));
				System.Diagnostics.Trace.TraceError (ex.ToString ());
			
			}
			return false;
		}
	}
}
