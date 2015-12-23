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

using hbool_t = System.UInt32;
using herr_t = System.Int32;
using hid_t = System.Int32;
using hsize_t = System.UInt64;

namespace HDF.PInvoke
{
    public unsafe sealed class H5G
    {
        /// <summary>
        /// Types of link storage for groups
        /// </summary>
        public enum storage_type_t
        {
            /// <summary>
            /// Unknown link storage type [value = -1].
            /// </summary>
            H5G_STORAGE_TYPE_UNKNOWN = -1,
            /// <summary>
            /// Links in group are stored with a "symbol table"
            /// (this is sometimes called "old-style" groups) [value = 0].
            /// </summary>
            H5G_STORAGE_TYPE_SYMBOL_TABLE,
            /// <summary>
            /// Links are stored in object header [value = 1].
            /// </summary>
            H5G_STORAGE_TYPE_COMPACT,
            /// <summary>
            /// Links are stored in fractal heap & indexed with v2 B-tree
            /// [value = 2].
            /// </summary>
            H5G_STORAGE_TYPE_DENSE
        }

        /// <summary>
        /// Information struct for group
        /// (for H5Gget_info/H5Gget_info_by_name/H5Gget_info_by_idx)
        /// </summary>
        public struct info_t
        {
            /// <summary>
            /// Type of storage for links in group
            /// </summary>
            public storage_type_t storage_type;
            /// <summary>
            /// Number of links in group
            /// </summary>
            public hsize_t nlinks;
            /// <summary>
            /// Current max. creation order value for group
            /// </summary>
            public long max_corder;
            /// <summary>
            /// Whether group has a file mounted on it
            /// </summary>
            public hbool_t mounted;
        }

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Gclose(hid_t group_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Gcreate2
            (hid_t loc_id, string name, hid_t lcpl_id, hid_t gcpl_id,
            hid_t gapl_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Gcreate_anon
            (hid_t loc_id, hid_t gcpl_id, hid_t gapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Gget_create_plist(hid_t group_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Gget_info
            (hid_t loc_id, ref info_t ginfo);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Gget_info_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t ginfo, hid_t lapl_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Gget_info_by_name
            (hid_t loc_id, string name, ref info_t ginfo, hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Gopen2
            (hid_t loc_id, string name, hid_t gapl_id);        
    }
}
