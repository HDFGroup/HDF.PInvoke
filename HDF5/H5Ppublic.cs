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
using off_t = System.IntPtr;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

using prp_create_func_t = HDF.PInvoke.H5P.prp_cb1_t;
using prp_set_func_t = HDF.PInvoke.H5P.prp_cb2_t;
using prp_get_func_t = HDF.PInvoke.H5P.prp_cb2_t;
using prp_delete_func_t = HDF.PInvoke.H5P.prp_cb2_t;
using prp_copy_func_t = HDF.PInvoke.H5P.prp_cb1_t;
using prp_close_func_t = HDF.PInvoke.H5P.prp_cb1_t;

namespace HDF.PInvoke
{
    public unsafe sealed partial class H5P
    {
        static H5P()
        {
            H5.open();
        }

        /* Common creation order flags (for links in groups and attributes on
         * objects) */
        public const uint CRT_ORDER_TRACKED = 0x0001;
        public const uint CRT_ORDER_INDEXED = 0x0002;

        /// <summary>
        /// Default value for all property list classes
        /// </summary>
        public const hid_t DEFAULT = 0;

        /* Define property list class callback function pointer types */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t cls_create_func_t
        (hid_t prop_id, IntPtr create_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t cls_copy_func_t
        (hid_t new_prop_id, hid_t old_prop_id, IntPtr copy_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t cls_close_func_t
        (hid_t prop_id, IntPtr close_data);

        /* Define property list callback function pointer types */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)]
        public delegate herr_t prp_cb1_t
        (string name, size_t size, IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t prp_cb2_t
        (hid_t prop_id, string name, size_t size, IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int prp_compare_func_t
        (IntPtr value1, IntPtr value2, size_t size);

        /* Define property list iteration function type */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t iterate_t
            (hid_t id, string name, IntPtr iter_data);

        /// <summary>
        /// Adds a path to the list of paths that will be searched in the
        /// destination file for a matching committed datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-AddMergeCommittedDtypePath
        /// </summary>
        /// <param name="ocpypl_id">Object copy property list identifier.</param>
        /// <param name="path">The path to be added.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Padd_merge_committed_dtype_path",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t add_merge_committed_dtype_path
            (hid_t ocpypl_id, byte[] path);

        /// <summary>
        /// Adds a path to the list of paths that will be searched in the
        /// destination file for a matching committed datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-AddMergeCommittedDtypePath
        /// </summary>
        /// <param name="ocpypl_id">Object copy property list identifier.</param>
        /// <param name="path">The path to be added.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Padd_merge_committed_dtype_path",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t add_merge_committed_dtype_path
            (hid_t ocpypl_id, string path);

        /// <summary>
        /// Verifies that all required filters are available.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-AllFiltersAvail
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list
        /// identifier.</param>
        /// <returns>Returns 1 if all filters are available and 0 if one or
        /// more is not currently available. Returns a negative value
        /// on error.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pall_filters_avail",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t all_filters_avail(hid_t plist_id);

        /// <summary>
        /// Terminates access to a property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Close
        /// </summary>
        /// <param name="plist">Identifier of the property list to which
        /// access is terminated.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pclose",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t close(hid_t plist);

        /// <summary>
        /// Closes an existing property list class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-CloseClass
        /// </summary>
        /// <param name="cls">Property list class to close</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pclose_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t close_class(hid_t cls);

        /// <summary>
        /// Copies an existing property list to create a new property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Copy
        /// </summary>
        /// <param name="plist">Identifier of property list to duplicate.</param>
        /// <returns>Returns a property list identifier if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pcopy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t copy(hid_t plist);

        /// <summary>
        /// Copies a property from one list or class to another.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-CopyProp
        /// </summary>
        /// <param name="dst_id">Identifier of the destination property list or
        /// class</param>
        /// <param name="src_id">Identifier of the source property list or
        /// class</param>
        /// <param name="name">Name of the property to copy</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pcopy_prop",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t copy_prop
            (hid_t dst_id, hid_t src_id, string name);

        /// <summary>
        /// Creates a new property list as an instance of a property list class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Create
        /// </summary>
        /// <param name="cls_id">The class of the property list to create.</param>
        /// <returns>Returns a property list identifier (<code>plist</code>)
        /// if successful; otherwise Fail (-1).</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pcreate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create(hid_t cls_id);

        /// <summary>
        /// Creates a new property list class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-CreateClass
        /// </summary>
        /// <param name="parent_class">Property list class to inherit from or
        /// <code>NULL</code></param>
        /// <param name="name">Name of property list class to register</param>
        /// <param name="create">Callback routine called when a property list
        /// is created</param>
        /// <param name="create_data">Pointer to user-defined class create data,
        /// to be passed along to class create callback</param>
        /// <param name="copy">Callback routine called when a property list is
        /// copied</param>
        /// <param name="copy_data">Pointer to user-defined class copy data, to
        /// be passed along to class copy callback</param>
        /// <param name="close">Callback routine called when a property list is
        /// being closed</param>
        /// <param name="close_data">Pointer to user-defined class close data,
        /// to be passed along to class close callback</param>
        /// <returns>On success, returns a valid property list class identifier;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pcreate_class",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create_class
            (hid_t parent_class, string name, cls_create_func_t create,
            IntPtr create_data, cls_copy_func_t copy, IntPtr copy_data,
            cls_close_func_t close, IntPtr close_data);

        /// <summary>
        /// Compares two property lists or classes for equality.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Equal
        /// </summary>
        /// <param name="id1">First property object to be compared</param>
        /// <param name="id2">Second property object to be compared</param>
        /// <returns>Returns 1 if equal; 0 if unequal. Returns a negative value
        /// on error.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pequal",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t equal(hid_t id1, hid_t id2);

        /// <summary>
        /// Queries whether a property name exists in a property list or class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Exist
        /// </summary>
        /// <param name="id">Identifier for the property to query</param>
        /// <param name="name">Name of property to check for</param>
        /// <returns>Returns 1 if the property exists in the property object;
        /// 0 if the property does not exist. Returns a negative value
        /// on error.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pexist",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t exist(hid_t id, string name);

        /// <summary>
        /// Determines whether fill value is defined.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-FillValueDefined
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="status">Status of fill value in property list.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pfill_value_defined",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t H5Pfill_value_defined
            (hid_t plist_id, ref H5D.fill_value_t status);

        /// <summary>
        /// Clears the list of paths stored in the object copy property list
        /// <paramref name="ocpypl_id"/>.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-FreeMergeCommittedDtypePaths
        /// </summary>
        /// <param name="ocpypl_id">Object copy property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pfree_merge_committed_dtype_paths",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t free_merge_committed_dtype_paths
            (hid_t ocpypl_id);

        /// <summary>
        /// Queries the value of a property.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Get
        /// </summary>
        /// <param name="plid">Identifier of the property list to query</param>
        /// <param name="name">Name of property to query</param>
        /// <param name="value">Pointer to a location to which to copy the
        /// value of of the property</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get
            (hid_t plid, string name, IntPtr value);

        /// <summary>
        /// Retrieves the current settings for alignment properties from a file
        /// access property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetAlignment
        /// </summary>
        /// <param name="plist">Identifier of a file access property list.</param>
        /// <param name="threshold">Pointer to location of return threshold
        /// value.</param>
        /// <param name="alignment">Pointer to location of return alignment
        /// value.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_alignment",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_alignment
            (hid_t plist, ref hsize_t threshold, ref hsize_t alignment);

        /// <summary>
        /// Retrieves the timing for storage space allocation.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetAllocTime
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="alloc_time">When to allocate dataset storage space.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_alloc_time",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_alloc_time
            (hid_t plist_id, ref H5D.alloc_time_t alloc_time);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the values of the append property that is set up in the
        /// dataset access property list.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Pget_append_flush.htm
        /// </summary>
        /// <param name="dapl_id">Dataset access property list identifier.</param>
        /// <param name="ndims">The number of elements for
        /// <paramref name="boundary"/>.</param>
        /// <param name="boundary">The dimension sizes used to determine the
        /// boundary.</param>
        /// <param name="func">The user-defined callback function.</param>
        /// <param name="udata">The user-defined input data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_append_flush",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_append_flush
            (hid_t dapl_id, uint ndims,
            [In][Out]hsize_t[] boundary,
            ref H5D.append_cb_t func, ref IntPtr udata);

#endif

        /// <summary>
        /// Retrieves tracking and indexing settings for attribute creation order.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetAttrCreationOrder
        /// </summary>
        /// <param name="ocpl_id">Object (group or dataset) creation property
        /// list identifier</param>
        /// <param name="crt_order_flags">Flags specifying whether to track and
        /// index attribute creation order</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_attr_creation_order",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_attr_creation_order
            (hid_t ocpl_id, ref uint crt_order_flags);

        /// <summary>
        /// Retrieves attribute storage phase change thresholds.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetAttrPhaseChange
        /// </summary>
        /// <param name="ocpl_id">Object creation property list identifier</param>
        /// <param name="max_compact">Maximum number of attributes to be stored
        /// in compact storage</param>
        /// <param name="min_dense">Minimum number of attributes to be stored
        /// in dense storage</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_attr_phase_change",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_attr_phase_change
            (hid_t ocpl_id, ref uint max_compact, ref uint min_dense);

        /// <summary>
        /// Gets B-tree split ratios for a dataset transfer property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetBTreeRatios
        /// </summary>
        /// <param name="plist">The dataset transfer property list identifier.</param>
        /// <param name="left">The B-tree split ratio for left-most nodes.</param>
        /// <param name="middle">The B-tree split ratio for right-most nodes
        /// and lone nodes.</param>
        /// <param name="right">The B-tree split ratio for all other nodes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_btree_ratios",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_btree_ratios
            (hid_t plist, ref double left, ref double middle, ref double right);

        /// <summary>
        /// Reads buffer settings.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetBuffer
        /// </summary>
        /// <param name="plist">Identifier for the dataset transfer property
        /// list.</param>
        /// <param name="tconv">Address of the pointer to application-allocated
        /// type conversion buffer.</param>
        /// <param name="bkg">Address of the pointer to application-allocated
        /// background buffer.</param>
        /// <returns>Returns buffer size, in bytes, if successful; otherwise 0
        /// on failure.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_buffer",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hsize_t get_buffer
            (hid_t plist, ref IntPtr tconv, ref IntPtr bkg);

        /// <summary>
        /// Queries the raw data chunk cache parameters.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetCache
        /// </summary>
        /// <param name="plist_id">Identifier of the file access property list.</param>
        /// <param name="mdc_nelmts">UNUSED.</param>
        /// <param name="rdcc_nelmts">Number of elements (objects) in the raw
        /// data chunk cache.</param>
        /// <param name="rdcc_nbytes">Total size of the raw data chunk cache,
        /// in bytes.</param>
        /// <param name="rdcc_w0">Preemption policy.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_cache",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_cache
            (hid_t plist_id, ref int mdc_nelmts, ref size_t rdcc_nelmts,
            ref size_t rdcc_nbytes, ref double rdcc_w0);

        /// <summary>
        /// Retrieves the character encoding used to create a link or attribute
        /// name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetCharEncoding
        /// </summary>
        /// <param name="plist_id">Link creation or attribute creation property
        /// list identifier</param>
        /// <param name="encoding">String encoding character set</param>
        /// <returns>Returns a non-negative valule if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_char_encoding",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_char_encoding
            (hid_t plist_id, ref H5T.cset_t encoding);

        /// <summary>
        /// Retrieves the size of chunks for the raw data of a chunked layout
        /// dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetChunk
        /// </summary>
        /// <param name="plist_id">Identifier of property list to query.</param>
        /// <param name="max_ndims">Length of the <paramref name="dims"/>
        /// array.</param>
        /// <param name="dims">Array to store the chunk dimensions.</param>
        /// <returns>Returns chunk dimensionality if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_chunk",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_chunk
            (hid_t plist_id, int max_ndims, [In][Out]hsize_t[] dims);

        /// <summary>
        /// Retrieves the raw data chunk cache parameters.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetChunkCache
        /// </summary>
        /// <param name="dapl_id">Dataset access property list identifier.</param>
        /// <param name="rdcc_nslots">Number of chunk slots in the raw data 
        /// chunk cache hash table.</param>
        /// <param name="rdcc_nbytes">Total size of the raw data chunk cache,
        /// in bytes.</param>
        /// <param name="rdcc_w0">Preemption policy.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_chunk_cache",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_chunk_cache
            (hid_t dapl_id, ref size_t rdcc_nslots, ref size_t rdcc_nbytes,
            ref double rdcc_w0);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the edge chunk option setting from a dataset creation
        /// property list.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/PartialEdgeChunks/H5Pget_chunk_opts.htm
        /// </summary>
        /// <param name="dcpl_id">Dataset creation property list identifier.</param>
        /// <param name="opts">Edge chunk option flag.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_chunk_opts",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_chunk_opts
            (hid_t dcpl_id, ref uint opts);

#endif

        /// <summary>
        /// Returns the property list class identifier for a property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetClass
        /// </summary>
        /// <param name="plist">Identifier of property list to query.</param>
        /// <returns>Returns a property list class identifier if successful.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_class(hid_t plist);

        /// <summary>
        /// Retrieves the name of a class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetClassName
        /// </summary>
        /// <param name="pcid">Identifier of the property class to query</param>
        /// <returns>If successful returns a pointer to an allocated string
        /// containing the class name; <code>NULL</code> if unsuccessful.</returns>
        /// <remarks>The pointer to the name must be freed by the user after
        /// each successful call.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_class_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern IntPtr get_class_name(hid_t pcid);

        /// <summary>
        /// Retrieves the parent class of a property class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetClassParent
        /// </summary>
        /// <param name="pcid">Identifier of the property class to query</param>
        /// <returns>If successful, returns a valid parent class object
        /// identifier; returns a negative value on failure.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_class_parent",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_class_parent(hid_t pcid);

        /// <summary>
        /// Retrieves the properties to be used when an object is copied.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetCopyObject
        /// </summary>
        /// <param name="ocp_plist_id">Object copy property list identifier</param>
        /// <param name="copy_options">Copy option(s) set in the object copy
        /// property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_copy_object",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_copy_object
            (hid_t ocp_plist_id, ref uint copy_options);

        /// <summary>
        /// Gets information about the write tracking feature used by the core VFD.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetCoreWriteTracking
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="is_enabled">Whether the feature is enabled.</param>
        /// <param name="page_size">Size, in bytes, of write aggregation pages.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_core_write_tracking",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_core_write_tracking
            (hid_t fapl_id, ref hbool_t is_enabled, ref size_t page_size);

        /// <summary>
        /// Determines whether property is set to enable creating missing
        /// intermediate groups.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetCreateIntermediateGroup
        /// </summary>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="crt_intermed_group">Flag specifying whether to create
        /// intermediate groups upon creation of an object</param>
        /// <returns>Returns a non-negative valule if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_create_intermediate_group",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_create_intermediate_group
            (hid_t lcpl_id, ref uint crt_intermed_group);

        /// <summary>
        /// Retrieves a data transform expression.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetDataTransform
        /// </summary>
        /// <param name="plist_id">Identifier of the property list or class</param>
        /// <param name="expression">Pointer to memory where the transform
        /// expression will be copied</param>
        /// <param name="size">Number of bytes of the transform expression to
        /// copy to</param>
        /// <returns>If successful, returns the size of the transform expression.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_data_transform",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_data_transform
            (hid_t plist_id, [In][Out]StringBuilder expression, size_t size);

#if HDF5_VER1_10
        /// <summary>
        /// Retrieves the setting for whether or not a file will create minimized dataset object headers.
        /// See https://portal.hdfgroup.org/display/HDF5/H5P_GET_DSET_NO_ATTRS_HINT
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier</param>
        /// <param name="minimize">Flag indicating whether the library will or will not
        /// create minimized dataset object headers</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_dset_no_attrs_hint",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_dset_no_attrs_hint(hid_t plist_id, ref hbool_t minimize);
#endif

        /// <summary>
        /// Returns low-lever driver identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetDriver
        /// </summary>
        /// <param name="plist_id">File access or data transfer property list
        /// identifier.</param>
        /// <returns>Returns a valid low-level driver identifier if successful.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_driver",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_driver(hid_t plist_id);

        /// <summary>
        /// Returns a pointer to file driver information.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetDriverInfo
        /// </summary>
        /// <param name="plist_id">File access or data transfer property list
        /// identifier.</param>
        /// <returns>Returns a pointer to a struct containing low-level driver
        /// information. Otherwise returns <code>NULL</code>.</returns>
        /// <remarks><code>NULL</code> is also returned if no driver-specific
        /// properties have been registered. No error is pushed on the stack
        /// in this case.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_driver_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern IntPtr get_driver_info(hid_t plist_id);

        /// <summary>
        /// Determines whether error-detection is enabled for dataset reads.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetEdcCheck
        /// </summary>
        /// <param name="plist">Dataset transfer property list identifier.</param>
        /// <returns>Returns <code>H5Z_ENABLE_EDC</code> or
        /// <code>H5Z_DISABLE_EDC</code>  if successful; otherwise returns a
        /// negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_driver_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern H5Z.EDC_t get_edc_check(hid_t plist);

        /// <summary>
        /// Retrieves the prefix for external raw data storage files as set in
        /// the dataset access property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetEfilePrefix
        /// </summary>
        /// <param name="dapl">Dataset access property list identifier.</param>
        /// <param name="prefix">Dataset external storage prefix.</param>
        /// <param name="size">Size of prefix buffer in bytes.</param>
        /// <returns>Returns the size of <paramref name="prefix"/> and the
        /// prefix string will be stored in <paramref name="prefix"/> if
        /// successful. Otherwise returns a negative value and the contents of
        /// <paramref name="prefix"/> will be undefined.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_efile_prefix",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_efile_prefix
            (hid_t dapl, [In][Out]byte[] prefix, size_t size);

        /// <summary>
        /// Retrieves the external link traversal file access flag from the
        /// specified link access property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetELinkAccFlags
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <param name="flags">File access flag for link traversal.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_elink_acc_flags",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_elink_acc_flags
            (hid_t lapl_id, ref uint flags);

        /// <summary>
        /// Retrieves the external link traversal callback function from the
        /// specified link access property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetELinkCb
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <param name="func">User-defined external link traversal callback
        /// function.</param>
        /// <param name="op_data">User-defined input data for the callback
        /// function.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_elink_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_elink_cb
            (hid_t lapl_id, ref H5L.elink_traverse_t func, ref IntPtr op_data);

        /// <summary>
        /// Retrieves the file access property list identifier associated with
        /// the link access property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetELinkFapl
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_elink_fapl",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_elink_fapl(hid_t lapl_id);

        /// <summary>
        /// Retrieves the size of the external link open file cache.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetELinkFileCacheSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="efc_size">External link open file cache size in number
        /// of files.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_elink_file_cache_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_elink_file_cache_size
            (hid_t fapl_id, ref uint efc_size);

        /// <summary>
        /// Retrieves prefix applied to external link paths.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetELinkPrefix
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <param name="prefix">Prefix applied to external link paths</param>
        /// <param name="size">Size of prefix, including null terminator</param>
        /// <returns>If successful, returns a non-negative value specifying the
        /// size in bytes of the prefix without the <code>NULL</code>
        /// terminator; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_elink_prefix",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_elink_prefix
            (hid_t lapl_id, byte[] prefix, size_t size);

        /// <summary>
        /// Queries data required to estimate required local heap or object
        /// header size.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetEstLinkInfo
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="est_num_entries">Estimated number of links to be
        /// inserted into group</param>
        /// <param name="est_name_len">Estimated average length of link names</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_est_link_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_est_link_info
            (hid_t gcpl_id, ref uint est_num_entries, ref uint est_name_len);

        /// <summary>
        /// Returns information about an external file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetExternal
        /// </summary>
        /// <param name="plist">Identifier of a dataset creation property list.</param>
        /// <param name="idx">External file index.</param>
        /// <param name="name_size">Maximum length of <paramref name="name"/>
        /// array.</param>
        /// <param name="name">Name of the external file.</param>
        /// <param name="offset">Pointer to a location to return an offset
        /// value.</param>
        /// <param name="size">Pointer to a location to return the size of the
        /// external file data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_external",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_external
            (hid_t plist, uint idx, size_t name_size, [In][Out]byte[] name,
            ref off_t offset, ref hsize_t size);

        /// <summary>
        /// Returns the number of external files for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetExternalCount
        /// </summary>
        /// <param name="plist">Identifier of a dataset creation property list.</param>
        /// <returns>Returns the number of external files if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_external_count",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_external_count(hid_t plist);

        /// <summary>
        /// Retrieves a data offset from the file access property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFamilyOffset
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="offset">Offset in bytes within the HDF5 file.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_family_offset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_family_offset
            (hid_t fapl_id, ref hsize_t offset);

        /// <summary>
        /// Queries core file driver properties.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFaplCore
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="increment">Size, in bytes, of memory increments.</param>
        /// <param name="backing_store">Boolean flag indicating whether to
        /// write the file contents to disk when the file is closed.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_fapl_core",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_fapl_core
            (hid_t fapl_id, ref size_t increment, ref hbool_t backing_store);

        /// <summary>
        /// Retrieves direct I/O driver settings.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFaplDirect
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="alignment">Required memory alignment boundary</param>
        /// <param name="block_size">File system block size</param>
        /// <param name="cbuf_size">Copy buffer size</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_fapl_direct",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_fapl_direct
            (hid_t fapl_id, ref size_t alignment, ref size_t block_size,
            ref size_t cbuf_size);

        /// <summary>
        /// Returns file access property list information.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFaplFamily
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="memb_size">Size in bytes of each file member.</param>
        /// <param name="memb_fapl_id">Identifier of file access property list
        /// for each family member.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_fapl_family",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_fapl_family
            (hid_t fapl_id, ref hsize_t memb_size, ref hid_t memb_fapl_id);

        /// <summary>
        /// Returns the file close degree.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFcloseDegree
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="fc_degree"> Pointer to a location to which to return
        /// the file close degree property, the value of
        /// <code>fc_degree</code>.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_fclose_degree",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_fclose_degree
            (hid_t fapl_id, ref H5F.close_degree_t fc_degree);

        /// <summary>
        /// Retrieves a copy of the file image designated as the initial
        /// content and structure of a file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFileImage
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="buf_ptr_ptr">On input, <code>NULL</code> or a pointer
        /// to a pointer to a buffer that contains the file image.</param>
        /// <param name="buf_len_ptr">On input, <code>NULL</code> or a pointer
        /// to a buffer specifying the required size of the buffer to hold the
        /// file image.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_file_image",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_file_image
            (hid_t fapl_id, ref IntPtr buf_ptr_ptr, ref size_t buf_len_ptr);

        /// <summary>
        /// Retrieves callback routines for working with file images.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFileImageCallbacks
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="callbacks_ptr">Pointer to the instance of the
        /// <code>H5FD.file_image_callbacks_t</code> struct in which the
        /// callback routines are to be returned.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>Struct fields must be initialized to <code>NULL</code>
        /// before the call is made.</remarks>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_file_image_callbacks",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_file_image_callbacks
            (hid_t fapl_id, ref H5FD.file_image_callbacks_t callbacks_ptr);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the file space management strategy and/or free-space
        /// section threshold for an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FileSpace/H5Pget_file_space.htm
        /// </summary>
        /// <param name="fcpl">The file creation property list identifier.</param>
        /// <param name="strategy">The current file space management strategy
        /// in use for the file.</param>
        /// <param name="threshold">The current free-space section threshold.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_file_space",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_file_space
            (hid_t fcpl, ref H5F.file_space_type_t strategy,
            ref hsize_t threshold);

        /// <summary>
        /// Retrieves the file space page size for a file creation property
        /// list.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFileSpacePageSize
        /// </summary>
        /// <param name="fcpl">The file creation property list identifier.</param>
        /// <param name="strategy">The current file space management strategy
        /// in use for the file.</param>
        /// <param name="fsp_size">File space page size</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_file_space_page_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_file_space_page_size
            (hid_t fcpl, ref hsize_t fsp_size);

        /// <summary>
        /// Retrieves the file space handling strategy, persisting free-space
        /// condition and threshold value for a file creation property list.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFileSpaceStrategy
        /// </summary>
        /// <param name="fcpl">The file creation property list identifier.</param>
        /// <param name="strategy">The current file space management strategy
        /// in use for the file.</param>
        /// <param name="persist">The boolean value indicating whether free
        /// space is persistent or not.</param>
        /// <param name="threshold">The free-space section size threshold value.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_file_space_strategy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_file_space_strategy(hid_t fcpl,
            ref H5F.fspace_strategy_t strategy, ref hbool_t persist,
            ref hsize_t threshold);

#endif

        /// <summary>
        /// Retrieves the time when fill value are written to a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFillTime
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="fill_time">Setting for the timing of writing fill
        /// values to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_fill_time",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_fill_time
            (hid_t plist_id, ref H5D.fill_time_t fill_time);

        /// <summary>
        /// Retrieves a dataset fill value.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFillValue
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="type_id">Datatype identifier for the value passed via
        /// <code>value</code>.</param>
        /// <param name="value">Pointer to buffer to contain the returned fill
        /// value.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_fill_value",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_fill_value
            (hid_t plist_id, hid_t type_id, IntPtr value);

        /// <summary>
        /// Returns information about a filter in a pipeline.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFilter2
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list 
        /// identifier.</param>
        /// <param name="idx">Sequence number within the filter pipeline of the
        /// filter for which information is sought.</param>
        /// <param name="flags">Bit vector specifying certain general
        /// properties of the filter.</param>
        /// <param name="cd_nelmts">Number of elements in
        /// <paramref name="cd_values"/>.</param>
        /// <param name="cd_values">Auxiliary data for the filter.</param>
        /// <param name="namelen">Anticipated number of characters in
        /// <paramref name="name"/>.</param>
        /// <param name="name">Name of the filter.</param>
        /// <param name="filter_config">Bit field.</param>
        /// <returns>Returns the filter identifier if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_filter2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern H5Z.filter_t get_filter
            (hid_t plist_id, uint idx, ref uint flags, ref size_t cd_nelmts,
            uint[] cd_values, size_t namelen, [In][Out]byte[] name,
            ref uint filter_config);


        /// <summary>
        /// Returns information about a filter in a pipeline.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFilter2
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list 
        /// identifier.</param>
        /// <param name="idx">Sequence number within the filter pipeline of the
        /// filter for which information is sought.</param>
        /// <param name="flags">Bit vector specifying certain general
        /// properties of the filter.</param>
        /// <param name="cd_nelmts">Number of elements in
        /// <paramref name="cd_values"/>.</param>
        /// <param name="cd_values">Auxiliary data for the filter.</param>
        /// <param name="namelen">Anticipated number of characters in
        /// <paramref name="name"/>.</param>
        /// <param name="name">Name of the filter.</param>
        /// <param name="filter_config">Bit field.</param>
        /// <returns>Returns the filter identifier if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_filter2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern H5Z.filter_t get_filter2(hid_t plist_id, uint filter, ref uint flags, ref hsize_t cd_nelmts, uint* cd_values, size_t namelen, string name, ref uint filter_config);
        

        /// <summary>
        /// Returns information about the specified filter.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetFilterById2
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list 
        /// identifier.</param>
        /// <param name="filter_id">Filter identifier.</param>
        /// <param name="flags">Bit vector specifying certain general
        /// properties of the filter.</param>
        /// <param name="cd_nelmts">Number of elements in
        /// <paramref name="cd_values"/>.</param>
        /// <param name="cd_values">Auxiliary data for the filter.</param>
        /// <param name="namelen">Anticipated number of characters in
        /// <paramref name="name"/>.</param>
        /// <param name="name">Name of the filter.</param>
        /// <param name="filter_config">Bit field.</param>
        /// <returns>Returns the filter identifier if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_filter_by_id2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_filter_by_id
            (hid_t plist_id, H5Z.filter_t filter_id, ref uint flags,
            ref size_t cd_nelmts, [In][Out]uint[] cd_values, size_t namelen,
            [In][Out]byte[] name, ref uint filter_config);

        /// <summary>
        /// Returns garbage collecting references setting.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetGCReferences
        /// </summary>
        /// <param name="plist">File access property list identifier.</param>
        /// <param name="gc_ref">Flag returning the state of reference garbage
        /// collection. A returned value of 1 indicates that garbage collection
        /// is on while 0 indicates that garbage collection is off.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_gc_references",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_gc_references
            (hid_t plist, ref uint gc_ref);

        /// <summary>
        /// Retrieves number of I/O vectors to be read/written in hyperslab I/O.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetHyperVectorSize
        /// </summary>
        /// <param name="dxpl_id">Dataset transfer property list identifier.</param>
        /// <param name="vector_size">Number of I/O vectors to accumulate in
        /// memory for I/O operations.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_hyper_vector_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_hyper_vector_size
            (hid_t dxpl_id, ref size_t vector_size);

        /// <summary>
        /// Queries the 1/2 rank of an indexed storage B-tree.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetIstoreK
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier</param>
        /// <param name="ik">Pointer to location to return the chunked storage
        /// B-tree 1/2 rank</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_istore_k",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_istore_k
            (hid_t fcpl_id, ref uint ik);

        /// <summary>
        /// Returns the layout of the raw data for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetLayout
        /// </summary>
        /// <param name="plist">Identifier for property list to query.</param>
        /// <returns>Returns the layout type (a non-negative value) of a
        /// dataset creation property list if successful. Otherwise, returns a
        /// negative value indicating failure.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_layout",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern H5D.layout_t get_layout(hid_t plist);

        /// <summary>
        /// Retrieves library version bounds settings that indirectly control
        /// the format versions used when creating objects.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetLibverBounds
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="libver_low">The earliest version of the library that
        /// will be used for writing objects.</param>
        /// <param name="libver_high">The latest version of the library that
        /// will be used for writing objects.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_libver_bounds",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_libver_bounds
            (hid_t fapl_id, ref H5F.libver_t libver_low,
            ref H5F.libver_t libver_high);

        /// <summary>
        /// Queries whether link creation order is tracked and/or indexed in a
        /// group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetLinkCreationOrder
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="crt_order_flags">Creation order flag(s)</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_link_creation_order",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_link_creation_order
            (hid_t gcpl_id, ref uint crt_order_flags);

        /// <summary>
        /// Queries the settings for conversion between compact and dense
        /// groups.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetLinkPhaseChange
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="max_compact">Maximum number of links for compact
        /// storage</param>
        /// <param name="min_dense">Minimum number of links for dense storage</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_link_phase_change",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_link_phase_change
            (hid_t gcpl_id, ref uint max_compact, ref uint min_dense);

        /// <summary>
        /// Retrieves the anticipated size of the local heap for original-style
        /// groups.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetLocalHeapSizeHint
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="size_hint">Anticipated size of local heap</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_local_heap_size_hint",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_local_heap_size_hint
            (hid_t gcpl_id, ref size_t size_hint);

        /// <summary>
        /// Retrieves the callback function from the specified object copy
        /// property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetMcdtSearchCb
        /// </summary>
        /// <param name="ocpypl_id">Object copy property list identifier</param>
        /// <param name="func">User-defined callback function</param>
        /// <param name="op_data">User-defined input data for the callback
        /// function</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_mcdt_search_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_mcdt_search_cb
            (hid_t ocpypl_id, ref H5O.mcdt_search_cb_t func, ref IntPtr op_data);

        /// <summary>
        /// Get the current initial metadata cache configuration from the
        /// indicated File Access Property List.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetMdcConfig
        /// </summary>
        /// <param name="plist_id">Identifier of the file access property list.</param>
        /// <param name="config_ptr">Pointer to the instance of
        /// <code>H5AC.cache_config_t</code> in which the current metadata
        /// cache configuration is to be reported.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_mdc_config",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_mdc_config
            (hid_t plist_id, IntPtr config_ptr);
  
#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the metadata cache image configuration values for a file
        /// access property list.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetMDCImageConfig
        /// </summary>
        /// <param name="fapl_id">Identifier of the file access property list.</param>
        /// <param name="config_ptr">Reference to the instance of
        /// <code>H5AC.cache_image_config_t</code> in which the current metadata
        /// cache image configuration is to be reported.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_mdc_image_config",
           CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_mdc_image_config
            (hid_t fapl_id, IntPtr config_ptr);

        /// <summary>
        /// Gets metadata cache logging options.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Pget_mdc_log_options.htm
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="is_enabled">Whether logging is enabled.</param>
        /// <param name="location">Log file location.</param>
        /// <param name="location_size">Size in bytes of the location string.</param>
        /// <param name="start_on_access">Whether the logging begins as soon as
        /// the file is opened or created.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_mdc_log_options",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_mdc_log_options
            (hid_t fapl_id, ref hbool_t is_enabled, [In][Out]StringBuilder location,
            ref size_t location_size, ref hbool_t start_on_access);

#endif

        /// <summary>
        /// Returns the current metadata block size setting.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetMetaBlockSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="size">Minimum size, in bytes, of metadata block
        /// allocations.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_meta_block_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_meta_block_size
            (hid_t fapl_id, ref hsize_t size);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the number of read attempts from a file access property
        /// list.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Pget_metadata_read_attempts.htm
        /// </summary>
        /// <param name="fapl">Identifier for a file access property list.</param>
        /// <param name="attempts"> The number of read attempts.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_metadata_read_attempts",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_metadata_read_attempts
            (hid_t fapl, ref uint attempts);

#endif

        /// <summary>
        /// Returns the number of filters in the pipeline.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetNFilters
        /// </summary>
        /// <param name="plist">Property list identifier.</param>
        /// <returns>Returns the number of filters in the pipeline if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_nfilters",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_nfilters(hid_t plist);

        /// <summary>
        /// Retrieves the maximum number of link traversals.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetNLinks
        /// </summary>
        /// <param name="lapl_id">File access property list identifier</param>
        /// <param name="nlinks">Maximum number of links to traverse</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_nlinks",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_nlinks
            (hid_t lapl_id, ref size_t nlinks);

        /// <summary>
        /// Queries the number of properties in a property list or class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetNProps
        /// </summary>
        /// <param name="plist_id">Identifier for property object to query</param>
        /// <param name="nprops">Number of properties in object</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_nprops",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_nprops
            (hid_t plist_id, ref size_t nprops);

        /// <summary>
        /// Determines whether times associated with an object are being recorded.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetObjTrackTimes
        /// </summary>
        /// <param name="ocpl_id">Object creation property list identifier</param>
        /// <param name="track_times">Boolean value, 1 or 0, specifying whether
        /// object times are being recorded</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_obj_track_times",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_obj_track_times
            (hid_t ocpl_id, ref hbool_t track_times);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the object flush property values from the file access
        /// property list.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Pget_object_flush_cb.htm
        /// </summary>
        /// <param name="fapl_id">Identifier for a file access property list.</param>
        /// <param name="func">The user-defined callback function.</param>
        /// <param name="udata">The user-defined input data for the callback
        /// function.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_object_flush_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_object_flush_cb
            (hid_t fapl_id, ref H5F.flush_cb_t func, ref IntPtr udata);

        /// <summary>
        /// Retrieves the maximum size for the page buffer and the minimum
        /// percentage for metadata and raw data pages.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetPageBufferSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="buf_size">Maximum size, in bytes, of the page buffer</param>
        /// <param name="min_meta_prec">Minimum metadata percentage to keep in
        /// the page buffer before allowing pages containing metadata to be
        /// evicted</param>
        /// <param name="min_raw_perc">Minimum raw data percentage to keep in
        /// the page buffer before allowing pages containing raw data to be
        /// evicted</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_page_buffer_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_page_buffer_size
            (hid_t fapl_id, ref IntPtr buf_size, ref uint min_meta_perc,
            ref uint min_raw_perc);

#endif

        /// <summary>
        /// Retrieves the configuration settings for a shared message index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSharedMesgIndex
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier.</param>
        /// <param name="index_num">Index being configured.</param>
        /// <param name="mesg_type_flags">Types of messages that may be stored
        /// in this index.</param>
        /// <param name="min_mesg_size">Minimum message size.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_shared_mesg_index",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_shared_mesg_index
            (hid_t fcpl_id, uint index_num, ref uint mesg_type_flags,
            ref uint min_mesg_size);

        /// <summary>
        /// Retrieves number of shared object header message indexes in file
        /// creation property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSharedMesgNIndexes
        /// </summary>
        /// <param name="fcpl_id">File creation property list</param>
        /// <param name="nindexes">Number of shared object header message
        /// indexes available in files created with this property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_shared_mesg_nindexes",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_shared_mesg_nindexes
            (hid_t fcpl_id, ref uint nindexes);

        /// <summary>
        /// Retrieves shared object header message phase change information.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSharedMesgPhaseChange
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier</param>
        /// <param name="max_list">Threshold above which storage of a shared
        /// object header message index shifts from list to B-tree</param>
        /// <param name="min_btree">Threshold below which storage of a shared
        /// object header message index reverts to list format</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_shared_mesg_phase_change",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_shared_mesg_phase_change
            (hid_t fcpl_id, ref uint max_list, ref uint min_btree);

        /// <summary>
        /// Returns maximum data sieve buffer size.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSieveBufSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="size">Maximum size, in bytes, of data sieve buffer.</param>
        /// <returns>Returns a non-negative value if successful.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_sieve_buf_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_sieve_buf_size
            (hid_t fapl_id, ref size_t size);

        /// <summary>
        /// Queries the size of a property value in bytes.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSize
        /// </summary>
        /// <param name="id">Identifier of property object to query</param>
        /// <param name="name">Name of property to query</param>
        /// <param name="size">Size of property in bytes</param>
        /// <returns>Returns a non-negative value if successful.
        /// Otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_size",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_size
            (hid_t id, string name, ref size_t size);

        /// <summary>
        /// Retrieves the size of the offsets and lengths used in an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSizes
        /// </summary>
        /// <param name="plist">Identifier of property list to query.</param>
        /// <param name="sizeof_addr">Pointer to location to return offset size
        /// in bytes.</param>
        /// <param name="sizeof_size">Pointer to location to return length size
        /// in bytes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_sizes",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_sizes
            (hid_t plist, ref size_t sizeof_addr, ref size_t sizeof_size);

        /// <summary>
        /// Retrieves the current small data block size setting.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSmallData
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="size">Maximum size, in bytes, of the small data block.</param>
        /// <returns>Returns a non-negative value if successful; otherwise a
        /// negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_small_data_block_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_small_data_block_size
            (hid_t fapl_id, ref hsize_t size);

        /// <summary>
        /// Retrieves the size of the symbol table B-tree 1/2 rank and the
        /// symbol table leaf node 1/2 size.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetSymK
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier</param>
        /// <param name="ik">Pointer to location to return the symbol table's
        /// B-tree 1/2 rank </param>
        /// <param name="lk">Pointer to location to return the symbol table's
        /// leaf node 1/2 size</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_sym_k",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_sym_k
            (hid_t fcpl_id, ref uint ik, ref uint lk);

        /// <summary>
        /// Gets user-defined datatype conversion callback function.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetTypeConvCb
        /// </summary>
        /// <param name="plist">Dataset transfer property list identifier.</param>
        /// <param name="func">User-defined type conversion callback function.</param>
        /// <param name="op_data">User-defined input data for the callback
        /// function.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_type_conv_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_type_conv_cb
            (hid_t plist, ref H5T.conv_except_func_t func, ref IntPtr op_data);

        /// <summary>
        /// Retrieves the size of a user block.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetUserblock
        /// </summary>
        /// <param name="plist">Identifier for property list to query.</param>
        /// <param name="size">Pointer to location to return user-block size.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_userblock",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_userblock
            (hid_t plist, ref hsize_t size);

        /// <summary>
        /// Retrieves the version information of various objects for a file
        /// creation property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-GetVersion
        /// </summary>
        /// <param name="plist">Identifier of the file creation property list.</param>
        /// <param name="super">Pointer to location to return super block
        /// version number.</param>
        /// <param name="freelist">Pointer to location to return global
        /// freelist version number.</param>
        /// <param name="stab">Pointer to location to return symbol table
        /// version number.</param>
        /// <param name="shhdr">Pointer to location to return shared object
        /// header version number.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_version",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_version
            (hid_t plist, ref uint super, ref uint freelist, ref uint stab,
            ref uint shhdr);

#if HDF5_VER1_10

        /// <summary>
        /// Gets the number of mappings for the virtual dataset.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_count.htm
        /// </summary>
        /// <param name="dcpl_id">The identifier of the virtual dataset
        /// creation property list.</param>
        /// <param name="count">The number of mappings.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_virtual_count",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_virtual_count
            (hid_t dcpl_id, ref size_t count);

        /// <summary>
        /// Gets the name of a source dataset used in the mapping.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_dsetname.htm
        /// </summary>
        /// <param name="dcpl_id">The identifier of the virtual dataset
        /// creation property list.</param>
        /// <param name="index">Mapping index.</param>
        /// <param name="name">A buffer containing the name of the source
        /// dataset.</param>
        /// <param name="size">The size, in bytes, of the <paramref name="name"/>
        /// buffer.</param>
        /// <returns>Returns the length of the dataset name if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_virtual_dsetname",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_virtual_dsetname
            (hid_t dcpl_id, size_t index, byte[] name, size_t size);

        /// <summary>
        /// Gets the name of a source dataset used in the mapping.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_dsetname.htm
        /// </summary>
        /// <param name="dcpl_id">The identifier of the virtual dataset
        /// creation property list.</param>
        /// <param name="index">Mapping index.</param>
        /// <param name="name">A buffer containing the name of the source
        /// dataset.</param>
        /// <param name="size">The size, in bytes, of the <paramref name="name"/>
        /// buffer.</param>
        /// <returns>Returns the length of the dataset name if successful;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_virtual_dsetname",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_virtual_dsetname
            (hid_t dcpl_id, size_t index, [In][Out]StringBuilder name, size_t size);

        /// <summary>
        /// Gets the filename of a source dataset used in the mapping.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_filename.htm
        /// </summary>
        /// <param name="dcpl_id">The identifier of the virtual dataset
        /// creation property list.</param>
        /// <param name="index">Mapping index.</param>
        /// <param name="name">A buffer containing the name of the file
        /// containing the source dataset.</param>
        /// <param name="size">The size, in bytes, of the name buffer.</param>
        /// <returns>Returns the length of the dataset name if successful;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_virtual_filename",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_virtual_filename
            (hid_t dcpl_id, size_t index, [In][Out]StringBuilder name, size_t size);

        /// <summary>
        /// Retrieves prefix applied to VDS source file paths
        /// See https://portal.hdfgroup.org/display/HDF5/H5P_GET_VIRTUAL_PREFIX
        /// </summary>
        /// <param name="dapl">Dataset access property list identifier.</param>
        /// <param name="prefix">Prefix applied to VDS source file paths.</param>
        /// <param name="size">Size of prefix buffer (including null terminator) in bytes.</param>
        /// <returns>Returns the size of <paramref name="prefix"/> and the
        /// prefix string will be stored in <paramref name="prefix"/> if
        /// successful. Otherwise returns a negative value and the contents of
        /// <paramref name="prefix"/> will be undefined.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_virtual_prefix",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_virtual_prefix
            (hid_t dapl, [In][Out]StringBuilder prefix, size_t size);

        /// <summary>
        /// Returns the maximum number of missing source files and/or datasets
        /// with the printf-style names when getting the extent for an
        /// unlimited virtual dataset.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_printf_gap.htm
        /// </summary>
        /// <param name="plist_id">Dataset access property list identifier for
        /// the virtual dataset</param>
        /// <param name="gap_size">Maximum number of the files and/or datasets
        /// allowed to be missing for determining the extent of an unlimited
        /// virtual dataset with printf-style mappings.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_virtual_printf_gap",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_virtual_printf_gap
            (hid_t plist_id, ref hsize_t gap_size);

        /// <summary>
        /// Gets a dataspace identifier for the selection within the source
        /// dataset used in the mapping.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_srcspace.htm
        /// </summary>
        /// <param name="dcpl_id">The identifier of the virtual dataset
        /// creation property list.</param>
        /// <param name="index">Mapping index.</param>
        /// <returns>Returns a valid dataspace identifier if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pget_virtual_srcspace",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_virtual_srcspace
            (hid_t dcpl_id, size_t index);

        /// <summary>
        /// Retrieves the view of a virtual dataset accessed with
        /// <paramref name="dapl_id"/>.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_view.htm
        /// </summary>
        /// <param name="dapl_id">Dataset access property list identifier for
        /// the virtual dataset</param>
        /// <param name="view">The flag specifying the view of the virtual
        /// dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_virtual_view",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_virtual_view
            (hid_t dapl_id, ref H5D.vds_view_t view);

        /// <summary>
        /// Gets a dataspace identifier for the selection within the virtual
        /// dataset used in the mapping.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pget_virtual_vspace.htm
        /// </summary>
        /// <param name="dcpl_id">The identifier of the virtual dataset
        /// creation property list.</param>
        /// <param name="index">Mapping index.</param>
        /// <returns>Returns a valid dataspace identifier if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_virtual_vspace",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_virtual_vspace
            (hid_t dcpl_id, size_t index);

#endif

        /// <summary>
        /// Gets the memory manager for variable-length datatype allocation in
        /// <code>H5D.read</code> and <code>H5D.vlen_reclaim</code>.
        /// </summary>
        /// <param name="plist">Identifier for the dataset transfer property
        /// list.</param>
        /// <param name="alloc">User’s allocate routine, or <code>NULL</code>
        /// for system <code>malloc</code>.</param>
        /// <param name="alloc_info">Extra parameter for user’s allocation
        /// routine. Contents are ignored if preceding parameter is
        /// <code>NULL</code>.</param>
        /// <param name="free">User’s free routine, or <code>NULL</code>
        /// for system <code>free</code>.</param>
        /// <param name="free_info">Extra parameter for user’s free
        /// routine. Contents are ignored if preceding parameter is
        /// <code>NULL</code>.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget_vlen_mem_manager",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_vlen_mem_manager
            (hid_t plist, ref H5MM.allocate_t alloc, ref IntPtr alloc_info,
            ref H5MM.free_t free, ref IntPtr free_info);

        /// <summary>
        /// Registers a temporary property with a property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Insert2
        /// </summary>
        /// <param name="plid">Property list identifier to create temporary
        /// property within</param>
        /// <param name="name">Name of property to create</param>
        /// <param name="size">Size of property in bytes</param>
        /// <param name="value">Initial value for the property</param>
        /// <param name="set">Callback routine called before a new value is
        /// copied into the property's value</param>
        /// <param name="get">Callback routine called when a property value is
        /// retrieved from the property</param>
        /// <param name="delete">Callback routine called when a property is
        /// deleted from a property list</param>
        /// <param name="copy">Callback routine called when a property is
        /// copied from an existing property list</param>
        /// <param name="compare">Callback routine called when a property is
        /// compared with another property list</param>
        /// <param name="close">Callback routine called when a property list is
        /// being closed and the property value will be disposed of</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pinsert2",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t insert
            (hid_t plid, string name, size_t size, IntPtr value,
            prp_set_func_t set, prp_get_func_t get, prp_delete_func_t delete,
            prp_copy_func_t copy, prp_compare_func_t compare,
            prp_close_func_t close);

        /// <summary>
        /// Determines whether a property list is a member of a class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-IsAClass
        /// </summary>
        /// <param name="plist">Property list identifier</param>
        /// <param name="pclass">Property list class identifier</param>
        /// <returns>Returns a positive value if true or zero if false; returns
        /// a negative value on failure.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pisa_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t isa_class(hid_t plist, hid_t pclass);

        /// <summary>
        /// Iterates over properties in a property class or list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Iterate
        /// </summary>
        /// <param name="id">Identifier of property object to iterate over</param>
        /// <param name="idx">Index of the property to begin with</param>
        /// <param name="iter_func">Function pointer to function to be called
        /// with each property iterated over</param>
        /// <param name="iter_data">Pointer to iteration data from user</param>
        /// <returns>Returns the value of the last call to
        /// <code>iter_func</code> if it was non-zero; zero if all properties
        /// have been processed. Returns a negative value on failure.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Piterate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int iterate
            (hid_t id, ref int idx, iterate_t iter_func, IntPtr iter_data);

        /// <summary>
        /// Modifies a filter in the filter pipeline.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-ModifyFilter
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list
        /// identifier</param>
        /// <param name="filter_id">Filter to be modified.</param>
        /// <param name="flags">Bit vector specifying certain general
        /// properties of the filter.</param>
        /// <param name="cd_nelmts">Number of elements in <code>cd_values</code>.</param>
        /// <param name="cd_values">Auxiliary data for the filter.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pmodify_filter",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t modify_filter
            (hid_t plist_id, H5Z.filter_t filter_id, uint flags,
            size_t cd_nelmts, uint[] cd_values);

        /// <summary>
        /// Registers a permanent property with a property list class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Register2
        /// </summary>
        /// <param name="cls">Property list class to register permanent property within</param>
        /// <param name="name">Name of property to register</param>
        /// <param name="size">Size of property in bytes</param>
        /// <param name="default_value">Default value for property in newly
        /// created property lists</param>
        /// <param name="create">Callback routine called when a property list
        /// is being created and the property value will be initialized</param>
        /// <param name="set">Callback routine called before a new value is
        /// copied into the property's value</param>
        /// <param name="get">Callback routine called when a property value is
        /// retrieved from the property</param>
        /// <param name="delete">Callback routine called when a property is
        /// deleted from a property list</param>
        /// <param name="copy">Callback routine called when a property is
        /// copied from a property list</param>
        /// <param name="compare">Callback routine called when a property is
        /// compared with another property list</param>
        /// <param name="close">Callback routine called when a property list is
        /// being closed and the property value will be disposed of</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pregister2",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t register
            (hid_t cls, string name, size_t size, IntPtr default_value,
            prp_create_func_t create, prp_set_func_t set, prp_get_func_t get,
            prp_delete_func_t delete, prp_copy_func_t copy,
            prp_compare_func_t compare, prp_close_func_t close);

        /// <summary>
        /// Removes a property from a property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Remove
        /// </summary>
        /// <param name="plid">Identifier of the property list to modify</param>
        /// <param name="name">Name of property to remove</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Premove",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t remove(hid_t plid, string name);

        /// <summary>
        /// Delete one or more filters in the filter pipeline.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-RemoveFilter
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list
        /// identifier.</param>
        /// <param name="filter">Filter to be deleted.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Premove_filter",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t remove_filter
            (hid_t plist_id, H5Z.filter_t filter);

        /// <summary>
        /// Sets a property list value.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Set
        /// </summary>
        /// <param name="plid">Property list identifier to modify</param>
        /// <param name="name">Name of property to modify</param>
        /// <param name="value">Pointer to value to set the property to</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set
            (hid_t plid, string name, IntPtr value);

        /// <summary>
        /// Sets alignment properties of a file access property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetAlignment
        /// </summary>
        /// <param name="plist">Identifier for a file access property list.</param>
        /// <param name="threshold">Threshold value.</param>
        /// <param name="alignment">Alignment value.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_alignment",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_alignment
            (hid_t plist, hsize_t threshold, hsize_t alignment);

        /// <summary>
        /// Sets the timing for storage space allocation.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetAllocTime
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="alloc_time">When to allocate dataset storage space.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_alloc_time",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_alloc_time
            (hid_t plist_id, H5D.alloc_time_t alloc_time);

#if HDF5_VER1_10

        /// <summary>
        /// Sets two actions to perform when the size of a dataset’s dimension
        /// being appended reaches a specified boundary.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Pset_append_flush.htm
        /// </summary>
        /// <param name="dapl_id">Dataset access property list identifier.</param>
        /// <param name="ndims">The number of elements for boundary.</param>
        /// <param name="boundary">The dimension sizes used to determine the
        /// boundary.</param>
        /// <param name="func">The user-defined callback function.</param>
        /// <param name="udata">The user-defined input data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_append_flush",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_append_flush
            (hid_t dapl_id, uint ndims,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] boundary,
            H5D.append_cb_t func, IntPtr udata);

#endif

        /// <summary>
        /// Sets tracking and indexing of attribute creation order.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetAttrCreationOrder
        /// </summary>
        /// <param name="ocpl_id">Object creation property list identifier</param>
        /// <param name="crt_order_flags">Flags specifying whether to track and
        /// index attribute creation order</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_attr_creation_order",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_attr_creation_order
            (hid_t ocpl_id, uint crt_order_flags);

        /// <summary>
        /// Sets attribute storage phase change thresholds.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetAttrPhaseChange
        /// </summary>
        /// <param name="ocpl_id">Object creation property list identifier</param>
        /// <param name="max_compact">Maximum number of attributes to be stored
        /// in compact storage</param>
        /// <param name="min_dense">Minimum number of attributes to be stored
        /// in dense storage </param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_attr_phase_change",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_attr_phase_change
            (hid_t ocpl_id, uint max_compact = 8, uint min_dense = 6);

        /// <summary>
        /// Sets B-tree split ratios for a dataset transfer property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetBTreeRatios
        /// </summary>
        /// <param name="plist">The dataset transfer property list identifier.</param>
        /// <param name="left">The B-tree split ratio for left-most nodes.</param>
        /// <param name="middle">The B-tree split ratio for right-most nodes
        /// and lone nodes.</param>
        /// <param name="right">The B-tree split ratio for all other nodes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_btree_ratios",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_btree_ratios
            (hid_t plist, double left, double middle, double right);

        /// <summary>
        /// Sets type conversion and background buffers.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetBuffer
        /// </summary>
        /// <param name="plist">Identifier for the dataset transfer property
        /// list.</param>
        /// <param name="size">Size, in bytes, of the type conversion and
        /// background buffers.</param>
        /// <param name="tconv">Pointer to application-allocated type
        /// conversion buffer.</param>
        /// <param name="bkg">Pointer to application-allocated background
        /// buffer.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_buffer",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_buffer
            (hid_t plist, hsize_t size, IntPtr tconv, IntPtr bkg);

        /// <summary>
        /// Sets the raw data chunk cache parameters.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetCache
        /// </summary>
        /// <param name="plist_id">File access property list identifier.</param>
        /// <param name="mdc_nelmts">UNSUSED</param>
        /// <param name="rdcc_nslots">The number of chunk slots in the raw
        /// data chunk cache for this dataset.</param>
        /// <param name="rdcc_nbytes">Total size of the raw data chunk cache 
        /// in bytes.</param>
        /// <param name="rdcc_w0">The chunk preemption policy for all datasets.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_cache",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_cache
            (hid_t plist_id, int mdc_nelmts, size_t rdcc_nslots,
            size_t rdcc_nbytes, double rdcc_w0);

        /// <summary>
        /// Sets the character encoding used to encode link and attribute names.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetCharEncoding
        /// </summary>
        /// <param name="plist_id">Link creation or attribute creation property
        /// list identifier</param>
        /// <param name="encoding">String encoding character set</param>
        /// <returns>Returns a non-negative valule if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_char_encoding",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_char_encoding
            (hid_t plist_id, H5T.cset_t encoding);

        /// <summary>
        /// Sets the size of the chunks used to store a chunked layout dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetChunk
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="ndims">The number of dimensions of each chunk.</param>
        /// <param name="dims">An array defining the size, in dataset elements,
        /// of each chunk.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_chunk",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_chunk
            (hid_t plist_id, int ndims,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]hsize_t[] dims);

        /// <summary>
        /// Sets the size of the chunks used to store a chunked layout dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetChunk
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="ndims">The number of dimensions of each chunk.</param>
        /// <param name="dims">An array defining the size, in dataset elements,
        /// of each chunk.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_chunk",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_chunk(hid_t plist_id, int ndims, hsize_t* dims);

#if HDF5_VER1_10

        /// <summary>
        /// Sets the edge chunk option in a dataset creation property list.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/PartialEdgeChunks/H5Pset_chunk_opts.htm
        /// </summary>
        /// <param name="dcpl_id">Dataset creation property list identifier.</param>
        /// <param name="opts">Edge chunk option flag.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_chunk_opts",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_chunk_opts
            (hid_t dcpl_id, uint opts);

#endif

        /// <summary>
        /// Sets the raw data chunk cache parameters.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetChunkCache
        /// </summary>
        /// <param name="dapl_id">Dataset access property list identifier.</param>
        /// <param name="rdcc_nslots">The number of chunk slots in the raw data
        /// chunk cache for this dataset.</param>
        /// <param name="rdcc_nbytes">The total size of the raw data chunk
        /// cache for this dataset.</param>
        /// <param name="rdcc_w0">The chunk preemption policy for this dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_chunk_cache",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_chunk_cache
            (hid_t dapl_id, size_t rdcc_nslots, size_t rdcc_nbytes,
            double rdcc_w0);

        /// <summary>
        /// Sets properties to be used when an object is copied.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetCopyObject
        /// </summary>
        /// <param name="ocpypl_id">Object copy property list identifier</param>
        /// <param name="copy_options">Copy option(s) to be set</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_copy_object",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_copy_object
            (hid_t ocpypl_id, uint copy_options);

        /// <summary>
        /// Sets write tracking information for core driver, <code>H5FD_CORE</code>.
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="is_enabled">Boolean value specifying whether feature
        /// is enabled.</param>
        /// <param name="page_size">Positive integer specifying size, in bytes,
        /// of write aggregation pages.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_core_write_tracking",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_core_write_tracking
            (hid_t fapl_id, hbool_t is_enabled, size_t page_size);

        /// <summary>
        /// Specifies in property list whether to create missing intermediate
        /// groups.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetCreateIntermediateGroup
        /// </summary>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="crt_intermed_group">Flag specifying whether to create
        /// intermediate groups upon the creation of an object</param>
        /// <returns>Returns a non-negative valule if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_create_intermediate_group",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_create_intermediate_group
            (hid_t lcpl_id, uint crt_intermed_group);

        /// <summary>
        /// Sets a data transform expression.
        /// </summary>
        /// <param name="plist_id">Identifier of the property list or class</param>
        /// <param name="expression">Pointer to the null-terminated data
        /// transform expression</param>
        /// <returns>Returns a non-negative valule if successful;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_data_transform",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_data_transform
            (hid_t plist_id, string expression);

        /// <summary>
        /// Sets deflate (GNU gzip) compression method and compression level.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetDeflate
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list
        /// identifier.</param>
        /// <param name="level">Compression level.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_deflate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_deflate(hid_t plist_id, uint level);

        /// <summary>
        /// Sets a file driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetDriver
        /// </summary>
        /// <param name="plist_id">File access or data transfer property list
        /// identifier.</param>
        /// <param name="new_driver_id">Driver identifier.</param>
        /// <param name="new_driver_info">Optional struct containing driver
        /// properties.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_driver",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_driver
            (hid_t plist_id, hid_t new_driver_id, IntPtr new_driver_info);

#if HDF5_VER1_10
        /// <summary>
        /// Sets the flag to create minimized dataset object headers.
        /// See https://portal.hdfgroup.org/display/HDF5/H5P_SET_DSET_NO_ATTRS_HINT
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier</param>
        /// <param name="minimize">Flag indicating whether the library will or will not
        /// create minimized dataset object headers</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_dset_no_attrs_hint",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_dset_no_attrs_hint(hid_t plist_id, hbool_t minimize);
#endif

        /// <summary>
        /// Sets whether to enable error-detection when reading a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetEdcCheck
        /// </summary>
        /// <param name="plist">Dataset transfer property list identifier.</param>
        /// <param name="check">Specifies whether error checking is enabled or
        /// disabled for dataset read operations.</param>
        /// <returns>Returns a non-negative value if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_edc_check",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_edc_check
            (hid_t plist, H5Z.EDC_t check);

        /// <summary>
        /// Sets the external dataset storage file prefix in the dataset access
        /// property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetEfilePrefix
        /// </summary>
        /// <param name="dapl">Dataset access property list identifier.</param>
        /// <param name="prefix">Dataset external storage prefix.</param>
        /// <returns>Returns a non-negative value if successful;
        /// otherwise returns a negative value.</returns>
        /// /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_efile_prefix",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_efile_prefix
            (hid_t dapl, string prefix);

        /// <summary>
        /// Sets the external link traversal file access flag in a link access
        /// property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetELinkAccFlags
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <param name="flags">The access flag for external link traversal.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_elink_acc_flags",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_elink_acc_flags
            (hid_t lapl_id, uint flags);

        /// <summary>
        /// Sets the external link traversal callback function in a link access
        /// property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetELinkCb
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <param name="func">User-defined external link traversal callback
        /// function.</param>
        /// <param name="op_data">User-defined input data for the callback
        /// function.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_elink_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_elink_cb
            (hid_t lapl_id, H5L.elink_traverse_t func, IntPtr op_data);

        /// <summary>
        /// Sets a file access property list for use in accessing a file
        /// pointed to by an external link.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetELinkFapl
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_elink_fapl",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_elink_fapl
            (hid_t lapl_id, hid_t fapl_id);

        /// <summary>
        /// Sets the number of files that can be held open in an external link
        /// open file cache.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetELinkFileCacheSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="efc_size">External link open file cache size in number
        /// of files.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_elink_file_cache_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_elink_file_cache_size
            (hid_t fapl_id, uint efc_size);

        /// <summary>
        /// Sets prefix to be applied to external link paths.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetELinkPrefix
        /// </summary>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <param name="prefix">Prefix to be applied to external link paths</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_elink_prefix",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_elink_prefix
            (hid_t lapl_id, string prefix);

        /// <summary>
        /// Sets estimated number of links and length of link names in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetEstLinkInfo
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="est_num_entries">Estimated number of links to be
        /// inserted into group</param>
        /// <param name="est_name_len">Estimated average length of link names</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_est_link_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_est_link_info
            (hid_t gcpl_id, uint est_num_entries, uint est_name_len);

#if HDF5_VER1_10
        /// <summary>
        /// Controls the library's behavior of evicting metadata associated with a closed object.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetEvictOnClose
        /// </summary>
        /// <param name="plist">Identifier of a file access property list.</param>
        /// <param name="evict_on_close">Boolean flag, whether the HDF5 object
        /// should be evicted on close.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_evict_on_close",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_evict_on_close
            (hid_t plist, hbool_t evict_on_close);
#endif

        /// <summary>
        /// Adds an external file to the list of external files.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetExternal
        /// </summary>
        /// <param name="plist">Identifier of a dataset creation property list.</param>
        /// <param name="name">Name of an external file.</param>
        /// <param name="offset">Offset, in bytes, from the beginning of the
        /// file to the location in the file where the data starts.</param>
        /// <param name="size">Number of bytes reserved in the file for the
        /// data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_external",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_external
            (hid_t plist, string name, off_t offset, hsize_t size);

        /// <summary>
        /// Sets offset property for low-level access to a file in a family of files.
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="offset">Offset in bytes within the HDF5 file.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_family_offset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_family_offset
            (hid_t fapl_id, hsize_t offset);

        /// <summary>
        /// Modifies the file access property list to use the
        /// <code>H5FD_CORE</code> driver.
        /// </summary>
        /// <param name="fapl">File access property list identifier.</param>
        /// <param name="increment">Size, in bytes, of memory increments.</param>
        /// <param name="backing_store">Boolean flag indicating whether to
        /// write the file contents to disk when the file is closed.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_core",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_core
            (hid_t fapl, IntPtr increment, hbool_t backing_store);

        /// <summary>
        /// Sets the file access property list to use the family driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFaplFamily
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="memb_size">Size in bytes of each file member.</param>
        /// <param name="memb_fapl_id">Identifier of file access property list
        /// for each family member.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_family",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_family
            (hid_t fapl_id, hsize_t memb_size, hid_t memb_fapl_id);

        /// <summary>
        /// Sets up use of the direct I/O driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFaplDirect
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="alignment">Required memory alignment boundary</param>
        /// <param name="block_size">File system block size</param>
        /// <param name="cbuf_size">Copy buffer size</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_direct",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_direct
            (hid_t fapl_id, size_t alignment, size_t block_size,
            size_t cbuf_size);

        /// <summary>
        /// Sets up the logging virtual file driver (<code>H5FD_LOG</code>) for
        /// use.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFaplLog
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="logfile">Name of the log file.</param>
        /// <param name="flags">Flags specifying the types of logging activity.</param>
        /// <param name="buf_size">The size of the logging buffers, in bytes.</param>
        /// <returns>Returns non-negative if successful. Otherwise returns
        /// negative.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_log",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_log
            ( hid_t fapl_id, string logfile, UInt64 flags, size_t buf_size );

        /// <summary>
        /// Modifies the file access property list to use the
        /// <code>H5FD_SEC2</code> driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFaplSec2
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_sec2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_sec2(hid_t fapl_id);

        /// <summary>
        /// Emulates the old split file driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFaplSplit
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="meta_ext">Metadata filename extension.</param>
        /// <param name="meta_plist_id">File access property list identifier
        /// for the metadata file.</param>
        /// <param name="raw_ext">Raw data filename extension.</param>
        /// <param name="raw_plist_id">File access property list identifier for
        /// the raw data file.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_split",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_split
            (hid_t fapl_id, string meta_ext, hid_t meta_plist_id,
            string raw_ext, hid_t raw_plist_id);

        /// <summary>
        /// Sets the standard I/O driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFaplStdio
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_stdio",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_stdio(hid_t fapl_id);

        /// <summary>
        /// Sets the Windows I/O driver.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFaplWindows
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fapl_windows",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fapl_windows(hid_t fapl_id);

        /// <summary>
        /// Sets the file close degree.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFcloseDegree
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="fc_degree">The file close degree property.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fclose_degree",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fclose_degree
            (hid_t fapl_id, H5F.close_degree_t fc_degree);

        /// <summary>
        /// Sets an initial file image in a memory buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFileImage
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="buf_ptr">Pointer to the initial file image, or
        /// <code>NULL</code> if no initial file image is desired</param>
        /// <param name="buf_len">Size of the supplied buffer, or 0 (zero) if
        /// no initial image is desired</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_file_image",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_file_image
            (hid_t fapl_id, IntPtr buf_ptr, size_t buf_len);

        /// <summary>
        /// Sets the callbacks for working with file images.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFileImageCallbacks
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="callbacks_ptr">Pointer to an instance of the
        /// <code>H5FD.file_image_callbacks_t</code> structure.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_file_image_callbacks",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_file_image_callbacks
            (hid_t fapl_id, ref H5FD.file_image_callbacks_t callbacks_ptr);

#if HDF5_VER1_10

        /// <summary>
        /// Sets the file space management strategy and/or free-space section
        /// threshold for an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FileSpace/H5Pset_file_space.htm
        /// </summary>
        /// <param name="fcpl">The file creation property list identifier.</param>
        /// <param name="strategy">The strategy for file space management.</param>
        /// <param name="threshold">The free-space section threshold. The
        /// library default is 1, which is to track all free-space sections.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_file_space",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_file_space
            (hid_t fcpl, H5F.file_space_type_t strategy, hsize_t threshold = 1);

        /// <summary>
        /// Sets the file space page size for a file creation property list.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFileSpacePageSize
        /// </summary>
        /// <param name="fcpl">The file creation property list identifier.</param>
        /// <param name="strategy">The current file space management strategy
        /// in use for the file.</param>
        /// <param name="fsp_size">File space page size</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_file_space_page_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_file_space_page_size
            (hid_t fcpl, hsize_t fsp_size);
        
        /// <summary>
        /// Sets the file space handling strategy and persisting free-space
        /// values for a file creation property list.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFileSpaceStrategy
        /// </summary>
        /// <param name="fcpl">The file creation property list identifier.</param>
        /// <param name="strategy">The file space handling strategy to be used.</param>
        /// <param name="persist">A boolean value to indicate whether free
        /// space should be persistent or not.</param>
        /// <param name="threshold">The smallest free-space section size that
        /// the free space manager will track.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_file_space_strategy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_file_space_strategy(hid_t fcpl,
            H5F.fspace_strategy_t strategy, hbool_t persist,
            hsize_t threshold);

#endif

        /// <summary>
        /// Sets the time when fill values are written to a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFillTime
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="fill_time">When to write fill values to a dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fill_time",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fill_time
            (hid_t plist_id, H5D.fill_time_t fill_time);

        /// <summary>
        /// Sets the fill value for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFillValue
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="type_id">Datatype of <paramref name="value"/>.</param>
        /// <param name="value">Pointer to buffer containing value to use as
        /// fill value.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fill_value",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fill_value
            (hid_t plist_id, hid_t type_id, IntPtr value);

        /// <summary>
        /// Adds a filter to the filter pipeline.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFilter
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list
        /// identifier.</param>
        /// <param name="filter_id">Filter identifier for the filter to be
        /// added to the pipeline.</param>
        /// <param name="flags">Bit vector specifying certain general
        /// properties of the filter.</param>
        /// <param name="cd_nelmts">Number of elements in
        /// <code>cd_values</code>.</param>
        /// <param name="cd_values">Auxiliary data for the filter.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_filter",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_filter
            (hid_t plist_id, H5Z.filter_t filter_id, uint flags,
            size_t cd_nelmts, uint[] cd_values);

        /// <summary>
        /// Sets user-defined filter callback function.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFilterCallback
        /// </summary>
        /// <param name="plist">Dataset transfer property list identifier.</param>
        /// <param name="func">User-defined filter callback function.</param>
        /// <param name="op_data">User-defined input data for the callback
        /// function.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_filter_callback",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_filter_callback
            (hid_t plist, H5Z.filter_func_t func, IntPtr op_data);

        /// <summary>
        /// Sets up use of the Fletcher32 checksum filter.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetFletcher32
        /// </summary>
        /// <param name="plist_id">Dataset or group creation property list
        /// identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_fletcher32",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fletcher32(hid_t plist_id);

        /// <summary>
        /// Sets garbage collecting references flag.
        /// </summary>
        /// <param name="plist">File access property list identifier.</param>
        /// <param name="gc_ref">Flag setting reference garbage collection to
        /// on (1) or off (0).</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_gc_reference",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_gc_reference(hid_t plist, uint gc_ref);

        /// <summary>
        /// Sets number of I/O vectors to be read/written in hyperslab I/O.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetHyperVectorSize
        /// </summary>
        /// <param name="dxpl_id">Dataset transfer property list identifier.</param>
        /// <param name="vector_size">Number of I/O vectors to accumulate in
        /// memory for I/O operations.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_hyper_vector_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_hyper_vector_size
            (hid_t dxpl_id, size_t vector_size);

        /// <summary>
        /// Sets the size of the parameter used to control the B-trees for
        /// indexing chunked datasets.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetIstoreK
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier</param>
        /// <param name="ik">1/2 rank of chunked storage B-tree</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_istore_k",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_istore_k(hid_t fcpl_id, uint ik);

        /// <summary>
        /// Sets the type of storage used to store the raw data for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetLayout
        /// </summary>
        /// <param name="plist">Identifier of property list to query.</param>
        /// <param name="layout">Type of storage layout for raw data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_layout",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_layout
            (hid_t plist, H5D.layout_t layout);

        /// <summary>
        /// Sets bounds on library versions, and indirectly format versions,
        /// to be used when creating objects.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetLibverBounds
        /// </summary>
        /// <param name="plist">File access property list identifier</param>
        /// <param name="low">The earliest version of the library that will be
        /// used for writing objects, indirectly specifying the earliest object
        /// format version that can be used when creating objects in the file.
        /// </param>
        /// <param name="high">The latest version of the library that will be
        /// used for writing objects, indirectly specifying the latest object
        /// format version that can be used when creating objects in the file.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_libver_bounds",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_libver_bounds
            (hid_t plist,
            H5F.libver_t low = H5F.libver_t.EARLIEST,
            H5F.libver_t high = H5F.libver_t.LATEST);

        /// <summary>
        /// Sets creation order tracking and indexing for links in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetLinkCreationOrder
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="crt_order_flags">Creation order flag(s)</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_link_creation_order",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_link_creation_order
            (hid_t gcpl_id, uint crt_order_flags);

        /// <summary>
        /// Sets the parameters for conversion between compact and dense groups.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetLinkPhaseChange
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="max_compact">Maximum number of links for compact
        /// storage</param>
        /// <param name="min_dense">Minimum number of links for dense storage</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_link_phase_change",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_link_phase_change
            (hid_t gcpl_id, uint max_compact = 8, uint min_dense = 6);

        /// <summary>
        /// Specifies the anticipated maximum size of a local heap.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetLocalHeapSizeHint
        /// </summary>
        /// <param name="gcpl_id">Group creation property list identifier</param>
        /// <param name="size_hint">Anticipated maximum size in bytes of
        /// local heap</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_local_heap_size_hint",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_local_heap_size_hint
            (hid_t gcpl_id, size_t size_hint);

        /// <summary>
        /// Sets the callback function that <code>H5Ocopy</code> will invoke
        /// before searching the entire destination file for a matching
        /// committed datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetMcdtSearchCb
        /// </summary>
        /// <param name="ocpypl_id">Object copy property list identifier</param>
        /// <param name="func">User-defined callback function</param>
        /// <param name="op_data">User-defined input data for the callback
        /// function</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_mcdt_search_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_mcdt_search_cb
            (hid_t ocpypl_id, H5O.mcdt_search_cb_t func, IntPtr op_data);

        /// <summary>
        /// Set the initial metadata cache configuration in the indicated File
        /// Access Property List to the supplied value.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetMdcConfig
        /// </summary>
        /// <param name="plist_id">Identifier of the file access property list.</param>
        /// <param name="config_ptr">Pointer to the instance of
        /// <code>H5AC.cache_config_t</code> containing the desired
        /// configuration.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_mdc_config",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_mdc_config
            (hid_t plist_id, IntPtr config_ptr);

#if HDF5_VER1_10

         /// <summary>
        /// Sets the metadata cache image option for a file access property list.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetMDCImageConfig
        /// </summary>
        /// <param name="fapl_id">Identifier of the file access property list.</param>
        /// <param name="config_ptr">Pointer to the instance of
        /// <code>H5AC.cache_image_config_t</code> containing the desired
        /// configuration.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_mdc_image_config",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_mdc_image_config
            (hid_t fapl_id, IntPtr config_ptr);

        /// <summary>
        /// Sets metadata cache logging options.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Pset_mdc_log_options.htm
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="is_enabled">Whether logging is enabled.</param>
        /// <param name="location">Log file name.</param>
        /// <param name="start_on_access">Whether the logging will begin as
        /// soon as the file is opened or created.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_mdc_log_options",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_mdc_log_options
            (hid_t fapl_id, hbool_t is_enabled, string location,
            hbool_t start_on_access);

#endif

        /// <summary>
        /// Sets the minimum metadata block size.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetMetaBlockSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="size">Minimum size, in bytes, of metadata block
        /// allocations.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_meta_block_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_meta_block_size
            (hid_t fapl_id, hsize_t size);

#if HDF5_VER1_10

        /// <summary>
        /// Retrieves the number of read attempts from a file access property
        /// list.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Pset_metadata_read_attempts.htm
        /// </summary>
        /// <param name="fapl">Identifier for a file access property list.</param>
        /// <param name="attempts">The number of read attempts.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_metadata_read_attempts",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_metadata_read_attempts
            (hid_t fapl, uint attempts);

#endif

        /// <summary>
        /// Sets up the use of the N-Bit filter.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetNbit
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_nbit",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_nbit(hid_t plist_id);

        /// <summary>
        /// Sets maximum number of soft or user-defined link traversals.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetNLinks
        /// </summary>
        /// <param name="lapl_id">File access property list identifier</param>
        /// <param name="nlinks">Maximum number of links to traverse</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_nlinks",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_nlinks
            (hid_t lapl_id, size_t nlinks);

        /// <summary>
        /// Sets the recording of times associated with an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetObjTrackTimes
        /// </summary>
        /// <param name="ocpl_id">Object creation property list identifier</param>
        /// <param name="track_times">Boolean value, 1 or 0, specifying whether
        /// object times are to be tracked</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_obj_track_times",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_obj_track_times
            (hid_t ocpl_id, hbool_t track_times);

#if HDF5_VER1_10

        /// <summary>
        /// Sets a callback function to invoke when an object flush occurs in
        /// the file.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5Pset_object_flush_cb.htm
        /// </summary>
        /// <param name="fapl_id">Identifier for a file access property list.</param>
        /// <param name="func">The user-defined callback function.</param>
        /// <param name="udata">The user-defined input data for the callback
        /// function.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_object_flush_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_object_flush_cb
            (hid_t plist_id, H5F.flush_cb_t func, IntPtr udata);

        /// <summary>
        /// Sets the maximum size for the page buffer and the minimum
        /// percentage for metadata and raw data pages.
        /// See https://support.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetPageBufferSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier</param>
        /// <param name="buf_size">Maximum size, in bytes, of the page buffer</param>
        /// <param name="min_meta_prec">Minimum metadata percentage to keep in
        /// the page buffer before allowing pages containing metadata to be
        /// evicted</param>
        /// <param name="min_raw_perc">Minimum raw data percentage to keep in
        /// the page buffer before allowing pages containing raw data to be
        /// evicted</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_page_buffer_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_page_buffer_size
            (hid_t fapl_id, IntPtr buf_size, uint min_meta_perc,
            uint min_raw_perc);

#endif

        /// <summary>
        /// Sets up the use of the scale-offset filter.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetScaleoffset
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="scale_type">Flag indicating compression method.</param>
        /// <param name="scale_factor">Parameter related to scale. Must be
        /// non-negative.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_scaleoffset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_scaleoffset
            (hid_t plist_id, H5Z.SO_scale_type_t scale_type, int scale_factor);

        /// <summary>
        /// Configures the specified shared object header message index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSharedMesgIndex
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier.</param>
        /// <param name="index_num">Index being configured</param>
        /// <param name="mesg_type_flags">Types of messages that should be
        /// stored in this index.</param>
        /// <param name="min_mesg_size">Minimum message size.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_shared_mesg_index",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_shared_mesg_index
            (hid_t fcpl_id, uint index_num, uint mesg_type_flags,
            uint min_mesg_size);

        /// <summary>
        /// Sets number of shared object header message indexes.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSharedMesgNIndexes
        /// </summary>
        /// <param name="plist_id">File creation property list</param>
        /// <param name="nindexes">Number of shared object header message
        /// indexes to be available in files created with this property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_shared_mesg_nindexes",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_shared_mesg_nindexes
            (hid_t plist_id, uint nindexes);

        /// <summary>
        /// Sets shared object header message storage phase change thresholds.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSharedMesgPhaseChange
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier</param>
        /// <param name="max_list">Threshold above which storage of a shared
        /// object header message index shifts from list to B-tree</param>
        /// <param name="min_btree">Threshold below which storage of a shared
        /// object header message index reverts to list format</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_shared_mesg_phase_change",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_shared_mesg_phase_change
            (hid_t fcpl_id, uint max_list, uint min_btree);

        /// <summary>
        /// Sets up use of the shuffle filter.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetShuffle
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_shuffle",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_shuffle(hid_t plist_id);

        /// <summary>
        /// Sets the maximum size of the data sieve buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSieveBufSize
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="size">Maximum size, in bytes, of data sieve buffer.</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_sieve_buf_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_sieve_buf_size
            (hid_t fapl_id, size_t size);

        /// <summary>
        /// Sets the byte size of the offsets and lengths used to address
        /// objects in an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSizes
        /// </summary>
        /// <param name="plist">Identifier of property list to modify.</param>
        /// <param name="sizeof_addr">Size of an object offset in bytes.</param>
        /// <param name="sizeof_size">Size of an object length in bytes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_sizes",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_sizes
            (hid_t plist, size_t sizeof_addr, size_t sizeof_size);

        /// <summary>
        /// Sets the size of a contiguous block reserved for small data.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSmallData
        /// </summary>
        /// <param name="fapl_id">File access property list identifier.</param>
        /// <param name="size">Maximum size, in bytes, of the small data block.</param>
        /// <returns>Returns a non-negative value if successful; otherwise a
        /// negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_small_data_block_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_small_data_block_size
            (hid_t fapl_id, hsize_t size = 2048);

        /// <summary>
        /// Sets the size of parameters used to control the symbol table nodes.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSymK
        /// </summary>
        /// <param name="fcpl_id">File creation property list identifier</param>
        /// <param name="ik">Symbol table tree rank</param>
        /// <param name="lk">Symbol table node size</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_sym_k",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_sym_k
            (hid_t fcpl_id, uint ik, uint lk);

        /// <summary>
        /// Sets up use of the SZIP compression filter.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetSzip
        /// </summary>
        /// <param name="plist">Dataset creation property list identifier.</param>
        /// <param name="options_mask">A bit-mask conveying the desired SZIP
        /// options.</param>
        /// <param name="pixels_per_block">The number of pixels or data
        /// elements in each data block.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_szip",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_szip
            (hid_t plist, uint options_mask, uint pixels_per_block);

        /// <summary>
        /// Sets user-defined datatype conversion callback function.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetTypeConvCb
        /// </summary>
        /// <param name="plist">Dataset transfer property list identifier.</param>
        /// <param name="func">User-defined type conversion callback function.</param>
        /// <param name="op_data">User-defined input data for the callback
        /// function.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_type_conv_cb",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_type_conv_cb
            (hid_t plist, H5T.conv_except_func_t func, IntPtr op_data);

        /// <summary>
        /// Sets user block size.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetUserblock
        /// </summary>
        /// <param name="plist">Identifier of property list to modify.</param>
        /// <param name="size">Size of the user-block in bytes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_userblock",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_userblock(hid_t plist, hsize_t size);

#if HDF5_VER1_10

        /// <summary>
        /// Sets the mapping between virtual and source datasets.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pset_virtual.htm
        /// </summary>
        /// <param name="dcpl_id">The identifier of the dataset creation
        /// property list that will be used when creating the virtual dataset.</param>
        /// <param name="vspace_id">The dataspace identifier with the selection
        /// within the virtual dataset applied, possibly an unlimited
        /// selection.</param>
        /// <param name="src_file_name">The name of the HDF5 file where the
        /// source dataset is or will be located.</param>
        /// <param name="src_dset_name">The path to the HDF5 dataset in the
        /// file specified by <paramref name="src_file_name"/>. </param>
        /// <param name="src_space_id">The source dataset’s dataspace
        /// identifier with a selection applied, possibly an unlimited
        /// selection</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_virtual",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_virtual
            (hid_t dcpl_id, hid_t vspace_id, string src_file_name,
            string src_dset_name, hid_t src_space_id);

        /// <summary>
        /// Sets prefix to be applied to VDS source file paths.
        /// See https://portal.hdfgroup.org/display/HDF5/H5P_SET_VIRTUAL_PREFIX
        /// </summary>
        /// <param name="dapl">Dataset access property list identifier.</param>
        /// <param name="prefix">Prefix to be applied to VDS source file paths.</param>
        /// <returns>Returns a non-negative value if successful;
        /// otherwise returns a negative value.</returns>
        /// /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_virtual_prefix",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_virtual_prefix
            (hid_t dapl, string prefix);

        /// <summary>
        /// Sets the maximum number of missing source files and/or datasets
        /// with the printf-style names when getting the extent of an unlimited
        /// virtual dataset.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pset_virtual_printf_gap.htm
        /// </summary>
        /// <param name="dapl_id">Dataset access property list identifier for
        /// the virtual dataset</param>
        /// <param name="gap_size">Maximum number of files and/or datasets
        /// allowed to be missing for determining the extent of an unlimited
        /// virtual dataset with printf-style mappings</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Pset_virtual_printf_gap",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_virtual_printf_gap
            (hid_t dapl_id, hsize_t gap_size);

        /// <summary>
        /// Sets the view of the virtual dataset (VDS) to include or exclude
        /// missing mapped elements.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Pset_virtual_view.htm
        /// </summary>
        /// <param name="plist_id">Identifier of the virtual dataset access
        /// property list.</param>
        /// <param name="view">Flag specifying the extent of the data to be
        /// included in the view.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_virtual_view",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_virtual_view
            (hid_t plist_id, H5D.vds_view_t view);

#endif

        /// <summary>
        /// Sets the memory manager for variable-length datatype allocation in
        /// <code>H5Dread</code> and <code>H5Dvlen_reclaim</code>.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetVLMemManager
        /// </summary>
        /// <param name="plist">Identifier for the dataset transfer property list.</param>
        /// <param name="alloc">User's allocate routine, or <code>NULL</code>
        /// for system <code>malloc</code>.</param>
        /// <param name="alloc_info">Extra parameter for user's allocation
        /// routine. Contents are ignored if preceding parameter is <code>NULL</code>.</param>
        /// <param name="free">User's free routine, or <code>NULL</code>
        /// for system <code>free</code>.</param>
        /// <param name="free_info">Extra parameter for user's free
        /// routine. Contents are ignored if preceding parameter is <code>NULL</code>.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_vlen_mem_manager",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_vlen_mem_manager
            (hid_t plist, H5MM.allocate_t alloc, IntPtr alloc_info,
            H5MM.free_t free, IntPtr free_info);

        /// <summary>
        /// Removes a property from a property list class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-Unregister
        /// </summary>
        /// <param name="cls">Property list class from which to remove
        /// permanent property</param>
        /// <param name="name">Name of property to remove</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Punregister",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t unregister(hid_t cls, string name);
    }
}
