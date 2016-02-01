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
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pcreate_class",
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
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pexist",
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
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Pget",
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
            (hid_t plist_id, int max_ndims, [Out] hsize_t[] dims);

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
