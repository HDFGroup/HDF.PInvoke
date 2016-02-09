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
using System.Security;

using herr_t = System.Int32;

namespace HDF.PInvoke
{
    public unsafe sealed class H5PL
    {
        public const int FILTER_PLUGIN = 0x0001;

        public const int ALL_PLUGIN = 0xffff;

        /// <summary>
        /// Query state of the loading of dynamic plugins.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5PL.html
        /// </summary>
        /// <param name="plugin_flags">List of dynamic plugin types that are
        /// enabled or disabled.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLget_loading_state",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_loading_state(ref int plugin_flags);

        /// <summary>
        /// Control the loading of dynamic plugins.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5PL.html
        /// </summary>
        /// <param name="plugin_flags">The list of dynamic plugin types to
        /// enable or disable.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLset_loading_state",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_loading_state(int plugin_flags);
    }
}
