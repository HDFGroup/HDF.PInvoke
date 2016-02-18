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

using herr_t = System.Int32;
using hsize_t = System.UInt64;
using size_t = System.IntPtr;

using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed partial class H5E
    {
        static H5DLLImporter m_importer;

        #region error classes

        /* HDF5 error class */
        static hid_t? H5E_ERR_CLS_g;

        /* Major error codes */
        static hid_t? H5E_DATASET_g;       /* Dataset */
        static hid_t? H5E_FUNC_g;          /* Function entry/exit */
        static hid_t? H5E_STORAGE_g;       /* Data storage */
        static hid_t? H5E_FILE_g;          /* File accessibilty */
        static hid_t? H5E_SOHM_g;          /* Shared Object Header Messages */
        static hid_t? H5E_SYM_g;           /* Symbol table */
        static hid_t? H5E_PLUGIN_g;        /* Plugin for dynamically loaded library */
        static hid_t? H5E_VFL_g;           /* Virtual File Layer */
        static hid_t? H5E_INTERNAL_g;      /* Internal error (too specific to document in detail) */
        static hid_t? H5E_BTREE_g;         /* B-Tree node */
        static hid_t? H5E_REFERENCE_g;     /* References */
        static hid_t? H5E_DATASPACE_g;     /* Dataspace */
        static hid_t? H5E_RESOURCE_g;      /* Resource unavailable */
        static hid_t? H5E_PLIST_g;         /* Property lists */
        static hid_t? H5E_LINK_g;          /* Links */
        static hid_t? H5E_DATATYPE_g;      /* Datatype */
        static hid_t? H5E_RS_g;            /* Reference Counted Strings */
        static hid_t? H5E_HEAP_g;          /* Heap */
        static hid_t? H5E_OHDR_g;          /* Object header */
        static hid_t? H5E_ATOM_g;          /* Object atom */
        static hid_t? H5E_ATTR_g;          /* Attribute */
        static hid_t? H5E_NONE_MAJOR_g;    /* No error */
        static hid_t? H5E_IO_g;            /* Low-level I/O */
        static hid_t? H5E_SLIST_g;         /* Skip Lists */
        static hid_t? H5E_EFL_g;           /* External file list */
        static hid_t? H5E_TST_g;           /* Ternary Search Trees */
        static hid_t? H5E_ARGS_g;          /* Invalid arguments to routine */
        static hid_t? H5E_ERROR_g;         /* Error API */
        static hid_t? H5E_PLINE_g;         /* Data filters */
        static hid_t? H5E_FSPACE_g;        /* Free Space Manager */
        static hid_t? H5E_CACHE_g;         /* Object cache */

        /* Minor error codes */
        static hid_t? H5E_SEEKERROR_g;     /* Seek failed */
        static hid_t? H5E_READERROR_g;     /* Read failed */
        static hid_t? H5E_WRITEERROR_g;    /* Write failed */
        static hid_t? H5E_CLOSEERROR_g;    /* Close failed */
        static hid_t? H5E_OVERFLOW_g;      /* Address overflowed */
        static hid_t? H5E_FCNTL_g;         /* File control (fcntl) failed */

        /* Resource errors */
        static hid_t? H5E_NOSPACE_g;       /* No space available for allocation */
        static hid_t? H5E_CANTALLOC_g;     /* Can't allocate space */
        static hid_t? H5E_CANTCOPY_g;      /* Unable to copy object */
        static hid_t? H5E_CANTFREE_g;      /* Unable to free object */
        static hid_t? H5E_ALREADYEXISTS_g; /* Object already exists */
        static hid_t? H5E_CANTLOCK_g;      /* Unable to lock object */
        static hid_t? H5E_CANTUNLOCK_g;    /* Unable to unlock object */
        static hid_t? H5E_CANTGC_g;        /* Unable to garbage collect */
        static hid_t? H5E_CANTGETSIZE_g;   /* Unable to compute size */
        static hid_t? H5E_OBJOPEN_g;       /* Object is already open */

        /* Heap errors */
        static hid_t? H5E_CANTRESTORE_g;   /* Can't restore condition */
        static hid_t? H5E_CANTCOMPUTE_g;   /* Can't compute value */
        static hid_t? H5E_CANTEXTEND_g;    /* Can't extend heap's space */
        static hid_t? H5E_CANTATTACH_g;    /* Can't attach object */
        static hid_t? H5E_CANTUPDATE_g;    /* Can't update object */
        static hid_t? H5E_CANTOPERATE_g;   /* Can't operate on object */

        /* Function entry/exit interface errors */
        static hid_t? H5E_CANTINIT_g;      /* Unable to initialize object */
        static hid_t? H5E_ALREADYINIT_g;   /* Object already initialized */
        static hid_t? H5E_CANTRELEASE_g;   /* Unable to release object */

        /* Property list errors */
        static hid_t? H5E_CANTGET_g;       /* Can't get value */
        static hid_t? H5E_CANTSET_g;       /* Can't set value */
        static hid_t? H5E_DUPCLASS_g;      /* Duplicate class name in parent class */
        static hid_t? H5E_SETDISALLOWED_g; /* Disallowed operation */

        /* Free space errors */
        static hid_t? H5E_CANTMERGE_g;     /* Can't merge objects */
        static hid_t? H5E_CANTREVIVE_g;    /* Can't revive object */
        static hid_t? H5E_CANTSHRINK_g;    /* Can't shrink container */

        /* Object header related errors */
        static hid_t? H5E_LINKCOUNT_g;     /* Bad object header link count */
        static hid_t? H5E_VERSION_g;       /* Wrong version number */
        static hid_t? H5E_ALIGNMENT_g;     /* Alignment error */
        static hid_t? H5E_BADMESG_g;       /* Unrecognized message */
        static hid_t? H5E_CANTDELETE_g;    /* Can't delete message */
        static hid_t? H5E_BADITER_g;       /* Iteration failed */
        static hid_t? H5E_CANTPACK_g;      /* Can't pack messages */
        static hid_t? H5E_CANTRESET_g;     /* Can't reset object */
        static hid_t? H5E_CANTRENAME_g;    /* Unable to rename object */

        /* System level errors */
        static hid_t? H5E_SYSERRSTR_g;     /* System error message */

        /* I/O pipeline errors */
        static hid_t? H5E_NOFILTER_g;      /* Requested filter is not available */
        static hid_t? H5E_CALLBACK_g;      /* Callback failed */
        static hid_t? H5E_CANAPPLY_g;      /* Error from filter 'can apply' callback */
        static hid_t? H5E_SETLOCAL_g;      /* Error from filter 'set local' callback */
        static hid_t? H5E_NOENCODER_g;     /* Filter present but encoding disabled */
        static hid_t? H5E_CANTFILTER_g;    /* Filter operation failed */

        /* Group related errors */
        static hid_t? H5E_CANTOPENOBJ_g;   /* Can't open object */
        static hid_t? H5E_CANTCLOSEOBJ_g;  /* Can't close object */
        static hid_t? H5E_COMPLEN_g;       /* Name component is too long */
        static hid_t? H5E_PATH_g;          /* Problem with path to object */

        /* No error */
        static hid_t? H5E_NONE_MINOR_g;    /* No error */

        /* Plugin errors */
        static hid_t? H5E_OPENERROR_g;     /* Can't open directory or file */

        /* File accessibilty errors */
        static hid_t? H5E_FILEEXISTS_g;    /* File already exists */
        static hid_t? H5E_FILEOPEN_g;      /* File already open */
        static hid_t? H5E_CANTCREATE_g;    /* Unable to create file */
        static hid_t? H5E_CANTOPENFILE_g;  /* Unable to open file */
        static hid_t? H5E_CANTCLOSEFILE_g; /* Unable to close file */
        static hid_t? H5E_NOTHDF5_g;       /* Not an HDF5 file */
        static hid_t? H5E_BADFILE_g;       /* Bad file ID accessed */
        static hid_t? H5E_TRUNCATED_g;     /* File has been truncated */
        static hid_t? H5E_MOUNT_g;         /* File mount error */

        /* Object atom related errors */
        static hid_t? H5E_BADATOM_g;       /* Unable to find atom information (already closed?) */
        static hid_t? H5E_BADGROUP_g;      /* Unable to find ID group information */
        static hid_t? H5E_CANTREGISTER_g;  /* Unable to register new atom */
        static hid_t? H5E_CANTINC_g;       /* Unable to increment reference count */
        static hid_t? H5E_CANTDEC_g;       /* Unable to decrement reference count */
        static hid_t? H5E_NOIDS_g;         /* Out of IDs for group */

        /* Cache related errors */
        static hid_t? H5E_CANTFLUSH_g;     /* Unable to flush data from cache */
        static hid_t? H5E_CANTSERIALIZE_g; /* Unable to serialize data from cache */
        static hid_t? H5E_CANTLOAD_g;      /* Unable to load metadata into cache */
        static hid_t? H5E_PROTECT_g;       /* Protected metadata error */
        static hid_t? H5E_NOTCACHED_g;     /* Metadata not currently cached */
        static hid_t? H5E_SYSTEM_g;        /* Internal error detected */
        static hid_t? H5E_CANTINS_g;       /* Unable to insert metadata into cache */
        static hid_t? H5E_CANTPROTECT_g;   /* Unable to protect metadata */
        static hid_t? H5E_CANTUNPROTECT_g; /* Unable to unprotect metadata */
        static hid_t? H5E_CANTPIN_g;       /* Unable to pin cache entry */
        static hid_t? H5E_CANTUNPIN_g;     /* Unable to un-pin cache entry */
        static hid_t? H5E_CANTMARKDIRTY_g; /* Unable to mark a pinned entry as dirty */
        static hid_t? H5E_CANTDIRTY_g;     /* Unable to mark metadata as dirty */
        static hid_t? H5E_CANTEXPUNGE_g;   /* Unable to expunge a metadata cache entry */
        static hid_t? H5E_CANTRESIZE_g;    /* Unable to resize a metadata cache entry */

        /* Link related errors */
        static hid_t? H5E_TRAVERSE_g;      /* Link traversal failure */
        static hid_t? H5E_NLINKS_g;        /* Too many soft links in path */
        static hid_t? H5E_NOTREGISTERED_g; /* Link class not registered */
        static hid_t? H5E_CANTMOVE_g;      /* Can't move object */
        static hid_t? H5E_CANTSORT_g;      /* Can't sort objects */

        /* Parallel MPI errors */
        static hid_t? H5E_MPI_g;           /* Some MPI function failed */
        static hid_t? H5E_MPIERRSTR_g;     /* MPI Error String */
        static hid_t? H5E_CANTRECV_g;      /* Can't receive data */

        /* Dataspace errors */
        static hid_t? H5E_CANTCLIP_g;      /* Can't clip hyperslab region */
        static hid_t? H5E_CANTCOUNT_g;     /* Can't count elements */
        static hid_t? H5E_CANTSELECT_g;    /* Can't select hyperslab */
        static hid_t? H5E_CANTNEXT_g;      /* Can't move to next iterator location */
        static hid_t? H5E_BADSELECT_g;     /* Invalid selection */
        static hid_t? H5E_CANTCOMPARE_g;   /* Can't compare objects */

        /* Argument errors */
        static hid_t? H5E_UNINITIALIZED_g; /* Information is uinitialized */
        static hid_t? H5E_UNSUPPORTED_g;   /* Feature is unsupported */
        static hid_t? H5E_BADTYPE_g;       /* Inappropriate type */
        static hid_t? H5E_BADRANGE_g;      /* Out of range */
        static hid_t? H5E_BADVALUE_g;      /* Bad value */

        /* B-tree related errors */
        static hid_t? H5E_NOTFOUND_g;      /* Object not found */
        static hid_t? H5E_EXISTS_g;        /* Object already exists */
        static hid_t? H5E_CANTENCODE_g;    /* Unable to encode value */
        static hid_t? H5E_CANTDECODE_g;    /* Unable to decode value */
        static hid_t? H5E_CANTSPLIT_g;     /* Unable to split node */
        static hid_t? H5E_CANTREDISTRIBUTE_g; /* Unable to redistribute records */
        static hid_t? H5E_CANTSWAP_g;      /* Unable to swap records */
        static hid_t? H5E_CANTINSERT_g;    /* Unable to insert object */
        static hid_t? H5E_CANTLIST_g;      /* Unable to list node */
        static hid_t? H5E_CANTMODIFY_g;    /* Unable to modify record */
        static hid_t? H5E_CANTREMOVE_g;    /* Unable to remove object */

        /* Datatype conversion errors */
        static hid_t? H5E_CANTCONVERT_g;   /* Can't convert datatypes */
        static hid_t? H5E_BADSIZE_g;       /* Bad size for object */

        #endregion

        #region

        /* HDF5 error class */
        public static hid_t ERR_CLS
        {
            get
            {
                if (!H5E_ERR_CLS_g.HasValue)
                {
                    hid_t val = -1;
                    if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ERR_CLS_g", ref val, Marshal.ReadInt32))
                    {
                        H5E_ERR_CLS_g = val;
                    }
                }
                return H5E_ERR_CLS_g.GetValueOrDefault();
            }
        }

        /* generated from H5Err.txt */
        public static hid_t ARGS { get { if (!H5E_ARGS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ARGS_g", ref val, Marshal.ReadInt32)) { H5E_ARGS_g = val; } } return H5E_ARGS_g.GetValueOrDefault(); } }
        public static hid_t RESOURCE { get { if (!H5E_RESOURCE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_RESOURCE_g", ref val, Marshal.ReadInt32)) { H5E_RESOURCE_g = val; } } return H5E_RESOURCE_g.GetValueOrDefault(); } }
        public static hid_t INTERNAL { get { if (!H5E_INTERNAL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_INTERNAL_g", ref val, Marshal.ReadInt32)) { H5E_INTERNAL_g = val; } } return H5E_INTERNAL_g.GetValueOrDefault(); } }
        public static hid_t FILE { get { if (!H5E_FILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FILE_g", ref val, Marshal.ReadInt32)) { H5E_FILE_g = val; } } return H5E_FILE_g.GetValueOrDefault(); } }
        public static hid_t IO { get { if (!H5E_IO_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_IO_g", ref val, Marshal.ReadInt32)) { H5E_IO_g = val; } } return H5E_IO_g.GetValueOrDefault(); } }
        public static hid_t FUNC { get { if (!H5E_FUNC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FUNC_g", ref val, Marshal.ReadInt32)) { H5E_FUNC_g = val; } } return H5E_FUNC_g.GetValueOrDefault(); } }
        public static hid_t ATOM { get { if (!H5E_ATOM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ATOM_g", ref val, Marshal.ReadInt32)) { H5E_ATOM_g = val; } } return H5E_ATOM_g.GetValueOrDefault(); } }
        public static hid_t CACHE { get { if (!H5E_CACHE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CACHE_g", ref val, Marshal.ReadInt32)) { H5E_CACHE_g = val; } } return H5E_CACHE_g.GetValueOrDefault(); } }
        public static hid_t LINK { get { if (!H5E_LINK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_LINK_g", ref val, Marshal.ReadInt32)) { H5E_LINK_g = val; } } return H5E_LINK_g.GetValueOrDefault(); } }
        public static hid_t BTREE { get { if (!H5E_BTREE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BTREE_g", ref val, Marshal.ReadInt32)) { H5E_BTREE_g = val; } } return H5E_BTREE_g.GetValueOrDefault(); } }
        public static hid_t SYM { get { if (!H5E_SYM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SYM_g", ref val, Marshal.ReadInt32)) { H5E_SYM_g = val; } } return H5E_SYM_g.GetValueOrDefault(); } }
        public static hid_t HEAP { get { if (!H5E_HEAP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_HEAP_g", ref val, Marshal.ReadInt32)) { H5E_HEAP_g = val; } } return H5E_HEAP_g.GetValueOrDefault(); } }
        public static hid_t OHDR { get { if (!H5E_OHDR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OHDR_g", ref val, Marshal.ReadInt32)) { H5E_OHDR_g = val; } } return H5E_OHDR_g.GetValueOrDefault(); } }
        public static hid_t DATATYPE { get { if (!H5E_DATATYPE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DATATYPE_g", ref val, Marshal.ReadInt32)) { H5E_DATATYPE_g = val; } } return H5E_DATATYPE_g.GetValueOrDefault(); } }
        public static hid_t DATASPACE { get { if (!H5E_DATASPACE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DATASPACE_g", ref val, Marshal.ReadInt32)) { H5E_DATASPACE_g = val; } } return H5E_DATASPACE_g.GetValueOrDefault(); } }
        public static hid_t DATASET { get { if (!H5E_DATASET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DATASET_g", ref val, Marshal.ReadInt32)) { H5E_DATASET_g = val; } } return H5E_DATASET_g.GetValueOrDefault(); } }
        public static hid_t STORAGE { get { if (!H5E_STORAGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_STORAGE_g", ref val, Marshal.ReadInt32)) { H5E_STORAGE_g = val; } } return H5E_STORAGE_g.GetValueOrDefault(); } }
        public static hid_t PLIST { get { if (!H5E_PLIST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PLIST_g", ref val, Marshal.ReadInt32)) { H5E_PLIST_g = val; } } return H5E_PLIST_g.GetValueOrDefault(); } }
        public static hid_t ATTR { get { if (!H5E_ATTR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ATTR_g", ref val, Marshal.ReadInt32)) { H5E_ATTR_g = val; } } return H5E_ATTR_g.GetValueOrDefault(); } }
        public static hid_t PLINE { get { if (!H5E_PLINE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PLINE_g", ref val, Marshal.ReadInt32)) { H5E_PLINE_g = val; } } return H5E_PLINE_g.GetValueOrDefault(); } }
        public static hid_t EFL { get { if (!H5E_EFL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_EFL_g", ref val, Marshal.ReadInt32)) { H5E_EFL_g = val; } } return H5E_EFL_g.GetValueOrDefault(); } }
        public static hid_t REFERENCE { get { if (!H5E_REFERENCE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_REFERENCE_g", ref val, Marshal.ReadInt32)) { H5E_REFERENCE_g = val; } } return H5E_REFERENCE_g.GetValueOrDefault(); } }
        public static hid_t VFL { get { if (!H5E_VFL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_VFL_g", ref val, Marshal.ReadInt32)) { H5E_VFL_g = val; } } return H5E_VFL_g.GetValueOrDefault(); } }
        public static hid_t TST { get { if (!H5E_TST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_TST_g", ref val, Marshal.ReadInt32)) { H5E_TST_g = val; } } return H5E_TST_g.GetValueOrDefault(); } }
        public static hid_t RS { get { if (!H5E_RS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_RS_g", ref val, Marshal.ReadInt32)) { H5E_RS_g = val; } } return H5E_RS_g.GetValueOrDefault(); } }
        public static hid_t ERROR { get { if (!H5E_ERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ERROR_g", ref val, Marshal.ReadInt32)) { H5E_ERROR_g = val; } } return H5E_ERROR_g.GetValueOrDefault(); } }
        public static hid_t SLIST { get { if (!H5E_SLIST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SLIST_g", ref val, Marshal.ReadInt32)) { H5E_SLIST_g = val; } } return H5E_SLIST_g.GetValueOrDefault(); } }
        public static hid_t FSPACE { get { if (!H5E_FSPACE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FSPACE_g", ref val, Marshal.ReadInt32)) { H5E_FSPACE_g = val; } } return H5E_FSPACE_g.GetValueOrDefault(); } }
        public static hid_t SOHM { get { if (!H5E_SOHM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SOHM_g", ref val, Marshal.ReadInt32)) { H5E_SOHM_g = val; } } return H5E_SOHM_g.GetValueOrDefault(); } }
        public static hid_t PLUGIN { get { if (!H5E_PLUGIN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PLUGIN_g", ref val, Marshal.ReadInt32)) { H5E_PLUGIN_g = val; } } return H5E_PLUGIN_g.GetValueOrDefault(); } }
        public static hid_t NONE_MAJOR { get { if (!H5E_NONE_MAJOR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NONE_MAJOR_g", ref val, Marshal.ReadInt32)) { H5E_NONE_MAJOR_g = val; } } return H5E_NONE_MAJOR_g.GetValueOrDefault(); } }
        public static hid_t UNINITIALIZED { get { if (!H5E_UNINITIALIZED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_UNINITIALIZED_g", ref val, Marshal.ReadInt32)) { H5E_UNINITIALIZED_g = val; } } return H5E_UNINITIALIZED_g.GetValueOrDefault(); } }
        public static hid_t UNSUPPORTED { get { if (!H5E_UNSUPPORTED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_UNSUPPORTED_g", ref val, Marshal.ReadInt32)) { H5E_UNSUPPORTED_g = val; } } return H5E_UNSUPPORTED_g.GetValueOrDefault(); } }
        public static hid_t BADTYPE { get { if (!H5E_BADTYPE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADTYPE_g", ref val, Marshal.ReadInt32)) { H5E_BADTYPE_g = val; } } return H5E_BADTYPE_g.GetValueOrDefault(); } }
        public static hid_t BADRANGE { get { if (!H5E_BADRANGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADRANGE_g", ref val, Marshal.ReadInt32)) { H5E_BADRANGE_g = val; } } return H5E_BADRANGE_g.GetValueOrDefault(); } }
        public static hid_t BADVALUE { get { if (!H5E_BADVALUE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADVALUE_g", ref val, Marshal.ReadInt32)) { H5E_BADVALUE_g = val; } } return H5E_BADVALUE_g.GetValueOrDefault(); } }
        public static hid_t NOSPACE { get { if (!H5E_NOSPACE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOSPACE_g", ref val, Marshal.ReadInt32)) { H5E_NOSPACE_g = val; } } return H5E_NOSPACE_g.GetValueOrDefault(); } }
        public static hid_t CANTALLOC { get { if (!H5E_CANTALLOC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTALLOC_g", ref val, Marshal.ReadInt32)) { H5E_CANTALLOC_g = val; } } return H5E_CANTALLOC_g.GetValueOrDefault(); } }
        public static hid_t CANTCOPY { get { if (!H5E_CANTCOPY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOPY_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOPY_g = val; } } return H5E_CANTCOPY_g.GetValueOrDefault(); } }
        public static hid_t CANTFREE { get { if (!H5E_CANTFREE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTFREE_g", ref val, Marshal.ReadInt32)) { H5E_CANTFREE_g = val; } } return H5E_CANTFREE_g.GetValueOrDefault(); } }
        public static hid_t ALREADYEXISTS { get { if (!H5E_ALREADYEXISTS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ALREADYEXISTS_g", ref val, Marshal.ReadInt32)) { H5E_ALREADYEXISTS_g = val; } } return H5E_ALREADYEXISTS_g.GetValueOrDefault(); } }
        public static hid_t CANTLOCK { get { if (!H5E_CANTLOCK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTLOCK_g", ref val, Marshal.ReadInt32)) { H5E_CANTLOCK_g = val; } } return H5E_CANTLOCK_g.GetValueOrDefault(); } }
        public static hid_t CANTUNLOCK { get { if (!H5E_CANTUNLOCK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUNLOCK_g", ref val, Marshal.ReadInt32)) { H5E_CANTUNLOCK_g = val; } } return H5E_CANTUNLOCK_g.GetValueOrDefault(); } }
        public static hid_t CANTGC { get { if (!H5E_CANTGC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTGC_g", ref val, Marshal.ReadInt32)) { H5E_CANTGC_g = val; } } return H5E_CANTGC_g.GetValueOrDefault(); } }
        public static hid_t CANTGETSIZE { get { if (!H5E_CANTGETSIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTGETSIZE_g", ref val, Marshal.ReadInt32)) { H5E_CANTGETSIZE_g = val; } } return H5E_CANTGETSIZE_g.GetValueOrDefault(); } }
        public static hid_t OBJOPEN { get { if (!H5E_OBJOPEN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OBJOPEN_g", ref val, Marshal.ReadInt32)) { H5E_OBJOPEN_g = val; } } return H5E_OBJOPEN_g.GetValueOrDefault(); } }
        public static hid_t FILEEXISTS { get { if (!H5E_FILEEXISTS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FILEEXISTS_g", ref val, Marshal.ReadInt32)) { H5E_FILEEXISTS_g = val; } } return H5E_FILEEXISTS_g.GetValueOrDefault(); } }
        public static hid_t FILEOPEN { get { if (!H5E_FILEOPEN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FILEOPEN_g", ref val, Marshal.ReadInt32)) { H5E_FILEOPEN_g = val; } } return H5E_FILEOPEN_g.GetValueOrDefault(); } }
        public static hid_t CANTCREATE { get { if (!H5E_CANTCREATE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCREATE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCREATE_g = val; } } return H5E_CANTCREATE_g.GetValueOrDefault(); } }
        public static hid_t CANTOPENFILE { get { if (!H5E_CANTOPENFILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTOPENFILE_g", ref val, Marshal.ReadInt32)) { H5E_CANTOPENFILE_g = val; } } return H5E_CANTOPENFILE_g.GetValueOrDefault(); } }
        public static hid_t CANTCLOSEFILE { get { if (!H5E_CANTCLOSEFILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCLOSEFILE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCLOSEFILE_g = val; } } return H5E_CANTCLOSEFILE_g.GetValueOrDefault(); } }
        public static hid_t NOTHDF5 { get { if (!H5E_NOTHDF5_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTHDF5_g", ref val, Marshal.ReadInt32)) { H5E_NOTHDF5_g = val; } } return H5E_NOTHDF5_g.GetValueOrDefault(); } }
        public static hid_t BADFILE { get { if (!H5E_BADFILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADFILE_g", ref val, Marshal.ReadInt32)) { H5E_BADFILE_g = val; } } return H5E_BADFILE_g.GetValueOrDefault(); } }
        public static hid_t TRUNCATED { get { if (!H5E_TRUNCATED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_TRUNCATED_g", ref val, Marshal.ReadInt32)) { H5E_TRUNCATED_g = val; } } return H5E_TRUNCATED_g.GetValueOrDefault(); } }
        public static hid_t MOUNT { get { if (!H5E_MOUNT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_MOUNT_g", ref val, Marshal.ReadInt32)) { H5E_MOUNT_g = val; } } return H5E_MOUNT_g.GetValueOrDefault(); } }
        public static hid_t SEEKERROR { get { if (!H5E_SEEKERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SEEKERROR_g", ref val, Marshal.ReadInt32)) { H5E_SEEKERROR_g = val; } } return H5E_SEEKERROR_g.GetValueOrDefault(); } }
        public static hid_t READERROR { get { if (!H5E_READERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_READERROR_g", ref val, Marshal.ReadInt32)) { H5E_READERROR_g = val; } } return H5E_READERROR_g.GetValueOrDefault(); } }
        public static hid_t WRITEERROR { get { if (!H5E_WRITEERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_WRITEERROR_g", ref val, Marshal.ReadInt32)) { H5E_WRITEERROR_g = val; } } return H5E_WRITEERROR_g.GetValueOrDefault(); } }
        public static hid_t CLOSEERROR { get { if (!H5E_CLOSEERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CLOSEERROR_g", ref val, Marshal.ReadInt32)) { H5E_CLOSEERROR_g = val; } } return H5E_CLOSEERROR_g.GetValueOrDefault(); } }
        public static hid_t OVERFLOW { get { if (!H5E_OVERFLOW_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OVERFLOW_g", ref val, Marshal.ReadInt32)) { H5E_OVERFLOW_g = val; } } return H5E_OVERFLOW_g.GetValueOrDefault(); } }
        public static hid_t FCNTL { get { if (!H5E_FCNTL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FCNTL_g", ref val, Marshal.ReadInt32)) { H5E_FCNTL_g = val; } } return H5E_FCNTL_g.GetValueOrDefault(); } }
        public static hid_t CANTINIT { get { if (!H5E_CANTINIT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINIT_g", ref val, Marshal.ReadInt32)) { H5E_CANTINIT_g = val; } } return H5E_CANTINIT_g.GetValueOrDefault(); } }
        public static hid_t ALREADYINIT { get { if (!H5E_ALREADYINIT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ALREADYINIT_g", ref val, Marshal.ReadInt32)) { H5E_ALREADYINIT_g = val; } } return H5E_ALREADYINIT_g.GetValueOrDefault(); } }
        public static hid_t CANTRELEASE { get { if (!H5E_CANTRELEASE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRELEASE_g", ref val, Marshal.ReadInt32)) { H5E_CANTRELEASE_g = val; } } return H5E_CANTRELEASE_g.GetValueOrDefault(); } }
        public static hid_t BADATOM { get { if (!H5E_BADATOM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADATOM_g", ref val, Marshal.ReadInt32)) { H5E_BADATOM_g = val; } } return H5E_BADATOM_g.GetValueOrDefault(); } }
        public static hid_t BADGROUP { get { if (!H5E_BADGROUP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADGROUP_g", ref val, Marshal.ReadInt32)) { H5E_BADGROUP_g = val; } } return H5E_BADGROUP_g.GetValueOrDefault(); } }
        public static hid_t CANTREGISTER { get { if (!H5E_CANTREGISTER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREGISTER_g", ref val, Marshal.ReadInt32)) { H5E_CANTREGISTER_g = val; } } return H5E_CANTREGISTER_g.GetValueOrDefault(); } }
        public static hid_t CANTINC { get { if (!H5E_CANTINC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINC_g", ref val, Marshal.ReadInt32)) { H5E_CANTINC_g = val; } } return H5E_CANTINC_g.GetValueOrDefault(); } }
        public static hid_t CANTDEC { get { if (!H5E_CANTDEC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDEC_g", ref val, Marshal.ReadInt32)) { H5E_CANTDEC_g = val; } } return H5E_CANTDEC_g.GetValueOrDefault(); } }
        public static hid_t NOIDS { get { if (!H5E_NOIDS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOIDS_g", ref val, Marshal.ReadInt32)) { H5E_NOIDS_g = val; } } return H5E_NOIDS_g.GetValueOrDefault(); } }
        public static hid_t CANTFLUSH { get { if (!H5E_CANTFLUSH_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTFLUSH_g", ref val, Marshal.ReadInt32)) { H5E_CANTFLUSH_g = val; } } return H5E_CANTFLUSH_g.GetValueOrDefault(); } }
        public static hid_t CANTSERIALIZE { get { if (!H5E_CANTSERIALIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSERIALIZE_g", ref val, Marshal.ReadInt32)) { H5E_CANTSERIALIZE_g = val; } } return H5E_CANTSERIALIZE_g.GetValueOrDefault(); } }
        public static hid_t CANTLOAD { get { if (!H5E_CANTLOAD_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTLOAD_g", ref val, Marshal.ReadInt32)) { H5E_CANTLOAD_g = val; } } return H5E_CANTLOAD_g.GetValueOrDefault(); } }
        public static hid_t PROTECT { get { if (!H5E_PROTECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PROTECT_g", ref val, Marshal.ReadInt32)) { H5E_PROTECT_g = val; } } return H5E_PROTECT_g.GetValueOrDefault(); } }
        public static hid_t NOTCACHED { get { if (!H5E_NOTCACHED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTCACHED_g", ref val, Marshal.ReadInt32)) { H5E_NOTCACHED_g = val; } } return H5E_NOTCACHED_g.GetValueOrDefault(); } }
        public static hid_t SYSTEM { get { if (!H5E_SYSTEM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SYSTEM_g", ref val, Marshal.ReadInt32)) { H5E_SYSTEM_g = val; } } return H5E_SYSTEM_g.GetValueOrDefault(); } }
        public static hid_t CANTINS { get { if (!H5E_CANTINS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINS_g", ref val, Marshal.ReadInt32)) { H5E_CANTINS_g = val; } } return H5E_CANTINS_g.GetValueOrDefault(); } }
        public static hid_t CANTPROTECT { get { if (!H5E_CANTPROTECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTPROTECT_g", ref val, Marshal.ReadInt32)) { H5E_CANTPROTECT_g = val; } } return H5E_CANTPROTECT_g.GetValueOrDefault(); } }
        public static hid_t CANTUNPROTECT { get { if (!H5E_CANTUNPROTECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUNPROTECT_g", ref val, Marshal.ReadInt32)) { H5E_CANTUNPROTECT_g = val; } } return H5E_CANTUNPROTECT_g.GetValueOrDefault(); } }
        public static hid_t CANTPIN { get { if (!H5E_CANTPIN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTPIN_g", ref val, Marshal.ReadInt32)) { H5E_CANTPIN_g = val; } } return H5E_CANTPIN_g.GetValueOrDefault(); } }
        public static hid_t CANTUNPIN { get { if (!H5E_CANTUNPIN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUNPIN_g", ref val, Marshal.ReadInt32)) { H5E_CANTUNPIN_g = val; } } return H5E_CANTUNPIN_g.GetValueOrDefault(); } }
        public static hid_t CANTMARKDIRTY { get { if (!H5E_CANTMARKDIRTY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMARKDIRTY_g", ref val, Marshal.ReadInt32)) { H5E_CANTMARKDIRTY_g = val; } } return H5E_CANTMARKDIRTY_g.GetValueOrDefault(); } }
        public static hid_t CANTDIRTY { get { if (!H5E_CANTDIRTY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDIRTY_g", ref val, Marshal.ReadInt32)) { H5E_CANTDIRTY_g = val; } } return H5E_CANTDIRTY_g.GetValueOrDefault(); } }
        public static hid_t CANTEXPUNGE { get { if (!H5E_CANTEXPUNGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTEXPUNGE_g", ref val, Marshal.ReadInt32)) { H5E_CANTEXPUNGE_g = val; } } return H5E_CANTEXPUNGE_g.GetValueOrDefault(); } }
        public static hid_t CANTRESIZE { get { if (!H5E_CANTRESIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRESIZE_g", ref val, Marshal.ReadInt32)) { H5E_CANTRESIZE_g = val; } } return H5E_CANTRESIZE_g.GetValueOrDefault(); } }
        public static hid_t NOTFOUND { get { if (!H5E_NOTFOUND_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTFOUND_g", ref val, Marshal.ReadInt32)) { H5E_NOTFOUND_g = val; } } return H5E_NOTFOUND_g.GetValueOrDefault(); } }
        public static hid_t EXISTS { get { if (!H5E_EXISTS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_EXISTS_g", ref val, Marshal.ReadInt32)) { H5E_EXISTS_g = val; } } return H5E_EXISTS_g.GetValueOrDefault(); } }
        public static hid_t CANTENCODE { get { if (!H5E_CANTENCODE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTENCODE_g", ref val, Marshal.ReadInt32)) { H5E_CANTENCODE_g = val; } } return H5E_CANTENCODE_g.GetValueOrDefault(); } }
        public static hid_t CANTDECODE { get { if (!H5E_CANTDECODE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDECODE_g", ref val, Marshal.ReadInt32)) { H5E_CANTDECODE_g = val; } } return H5E_CANTDECODE_g.GetValueOrDefault(); } }
        public static hid_t CANTSPLIT { get { if (!H5E_CANTSPLIT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSPLIT_g", ref val, Marshal.ReadInt32)) { H5E_CANTSPLIT_g = val; } } return H5E_CANTSPLIT_g.GetValueOrDefault(); } }
        public static hid_t CANTREDISTRIBUTE { get { if (!H5E_CANTREDISTRIBUTE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREDISTRIBUTE_g", ref val, Marshal.ReadInt32)) { H5E_CANTREDISTRIBUTE_g = val; } } return H5E_CANTREDISTRIBUTE_g.GetValueOrDefault(); } }
        public static hid_t CANTSWAP { get { if (!H5E_CANTSWAP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSWAP_g", ref val, Marshal.ReadInt32)) { H5E_CANTSWAP_g = val; } } return H5E_CANTSWAP_g.GetValueOrDefault(); } }
        public static hid_t CANTINSERT { get { if (!H5E_CANTINSERT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINSERT_g", ref val, Marshal.ReadInt32)) { H5E_CANTINSERT_g = val; } } return H5E_CANTINSERT_g.GetValueOrDefault(); } }
        public static hid_t CANTLIST { get { if (!H5E_CANTLIST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTLIST_g", ref val, Marshal.ReadInt32)) { H5E_CANTLIST_g = val; } } return H5E_CANTLIST_g.GetValueOrDefault(); } }
        public static hid_t CANTMODIFY { get { if (!H5E_CANTMODIFY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMODIFY_g", ref val, Marshal.ReadInt32)) { H5E_CANTMODIFY_g = val; } } return H5E_CANTMODIFY_g.GetValueOrDefault(); } }
        public static hid_t CANTREMOVE { get { if (!H5E_CANTREMOVE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREMOVE_g", ref val, Marshal.ReadInt32)) { H5E_CANTREMOVE_g = val; } } return H5E_CANTREMOVE_g.GetValueOrDefault(); } }
        public static hid_t LINKCOUNT { get { if (!H5E_LINKCOUNT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_LINKCOUNT_g", ref val, Marshal.ReadInt32)) { H5E_LINKCOUNT_g = val; } } return H5E_LINKCOUNT_g.GetValueOrDefault(); } }
        public static hid_t VERSION { get { if (!H5E_VERSION_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_VERSION_g", ref val, Marshal.ReadInt32)) { H5E_VERSION_g = val; } } return H5E_VERSION_g.GetValueOrDefault(); } }
        public static hid_t ALIGNMENT { get { if (!H5E_ALIGNMENT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ALIGNMENT_g", ref val, Marshal.ReadInt32)) { H5E_ALIGNMENT_g = val; } } return H5E_ALIGNMENT_g.GetValueOrDefault(); } }
        public static hid_t BADMESG { get { if (!H5E_BADMESG_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADMESG_g", ref val, Marshal.ReadInt32)) { H5E_BADMESG_g = val; } } return H5E_BADMESG_g.GetValueOrDefault(); } }
        public static hid_t CANTDELETE { get { if (!H5E_CANTDELETE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDELETE_g", ref val, Marshal.ReadInt32)) { H5E_CANTDELETE_g = val; } } return H5E_CANTDELETE_g.GetValueOrDefault(); } }
        public static hid_t BADITER { get { if (!H5E_BADITER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADITER_g", ref val, Marshal.ReadInt32)) { H5E_BADITER_g = val; } } return H5E_BADITER_g.GetValueOrDefault(); } }
        public static hid_t CANTPACK { get { if (!H5E_CANTPACK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTPACK_g", ref val, Marshal.ReadInt32)) { H5E_CANTPACK_g = val; } } return H5E_CANTPACK_g.GetValueOrDefault(); } }
        public static hid_t CANTRESET { get { if (!H5E_CANTRESET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRESET_g", ref val, Marshal.ReadInt32)) { H5E_CANTRESET_g = val; } } return H5E_CANTRESET_g.GetValueOrDefault(); } }
        public static hid_t CANTRENAME { get { if (!H5E_CANTRENAME_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRENAME_g", ref val, Marshal.ReadInt32)) { H5E_CANTRENAME_g = val; } } return H5E_CANTRENAME_g.GetValueOrDefault(); } }
        public static hid_t CANTOPENOBJ { get { if (!H5E_CANTOPENOBJ_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTOPENOBJ_g", ref val, Marshal.ReadInt32)) { H5E_CANTOPENOBJ_g = val; } } return H5E_CANTOPENOBJ_g.GetValueOrDefault(); } }
        public static hid_t CANTCLOSEOBJ { get { if (!H5E_CANTCLOSEOBJ_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCLOSEOBJ_g", ref val, Marshal.ReadInt32)) { H5E_CANTCLOSEOBJ_g = val; } } return H5E_CANTCLOSEOBJ_g.GetValueOrDefault(); } }
        public static hid_t COMPLEN { get { if (!H5E_COMPLEN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_COMPLEN_g", ref val, Marshal.ReadInt32)) { H5E_COMPLEN_g = val; } } return H5E_COMPLEN_g.GetValueOrDefault(); } }
        public static hid_t PATH { get { if (!H5E_PATH_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PATH_g", ref val, Marshal.ReadInt32)) { H5E_PATH_g = val; } } return H5E_PATH_g.GetValueOrDefault(); } }
        public static hid_t CANTCONVERT { get { if (!H5E_CANTCONVERT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCONVERT_g", ref val, Marshal.ReadInt32)) { H5E_CANTCONVERT_g = val; } } return H5E_CANTCONVERT_g.GetValueOrDefault(); } }
        public static hid_t BADSIZE { get { if (!H5E_BADSIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADSIZE_g", ref val, Marshal.ReadInt32)) { H5E_BADSIZE_g = val; } } return H5E_BADSIZE_g.GetValueOrDefault(); } }
        public static hid_t CANTCLIP { get { if (!H5E_CANTCLIP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCLIP_g", ref val, Marshal.ReadInt32)) { H5E_CANTCLIP_g = val; } } return H5E_CANTCLIP_g.GetValueOrDefault(); } }
        public static hid_t CANTCOUNT { get { if (!H5E_CANTCOUNT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOUNT_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOUNT_g = val; } } return H5E_CANTCOUNT_g.GetValueOrDefault(); } }
        public static hid_t CANTSELECT { get { if (!H5E_CANTSELECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSELECT_g", ref val, Marshal.ReadInt32)) { H5E_CANTSELECT_g = val; } } return H5E_CANTSELECT_g.GetValueOrDefault(); } }
        public static hid_t CANTNEXT { get { if (!H5E_CANTNEXT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTNEXT_g", ref val, Marshal.ReadInt32)) { H5E_CANTNEXT_g = val; } } return H5E_CANTNEXT_g.GetValueOrDefault(); } }
        public static hid_t BADSELECT { get { if (!H5E_BADSELECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADSELECT_g", ref val, Marshal.ReadInt32)) { H5E_BADSELECT_g = val; } } return H5E_BADSELECT_g.GetValueOrDefault(); } }
        public static hid_t CANTCOMPARE { get { if (!H5E_CANTCOMPARE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOMPARE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOMPARE_g = val; } } return H5E_CANTCOMPARE_g.GetValueOrDefault(); } }
        public static hid_t CANTGET { get { if (!H5E_CANTGET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTGET_g", ref val, Marshal.ReadInt32)) { H5E_CANTGET_g = val; } } return H5E_CANTGET_g.GetValueOrDefault(); } }
        public static hid_t CANTSET { get { if (!H5E_CANTSET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSET_g", ref val, Marshal.ReadInt32)) { H5E_CANTSET_g = val; } } return H5E_CANTSET_g.GetValueOrDefault(); } }
        public static hid_t DUPCLASS { get { if (!H5E_DUPCLASS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DUPCLASS_g", ref val, Marshal.ReadInt32)) { H5E_DUPCLASS_g = val; } } return H5E_DUPCLASS_g.GetValueOrDefault(); } }
        public static hid_t SETDISALLOWED { get { if (!H5E_SETDISALLOWED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SETDISALLOWED_g", ref val, Marshal.ReadInt32)) { H5E_SETDISALLOWED_g = val; } } return H5E_SETDISALLOWED_g.GetValueOrDefault(); } }
        public static hid_t TRAVERSE { get { if (!H5E_TRAVERSE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_TRAVERSE_g", ref val, Marshal.ReadInt32)) { H5E_TRAVERSE_g = val; } } return H5E_TRAVERSE_g.GetValueOrDefault(); } }
        public static hid_t NLINKS { get { if (!H5E_NLINKS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NLINKS_g", ref val, Marshal.ReadInt32)) { H5E_NLINKS_g = val; } } return H5E_NLINKS_g.GetValueOrDefault(); } }
        public static hid_t NOTREGISTERED { get { if (!H5E_NOTREGISTERED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTREGISTERED_g", ref val, Marshal.ReadInt32)) { H5E_NOTREGISTERED_g = val; } } return H5E_NOTREGISTERED_g.GetValueOrDefault(); } }
        public static hid_t CANTMOVE { get { if (!H5E_CANTMOVE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMOVE_g", ref val, Marshal.ReadInt32)) { H5E_CANTMOVE_g = val; } } return H5E_CANTMOVE_g.GetValueOrDefault(); } }
        public static hid_t CANTSORT { get { if (!H5E_CANTSORT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSORT_g", ref val, Marshal.ReadInt32)) { H5E_CANTSORT_g = val; } } return H5E_CANTSORT_g.GetValueOrDefault(); } }
        public static hid_t MPI { get { if (!H5E_MPI_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_MPI_g", ref val, Marshal.ReadInt32)) { H5E_MPI_g = val; } } return H5E_MPI_g.GetValueOrDefault(); } }
        public static hid_t MPIERRSTR { get { if (!H5E_MPIERRSTR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_MPIERRSTR_g", ref val, Marshal.ReadInt32)) { H5E_MPIERRSTR_g = val; } } return H5E_MPIERRSTR_g.GetValueOrDefault(); } }
        public static hid_t CANTRECV { get { if (!H5E_CANTRECV_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRECV_g", ref val, Marshal.ReadInt32)) { H5E_CANTRECV_g = val; } } return H5E_CANTRECV_g.GetValueOrDefault(); } }
        public static hid_t CANTRESTORE { get { if (!H5E_CANTRESTORE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRESTORE_g", ref val, Marshal.ReadInt32)) { H5E_CANTRESTORE_g = val; } } return H5E_CANTRESTORE_g.GetValueOrDefault(); } }
        public static hid_t CANTCOMPUTE { get { if (!H5E_CANTCOMPUTE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOMPUTE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOMPUTE_g = val; } } return H5E_CANTCOMPUTE_g.GetValueOrDefault(); } }
        public static hid_t CANTEXTEND { get { if (!H5E_CANTEXTEND_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTEXTEND_g", ref val, Marshal.ReadInt32)) { H5E_CANTEXTEND_g = val; } } return H5E_CANTEXTEND_g.GetValueOrDefault(); } }
        public static hid_t CANTATTACH { get { if (!H5E_CANTATTACH_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTATTACH_g", ref val, Marshal.ReadInt32)) { H5E_CANTATTACH_g = val; } } return H5E_CANTATTACH_g.GetValueOrDefault(); } }
        public static hid_t CANTUPDATE { get { if (!H5E_CANTUPDATE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUPDATE_g", ref val, Marshal.ReadInt32)) { H5E_CANTUPDATE_g = val; } } return H5E_CANTUPDATE_g.GetValueOrDefault(); } }
        public static hid_t CANTOPERATE { get { if (!H5E_CANTOPERATE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTOPERATE_g", ref val, Marshal.ReadInt32)) { H5E_CANTOPERATE_g = val; } } return H5E_CANTOPERATE_g.GetValueOrDefault(); } }
        public static hid_t CANTMERGE { get { if (!H5E_CANTMERGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMERGE_g", ref val, Marshal.ReadInt32)) { H5E_CANTMERGE_g = val; } } return H5E_CANTMERGE_g.GetValueOrDefault(); } }
        public static hid_t CANTREVIVE { get { if (!H5E_CANTREVIVE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREVIVE_g", ref val, Marshal.ReadInt32)) { H5E_CANTREVIVE_g = val; } } return H5E_CANTREVIVE_g.GetValueOrDefault(); } }
        public static hid_t CANTSHRINK { get { if (!H5E_CANTSHRINK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSHRINK_g", ref val, Marshal.ReadInt32)) { H5E_CANTSHRINK_g = val; } } return H5E_CANTSHRINK_g.GetValueOrDefault(); } }
        public static hid_t NOFILTER { get { if (!H5E_NOFILTER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOFILTER_g", ref val, Marshal.ReadInt32)) { H5E_NOFILTER_g = val; } } return H5E_NOFILTER_g.GetValueOrDefault(); } }
        public static hid_t CALLBACK { get { if (!H5E_CALLBACK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CALLBACK_g", ref val, Marshal.ReadInt32)) { H5E_CALLBACK_g = val; } } return H5E_CALLBACK_g.GetValueOrDefault(); } }
        public static hid_t CANAPPLY { get { if (!H5E_CANAPPLY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANAPPLY_g", ref val, Marshal.ReadInt32)) { H5E_CANAPPLY_g = val; } } return H5E_CANAPPLY_g.GetValueOrDefault(); } }
        public static hid_t SETLOCAL { get { if (!H5E_SETLOCAL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SETLOCAL_g", ref val, Marshal.ReadInt32)) { H5E_SETLOCAL_g = val; } } return H5E_SETLOCAL_g.GetValueOrDefault(); } }
        public static hid_t NOENCODER { get { if (!H5E_NOENCODER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOENCODER_g", ref val, Marshal.ReadInt32)) { H5E_NOENCODER_g = val; } } return H5E_NOENCODER_g.GetValueOrDefault(); } }
        public static hid_t CANTFILTER { get { if (!H5E_CANTFILTER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTFILTER_g", ref val, Marshal.ReadInt32)) { H5E_CANTFILTER_g = val; } } return H5E_CANTFILTER_g.GetValueOrDefault(); } }
        public static hid_t SYSERRSTR { get { if (!H5E_SYSERRSTR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SYSERRSTR_g", ref val, Marshal.ReadInt32)) { H5E_SYSERRSTR_g = val; } } return H5E_SYSERRSTR_g.GetValueOrDefault(); } }
        public static hid_t OPENERROR { get { if (!H5E_OPENERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OPENERROR_g", ref val, Marshal.ReadInt32)) { H5E_OPENERROR_g = val; } } return H5E_OPENERROR_g.GetValueOrDefault(); } }
        public static hid_t NONE_MINOR { get { if (!H5E_NONE_MINOR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NONE_MINOR_g", ref val, Marshal.ReadInt32)) { H5E_NONE_MINOR_g = val; } } return H5E_NONE_MINOR_g.GetValueOrDefault(); } }

        #endregion
    }
}