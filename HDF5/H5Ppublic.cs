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

using prp_create_func_t = HDF.PInvoke.H5P.prp_cb1_t;
using prp_set_func_t = HDF.PInvoke.H5P.prp_cb2_t;
using prp_get_func_t = HDF.PInvoke.H5P.prp_cb2_t;
using prp_delete_func_t = HDF.PInvoke.H5P.prp_cb2_t;
using prp_copy_func_t = HDF.PInvoke.H5P.prp_cb1_t;
using prp_close_func_t = HDF.PInvoke.H5P.prp_cb1_t;

namespace HDF.PInvoke
{
    public unsafe sealed class H5P
    {
        static H5DLLImporter m_importer;

        static H5P()
        {
            m_importer = H5DLLImporter.Create();
        }

        #region The library's property list classes

        public static hid_t ROOT { get { if (!H5P_CLS_ROOT_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_ROOT_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_ROOT_ID_g = val; } } return H5P_CLS_ROOT_ID_g.GetValueOrDefault(); } }
        public static hid_t OBJECT_CREATE { get { if (!H5P_CLS_OBJECT_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_OBJECT_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_OBJECT_CREATE_ID_g = val; } } return H5P_CLS_OBJECT_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t FILE_CREATE { get { if (!H5P_CLS_FILE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_FILE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_FILE_CREATE_ID_g = val; } } return H5P_CLS_FILE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t FILE_ACCESS { get { if (!H5P_CLS_FILE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_FILE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_FILE_ACCESS_ID_g = val; } } return H5P_CLS_FILE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t DATASET_CREATE { get { if (!H5P_CLS_DATASET_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATASET_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATASET_CREATE_ID_g = val; } } return H5P_CLS_DATASET_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t DATASET_ACCESS { get { if (!H5P_CLS_DATASET_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATASET_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATASET_ACCESS_ID_g = val; } } return H5P_CLS_DATASET_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t DATASET_XFER { get { if (!H5P_CLS_DATASET_XFER_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATASET_XFER_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATASET_XFER_ID_g = val; } } return H5P_CLS_DATASET_XFER_ID_g.GetValueOrDefault(); } }
        public static hid_t FILE_MOUNT { get { if (!H5P_CLS_FILE_MOUNT_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_FILE_MOUNT_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_FILE_MOUNT_ID_g = val; } } return H5P_CLS_FILE_MOUNT_ID_g.GetValueOrDefault(); } }
        public static hid_t GROUP_CREATE { get { if (!H5P_CLS_GROUP_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_GROUP_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_GROUP_CREATE_ID_g = val; } } return H5P_CLS_GROUP_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t GROUP_ACCESS { get { if (!H5P_CLS_GROUP_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_GROUP_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_GROUP_ACCESS_ID_g = val; } } return H5P_CLS_GROUP_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t DATATYPE_CREATE { get { if (!H5P_CLS_DATATYPE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATATYPE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATATYPE_CREATE_ID_g = val; } } return H5P_CLS_DATATYPE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t DATATYPE_ACCESS { get { if (!H5P_CLS_DATATYPE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATATYPE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATATYPE_ACCESS_ID_g = val; } } return H5P_CLS_DATATYPE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t STRING_CREATE { get { if (!H5P_CLS_STRING_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_STRING_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_STRING_CREATE_ID_g = val; } } return H5P_CLS_STRING_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t ATTRIBUTE_CREATE { get { if (!H5P_CLS_ATTRIBUTE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_ATTRIBUTE_CREATE_ID_g;", ref val, Marshal.ReadInt32)) { H5P_CLS_ATTRIBUTE_CREATE_ID_g = val; } } return H5P_CLS_ATTRIBUTE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t OBJECT_COPY { get { if (!H5P_CLS_OBJECT_COPY_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_OBJECT_COPY_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_OBJECT_COPY_ID_g = val; } } return H5P_CLS_OBJECT_COPY_ID_g.GetValueOrDefault(); } }
        public static hid_t LINK_CREATE { get { if (!H5P_CLS_LINK_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_LINK_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_LINK_CREATE_ID_g = val; } } return H5P_CLS_LINK_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LINK_ACCESS { get { if (!H5P_CLS_LINK_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_LINK_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_LINK_ACCESS_ID_g = val; } } return H5P_CLS_LINK_ACCESS_ID_g.GetValueOrDefault(); } }

        #endregion

        #region The library's default property lists

        public static hid_t FILE_CREATE_DEFAULT { get { if (!H5P_LST_FILE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_FILE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_FILE_CREATE_ID_g = val; } } return H5P_LST_FILE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t FILE_ACCESS_DEFAULT { get { if (!H5P_LST_FILE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_FILE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_FILE_ACCESS_ID_g = val; } } return H5P_LST_FILE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t DATASET_CREATE_DEFAULT { get { if (!H5P_LST_DATASET_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATASET_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATASET_CREATE_ID_g = val; } } return H5P_LST_DATASET_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t DATASET_ACCESS_DEFAULT { get { if (!H5P_LST_DATASET_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATASET_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATASET_ACCESS_ID_g = val; } } return H5P_LST_DATASET_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t DATASET_XFER_DEFAULT { get { if (!H5P_LST_DATASET_XFER_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATASET_XFER_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATASET_XFER_ID_g = val; } } return H5P_LST_DATASET_XFER_ID_g.GetValueOrDefault(); } }
        public static hid_t FILE_MOUNT_DEFAULT { get { if (!H5P_LST_FILE_MOUNT_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_FILE_MOUNT_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_FILE_MOUNT_ID_g = val; } } return H5P_LST_FILE_MOUNT_ID_g.GetValueOrDefault(); } }
        public static hid_t GROUP_CREATE_DEFAULT { get { if (!H5P_LST_GROUP_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_GROUP_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_GROUP_CREATE_ID_g = val; } } return H5P_LST_GROUP_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t GROUP_ACCESS_DEFAULT { get { if (!H5P_LST_GROUP_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_GROUP_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_GROUP_ACCESS_ID_g = val; } } return H5P_LST_GROUP_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t DATATYPE_CREATE_DEFAULT { get { if (!H5P_LST_DATATYPE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATATYPE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATATYPE_CREATE_ID_g = val; } } return H5P_LST_DATATYPE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t DATATYPE_ACCESS_DEFAULT { get { if (!H5P_LST_DATATYPE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATATYPE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATATYPE_ACCESS_ID_g = val; } } return H5P_LST_DATATYPE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t ATTRIBUTE_CREATE_DEFAULT { get { if (!H5P_LST_ATTRIBUTE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_ATTRIBUTE_CREATE_ID_g;", ref val, Marshal.ReadInt32)) { H5P_LST_ATTRIBUTE_CREATE_ID_g = val; } } return H5P_LST_ATTRIBUTE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t OBJECT_COPY_DEFAULT { get { if (!H5P_LST_OBJECT_COPY_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_OBJECT_COPY_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_OBJECT_COPY_ID_g = val; } } return H5P_LST_OBJECT_COPY_ID_g.GetValueOrDefault(); } }
        public static hid_t LINK_CREATE_DEFAULT { get { if (!H5P_LST_LINK_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_LINK_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_LINK_CREATE_ID_g = val; } } return H5P_LST_LINK_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LINK_ACCESS_DEFAULT { get { if (!H5P_LST_LINK_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_LINK_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_LINK_ACCESS_ID_g = val; } } return H5P_LST_LINK_ACCESS_ID_g.GetValueOrDefault(); } }

        #endregion

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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t prp_cb1_t
        (string name, size_t size, IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t prp_cb2_t
        (hid_t prop_id, string name, size_t size, IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int prp_compare_func_t
        (IntPtr value1, IntPtr value2, size_t size);

        /* Define property list iteration function type */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t iterate_t
            (hid_t id, string name, IntPtr iter_data);


        #region Property list class IDs

        static hid_t? H5P_CLS_ROOT_ID_g;
        static hid_t? H5P_CLS_OBJECT_CREATE_ID_g;
        static hid_t? H5P_CLS_FILE_CREATE_ID_g;
        static hid_t? H5P_CLS_FILE_ACCESS_ID_g;
        static hid_t? H5P_CLS_DATASET_CREATE_ID_g;
        static hid_t? H5P_CLS_DATASET_ACCESS_ID_g;
        static hid_t? H5P_CLS_DATASET_XFER_ID_g;
        static hid_t? H5P_CLS_FILE_MOUNT_ID_g;
        static hid_t? H5P_CLS_GROUP_CREATE_ID_g;
        static hid_t? H5P_CLS_GROUP_ACCESS_ID_g;
        static hid_t? H5P_CLS_DATATYPE_CREATE_ID_g;
        static hid_t? H5P_CLS_DATATYPE_ACCESS_ID_g;
        static hid_t? H5P_CLS_STRING_CREATE_ID_g;
        static hid_t? H5P_CLS_ATTRIBUTE_CREATE_ID_g;
        static hid_t? H5P_CLS_OBJECT_COPY_ID_g;
        static hid_t? H5P_CLS_LINK_CREATE_ID_g;
        static hid_t? H5P_CLS_LINK_ACCESS_ID_g;

        #endregion

        #region Default property list IDs

        static hid_t? H5P_LST_FILE_CREATE_ID_g;
        static hid_t? H5P_LST_FILE_ACCESS_ID_g;
        static hid_t? H5P_LST_DATASET_CREATE_ID_g;
        static hid_t? H5P_LST_DATASET_ACCESS_ID_g;
        static hid_t? H5P_LST_DATASET_XFER_ID_g;
        static hid_t? H5P_LST_FILE_MOUNT_ID_g;
        static hid_t? H5P_LST_GROUP_CREATE_ID_g;
        static hid_t? H5P_LST_GROUP_ACCESS_ID_g;
        static hid_t? H5P_LST_DATATYPE_CREATE_ID_g;
        static hid_t? H5P_LST_DATATYPE_ACCESS_ID_g;
        static hid_t? H5P_LST_ATTRIBUTE_CREATE_ID_g;
        static hid_t? H5P_LST_OBJECT_COPY_ID_g;
        static hid_t? H5P_LST_LINK_CREATE_ID_g;
        static hid_t? H5P_LST_LINK_ACCESS_ID_g;

        #endregion

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
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pcopy_prop",
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
            (hid_t plist_id, int max_ndims, [Out] hsize_t[] dims);

        /// <summary>
        /// Sets the size of the chunks used to store a chunked layout dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5P.html#Property-SetChunk
        /// </summary>
        /// <param name="plist_id">Dataset creation property list identifier.</param>
        /// <param name="ndims">The number of dimensions of each chunk.</param>
        /// <param name="dim">An array defining the size, in dataset elements,
        /// of each chunk.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pset_chunk",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_chunk
            (hid_t plist_id, int ndims, hsize_t* dim);

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
            H5F.libver_t low = H5F.libver_t.LIBVER_EARLIEST,
            H5F.libver_t high = H5F.libver_t.LIBVER_LATEST);
    }
}
