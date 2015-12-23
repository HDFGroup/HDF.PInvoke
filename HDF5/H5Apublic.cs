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
using htri_t = System.Int32;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

// See the typedef for message creation indexes in H5Opublic.h
using H5O_msg_crt_idx_t = System.UInt32;

namespace HDF.PInvoke
{
    public unsafe sealed class H5A
    {
        /// <summary>
        /// Information struct for attribute
        /// (for H5Aget_info/H5Aget_info_by_idx)
        /// </summary>
        public struct info_t
        {
            /// <summary>
            /// Indicate if creation order is valid
            /// </summary>
            hbool_t corder_valid;
            /// <summary>
            /// Creation order
            /// </summary>
            H5O_msg_crt_idx_t corder;
            /// <summary>
            /// Character set of attribute name
            /// </summary>
            H5T.cset_t cset;
            /// <summary>
            /// Size of raw data
            /// </summary>
            hsize_t data_size;
        };

        /// <summary>
        /// Delegate for H5Aiterate2() callbacks
        /// </summary>
        public delegate herr_t operator2_t
            (hid_t location_id, string attr_name, info_t ainfo, object op_data);


        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Aclose(hid_t attr_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Acreate2
            (hid_t loc_id, string attr_name, hid_t type_id, hid_t space_id,
            hid_t acpl_id, hid_t aapl_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Acreate_by_name
            (hid_t loc_id, string obj_name, string attr_name, hid_t type_id,
            hid_t space_id, hid_t acpl_id, hid_t aapl_id, hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Adelete(hid_t loc_id, string name);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Adelete_by_idx
            (hid_t loc_id, string obj_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t lapl_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Adelete_by_name
            (hid_t loc_id, string obj_name, string attr_name, hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t H5Aexists(hid_t obj_id, string attr_name);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t H5Aexists_by_name
            (hid_t obj_id, string obj_name, string attr_name, hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Aget_create_plist(hid_t attr_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Aget_info(hid_t attr_id, ref info_t ainfo);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Aget_info_by_idx
            (hid_t loc_id, string obj_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t ainfo, hid_t lapl_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Aget_info_by_name
            (hid_t loc_id, string obj_name, string attr_name, ref info_t ainfo,
            hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t H5Aget_name(
            hid_t attr_id, size_t buf_size, [Out] string buf);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t H5Aget_name_by_idx
            (hid_t loc_id, string obj_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, [Out] string name, size_t size,
            hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Aget_space(hid_t attr_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hsize_t H5Aget_storage_size(hid_t attr_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Aget_type(hid_t attr_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Aiterate2
            (hid_t loc_id, H5.index_t idx_type, H5.iter_order_t order,
            ref hsize_t idx, operator2_t op, IntPtr op_data);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Aiterate_by_name(hid_t loc_id,
            string obj_name, H5.index_t idx_type, H5.iter_order_t order,
            ref hsize_t idx, operator2_t op, IntPtr op_data, hid_t lapd_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Aopen
            (hid_t obj_id, string attr_name, hid_t aapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Aopen_by_idx
            (hid_t loc_id, string obj_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t aapl_id, hid_t lapl_id);
        
        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Aopen_by_name
            (hid_t loc_id, string obj_name, string attr_name, hid_t aapl_id,
            hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Aread
            (hid_t attr_id, hid_t type_id, IntPtr buf);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Arename
            (hid_t loc_id, string old_name, string new_name);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Arename_by_name
            (hid_t loc_id, string obj_name, string old_attr_name,
            string new_attr_name, hid_t lapl_id);

        [DllImport(Constants.DLLFileName,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Awrite
            (hid_t attr_id, hid_t type_id, IntPtr buf);
    }
}
