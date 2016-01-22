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
using hid_t = System.Int32;

namespace HDF.PInvoke
{
    public unsafe sealed class H5P
    {
        static H5DLLImporter m_importer;

        static H5P()
        {
            m_importer = H5DLLImporter.Create();
        }

        #region
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

        #region
        public static hid_t CLS_ROOT { get { if (!H5P_CLS_ROOT_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_ROOT_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_ROOT_ID_g = val; } } return H5P_CLS_ROOT_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_OBJECT_CREATE { get { if (!H5P_CLS_OBJECT_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_OBJECT_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_OBJECT_CREATE_ID_g = val; } } return H5P_CLS_OBJECT_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_FILE_CREATE { get { if (!H5P_CLS_FILE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_FILE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_FILE_CREATE_ID_g = val; } } return H5P_CLS_FILE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_FILE_ACCESS { get { if (!H5P_CLS_FILE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_FILE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_FILE_ACCESS_ID_g = val; } } return H5P_CLS_FILE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_DATASET_CREATE { get { if (!H5P_CLS_DATASET_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATASET_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATASET_CREATE_ID_g = val; } } return H5P_CLS_DATASET_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_DATASET_ACCESS { get { if (!H5P_CLS_DATASET_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATASET_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATASET_ACCESS_ID_g = val; } } return H5P_CLS_DATASET_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_DATASET_XFER { get { if (!H5P_CLS_DATASET_XFER_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATASET_XFER_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATASET_XFER_ID_g = val; } } return H5P_CLS_DATASET_XFER_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_FILE_MOUNT { get { if (!H5P_CLS_FILE_MOUNT_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_FILE_MOUNT_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_FILE_MOUNT_ID_g = val; } } return H5P_CLS_FILE_MOUNT_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_GROUP_CREATE { get { if (!H5P_CLS_GROUP_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_GROUP_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_GROUP_CREATE_ID_g = val; } } return H5P_CLS_GROUP_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_GROUP_ACCESS { get { if (!H5P_CLS_GROUP_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_GROUP_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_GROUP_ACCESS_ID_g = val; } } return H5P_CLS_GROUP_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_DATATYPE_CREATE { get { if (!H5P_CLS_DATATYPE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATATYPE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATATYPE_CREATE_ID_g = val; } } return H5P_CLS_DATATYPE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_DATATYPE_ACCESS { get { if (!H5P_CLS_DATATYPE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_DATATYPE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_DATATYPE_ACCESS_ID_g = val; } } return H5P_CLS_DATATYPE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_STRING_CREATE { get { if (!H5P_CLS_STRING_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_STRING_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_STRING_CREATE_ID_g = val; } } return H5P_CLS_STRING_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_ATTRIBUTE_CREATE { get { if (!H5P_CLS_ATTRIBUTE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_ATTRIBUTE_CREATE_ID_g;", ref val, Marshal.ReadInt32)) { H5P_CLS_ATTRIBUTE_CREATE_ID_g = val; } } return H5P_CLS_ATTRIBUTE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_OBJECT_COPY { get { if (!H5P_CLS_OBJECT_COPY_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_OBJECT_COPY_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_OBJECT_COPY_ID_g = val; } } return H5P_CLS_OBJECT_COPY_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_LINK_CREATE { get { if (!H5P_CLS_LINK_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_LINK_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_LINK_CREATE_ID_g = val; } } return H5P_CLS_LINK_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t CLS_LINK_ACCESS { get { if (!H5P_CLS_LINK_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_LINK_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_LINK_ACCESS_ID_g = val; } } return H5P_CLS_LINK_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_FILE_CREATE { get { if (!H5P_LST_FILE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_FILE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_FILE_CREATE_ID_g = val; } } return H5P_LST_FILE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_FILE_ACCESS { get { if (!H5P_LST_FILE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_FILE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_FILE_ACCESS_ID_g = val; } } return H5P_LST_FILE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_DATASET_CREATE { get { if (!H5P_LST_DATASET_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATASET_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATASET_CREATE_ID_g = val; } } return H5P_LST_DATASET_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_DATASET_ACCESS { get { if (!H5P_LST_DATASET_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATASET_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATASET_ACCESS_ID_g = val; } } return H5P_LST_DATASET_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_DATASET_XFER { get { if (!H5P_LST_DATASET_XFER_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATASET_XFER_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATASET_XFER_ID_g = val; } } return H5P_LST_DATASET_XFER_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_FILE_MOUNT { get { if (!H5P_LST_FILE_MOUNT_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_FILE_MOUNT_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_FILE_MOUNT_ID_g = val; } } return H5P_LST_FILE_MOUNT_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_GROUP_CREATE { get { if (!H5P_LST_GROUP_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_GROUP_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_GROUP_CREATE_ID_g = val; } } return H5P_LST_GROUP_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_GROUP_ACCESS { get { if (!H5P_LST_GROUP_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_GROUP_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_GROUP_ACCESS_ID_g = val; } } return H5P_LST_GROUP_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_DATATYPE_CREATE { get { if (!H5P_LST_DATATYPE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATATYPE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATATYPE_CREATE_ID_g = val; } } return H5P_LST_DATATYPE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_DATATYPE_ACCESS { get { if (!H5P_LST_DATATYPE_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_DATATYPE_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_DATATYPE_ACCESS_ID_g = val; } } return H5P_LST_DATATYPE_ACCESS_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_ATTRIBUTE_CREATE { get { if (!H5P_LST_ATTRIBUTE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_ATTRIBUTE_CREATE_ID_g;", ref val, Marshal.ReadInt32)) { H5P_LST_ATTRIBUTE_CREATE_ID_g = val; } } return H5P_LST_ATTRIBUTE_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_OBJECT_COPY { get { if (!H5P_LST_OBJECT_COPY_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_OBJECT_COPY_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_OBJECT_COPY_ID_g = val; } } return H5P_LST_OBJECT_COPY_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_LINK_CREATE { get { if (!H5P_LST_LINK_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_LINK_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_LINK_CREATE_ID_g = val; } } return H5P_LST_LINK_CREATE_ID_g.GetValueOrDefault(); } }
        public static hid_t LST_LINK_ACCESS { get { if (!H5P_LST_LINK_ACCESS_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_LST_LINK_ACCESS_ID_g", ref val, Marshal.ReadInt32)) { H5P_LST_LINK_ACCESS_ID_g = val; } } return H5P_LST_LINK_ACCESS_ID_g.GetValueOrDefault(); } }
        #endregion

        /// <summary>
        /// Default value for all property list classes
        /// </summary>
        public const hid_t DEFAULT = 0;


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
