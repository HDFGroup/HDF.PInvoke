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
using htri_t = System.Int32;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;
using time_t = System.UInt64;
using uint64_t = System.UInt64;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed class H5O
    {
        static H5O() { H5.open(); }

        #region Flags for object copy (H5Ocopy)

        /// <summary>
        /// Copy only immediate members
        /// </summary>
        public const uint COPY_SHALLOW_HIERARCHY_FLAG = 0x0001u;
        /// <summary>
        /// Expand soft links into new objects
        /// </summary>
        public const uint COPY_EXPAND_SOFT_LINK_FLAG = 0x0002u;
        /// <summary>
        /// Expand external links into new objects
        /// </summary>
        public const uint COPY_EXPAND_EXT_LINK_FLAG = 0x0004u;
        /// <summary>
        /// Copy objects that are pointed by references
        /// </summary>
        public const uint COPY_EXPAND_REFERENCE_FLAG = 0x0008u;
        /// <summary>
        /// Copy object without copying attributes
        /// </summary>
        public const uint COPY_WITHOUT_ATTR_FLAG = 0x0010u;
        /// <summary>
        /// Copy <code>NULL</code> messages (empty space)
        /// </summary>
        public const uint COPY_PRESERVE_NULL_FLAG = 0x0020u;
        /// <summary>
        /// Merge committed datatypes in dest file
        /// </summary>
        public const uint COPY_MERGE_COMMITTED_DTYPE_FLAG = 0x0040u;
        /// <summary>
        /// All object copying flags (for internal checking
        /// </summary>
        public const uint COPY_ALL = 0x007Fu;

        #endregion

        #region Flags for shared message indexes
        /* Pass these flags in using the mesg_type_flags parameter in
         * H5P_set_shared_mesg_index.
         * (Developers: These flags correspond to object header message type IDs,
         * but we need to assign each kind of message to a different bit so that
         * one index can hold multiple types.)
         */
        /// <summary>
        /// No shared messages
        /// </summary>
        public const uint SHMESG_NONE_FLAG = 0x0000;
        /// <summary>
        /// Simple Dataspace Message.
        /// </summary>
        public const uint SHMESG_SDSPACE_FLAG = ((uint)1 << 0x0001);
        /// <summary>
        /// Datatype Message.
        /// </summary>
        public const uint SHMESG_DTYPE_FLAG = ((uint)1 << 0x0003);
        /// <summary>
        /// Fill Value Message.
        /// </summary>
        public const uint SHMESG_FILL_FLAG = ((uint)1 << 0x0005);
        /// <summary>
        /// Filter pipeline message.
        /// </summary>
        public const uint SHMESG_PLINE_FLAG = ((uint)1 << 0x000b);
        /// <summary>
        /// Attribute Message.
        /// </summary>
        public const uint SHMESG_ATTR_FLAG = ((uint)1 << 0x000c);
        
        public const uint SHMESG_ALL_FLAG = (SHMESG_SDSPACE_FLAG |
            SHMESG_DTYPE_FLAG | SHMESG_FILL_FLAG | SHMESG_PLINE_FLAG |
            SHMESG_ATTR_FLAG);

        #endregion

        #region Object header status flag definitions

        /// <summary>
        /// 2-bit field indicating # of bytes to store the size of chunk 0's
        /// data
        /// </summary>
        public const uint HDR_CHUNK0_SIZE = 0x03;
        /// <summary>
        /// Attribute creation order is tracked
        /// </summary>
        public const uint HDR_ATTR_CRT_ORDER_TRACKED = 0x04;
        /// <summary>
        /// Attribute creation order has index
        /// </summary>
        public const uint HDR_ATTR_CRT_ORDER_INDEXED = 0x08;
        /// <summary>
        /// Non-default attribute storage phase change values stored
        /// </summary>
        public const uint HDR_ATTR_STORE_PHASE_CHANGE = 0x10;
        /// <summary>
        /// Store access, modification, change and birth times for object
        /// </summary>
        public const uint HDR_STORE_TIMES = 0x20;

        public const uint HDR_ALL_FLAGS = (HDR_CHUNK0_SIZE |
            HDR_ATTR_CRT_ORDER_TRACKED | HDR_ATTR_CRT_ORDER_INDEXED |
            HDR_ATTR_STORE_PHASE_CHANGE | HDR_STORE_TIMES);

        #endregion

        #region Maximum shared message values.
        /* Number of indexes is 8 to allow room to add new types of messages. */

        public const uint SHMESG_MAX_NINDEXES = 8;
        public const uint SHMESG_MAX_LIST_SIZE = 5000;

        #endregion

        /// <summary>
        /// Types of objects in file
        /// </summary>
        public enum type_t
        {
            /// <summary>
            /// Unknown object type [value = -1]
            /// </summary>
            UNKNOWN = -1,
            /// <summary>
            /// Object is a group [value = 0]
            /// </summary>
            GROUP,
            /// <summary>
            /// Object is a dataset [value = 1]
            /// </summary>
            DATASET,
            /// <summary>
            /// Object is a named data type [value = 2]
            /// </summary>
            NAMED_DATATYPE,
            /// <summary>
            /// Number of different object types (must be last!) [value = 3]
            /// </summary>
            NTYPES
        }

        /// <summary>
        /// Information struct for object header metadata
        /// (for H5Oget_info/H5Oget_info_by_name/H5Oget_info_by_idx)
        /// </summary>
        public struct hdr_info_t
        {
            /// <summary>
            /// Version number of header format in file
            /// </summary>
            public uint version;
            /// <summary>
            /// Number of object header messages
            /// </summary>
            public uint nmesgs;
            /// <summary>
            /// Number of object header chunks
            /// </summary>
            public uint nchunks;
            /// <summary>
            /// Object header status flags
            /// </summary>
            public uint flags;
            public space_t space;
            public mesg_t mesg;

            public struct space_t
            {
                /// <summary>
                /// Total space for storing object header in file
                /// </summary>
                public hsize_t total;
                /// <summary>
                /// Space within header for object header metadata information
                /// </summary>
                public hsize_t meta;
                /// <summary>
                /// Space within header for actual message information
                /// </summary>
                public hsize_t mesg;
                /// <summary>
                /// Free space within object header
                /// </summary>
                public hsize_t free;
            };

            public struct mesg_t
            {
                /// <summary>
                /// Flags to indicate presence of message type in header
                /// </summary>
                public uint64_t present;
                /// <summary>
                /// Flags to indicate message type is shared in header
                /// </summary>
                public uint64_t shared;
            };
        }

        public struct meta_size_t
        {
            /// <summary>
            /// v1/v2 B-tree and local/fractal heap for groups, B-tree for
            /// chunked datasets
            /// </summary>
            public H5.ih_info_t obj;
            /// <summary>
            /// v2 B-tree and heap for attributes
            /// </summary>
            public H5.ih_info_t attr;
        }

        /// <summary>
        /// Information struct for object
        /// (for H5Oget_info/H5Oget_info_by_name/H5Oget_info_by_idx)
        /// </summary>
        public struct info_t
        {
            /// <summary>
            /// File number that object is located in
            /// </summary>
            public uint fileno;
            /// <summary>
            /// Object address in file
            /// </summary>
            public haddr_t addr;
            /// <summary>
            /// Basic object type (group, dataset, etc.)
            /// </summary>
            public type_t type;
            /// <summary>
            /// Reference count of object
            /// </summary>
            public uint rc;
            /// <summary>
            /// Access time
            /// </summary>
            public time_t atime;
            /// <summary>
            /// Modification time
            /// </summary>
            public time_t mtime;
            /// <summary>
            /// Change time
            /// </summary>
            public time_t ctime;
            /// <summary>
            /// Birth time
            /// </summary>
            public time_t btime;
            /// <summary>
            /// # of attributes attached to object
            /// </summary>
            public hsize_t num_attrs;
            /// <summary>
            /// Object header information
            /// </summary>
            public hdr_info_t hdr;
            /// <summary>
            /// Extra metadata storage for object and attributes
            /// </summary>
            public meta_size_t meta_size;
        }

        /// <summary>
        /// Prototype for H5Ovisit/H5Ovisit_by_name() operator
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="info"></param>
        /// <param name="op_data"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t iterate_t
        (hid_t obj, IntPtr name, ref info_t info, IntPtr op_data);

        public enum mcdt_search_ret_t
        {
            /// <summary>
            /// Abort H5Ocopy [value = -1]
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// Continue the global search of all committed datatypes in the
            /// destination file [value = 0]
            /// </summary>
            CONT,
            /// <summary>
            /// Stop the search, but continue copying. The committed datatype
            /// will be copied but not merged. [value = 1]
            /// </summary>
            STOP
        }

        /// <summary>
        /// Callback to invoke when completing the search for a matching
        /// committed datatype from the committed dtype list
        /// </summary>
        /// <param name="op_data">Pointer to user-defined input data.</param>
        /// <returns>Returns one of the <code>MCDT_SEARCH_*</code> values.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate mcdt_search_ret_t mcdt_search_cb_t(IntPtr op_data);

#if HDF5_VER1_10

        /// <summary>
        /// Determines if an HDF5 object (dataset, group, committed datatype)
        /// has had flushes of metadata entries disabled.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Oare_mdc_flushes_disabled.htm
        /// </summary>
        /// <param name="object_id">Identifier of an object in the cache.</param>
        /// <param name="are_disabled">Flushes enabled/disabled.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Oare_mdc_flushes_disabled",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t are_mdc_flushes_disabled
            (hid_t object_id, ref hbool_t are_disabled);

#endif

        /// <summary>
        /// Closes an object in an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Close
        /// </summary>
        /// <param name="object_id">Object identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oclose",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t
            close
            (hid_t object_id);

        /// <summary>
        /// Copies an object in an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Copy
        /// </summary>
        /// <param name="src_loc_id">Object identifier indicating the location
        /// of the source object to be copied</param>
        /// <param name="src_name">Name of the source object to be copied</param>
        /// <param name="dst_loc_id">Location identifier specifying the
        /// destination</param>
        /// <param name="dst_name">Name to be assigned to the new copy</param>
        /// <param name="ocpypl_id">Object copy property list</param>
        /// <param name="lcpl_id">Link creation property list for the new hard
        /// link</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ocopy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t copy
            (hid_t src_loc_id, byte[] src_name, hid_t dst_loc_id,
            byte[] dst_name, hid_t ocpypl_id = H5P.DEFAULT,
            hid_t lcpl_id = H5P.DEFAULT);

        /// <summary>
        /// Copies an object in an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Copy
        /// </summary>
        /// <param name="src_loc_id">Object identifier indicating the location
        /// of the source object to be copied</param>
        /// <param name="src_name">Name of the source object to be copied</param>
        /// <param name="dst_loc_id">Location identifier specifying the
        /// destination</param>
        /// <param name="dst_name">Name to be assigned to the new copy</param>
        /// <param name="ocpypl_id">Object copy property list</param>
        /// <param name="lcpl_id">Link creation property list for the new hard
        /// link</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ocopy",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t copy
            (hid_t src_loc_id, string src_name, hid_t dst_loc_id,
            string dst_name, hid_t ocpypl_id = H5P.DEFAULT,
            hid_t lcpl_id = H5P.DEFAULT);

        /// <summary>
        /// Decrements an object's reference count.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-DecrRefCount
        /// </summary>
        /// <param name="object_id">Object identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>
        /// This function must be used with care! Improper use can lead to
        /// inaccessible data, wasted space in the file, or file corruption.
        /// </remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Odecr_refcount",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t decr_refcount(hid_t object_id);

#if HDF5_VER1_10

        /// <summary>
        /// Prevents metadata entries for an HDF5 object from being flushed
        /// from the metadata cache to storage.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Odisable_mdc_flushes.htm
        /// </summary>
        /// <param name="object_id">Identifier of the object that will have
        /// flushes disabled.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Odisable_mdc_flushes",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t disable_mdc_flushes(hid_t object_id);

        /// <summary>
        /// Allow metadata entries for an HDF5 object to be flushed
        /// from the metadata cache to storage.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Oenable_mdc_flushes.htm
        /// </summary>
        /// <param name="object_id">Identifier of the object that will have
        /// flushes (re-)enabled.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oenable_mdc_flushes",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t enable_mdc_flushes(hid_t object_id);

#endif

        /// <summary>
        /// Determines whether a link resolves to an actual object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-ExistsByName
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to query.</param>
        /// <param name="name">The name of the link to check.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns 1 or 0 if successful; otherwise returns a negative
        /// value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oexists_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists_by_name
            (hid_t loc_id, byte[] name, hid_t lapl_id = H5P.DEFAULT);

#if HDF5_VER1_10

        /// <summary>
        /// Flushes all buffers associated with an HDF5 object to disk.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Oflush.htm
        /// </summary>
        /// <param name="obj_id">Identifier of the object to be flushed.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oflush",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t flush(hid_t obj_id);

#endif

        /// <summary>
        /// Determines whether a link resolves to an actual object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-ExistsByName
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to query.</param>
        /// <param name="name">The name of the link to check.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns 1 or 0 if successful; otherwise returns a negative
        /// value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oexists_by_name",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists_by_name
            (hid_t loc_id, string name, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves comment for specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetComment
        /// </summary>
        /// <param name="obj_id">Identifier for the target object.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="size">Size of the <paramref name="comment"/> buffer.</param>
        /// <returns>Upon success, returns the number of characters in the
        /// comment, not including the <code>NULL</code> terminator, or zero
        /// (0) if the object has no comment. The value returned may be larger
        /// than <code>size</code>. Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_comment",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_comment
            (hid_t obj_id, [In][Out]StringBuilder comment, size_t size);

        /// <summary>
        /// Retrieves comment for specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetCommentByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file, group, dataset, or named
        /// datatype.</param>
        /// <param name="name">Name of the object whose comment is to be
        /// retrieved, specified as a path relative to
        /// <paramref name="loc_id"/>.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="size">Size of the <paramref name="comment"/> buffer.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Upon success, returns the number of characters in the
        /// comment, not including the <code>NULL</code> terminator, or zero
        /// (0) if the object has no comment. The value returned may be larger
        /// than <paramref name="size"/>. Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_comment_by_name",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_comment_by_name
            (hid_t loc_id, byte[] name, [In][Out]StringBuilder comment, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves comment for specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetCommentByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file, group, dataset, or named
        /// datatype.</param>
        /// <param name="name">Name of the object whose comment is to be
        /// retrieved, specified as a path relative to
        /// <paramref name="loc_id"/>.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="size">Size of the <paramref name="comment"/> buffer.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Upon success, returns the number of characters in the
        /// comment, not including the <code>NULL</code> terminator, or zero
        /// (0) if the object has no comment. The value returned may be larger
        /// than <paramref name="size"/>. Otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_comment_by_name",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_comment_by_name
            (hid_t loc_id, string name, [In][Out]StringBuilder comment, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the metadata for an object specified by an identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfo
        /// </summary>
        /// <param name="loc_id">Identifier for object of type specified by
        /// <code>H5O.type_t</code></param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info1",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info(hid_t loc_id, ref info_t oinfo);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by an
        /// index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="group_name">Name of group in which object is located</param>
        /// <param name="idx_type">Index or field that determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Object for which information is to be returned</param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_idx1",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by an
        /// index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="group_name">Name of group in which object is located</param>
        /// <param name="idx_type">Index or field that determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Object for which information is to be returned</param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_idx1",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by
        /// location and relative name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByName
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="name">Name of object, relative to
        /// <paramref name="loc_id"/></param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_name1",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_name
            (hid_t loc_id, byte[] name, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by
        /// location and relative name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByName
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="name">Name of group, relative to
        /// <paramref name="loc_id"/></param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_name1",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_name
            (hid_t loc_id, string name, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

#else

        /// <summary>
        /// Retrieves the metadata for an object specified by an identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfo
        /// </summary>
        /// <param name="loc_id">Identifier for object of type specified by
        /// <code>H5O.type_t</code></param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info(hid_t loc_id, ref info_t oinfo);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by an
        /// index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="group_name">Name of group in which object is located</param>
        /// <param name="idx_type">Index or field that determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Object for which information is to be returned</param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by an
        /// index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="group_name">Name of group in which object is located</param>
        /// <param name="idx_type">Index or field that determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Object for which information is to be returned</param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_idx",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by
        /// location and relative name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByName
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="name">Name of object, relative to
        /// <paramref name="loc_id"/></param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_name
            (hid_t loc_id, byte[] name, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves the metadata for an object, identifying the object by
        /// location and relative name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-GetInfoByName
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of group in which object is located</param>
        /// <param name="name">Name of group, relative to
        /// <paramref name="loc_id"/></param>
        /// <param name="oinfo">Buffer in which to return object information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oget_info_by_name",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_name
            (hid_t loc_id, string name, ref info_t oinfo,
            hid_t lapl_id = H5P.DEFAULT);

#endif

        /// <summary>
        /// Increments an object's reference count.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-IncrRefCount
        /// </summary>
        /// <param name="object_id">Object identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>This function must be used with care! Improper use can
        /// lead to inaccessible data, wasted space in the file, or file
        /// corruption.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oincr_refcount",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t incr_refcount(hid_t object_id);

        /// <summary>
        /// Creates a hard link to an object in an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Link
        /// </summary>
        /// <param name="obj_id">Object to be linked.</param>
        /// <param name="new_loc_id">File or group identifier specifying
        /// location at which object is to be linked.</param>
        /// <param name="new_name">Name of link to be created, relative to
        /// <paramref name="new_loc_id"/>.</param>
        /// <param name="lcpl_id">Link creation property list identifier.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Olink",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t link
            (hid_t obj_id, hid_t new_loc_id, byte[] new_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a hard link to an object in an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Link
        /// </summary>
        /// <param name="obj_id">Object to be linked.</param>
        /// <param name="new_loc_id">File or group identifier specifying
        /// location at which object is to be linked.</param>
        /// <param name="new_name">Name of link to be created, relative to
        /// <paramref name="new_loc_id"/>.</param>
        /// <param name="lcpl_id">Link creation property list identifier.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Olink",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t link
            (hid_t obj_id, hid_t new_loc_id, string new_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an object in an HDF5 file by location identifier and path name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Open
        /// </summary>
        /// <param name="loc_id">File or group identifier</param>
        /// <param name="name">Path to the object, relative to
        /// <paramref name="loc_id"/>.</param>
        /// <param name="lapl_id">Access property list identifier for the link
        /// pointing to the object</param>
        /// <returns>Returns an object identifier for the opened object if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oopen",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open
            (hid_t loc_id, byte[] name, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an object in an HDF5 file by location identifier and path name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Open
        /// </summary>
        /// <param name="loc_id">File or group identifier</param>
        /// <param name="name">Path to the object, relative to
        /// <paramref name="loc_id"/>.</param>
        /// <param name="lapl_id">Access property list identifier for the link
        /// pointing to the object</param>
        /// <returns>Returns an object identifier for the opened object if
        /// successful; otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oopen",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open
            (hid_t loc_id, string name, hid_t lapl_id = H5P.DEFAULT);
        
        /// <summary>
        /// Opens an object using its address within an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-OpenByAddr
        /// </summary>
        /// <param name="loc_id">File or group identifier</param>
        /// <param name="addr">Object’s address in the file</param>
        /// <returns>Returns an object identifier for the opened object if
        /// successful; otherwise returns a negative value.</returns>
        /// <remarks>This function must be used with care! Improper use can
        /// lead to inaccessible data, wasted space in the file, or file
        /// corruption.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oopen_by_addr",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open_by_addr(hid_t loc_id, haddr_t addr);
        
        /// <summary>
        /// Open the n-th object in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-OpenByIdx
        /// </summary>
        /// <param name="loc_id">A file or group identifier.</param>
        /// <param name="group_name">Name of group, relative to
        /// <paramref name="loc_id"/>, in which object is located</param>
        /// <param name="idx_type">Type of index by which objects are ordered</param>
        /// <param name="order">Order of iteration within index</param>
        /// <param name="n">Object to open</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an object identifier for the opened object if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oopen_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open_by_idx
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t lapl_id = H5P.DEFAULT);

#if HDF5_VER1_10

        /// <summary>
        /// Refreshes all buffers associated with an HDF5 object.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Orefresh.htm
        /// </summary>
        /// <param name="oid">Identifier of the object to be refreshed.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Orefresh",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t refresh(hid_t oid);

#endif

        /// <summary>
        /// Open the n-th object in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-OpenByIdx
        /// </summary>
        /// <param name="loc_id">A file or group identifier.</param>
        /// <param name="group_name">Name of group, relative to
        /// <paramref name="loc_id"/>, in which object is located</param>
        /// <param name="idx_type">Type of index by which objects are ordered</param>
        /// <param name="order">Order of iteration within index</param>
        /// <param name="n">Object to open</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an object identifier for the opened object if
        /// successful; otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Oopen_by_idx",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t lapl_id = H5P.DEFAULT);

#if HDF5_VER1_10

        /// <summary>
        /// Recursively visits all objects accessible from a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Visit
        /// </summary>
        /// <param name="obj_id">Identifier of the object at which the
        /// recursive iteration begins.</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the
        /// object to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the object</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ovisit1",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit
            (hid_t obj_id, H5.index_t idx_type, H5.iter_order_t order,
            iterate_t op, IntPtr op_data);

        /// <summary>
        /// Recursively visits all objects starting from a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-VisitByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file or group</param>
        /// <param name="obj_name">Name of the object, generally relative to
        /// <paramref name="loc_id"/>, that will serve as root of the iteration</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the
        /// object to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the object</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ovisit_by_name1",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit_by_name
            (hid_t loc_id, byte[] obj_name, H5.index_t idx_type,
            H5.iter_order_t order, iterate_t op, IntPtr op_data,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Recursively visits all objects starting from a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-VisitByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file or group</param>
        /// <param name="obj_name">Name of the object, generally relative to
        /// <paramref name="loc_id"/>, that will serve as root of the iteration</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the
        /// object to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the object</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ovisit_by_name1",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit_by_name
            (hid_t loc_id, string obj_name, H5.index_t idx_type,
            H5.iter_order_t order, iterate_t op, IntPtr op_data,
            hid_t lapl_id = H5P.DEFAULT);

#else

        /// <summary>
        /// Recursively visits all objects accessible from a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-Visit
        /// </summary>
        /// <param name="obj_id">Identifier of the object at which the
        /// recursive iteration begins.</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the
        /// object to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the object</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ovisit",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit
            (hid_t obj_id, H5.index_t idx_type, H5.iter_order_t order,
            iterate_t op, IntPtr op_data);

        /// <summary>
        /// Recursively visits all objects starting from a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-VisitByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file or group</param>
        /// <param name="obj_name">Name of the object, generally relative to
        /// <paramref name="loc_id"/>, that will serve as root of the iteration</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the
        /// object to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the object</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ovisit_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit_by_name
            (hid_t loc_id, byte[] obj_name, H5.index_t idx_type,
            H5.iter_order_t order, iterate_t op, IntPtr op_data,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Recursively visits all objects starting from a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5O.html#Object-VisitByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file or group</param>
        /// <param name="obj_name">Name of the object, generally relative to
        /// <paramref name="loc_id"/>, that will serve as root of the iteration</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the
        /// object to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the object</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ovisit_by_name",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit_by_name
            (hid_t loc_id, string obj_name, H5.index_t idx_type,
            H5.iter_order_t order, iterate_t op, IntPtr op_data,
            hid_t lapl_id = H5P.DEFAULT);

#endif

    }
}
