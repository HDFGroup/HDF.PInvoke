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
using hssize_t = System.Int64;
using htri_t = System.Int32;
using size_t = System.UInt64;
using ssize_t = System.Int32;

namespace HDF.PInvoke
{
    public unsafe sealed class H5F
    {
        /// <summary>
        /// Flags for H5F.open() and H5F.create() calls
        /// </summary>
        public enum open_create_flags : uint
        {
            /// <summary>
            /// absence of rdwr => rd-only [value = 0].
            /// </summary>
            H5F_ACC_RDONLY = 0x0000u,
            /// <summary>
            /// open for read and write [value = 1].
            /// </summary>
            H5F_ACC_RDWR = 0x0001u,
            /// <summary>
            /// overwrite existing files [value = 2].
            /// </summary>
            H5F_ACC_TRUNC = 0x0002u,
            /// <summary>
            /// fail if file already exists [value = 4].
            /// </summary>
            H5F_ACC_EXCL = 0x0004u,
            /// <summary>
            /// print debug info [value = 8].
            /// </summary>
            H5F_ACC_DEBUG = 0x0008u,
            /// <summary>
            /// create non-existing files [value = 16].
            /// </summary>
            H5F_ACC_CREAT = 0x0010u
        }

        ///<summary
        /// Value passed to H5P.set_elink_acc_flags to cause flags to be taken
        /// from the parent file.
        ///</summary>
        public const uint H5F_ACC_DEFAULT = 0xffffu; 	/*ignore setting on lapl     */

        ///<summary
        /// Flags for H5F.get_obj_count() & H5F.get_obj_ids() calls
        ///</sumary>
        public enum get_obj_count_or_get_obj_ids_flags : uint
        {
            /// <summary>
            /// File objects [value = 1].
            /// </summary>
            H5F_OBJ_FILE = 0x0001u,
            /// <summary>
            /// Dataset objects [value = 2].
            /// </summary>
            H5F_OBJ_DATASET = 0x0002u,
            /// <summary>
            /// Group objects [value = 4].
            /// </summary>
            H5F_OBJ_GROUP = 0x0004u,
            /// <summary>
            /// Named datatype objects [value = 8].
            /// </summary>
            H5F_OBJ_DATATYPE = 0x0008u,
            /// <summary>
            /// Attribute objects [value = 16].
            /// </summary>
            H5F_OBJ_ATTR = 0x0010u,
            /// <summary>
            /// All objects [value = 31].
            /// H5F_OBJ_FILE|H5F_OBJ_DATASET|H5F_OBJ_GROUP|H5F_OBJ_DATATYPE|H5F_OBJ_ATTR)
            /// </summary>
            H5F_OBJ_ALL = 0x001Fu,
            /// <summary>
            /// All objects [value = 32].
            /// Restrict search to objects opened through current file ID
            /// (as opposed to objects opened through any file ID accessing this file)
            /// </summary>
            H5F_OBJ_LOCAL = 0x0020u
        }

        public hsize_t H5F_FAMILY_DEFAULT = 0;

        /// <summary>
        /// The difference between a single file and a set of mounted files
        /// </summary>
        public enum scope_t
        {
            /// <summary>
            /// specified file handle only [value = 0].
            /// </summary>
            H5F_SCOPE_LOCAL = 0,
            /// <summary>
            /// entire virtual file [value = 1].
            /// </summary>
            H5F_SCOPE_GLOBAL = 1
        }


        /// <summary>
        /// Unlimited file size for H5P.set_external()
        /// </summary>
        public const hsize_t H5F_UNLIMITED = unchecked((hsize_t)(-1));
        
        /// <summary>
        /// Flags that control the behavior of H5F.close()
        /// </summary>
        public enum close_degree_t
        {
            /// <summary>
            /// Use the degree pre-defined by underlining VFL [value = 0].
            /// </summary>
            H5F_CLOSE_DEFAULT = 0,
            /// <summary>
            /// file closes only after all opened objects are closed [value = 1].
            /// </summary>
            H5F_CLOSE_WEAK = 1,
            /// <summary>
            /// if no opened objects, file is close; otherwise, file close
            /// fails [value = 2].
            /// </summary>
            H5F_CLOSE_SEMI = 2,
            /// if there are opened objects, close them first, then close file
            /// [value = 3].
            /// </summary>
            H5F_CLOSE_STRONG = 3
        }

        /// <summary>
        /// Current "global" information about a file
        /// (just size info currently)
        /// </summary>
        public struct info_t
        {
            /// <summary>
            /// Superblock extension size
            /// </summary>
            public hsize_t super_ext_size;
            public sohm_t sohm;
            public struct sohm_t
            {
                /// <summary>
                /// Shared object header message header size
                /// </summary>
                public hsize_t hdr_size;
                /// <summary>
                /// Shared object header message index & heap size
                /// </summary>
                public H5.ih_info_t msgs_info;
            }
        }

        /// <summary>
        ///  Types of allocation requests
        /// </summary>
        public enum mem_t
        {
            /// <summary>
            /// Data should not appear in the free list. [value = -1].
            /// </summary>
            H5FD_MEM_NOLIST = -1,
            /// <summary>
            /// Value not yet set.  Can also be the datatype set in a larger
            /// allocation that will be suballocated by the library.
            /// Must be zero. [value = 0].
            /// </summary>
            H5FD_MEM_DEFAULT = 0,
            /// <summary>
            /// Superblock data [value = 1].
            /// </summary>
            H5FD_MEM_SUPER = 1,
            /// <summary>
            /// B-tree data [value = 2].
            /// </summary>
            H5FD_MEM_BTREE = 2,
            /// <summary>
            /// Raw data (content of datasets, etc.) [value = 3].
            /// </summary>
            H5FD_MEM_DRAW = 3,
            /// <summary>
            /// Global heap data [value = 4].
            /// </summary>
            H5FD_MEM_GHEAP = 4,
            /// <summary>
            /// Local heap data [value = 5].
            /// </summary>
            H5FD_MEM_LHEAP = 5,
            /// <summary>
            /// Object header data [value = 6].
            /// </summary>
            H5FD_MEM_OHDR = 6,
            /// <summary>
            /// Sentinel value [value = 7].
            /// </summary>
            H5FD_MEM_NTYPES
        }

        /// <summary>
        /// Library's file format versions
        /// </summary>
        public enum libver_t
        {
            /// <summary>
            /// Use the earliest possible format for storing objects
            /// </summary>
            H5F_LIBVER_EARLIEST,
            /// <summary>
            /// Use the latest possible format available for storing objects
            /// </summary>
            H5F_LIBVER_LATEST
        }

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fclear_elink_file_cache(hid_t file_id);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fclose(hid_t file_id);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Fcreate(string filename, uint flags, hid_t create_plist, hid_t access_plist);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fflush(hid_t object_id, scope_t scope);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Fget_access_plist(hid_t file_id);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Fget_create_plist(hid_t file_id);
        
        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fget_filesize(hid_t file_id, ref hsize_t size);
        
        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t H5Fget_file_image(hid_t file_id, IntPtr buf_ptr, size_t buf_len);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hssize_t H5Fget_freespace(hid_t file_id);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fget_info(hid_t obj_id, H5F.info_t bh_info);
        
        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fget_intent(hid_t file_id, uint intent);
        
        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fget_mdc_config(hid_t file_id, ref H5AC.cache_config_t config_ptr);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fget_mdc_hit_rate(hid_t file_id, ref double hit_rate_ptr);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fget_mdc_size(hid_t file_id, ref size_t max_size_ptr, ref size_t min_clean_size_ptr, ref size_t cur_size_ptr, ref int cur_num_entries_ptr);
        
        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t H5Fget_name(hid_t obj_id, string name, size_t size);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static int H5Fget_obj_count(hid_t file_id, H5F.get_obj_count_or_get_obj_ids_flags types);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t H5Fget_obj_ids(hid_t file_id, uint types, size_t max_objs, ref hid_t obj_id_list);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fget_vfd_handle(hid_t file_id, hid_t fapl, IntPtr file_handle);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t H5Fis_hdf5(string filename);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fmount(hid_t loc, string name, hid_t child, hid_t plist);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Fopen(string filename, uint flags, hid_t access_plist);
        
        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t H5Freopen(hid_t file_id);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Freset_mdc_hit_rate_stats(hid_t file_id);

        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Fset_mdc_config(hid_t file_id, ref H5AC.cache_config_t config_ptr);
        
        [DllImport(Constants.DLLFileName, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t H5Funmount(hid_t loc, string name);
    }
}