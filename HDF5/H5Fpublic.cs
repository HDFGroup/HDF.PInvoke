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

using haddr_t = System.UInt64;
using hbool_t = System.UInt32;
using herr_t = System.Int32;
using hsize_t = System.UInt64;
using hssize_t = System.Int64;
using htri_t = System.Int32;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;
using uint32_t = System.UInt32;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed class H5F
    {
        static H5F() { H5.open(); }

        // Flags for H5F.open() and H5F.create() calls

        /// <summary>
        /// absence of rdwr => rd-only
        /// </summary>
        public const uint ACC_RDONLY = 0x0000u;
        /// <summary>
        /// open for read and write
        /// </summary>
        public const uint ACC_RDWR = 0x0001u;
        /// <summary>
        /// overwrite existing files
        /// </summary>
        public const uint ACC_TRUNC = 0x0002u;
        /// <summary>
        /// fail if file already exists
        /// </summary>
        public const uint ACC_EXCL = 0x0004u;
        /// <summary>
        /// create non-existing files
        /// </summary>
        public const uint ACC_CREAT = 0x0010u;

#if HDF5_VER1_10

        /// <summary>
        /// indicate that this file is open for writing in a
        /// single-writer/multi-reader (SWMR) scenario.  Note that the
        /// process(es) opening the file for reading must open the file
        /// with <code>RDONLY</code> access, and use the special
        /// <code>SWMR_READ</code> access flag.
        /// </summary>
        public const uint ACC_SWMR_WRITE = 0x0020u;
        /// <summary>
        /// indicate that this file is open for reading in a 
        /// single-writer/multi-reader (SWMR) scenario. Note that the
        /// process(es) opening the file for SWMR reading must also
        /// open the file with the <code>RDONLY</code> flag.
        /// </summary>
        public const uint ACC_SWMR_READ = 0x0040u;

#endif

        /// <summary>
        /// Value passed to H5P.set_elink_acc_flags to cause flags to be taken
        /// from the parent file.
        /// </summary>
        public const uint ACC_DEFAULT = 0xffffu;


        // Flags for H5F.get_obj_count() & H5F.get_obj_ids() calls

        /// <summary>
        /// File objects
        /// </summary>
        public const uint OBJ_FILE = 0x0001u;
        /// <summary>
        /// Dataset objects
        /// </summary>
        public const uint OBJ_DATASET = 0x0002u;
        /// <summary>
        /// Group objects
        /// </summary>
        public const uint OBJ_GROUP = 0x0004u;
        /// <summary>
        /// Named datatype objects
        /// </summary>
        public const uint OBJ_DATATYPE = 0x0008u;
        /// <summary>
        /// Attribute objects
        /// </summary>
        public const uint OBJ_ATTR = 0x0010u;
        /// <summary>
        /// All objects:
        /// H5F_OBJ_FILE|H5F_OBJ_DATASET|H5F_OBJ_GROUP|H5F_OBJ_DATATYPE|H5F_OBJ_ATTR)
        /// </summary>
        public const uint OBJ_ALL = 0x001Fu;
        /// <summary>
        /// All local objects:
        /// Restrict search to objects opened through current file ID
        /// (as opposed to objects opened through any file ID accessing this
        /// file)
        /// </summary>
        public const uint OBJ_LOCAL = 0x0020u;
        

        public hsize_t FAMILY_DEFAULT = 0;

        /// <summary>
        /// The difference between a single file and a set of mounted files
        /// </summary>
        public enum scope_t
        {
            /// <summary>
            /// specified file handle only [value = 0].
            /// </summary>
            LOCAL = 0,
            /// <summary>
            /// entire virtual file [value = 1].
            /// </summary>
            GLOBAL = 1
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
            DEFAULT = 0,
            /// <summary>
            /// file closes only after all opened objects are closed [value = 1].
            /// </summary>
            WEAK = 1,
            /// <summary>
            /// if no opened objects, file is close; otherwise, file close
            /// fails [value = 2].
            /// </summary>
            SEMI = 2,
            /// <summary>
            /// if there are opened objects, close them first, then close file
            /// [value = 3].
            /// </summary>
            STRONG = 3
        }

        /// <summary>
        /// Current "global" information about a file
        /// (just size info currently)
        /// </summary>
#if HDF5_VER1_10

        public struct info_t
        {
            public super_t super;
            public free_t free;
            public sohm_t sohm;
            public struct super_t
            {
                /// <summary>
                /// Superblock version #
                /// </summary>
                public uint version;
                /// <summary>
                /// Superblock size
                /// </summary>
                public hsize_t super_size;
                /// <summary>
                /// Superblock extension size
                /// </summary>
                public hsize_t super_ext_size;
            }
            public struct free_t
            {
                /// <summary>
                /// Version # of file free space management
                /// </summary>
                public uint version;
                /// <summary>
                /// Free space manager metadata size
                /// </summary>
                public hsize_t meta_size;
                /// <summary>
                /// Amount of free space in the file
                /// </summary>
                public hsize_t tot_space;
            }
            public struct sohm_t
            {
                /// <summary>
                /// Version # of shared object header info
                /// </summary>
                public uint version;
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

#else

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
                /// Shared object header message index and heap size
                /// </summary>
                public H5.ih_info_t msgs_info;
            }
        }

#endif

        /// <summary>
        ///  Types of allocation requests
        /// </summary>
        public enum mem_t
        {
            /// <summary>
            /// Data should not appear in the free list. [value = -1].
            /// </summary>
            NOLIST = -1,
            /// <summary>
            /// Value not yet set.  Can also be the datatype set in a larger
            /// allocation that will be suballocated by the library.
            /// Must be zero. [value = 0].
            /// </summary>
            DEFAULT = 0,
            /// <summary>
            /// Superblock data [value = 1].
            /// </summary>
            SUPER = 1,
            /// <summary>
            /// B-tree data [value = 2].
            /// </summary>
            BTREE = 2,
            /// <summary>
            /// Raw data (content of datasets, etc.) [value = 3].
            /// </summary>
            DRAW = 3,
            /// <summary>
            /// Global heap data [value = 4].
            /// </summary>
            GHEAP = 4,
            /// <summary>
            /// Local heap data [value = 5].
            /// </summary>
            LHEAP = 5,
            /// <summary>
            /// Object header data [value = 6].
            /// </summary>
            OHDR = 6,
            /// <summary>
            /// Sentinel value [value = 7].
            /// </summary>
            NTYPES
        }

#if HDF5_VER1_10

        /// <summary>
        /// Free space section information
        /// </summary>
        public struct sect_info_t
        {
            /// <summary>
            /// Address of free space section
            /// </summary>
            public haddr_t addr;
            /// <summary>
            /// Size of free space section
            /// </summary>
            public hsize_t size;
        }

        /// <summary>
        /// Library's file format versions
        /// </summary>
        public enum libver_t
        {
            ERROR = -1,
            /// <summary>
            /// Use the earliest possible format for storing objects
            /// </summary>
            EARLIEST = 0,
            /// <summary>
            /// Use the latest v18 format for storing objects
            /// </summary>
            V18 = 1,
            /// <summary>
            /// Use the latest v110 format for storing objects
            /// </summary>
            V110 = 2,
            /// <summary>
            /// Use the latest possible format for storing objects
            /// </summary>
            LATEST = 2,
            NBOUNDS
        }

#else
        /// <summary>
        /// Library's file format versions
        /// </summary>
        public enum libver_t
        {
            /// <summary>
            /// Use the earliest possible format for storing objects
            /// </summary>
            EARLIEST,
            /// <summary>
            /// Use the latest possible format available for storing objects
            /// </summary>
            LATEST
        }
#endif

#if HDF5_VER1_10

        /// <summary>
        /// File space handling strategy
        /// </summary>
        public enum fspace_strategy_t
        {
            /// <summary>
            /// Mechanisms: free-space managers, aggregators, and virtual
            /// file drivers. This is the library default when not set.
            /// </summary>
            FSM_AGGR = 0,
            /// <summary>
            /// Mechanisms: free-space managers with embedded paged
            /// aggregation and virtual file drivers
            /// </summary>
            PAGE = 1,
            /// <summary>
            /// Mechanisms: aggregators and virtual file drivers
            /// </summary>
            AGGR = 2,
            /// <summary>
            /// Mechanisms: virtual file drivers 
            /// </summary>
            AGGR_NONE = 3,
            NTYPES
        }

        /// <summary>
        /// Deprecated: File space handling strategy for release 1.10.0
        /// They are mapped to H5F_fspace_strategy_t as defined above from
        /// release 1.10.1 onwards
        /// </summary>
        public enum file_space_type_t
        {
            /// <summary>
            /// Default (or current) free space strategy setting
            /// </summary>
            DEFAULT = 0,
            /// <summary>
            /// Persistent free space managers, aggregators, virtual file
            /// driver
            /// </summary>
            ALL_PERSIST = 1,
            /// <summary>
            /// Non-persistent free space managers, aggregators, virtual file
            /// driver. This is the library default
            /// </summary>
            ALL = 2,
            /// <summary>
            /// Aggregators, Virtual file driver 
            /// </summary>
            AGGR_VFD = 3,
            /// <summary>
            /// Virtual file driver
            /// </summary>
            VFD = 4,
            NTYPES
        }

        public const int NUM_METADATA_READ_RETRY_TYPES = 21;
        /// <summary>
        /// Data structure to report the collection of read retries for
        /// metadata items with checksum. Used by public routine
        /// <code>H5F.get_metadata_read_retry_info</code>
        /// </summary>
        /*
         * 
         * typedef struct H5F_retry_info_t {
         *    unsigned nbins;
         *    uint32_t *retries[H5F_NUM_METADATA_READ_RETRY_TYPES];
         * } H5F_retry_info_t;
         * 
         */
        public struct retry_info_t
        {
            public uint nbins;
            public IntPtr retries0;
            public IntPtr retries1;
            public IntPtr retries2;
            public IntPtr retries3;
            public IntPtr retries4;
            public IntPtr retries5;
            public IntPtr retries6;
            public IntPtr retries7;
            public IntPtr retries8;
            public IntPtr retries9;
            public IntPtr retries10;
            public IntPtr retries11;
            public IntPtr retries12;
            public IntPtr retries13;
            public IntPtr retries14;
            public IntPtr retries15;
            public IntPtr retries16;
            public IntPtr retries17;
            public IntPtr retries18;
            public IntPtr retries19;
            public IntPtr retries20;
        }

        /// <summary>
        /// Callback for <code>H5P.set_object_flush_cb</code> in a file access
        /// property list
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t flush_cb_t(hid_t object_id, IntPtr udata);

#endif

        /// <summary>
        /// Clears the external link open file cache.
        /// https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-ClearELinkFileCache
        /// </summary>
        /// <param name="file_id">File identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Fclear_elink_file_cache",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t clear_elink_file_cache(hid_t file_id);

        /// <summary>
        /// Terminates access to an HDF5 file.
        /// https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Close
        /// </summary>
        /// <param name="file_id">Identifier of a file to which access is
        /// terminated.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fclose",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t close(hid_t file_id);

        /// <summary>
        /// Creates an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-CreateS
        /// </summary>
        /// <param name="filename">Name of the file to access.</param>
        /// <param name="flags">File access flags (H5.ACC_*).</param>
        /// <param name="create_plist">File creation property list identifier.
        /// </param>
        /// <param name="access_plist">File access property list identifier.
        /// </param>
        /// <returns>Returns a file identifier if successful; otherwise returns
        /// a negative value.</returns>
        /// <remarks><paramref name="filename"/> MUST be an ASCII string.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fcreate",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t create
            (string filename, uint flags,
            hid_t create_plist = H5P.DEFAULT, hid_t access_plist = H5P.DEFAULT);

        /// <summary>
        /// Flushes all buffers associated with a file to disk.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Flush
        /// </summary>
        /// <param name="object_id">Identifier of object used to identify the
        /// file.</param>
        /// <param name="scope">Specifies the scope of the flushing
        /// action.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fflush",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t flush(hid_t object_id, scope_t scope);

#if HDF5_VER1_10

        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Fformat_convert_super",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t format_convert_super(hid_t fid);

#endif

        /// <summary>
        /// Returns a file access property list identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetAccessPlist
        /// </summary>
        /// <param name="file_id">Identifier of file of which to get access
        /// property list</param>
        /// <returns>Returns a file access property list identifier if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_access_plist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t get_access_plist(hid_t file_id);

        /// <summary>
        /// Returns a file creation property list identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetCreatePlist
        /// </summary>
        /// <param name="file_id">Identifier of file of which to get creation
        /// property list</param>
        /// <returns>Returns a file access property list identifier if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_create_plist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t get_create_plist(hid_t file_id);

#if HDF5_VER1_10
        /// <summary>
        /// Retrieves the setting for whether or not a file will create minimized dataset object headers.
        /// See https://portal.hdfgroup.org/display/HDF5/H5F_GET_DSET_NO_ATTRS_HINT
        /// </summary>
        /// <param name="file_id">File identifier</param>
        /// <param name="minimize">Flag indicating whether the library will or will not
        /// create minimized dataset object headers</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Fget_dset_no_attrs_hint",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_dset_no_attrs_hint(hid_t file_id, ref hbool_t minimize);
#endif

        /// <summary>
        /// Retrieves a copy of the image of an existing, open file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetFileImage
        /// </summary>
        /// <param name="file_id">Target file identifier</param>
        /// <param name="buf_ptr">Pointer to the buffer into which the image of
        /// the HDF5 file is to be copied</param>
        /// <param name="buf_len">Size of the supplied buffer</param>
        /// <returns>If successful, returns the size in bytes of the buffer
        /// required to store the file image if successful; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_file_image", 
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_file_image
            (hid_t file_id, IntPtr buf_ptr, IntPtr buf_len);

        /// <summary>
        /// Returns the size of an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetFilesize
        /// </summary>
        /// <param name="file_id">Identifier of a currently-open HDF5
        /// file</param>
        /// <param name="size">Size of the file, in bytes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_filesize",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_filesize
            (hid_t file_id, ref hsize_t size);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves free-space section information for a file.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FileSpace/H5Fget_free_sections.htm
        /// </summary>
        /// <param name="file_id">Identifier of a currently-open HDF5
        /// file</param>
        /// <param name="type">The file memory allocation type.</param>
        /// <param name="nsects">The number of free-space sections.</param>
        /// <param name="sect_info">Array of instances of
        /// <code>H5F.sect_info_t</code> in which the free-space section
        /// information is to be returned.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_free_sections",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_free_sections
            (hid_t file_id, mem_t type, size_t nsects,
            sect_info_t[] sect_info);

#endif

        /// <summary>
        /// Returns the amount of free space in a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetFreespace
        /// </summary>
        /// <param name="file_id">Identifier of a currently-open HDF5
        /// file</param>
        /// <returns>Returns the amount of free space in the file if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_freespace", 
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hssize_t get_freespace(hid_t file_id);

        /// <summary>
        /// Returns global information for a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetInfo
        /// </summary>
        /// <param name="obj_id">Object identifier for any object in the
        /// file.</param>
        /// <param name="bh_info">Struct containing global file
        /// information.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
 #if HDF5_VER1_10
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_info2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
 #else
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
 #endif
        public extern static herr_t get_info
            (hid_t obj_id, ref H5F.info_t bh_info);

        /// <summary>
        /// Determines the read/write or read-only status of a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetIntent
        /// </summary>
        /// <param name="file_id">File identifier for a currently-open HDF5
        /// file.</param>
        /// <param name="intent">Intended access mode flag as originally passed
        /// with H5Fopen.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_intent",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_intent(hid_t file_id, ref uint intent);

        /// <summary>
        /// Obtain current metadata cache configuration for target file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetMdcConfig
        /// </summary>
        /// <param name="file_id">Identifier of the target file</param>
        /// <param name="config_ptr">Pointer to the instance of
        /// <code>H5AC_cache_config_t</code> in which the current metadata
        /// cache configuration is to be reported.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_mdc_config",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_mdc_config
            (hid_t file_id, ref H5AC.cache_config_t config_ptr);

        /// <summary>
        /// Obtain target file's metadata cache hit rate.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetMdcHitRate
        /// </summary>
        /// <param name="file_id">Identifier of the target file</param>
        /// <param name="hit_rate_ptr">Pointer to the double in which the hit
        /// rate is returned.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_mdc_hit_rate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_mdc_hit_rate
            (hid_t file_id, ref double hit_rate_ptr);

#if HDF5_VER1_10

        /// <summary>
        /// Obtain information about a cache image if it exists.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetMdcImageInfo
        /// </summary>
        /// <param name="file_id">Identifier of the target file.</param>
        /// <param name="image_addr">Offset of the cache image if it exists,
        /// or <code>HADDR_UNDEF</code> if it does not.
        /// <param name="image_len">Length of the cache image if it exists,
        /// or 0 if it does not.
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_mdc_image_info",
           CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_mdc_image_info
            (hid_t file_id, ref haddr_t image_addr, ref haddr_t image_len);

        /// <summary>
        /// Gets the current metadata cache logging status.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Fget_mdc_logging_status.htm
        /// </summary>
        /// <param name="file_id">Identifier of an open HDF5 file.</param>
        /// <param name="is_enabled">Whether logging is enabled.</param>
        /// <param name="is_currently_logging">Whether events are currently
        /// being logged.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Fget_mdc_logging_status",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_mdc_logging_status
            (hid_t file_id, ref hbool_t is_enabled,
            ref hbool_t is_currently_logging);

#endif

        /// <summary>
        /// Obtain current metadata cache size data for specified file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetMdcSize
        /// </summary>
        /// <param name="file_id">Identifier of the target file</param>
        /// <param name="max_size_ptr">Pointer to the location in which the
        /// current cache maximum size is to be returned, or NULL if this datum
        /// is not desired.</param>
        /// <param name="min_clean_size_ptr">Pointer to the location in which
        /// the current cache minimum clean size is to be returned, or NULL if
        /// that datum is not desired.</param>
        /// <param name="cur_size_ptr">Pointer to the location in which the
        /// current cache size is to be returned, or NULL if that datum is not
        /// desired.</param>
        /// <param name="cur_num_entries_ptr">Pointer to the location in which
        /// the current number of entries in the cache is to be returned, or
        /// NULL if that datum is not desired.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_mdc_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_mdc_size
            (hid_t file_id, ref size_t max_size_ptr,
            ref size_t min_clean_size_ptr, ref size_t cur_size_ptr,
            ref int cur_num_entries_ptr);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the collection of read retries for metadata entries with
        /// checksum.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Fget_metadata_read_retry_info.htm
        /// </summary>
        /// <param name="file_id">Identifier for a currently opened HDF5 file.</param>
        /// <param name="info">Struct containing the collection of read retries
        /// for metadata entries with checksum.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Fget_metadata_read_retry_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_metadata_read_retry_info
            (hid_t file_id, ref retry_info_t info);

#endif

        /// <summary>
        /// Retrieves name of file to which object belongs.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetName
        /// </summary>
        /// <param name="obj_id">Identifier of the object for which the
        /// associated filename is sought.</param>
        /// <param name="name">Buffer to contain the returned filename.</param>
        /// <param name="size">Buffer size, in bytes.</param>
        /// <returns>Returns the length of the filename if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_name",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name
            (hid_t obj_id, StringBuilder name, size_t size);

        /// <summary>
        /// Returns the number of open object identifiers for an open file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetObjCount
        /// </summary>
        /// <param name="file_id">Identifier of a currently-open HDF5 file.
        /// </param>
        /// <param name="types">Type of object for which identifiers are to be
        /// returned.</param>
        /// <returns>Returns the number of open objects if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_obj_count",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_obj_count
            (hid_t file_id, uint types);

        /// <summary>
        /// Returns a list of open object identifiers.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetObjIDs
        /// </summary>
        /// <param name="file_id">Identifier of a currently-open HDF5 file.</param>
        /// <param name="types">Type of object for which identifiers are to be
        /// returned.</param>
        /// <param name="max_objs">Maximum number of object identifiers to be
        /// returned.</param>
        /// <param name="obj_id_list">Pointer to the returned list of open
        /// object identifiers.</param>
        /// <returns>Returns number of objects placed into obj_id_list if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_obj_ids",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_obj_ids
            (hid_t file_id, uint types, size_t max_objs, IntPtr obj_id_list);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves statistics about page access when it is enabled.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetPBStats
        /// </summary>
        /// <param name="file_id">File identifier</param>
        /// <param name="accesses">Two integer array for the number of metadata
        /// and raw data accesses to the page buffer</param>
        /// <param name="hits">Two integer array for the number of metadata
        /// and raw data hits in the page buffer</param>
        /// <param name="misses">Two integer array for the number of metadata
        /// and raw data misses in the page buffer</param>
        /// <param name="evictions">Two integer array for the number of
        /// metadata and raw data evictions from the page buffer</param>
        /// <param name="bypasses">Two integer array for the number of metadata
        /// and raw data accesses that bypass the page buffer</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Fget_page_buffering_stats",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_page_buffering_stats
            (hid_t file_id,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 2)] int accesses,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 2)] int hits,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 2)] int misses,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 2)] int evictions,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 2)] int bypasses);

#endif

        /// <summary>
        /// Returns pointer to the file handle from the virtual file driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-GetVfdHandle
        /// </summary>
        /// <param name="file_id">Identifier of the file to be queried.</param>
        /// <param name="fapl">File access property list identifier.</param>
        /// <param name="file_handle">Pointer to the file handle being used by
        /// the low-level virtual file driver.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fget_vfd_handle",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_vfd_handle
            (hid_t file_id, hid_t fapl, ref IntPtr file_handle);

        /// <summary>
        /// Determines whether a file is in the HDF5 format.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-IsHDF5
        /// </summary>
        /// <returns>When successful, returns a positive value, for TRUE,
        /// or 0 (zero), for FALSE. On any error, including the case that
        /// the file does not exist, returns a negative value.</returns>
        /// <remarks><paramref name="filename"/> MUST be an ASCII string.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fis_hdf5",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t is_hdf5(string filename);

        /// <summary>
        /// Mounts a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Mount
        /// </summary>
        /// <param name="loc">Identifier for of file or group in which name
        /// is defined.</param>
        /// <param name="name">Name of the group onto which the file specified
        /// by <paramref name="child"/> is to be mounted.</param>
        /// <param name="child">Identifier of the file to be mounted.</param>
        /// <param name="plist">File mount property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fmount",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t mount
            (hid_t loc, byte[] name, hid_t child, hid_t plist = H5P.DEFAULT);

        /// <summary>
        /// Mounts a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Mount
        /// </summary>
        /// <param name="loc">Identifier for of file or group in which name
        /// is defined.</param>
        /// <param name="name">Name of the group onto which the file specified
        /// by <paramref name="child"/> is to be mounted.</param>
        /// <param name="child">Identifier of the file to be mounted.</param>
        /// <param name="plist">File mount property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fmount",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t mount
            (hid_t loc, string name, hid_t child, hid_t plist = H5P.DEFAULT);

        /// <summary>
        /// Opens an existing HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Open
        /// </summary>
        /// <param name="filename">Name of the file to be opened.</param>
        /// <param name="flags">File access flags. (<code>H5F_ACC_RDWR</code>
        /// or <code>H5F_ACC_RDONLY</code>)</param>
        /// <param name="plist">Identifier for the file access properties
        /// list.</param>
        /// <returns>Returns a file identifier if successful; otherwise returns
        /// a negative value.</returns>
        /// <remarks><paramref name="filename"/> MUST be an ASCII string!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fopen",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open
            (string filename, uint flags, hid_t plist = H5P.DEFAULT);

        /// <summary>
        /// Returns a new identifier for a previously-opened HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Reopen
        /// </summary>
        /// <param name="file_id">Identifier of a file for which an additional
        /// identifier is required.</param>
        /// <returns>Returns a new file identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Freopen",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t reopen(hid_t file_id);

        /// <summary>
        /// Reset hit rate statistics counters for the target file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-ResetMdcHitRateStats
        /// </summary>
        /// <param name="file_id">Identifier of the target file.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Freset_mdc_hit_rate_stats",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t reset_mdc_hit_rate_stats(hid_t file_id);

#if HDF5_VER1_10

        /// <summary>
        /// Resets the page buffer statistics.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-ResetPBStats
        /// </summary>
        /// <param name="file_id">File identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Freset_page_buffering_stats",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t reset_page_buffering_stats(hid_t file_id);

        /// <summary>
        /// Sets the flag to create minimized dataset object headers.
        /// See https://portal.hdfgroup.org/display/HDF5/H5F_SET_DSET_NO_ATTRS_HINT
        /// </summary>
        /// <param name="file_id">File identifier</param>
        /// <param name="minimize">Flag indicating whether the library will or will not
        /// create minimized dataset object headers</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Fset_dset_no_attrs_hint",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_dset_no_attrs_hint(hid_t file_id, hbool_t minimize);

#endif

        /// <summary>
        /// Attempt to configure metadata cache of target file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-SetMdcConfig
        /// </summary>
        /// <param name="file_id">Identifier of the target file</param>
        /// <param name="config_ptr">Pointer to the instance of
        /// <code>H5AC_cache_config_t</code> containing the desired
        /// configuration.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fset_mdc_config",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t set_mdc_config
            (hid_t file_id, ref H5AC.cache_config_t config_ptr);

#if HDF5_VER1_10

        /// <summary>
        /// Starts logging metadata cache events if logging was previously
        /// enabled.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Fstart_mdc_logging.htm
        /// </summary>
        /// <param name="file_id">Identifier of an open HDF5 file.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fstart_mdc_logging",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t start_mdc_logging(hid_t file_id);

        /// <summary>
        /// Enables SWMR writing mode for a file.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Fstart_swmr_write.htm
        /// </summary>
        /// <param name="file_id">A file identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fstart_swmr_write",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t start_swmr_write(hid_t file_id);

        /// <summary>
        /// Stops logging metadata cache events if logging was previously
        /// enabled and is currently ongoing.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Fstop_mdc_logging.htm
        /// </summary>
        /// <param name="file_id">Identifier of an open HDF5 file.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Fstop_mdc_logging",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t stop_mdc_logging(hid_t file_id);

#endif

        /// <summary>
        /// Unmounts a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Unmount
        /// </summary>
        /// <param name="loc">File or group identifier for the location at
        /// which the specified file is to be unmounted.</param>
        /// <param name="name">Name of the mount point.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Funmount",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t unmount(hid_t loc, byte[] name);

        /// <summary>
        /// Unmounts a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5F.html#File-Unmount
        /// </summary>
        /// <param name="loc">File or group identifier for the location at
        /// which the specified file is to be unmounted.</param>
        /// <param name="name">Name of the mount point.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Funmount",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t unmount(hid_t loc, string name);
    }
}