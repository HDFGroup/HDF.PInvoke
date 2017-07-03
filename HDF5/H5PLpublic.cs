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
using System.Text;

using herr_t = System.Int32;
using ssize_t = System.IntPtr;
using uint32_t = System.UInt32;

namespace HDF.PInvoke
{
    public unsafe sealed class H5PL
    {
        static H5PL() { H5.open(); }

        public enum type_t
        {
            TYPE_ERROR = -1,
            TYPE_FILTER = 0,
            TYPE_NONE = 1
        };

        public const int FILTER_PLUGIN = 0x0001;

        public const int ALL_PLUGIN = 0xffff;

        /// <summary>
        /// Append a plugin path to the plugin search path.
        /// See https://support.hdfgroup.org/HDF5/doc1.8/RM/RM_H5PL.html#Plugin-Append
        /// </summary>
        /// <param name="plugin_path">The plugin path</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLappend",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t append(string plugin_path);

        /// <summary>
        /// Query the plugin path at the specified index.
        /// See https://support.hdfgroup.org/HDF5/doc1.8/RM/RM_H5PL.html#Plugin-Get
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="pathname">Path name</param>
        /// <param name="size">Buffer size (in bytes)</param>
        /// <returns>Returns the length of the path, a non-negative value if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLget",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get(uint32_t index,
            StringBuilder pathname, IntPtr size);

        /// <summary>
        /// Query state of the loading of dynamic plugins.
        /// See https://support.hdfgroup.org/HDF5/doc1.8/RM/RM_H5PL.html#Plugin-GetLoadingState
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
        /// Insert a plugin path at the specified index.
        /// See https://support.hdfgroup.org/HDF5/doc1.8/RM/RM_H5PL.html#Plugin-Insert
        /// </summary>
        /// <param name="plugin_path">The plugin path</param>
        /// <param name="index">Index</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLinsert",
           CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t insert(string plugin_path,
            uint32_t index);

        /// <summary>
        /// Insert a plugin path at the beginning of the list.
        /// See https://support.hdfgroup.org/HDF5/doc1.8/RM/RM_H5PL.html#Plugin-Prepend
        /// </summary>
        /// <param name="plugin_path">The plugin path</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLprepend",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t prepend(string plugin_path);

        /// <summary>
        /// Remove the plugin path at the specified index.
        /// See https://support.hdfgroup.org/HDF5/doc1.8/RM/RM_H5PL.html#Plugin-Remove
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLremove",
           CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t remove(uint32_t index);

        /// <summary>
        /// Replace the plugin path at the specified index.
        /// See https://support.hdfgroup.org/HDF5/doc1.8/RM/RM_H5PL.html#Plugin-Replace
        /// </summary>
        /// <param name="plugin_path">The plugin path</param>
        /// <param name="index">Index</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLreplace",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t replace(string plugin_path,
            uint32_t index);

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

        /// <summary>
        /// Query the size of the current list of plugin paths.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5PL.html
        /// </summary>
        /// <param name="listsize">The size of the current list of plugin
        /// paths.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5PLsize",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t size(ref uint32_t listsize);
    }
}
