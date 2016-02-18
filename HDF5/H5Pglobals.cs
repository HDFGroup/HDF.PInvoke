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

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed partial class H5P
    {
        static H5DLLImporter m_importer;

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
        public static hid_t ATTRIBUTE_CREATE { get { if (!H5P_CLS_ATTRIBUTE_CREATE_ID_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5P_CLS_ATTRIBUTE_CREATE_ID_g", ref val, Marshal.ReadInt32)) { H5P_CLS_ATTRIBUTE_CREATE_ID_g = val; } } return H5P_CLS_ATTRIBUTE_CREATE_ID_g.GetValueOrDefault(); } }
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
    }
}
