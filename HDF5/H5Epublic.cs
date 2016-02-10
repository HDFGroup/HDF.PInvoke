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

#if X86
using ssize_t = System.Int32;
#else
using ssize_t = System.Int64;
#endif

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed class H5E
    {
        static H5DLLImporter m_importer;

        static H5E()
        {
            m_importer = H5DLLImporter.Create();
        }

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
        public static hid_t H5E_ERR_CLS
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
        public static hid_t H5E_ARGS { get { if (!H5E_ARGS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ARGS_g", ref val, Marshal.ReadInt32)) { H5E_ARGS_g = val; } } return H5E_ARGS_g.GetValueOrDefault(); } }
        public static hid_t H5E_RESOURCE { get { if (!H5E_RESOURCE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_RESOURCE_g", ref val, Marshal.ReadInt32)) { H5E_RESOURCE_g = val; } } return H5E_RESOURCE_g.GetValueOrDefault(); } }
        public static hid_t H5E_INTERNAL { get { if (!H5E_INTERNAL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_INTERNAL_g", ref val, Marshal.ReadInt32)) { H5E_INTERNAL_g = val; } } return H5E_INTERNAL_g.GetValueOrDefault(); } }
        public static hid_t H5E_FILE { get { if (!H5E_FILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FILE_g", ref val, Marshal.ReadInt32)) { H5E_FILE_g = val; } } return H5E_FILE_g.GetValueOrDefault(); } }
        public static hid_t H5E_IO { get { if (!H5E_IO_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_IO_g", ref val, Marshal.ReadInt32)) { H5E_IO_g = val; } } return H5E_IO_g.GetValueOrDefault(); } }
        public static hid_t H5E_FUNC { get { if (!H5E_FUNC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FUNC_g", ref val, Marshal.ReadInt32)) { H5E_FUNC_g = val; } } return H5E_FUNC_g.GetValueOrDefault(); } }
        public static hid_t H5E_ATOM { get { if (!H5E_ATOM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ATOM_g", ref val, Marshal.ReadInt32)) { H5E_ATOM_g = val; } } return H5E_ATOM_g.GetValueOrDefault(); } }
        public static hid_t H5E_CACHE { get { if (!H5E_CACHE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CACHE_g", ref val, Marshal.ReadInt32)) { H5E_CACHE_g = val; } } return H5E_CACHE_g.GetValueOrDefault(); } }
        public static hid_t H5E_LINK { get { if (!H5E_LINK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_LINK_g", ref val, Marshal.ReadInt32)) { H5E_LINK_g = val; } } return H5E_LINK_g.GetValueOrDefault(); } }
        public static hid_t H5E_BTREE { get { if (!H5E_BTREE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BTREE_g", ref val, Marshal.ReadInt32)) { H5E_BTREE_g = val; } } return H5E_BTREE_g.GetValueOrDefault(); } }
        public static hid_t H5E_SYM { get { if (!H5E_SYM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SYM_g", ref val, Marshal.ReadInt32)) { H5E_SYM_g = val; } } return H5E_SYM_g.GetValueOrDefault(); } }
        public static hid_t H5E_HEAP { get { if (!H5E_HEAP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_HEAP_g", ref val, Marshal.ReadInt32)) { H5E_HEAP_g = val; } } return H5E_HEAP_g.GetValueOrDefault(); } }
        public static hid_t H5E_OHDR { get { if (!H5E_OHDR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OHDR_g", ref val, Marshal.ReadInt32)) { H5E_OHDR_g = val; } } return H5E_OHDR_g.GetValueOrDefault(); } }
        public static hid_t H5E_DATATYPE { get { if (!H5E_DATATYPE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DATATYPE_g", ref val, Marshal.ReadInt32)) { H5E_DATATYPE_g = val; } } return H5E_DATATYPE_g.GetValueOrDefault(); } }
        public static hid_t H5E_DATASPACE { get { if (!H5E_DATASPACE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DATASPACE_g", ref val, Marshal.ReadInt32)) { H5E_DATASPACE_g = val; } } return H5E_DATASPACE_g.GetValueOrDefault(); } }
        public static hid_t H5E_DATASET { get { if (!H5E_DATASET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DATASET_g", ref val, Marshal.ReadInt32)) { H5E_DATASET_g = val; } } return H5E_DATASET_g.GetValueOrDefault(); } }
        public static hid_t H5E_STORAGE { get { if (!H5E_STORAGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_STORAGE_g", ref val, Marshal.ReadInt32)) { H5E_STORAGE_g = val; } } return H5E_STORAGE_g.GetValueOrDefault(); } }
        public static hid_t H5E_PLIST { get { if (!H5E_PLIST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PLIST_g", ref val, Marshal.ReadInt32)) { H5E_PLIST_g = val; } } return H5E_PLIST_g.GetValueOrDefault(); } }
        public static hid_t H5E_ATTR { get { if (!H5E_ATTR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ATTR_g", ref val, Marshal.ReadInt32)) { H5E_ATTR_g = val; } } return H5E_ATTR_g.GetValueOrDefault(); } }
        public static hid_t H5E_PLINE { get { if (!H5E_PLINE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PLINE_g", ref val, Marshal.ReadInt32)) { H5E_PLINE_g = val; } } return H5E_PLINE_g.GetValueOrDefault(); } }
        public static hid_t H5E_EFL { get { if (!H5E_EFL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_EFL_g", ref val, Marshal.ReadInt32)) { H5E_EFL_g = val; } } return H5E_EFL_g.GetValueOrDefault(); } }
        public static hid_t H5E_REFERENCE { get { if (!H5E_REFERENCE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_REFERENCE_g", ref val, Marshal.ReadInt32)) { H5E_REFERENCE_g = val; } } return H5E_REFERENCE_g.GetValueOrDefault(); } }
        public static hid_t H5E_VFL { get { if (!H5E_VFL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_VFL_g", ref val, Marshal.ReadInt32)) { H5E_VFL_g = val; } } return H5E_VFL_g.GetValueOrDefault(); } }
        public static hid_t H5E_TST { get { if (!H5E_TST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_TST_g", ref val, Marshal.ReadInt32)) { H5E_TST_g = val; } } return H5E_TST_g.GetValueOrDefault(); } }
        public static hid_t H5E_RS { get { if (!H5E_RS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_RS_g", ref val, Marshal.ReadInt32)) { H5E_RS_g = val; } } return H5E_RS_g.GetValueOrDefault(); } }
        public static hid_t H5E_ERROR { get { if (!H5E_ERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ERROR_g", ref val, Marshal.ReadInt32)) { H5E_ERROR_g = val; } } return H5E_ERROR_g.GetValueOrDefault(); } }
        public static hid_t H5E_SLIST { get { if (!H5E_SLIST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SLIST_g", ref val, Marshal.ReadInt32)) { H5E_SLIST_g = val; } } return H5E_SLIST_g.GetValueOrDefault(); } }
        public static hid_t H5E_FSPACE { get { if (!H5E_FSPACE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FSPACE_g", ref val, Marshal.ReadInt32)) { H5E_FSPACE_g = val; } } return H5E_FSPACE_g.GetValueOrDefault(); } }
        public static hid_t H5E_SOHM { get { if (!H5E_SOHM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SOHM_g", ref val, Marshal.ReadInt32)) { H5E_SOHM_g = val; } } return H5E_SOHM_g.GetValueOrDefault(); } }
        public static hid_t H5E_PLUGIN { get { if (!H5E_PLUGIN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PLUGIN_g", ref val, Marshal.ReadInt32)) { H5E_PLUGIN_g = val; } } return H5E_PLUGIN_g.GetValueOrDefault(); } }
        public static hid_t H5E_NONE_MAJOR { get { if (!H5E_NONE_MAJOR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NONE_MAJOR_g", ref val, Marshal.ReadInt32)) { H5E_NONE_MAJOR_g = val; } } return H5E_NONE_MAJOR_g.GetValueOrDefault(); } }
        public static hid_t H5E_UNINITIALIZED { get { if (!H5E_UNINITIALIZED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_UNINITIALIZED_g", ref val, Marshal.ReadInt32)) { H5E_UNINITIALIZED_g = val; } } return H5E_UNINITIALIZED_g.GetValueOrDefault(); } }
        public static hid_t H5E_UNSUPPORTED { get { if (!H5E_UNSUPPORTED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_UNSUPPORTED_g", ref val, Marshal.ReadInt32)) { H5E_UNSUPPORTED_g = val; } } return H5E_UNSUPPORTED_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADTYPE { get { if (!H5E_BADTYPE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADTYPE_g", ref val, Marshal.ReadInt32)) { H5E_BADTYPE_g = val; } } return H5E_BADTYPE_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADRANGE { get { if (!H5E_BADRANGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADRANGE_g", ref val, Marshal.ReadInt32)) { H5E_BADRANGE_g = val; } } return H5E_BADRANGE_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADVALUE { get { if (!H5E_BADVALUE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADVALUE_g", ref val, Marshal.ReadInt32)) { H5E_BADVALUE_g = val; } } return H5E_BADVALUE_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOSPACE { get { if (!H5E_NOSPACE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOSPACE_g", ref val, Marshal.ReadInt32)) { H5E_NOSPACE_g = val; } } return H5E_NOSPACE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTALLOC { get { if (!H5E_CANTALLOC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTALLOC_g", ref val, Marshal.ReadInt32)) { H5E_CANTALLOC_g = val; } } return H5E_CANTALLOC_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCOPY { get { if (!H5E_CANTCOPY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOPY_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOPY_g = val; } } return H5E_CANTCOPY_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTFREE { get { if (!H5E_CANTFREE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTFREE_g", ref val, Marshal.ReadInt32)) { H5E_CANTFREE_g = val; } } return H5E_CANTFREE_g.GetValueOrDefault(); } }
        public static hid_t H5E_ALREADYEXISTS { get { if (!H5E_ALREADYEXISTS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ALREADYEXISTS_g", ref val, Marshal.ReadInt32)) { H5E_ALREADYEXISTS_g = val; } } return H5E_ALREADYEXISTS_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTLOCK { get { if (!H5E_CANTLOCK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTLOCK_g", ref val, Marshal.ReadInt32)) { H5E_CANTLOCK_g = val; } } return H5E_CANTLOCK_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTUNLOCK { get { if (!H5E_CANTUNLOCK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUNLOCK_g", ref val, Marshal.ReadInt32)) { H5E_CANTUNLOCK_g = val; } } return H5E_CANTUNLOCK_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTGC { get { if (!H5E_CANTGC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTGC_g", ref val, Marshal.ReadInt32)) { H5E_CANTGC_g = val; } } return H5E_CANTGC_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTGETSIZE { get { if (!H5E_CANTGETSIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTGETSIZE_g", ref val, Marshal.ReadInt32)) { H5E_CANTGETSIZE_g = val; } } return H5E_CANTGETSIZE_g.GetValueOrDefault(); } }
        public static hid_t H5E_OBJOPEN { get { if (!H5E_OBJOPEN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OBJOPEN_g", ref val, Marshal.ReadInt32)) { H5E_OBJOPEN_g = val; } } return H5E_OBJOPEN_g.GetValueOrDefault(); } }
        public static hid_t H5E_FILEEXISTS { get { if (!H5E_FILEEXISTS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FILEEXISTS_g", ref val, Marshal.ReadInt32)) { H5E_FILEEXISTS_g = val; } } return H5E_FILEEXISTS_g.GetValueOrDefault(); } }
        public static hid_t H5E_FILEOPEN { get { if (!H5E_FILEOPEN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FILEOPEN_g", ref val, Marshal.ReadInt32)) { H5E_FILEOPEN_g = val; } } return H5E_FILEOPEN_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCREATE { get { if (!H5E_CANTCREATE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCREATE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCREATE_g = val; } } return H5E_CANTCREATE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTOPENFILE { get { if (!H5E_CANTOPENFILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTOPENFILE_g", ref val, Marshal.ReadInt32)) { H5E_CANTOPENFILE_g = val; } } return H5E_CANTOPENFILE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCLOSEFILE { get { if (!H5E_CANTCLOSEFILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCLOSEFILE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCLOSEFILE_g = val; } } return H5E_CANTCLOSEFILE_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOTHDF5 { get { if (!H5E_NOTHDF5_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTHDF5_g", ref val, Marshal.ReadInt32)) { H5E_NOTHDF5_g = val; } } return H5E_NOTHDF5_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADFILE { get { if (!H5E_BADFILE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADFILE_g", ref val, Marshal.ReadInt32)) { H5E_BADFILE_g = val; } } return H5E_BADFILE_g.GetValueOrDefault(); } }
        public static hid_t H5E_TRUNCATED { get { if (!H5E_TRUNCATED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_TRUNCATED_g", ref val, Marshal.ReadInt32)) { H5E_TRUNCATED_g = val; } } return H5E_TRUNCATED_g.GetValueOrDefault(); } }
        public static hid_t H5E_MOUNT { get { if (!H5E_MOUNT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_MOUNT_g", ref val, Marshal.ReadInt32)) { H5E_MOUNT_g = val; } } return H5E_MOUNT_g.GetValueOrDefault(); } }
        public static hid_t H5E_SEEKERROR { get { if (!H5E_SEEKERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SEEKERROR_g", ref val, Marshal.ReadInt32)) { H5E_SEEKERROR_g = val; } } return H5E_SEEKERROR_g.GetValueOrDefault(); } }
        public static hid_t H5E_READERROR { get { if (!H5E_READERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_READERROR_g", ref val, Marshal.ReadInt32)) { H5E_READERROR_g = val; } } return H5E_READERROR_g.GetValueOrDefault(); } }
        public static hid_t H5E_WRITEERROR { get { if (!H5E_WRITEERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_WRITEERROR_g", ref val, Marshal.ReadInt32)) { H5E_WRITEERROR_g = val; } } return H5E_WRITEERROR_g.GetValueOrDefault(); } }
        public static hid_t H5E_CLOSEERROR { get { if (!H5E_CLOSEERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CLOSEERROR_g", ref val, Marshal.ReadInt32)) { H5E_CLOSEERROR_g = val; } } return H5E_CLOSEERROR_g.GetValueOrDefault(); } }
        public static hid_t H5E_OVERFLOW { get { if (!H5E_OVERFLOW_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OVERFLOW_g", ref val, Marshal.ReadInt32)) { H5E_OVERFLOW_g = val; } } return H5E_OVERFLOW_g.GetValueOrDefault(); } }
        public static hid_t H5E_FCNTL { get { if (!H5E_FCNTL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_FCNTL_g", ref val, Marshal.ReadInt32)) { H5E_FCNTL_g = val; } } return H5E_FCNTL_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTINIT { get { if (!H5E_CANTINIT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINIT_g", ref val, Marshal.ReadInt32)) { H5E_CANTINIT_g = val; } } return H5E_CANTINIT_g.GetValueOrDefault(); } }
        public static hid_t H5E_ALREADYINIT { get { if (!H5E_ALREADYINIT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ALREADYINIT_g", ref val, Marshal.ReadInt32)) { H5E_ALREADYINIT_g = val; } } return H5E_ALREADYINIT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTRELEASE { get { if (!H5E_CANTRELEASE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRELEASE_g", ref val, Marshal.ReadInt32)) { H5E_CANTRELEASE_g = val; } } return H5E_CANTRELEASE_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADATOM { get { if (!H5E_BADATOM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADATOM_g", ref val, Marshal.ReadInt32)) { H5E_BADATOM_g = val; } } return H5E_BADATOM_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADGROUP { get { if (!H5E_BADGROUP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADGROUP_g", ref val, Marshal.ReadInt32)) { H5E_BADGROUP_g = val; } } return H5E_BADGROUP_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTREGISTER { get { if (!H5E_CANTREGISTER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREGISTER_g", ref val, Marshal.ReadInt32)) { H5E_CANTREGISTER_g = val; } } return H5E_CANTREGISTER_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTINC { get { if (!H5E_CANTINC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINC_g", ref val, Marshal.ReadInt32)) { H5E_CANTINC_g = val; } } return H5E_CANTINC_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTDEC { get { if (!H5E_CANTDEC_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDEC_g", ref val, Marshal.ReadInt32)) { H5E_CANTDEC_g = val; } } return H5E_CANTDEC_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOIDS { get { if (!H5E_NOIDS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOIDS_g", ref val, Marshal.ReadInt32)) { H5E_NOIDS_g = val; } } return H5E_NOIDS_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTFLUSH { get { if (!H5E_CANTFLUSH_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTFLUSH_g", ref val, Marshal.ReadInt32)) { H5E_CANTFLUSH_g = val; } } return H5E_CANTFLUSH_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTSERIALIZE { get { if (!H5E_CANTSERIALIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSERIALIZE_g", ref val, Marshal.ReadInt32)) { H5E_CANTSERIALIZE_g = val; } } return H5E_CANTSERIALIZE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTLOAD { get { if (!H5E_CANTLOAD_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTLOAD_g", ref val, Marshal.ReadInt32)) { H5E_CANTLOAD_g = val; } } return H5E_CANTLOAD_g.GetValueOrDefault(); } }
        public static hid_t H5E_PROTECT { get { if (!H5E_PROTECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PROTECT_g", ref val, Marshal.ReadInt32)) { H5E_PROTECT_g = val; } } return H5E_PROTECT_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOTCACHED { get { if (!H5E_NOTCACHED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTCACHED_g", ref val, Marshal.ReadInt32)) { H5E_NOTCACHED_g = val; } } return H5E_NOTCACHED_g.GetValueOrDefault(); } }
        public static hid_t H5E_SYSTEM { get { if (!H5E_SYSTEM_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SYSTEM_g", ref val, Marshal.ReadInt32)) { H5E_SYSTEM_g = val; } } return H5E_SYSTEM_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTINS { get { if (!H5E_CANTINS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINS_g", ref val, Marshal.ReadInt32)) { H5E_CANTINS_g = val; } } return H5E_CANTINS_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTPROTECT { get { if (!H5E_CANTPROTECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTPROTECT_g", ref val, Marshal.ReadInt32)) { H5E_CANTPROTECT_g = val; } } return H5E_CANTPROTECT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTUNPROTECT { get { if (!H5E_CANTUNPROTECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUNPROTECT_g", ref val, Marshal.ReadInt32)) { H5E_CANTUNPROTECT_g = val; } } return H5E_CANTUNPROTECT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTPIN { get { if (!H5E_CANTPIN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTPIN_g", ref val, Marshal.ReadInt32)) { H5E_CANTPIN_g = val; } } return H5E_CANTPIN_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTUNPIN { get { if (!H5E_CANTUNPIN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUNPIN_g", ref val, Marshal.ReadInt32)) { H5E_CANTUNPIN_g = val; } } return H5E_CANTUNPIN_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTMARKDIRTY { get { if (!H5E_CANTMARKDIRTY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMARKDIRTY_g", ref val, Marshal.ReadInt32)) { H5E_CANTMARKDIRTY_g = val; } } return H5E_CANTMARKDIRTY_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTDIRTY { get { if (!H5E_CANTDIRTY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDIRTY_g", ref val, Marshal.ReadInt32)) { H5E_CANTDIRTY_g = val; } } return H5E_CANTDIRTY_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTEXPUNGE { get { if (!H5E_CANTEXPUNGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTEXPUNGE_g", ref val, Marshal.ReadInt32)) { H5E_CANTEXPUNGE_g = val; } } return H5E_CANTEXPUNGE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTRESIZE { get { if (!H5E_CANTRESIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRESIZE_g", ref val, Marshal.ReadInt32)) { H5E_CANTRESIZE_g = val; } } return H5E_CANTRESIZE_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOTFOUND { get { if (!H5E_NOTFOUND_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTFOUND_g", ref val, Marshal.ReadInt32)) { H5E_NOTFOUND_g = val; } } return H5E_NOTFOUND_g.GetValueOrDefault(); } }
        public static hid_t H5E_EXISTS { get { if (!H5E_EXISTS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_EXISTS_g", ref val, Marshal.ReadInt32)) { H5E_EXISTS_g = val; } } return H5E_EXISTS_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTENCODE { get { if (!H5E_CANTENCODE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTENCODE_g", ref val, Marshal.ReadInt32)) { H5E_CANTENCODE_g = val; } } return H5E_CANTENCODE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTDECODE { get { if (!H5E_CANTDECODE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDECODE_g", ref val, Marshal.ReadInt32)) { H5E_CANTDECODE_g = val; } } return H5E_CANTDECODE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTSPLIT { get { if (!H5E_CANTSPLIT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSPLIT_g", ref val, Marshal.ReadInt32)) { H5E_CANTSPLIT_g = val; } } return H5E_CANTSPLIT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTREDISTRIBUTE { get { if (!H5E_CANTREDISTRIBUTE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREDISTRIBUTE_g", ref val, Marshal.ReadInt32)) { H5E_CANTREDISTRIBUTE_g = val; } } return H5E_CANTREDISTRIBUTE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTSWAP { get { if (!H5E_CANTSWAP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSWAP_g", ref val, Marshal.ReadInt32)) { H5E_CANTSWAP_g = val; } } return H5E_CANTSWAP_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTINSERT { get { if (!H5E_CANTINSERT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTINSERT_g", ref val, Marshal.ReadInt32)) { H5E_CANTINSERT_g = val; } } return H5E_CANTINSERT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTLIST { get { if (!H5E_CANTLIST_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTLIST_g", ref val, Marshal.ReadInt32)) { H5E_CANTLIST_g = val; } } return H5E_CANTLIST_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTMODIFY { get { if (!H5E_CANTMODIFY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMODIFY_g", ref val, Marshal.ReadInt32)) { H5E_CANTMODIFY_g = val; } } return H5E_CANTMODIFY_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTREMOVE { get { if (!H5E_CANTREMOVE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREMOVE_g", ref val, Marshal.ReadInt32)) { H5E_CANTREMOVE_g = val; } } return H5E_CANTREMOVE_g.GetValueOrDefault(); } }
        public static hid_t H5E_LINKCOUNT { get { if (!H5E_LINKCOUNT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_LINKCOUNT_g", ref val, Marshal.ReadInt32)) { H5E_LINKCOUNT_g = val; } } return H5E_LINKCOUNT_g.GetValueOrDefault(); } }
        public static hid_t H5E_VERSION { get { if (!H5E_VERSION_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_VERSION_g", ref val, Marshal.ReadInt32)) { H5E_VERSION_g = val; } } return H5E_VERSION_g.GetValueOrDefault(); } }
        public static hid_t H5E_ALIGNMENT { get { if (!H5E_ALIGNMENT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_ALIGNMENT_g", ref val, Marshal.ReadInt32)) { H5E_ALIGNMENT_g = val; } } return H5E_ALIGNMENT_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADMESG { get { if (!H5E_BADMESG_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADMESG_g", ref val, Marshal.ReadInt32)) { H5E_BADMESG_g = val; } } return H5E_BADMESG_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTDELETE { get { if (!H5E_CANTDELETE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTDELETE_g", ref val, Marshal.ReadInt32)) { H5E_CANTDELETE_g = val; } } return H5E_CANTDELETE_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADITER { get { if (!H5E_BADITER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADITER_g", ref val, Marshal.ReadInt32)) { H5E_BADITER_g = val; } } return H5E_BADITER_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTPACK { get { if (!H5E_CANTPACK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTPACK_g", ref val, Marshal.ReadInt32)) { H5E_CANTPACK_g = val; } } return H5E_CANTPACK_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTRESET { get { if (!H5E_CANTRESET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRESET_g", ref val, Marshal.ReadInt32)) { H5E_CANTRESET_g = val; } } return H5E_CANTRESET_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTRENAME { get { if (!H5E_CANTRENAME_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRENAME_g", ref val, Marshal.ReadInt32)) { H5E_CANTRENAME_g = val; } } return H5E_CANTRENAME_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTOPENOBJ { get { if (!H5E_CANTOPENOBJ_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTOPENOBJ_g", ref val, Marshal.ReadInt32)) { H5E_CANTOPENOBJ_g = val; } } return H5E_CANTOPENOBJ_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCLOSEOBJ { get { if (!H5E_CANTCLOSEOBJ_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCLOSEOBJ_g", ref val, Marshal.ReadInt32)) { H5E_CANTCLOSEOBJ_g = val; } } return H5E_CANTCLOSEOBJ_g.GetValueOrDefault(); } }
        public static hid_t H5E_COMPLEN { get { if (!H5E_COMPLEN_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_COMPLEN_g", ref val, Marshal.ReadInt32)) { H5E_COMPLEN_g = val; } } return H5E_COMPLEN_g.GetValueOrDefault(); } }
        public static hid_t H5E_PATH { get { if (!H5E_PATH_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_PATH_g", ref val, Marshal.ReadInt32)) { H5E_PATH_g = val; } } return H5E_PATH_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCONVERT { get { if (!H5E_CANTCONVERT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCONVERT_g", ref val, Marshal.ReadInt32)) { H5E_CANTCONVERT_g = val; } } return H5E_CANTCONVERT_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADSIZE { get { if (!H5E_BADSIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADSIZE_g", ref val, Marshal.ReadInt32)) { H5E_BADSIZE_g = val; } } return H5E_BADSIZE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCLIP { get { if (!H5E_CANTCLIP_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCLIP_g", ref val, Marshal.ReadInt32)) { H5E_CANTCLIP_g = val; } } return H5E_CANTCLIP_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCOUNT { get { if (!H5E_CANTCOUNT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOUNT_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOUNT_g = val; } } return H5E_CANTCOUNT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTSELECT { get { if (!H5E_CANTSELECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSELECT_g", ref val, Marshal.ReadInt32)) { H5E_CANTSELECT_g = val; } } return H5E_CANTSELECT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTNEXT { get { if (!H5E_CANTNEXT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTNEXT_g", ref val, Marshal.ReadInt32)) { H5E_CANTNEXT_g = val; } } return H5E_CANTNEXT_g.GetValueOrDefault(); } }
        public static hid_t H5E_BADSELECT { get { if (!H5E_BADSELECT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_BADSELECT_g", ref val, Marshal.ReadInt32)) { H5E_BADSELECT_g = val; } } return H5E_BADSELECT_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCOMPARE { get { if (!H5E_CANTCOMPARE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOMPARE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOMPARE_g = val; } } return H5E_CANTCOMPARE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTGET { get { if (!H5E_CANTGET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTGET_g", ref val, Marshal.ReadInt32)) { H5E_CANTGET_g = val; } } return H5E_CANTGET_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTSET { get { if (!H5E_CANTSET_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSET_g", ref val, Marshal.ReadInt32)) { H5E_CANTSET_g = val; } } return H5E_CANTSET_g.GetValueOrDefault(); } }
        public static hid_t H5E_DUPCLASS { get { if (!H5E_DUPCLASS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_DUPCLASS_g", ref val, Marshal.ReadInt32)) { H5E_DUPCLASS_g = val; } } return H5E_DUPCLASS_g.GetValueOrDefault(); } }
        public static hid_t H5E_SETDISALLOWED { get { if (!H5E_SETDISALLOWED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SETDISALLOWED_g", ref val, Marshal.ReadInt32)) { H5E_SETDISALLOWED_g = val; } } return H5E_SETDISALLOWED_g.GetValueOrDefault(); } }
        public static hid_t H5E_TRAVERSE { get { if (!H5E_TRAVERSE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_TRAVERSE_g", ref val, Marshal.ReadInt32)) { H5E_TRAVERSE_g = val; } } return H5E_TRAVERSE_g.GetValueOrDefault(); } }
        public static hid_t H5E_NLINKS { get { if (!H5E_NLINKS_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NLINKS_g", ref val, Marshal.ReadInt32)) { H5E_NLINKS_g = val; } } return H5E_NLINKS_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOTREGISTERED { get { if (!H5E_NOTREGISTERED_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOTREGISTERED_g", ref val, Marshal.ReadInt32)) { H5E_NOTREGISTERED_g = val; } } return H5E_NOTREGISTERED_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTMOVE { get { if (!H5E_CANTMOVE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMOVE_g", ref val, Marshal.ReadInt32)) { H5E_CANTMOVE_g = val; } } return H5E_CANTMOVE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTSORT { get { if (!H5E_CANTSORT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSORT_g", ref val, Marshal.ReadInt32)) { H5E_CANTSORT_g = val; } } return H5E_CANTSORT_g.GetValueOrDefault(); } }
        public static hid_t H5E_MPI { get { if (!H5E_MPI_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_MPI_g", ref val, Marshal.ReadInt32)) { H5E_MPI_g = val; } } return H5E_MPI_g.GetValueOrDefault(); } }
        public static hid_t H5E_MPIERRSTR { get { if (!H5E_MPIERRSTR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_MPIERRSTR_g", ref val, Marshal.ReadInt32)) { H5E_MPIERRSTR_g = val; } } return H5E_MPIERRSTR_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTRECV { get { if (!H5E_CANTRECV_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRECV_g", ref val, Marshal.ReadInt32)) { H5E_CANTRECV_g = val; } } return H5E_CANTRECV_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTRESTORE { get { if (!H5E_CANTRESTORE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTRESTORE_g", ref val, Marshal.ReadInt32)) { H5E_CANTRESTORE_g = val; } } return H5E_CANTRESTORE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTCOMPUTE { get { if (!H5E_CANTCOMPUTE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTCOMPUTE_g", ref val, Marshal.ReadInt32)) { H5E_CANTCOMPUTE_g = val; } } return H5E_CANTCOMPUTE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTEXTEND { get { if (!H5E_CANTEXTEND_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTEXTEND_g", ref val, Marshal.ReadInt32)) { H5E_CANTEXTEND_g = val; } } return H5E_CANTEXTEND_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTATTACH { get { if (!H5E_CANTATTACH_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTATTACH_g", ref val, Marshal.ReadInt32)) { H5E_CANTATTACH_g = val; } } return H5E_CANTATTACH_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTUPDATE { get { if (!H5E_CANTUPDATE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTUPDATE_g", ref val, Marshal.ReadInt32)) { H5E_CANTUPDATE_g = val; } } return H5E_CANTUPDATE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTOPERATE { get { if (!H5E_CANTOPERATE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTOPERATE_g", ref val, Marshal.ReadInt32)) { H5E_CANTOPERATE_g = val; } } return H5E_CANTOPERATE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTMERGE { get { if (!H5E_CANTMERGE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTMERGE_g", ref val, Marshal.ReadInt32)) { H5E_CANTMERGE_g = val; } } return H5E_CANTMERGE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTREVIVE { get { if (!H5E_CANTREVIVE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTREVIVE_g", ref val, Marshal.ReadInt32)) { H5E_CANTREVIVE_g = val; } } return H5E_CANTREVIVE_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTSHRINK { get { if (!H5E_CANTSHRINK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTSHRINK_g", ref val, Marshal.ReadInt32)) { H5E_CANTSHRINK_g = val; } } return H5E_CANTSHRINK_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOFILTER { get { if (!H5E_NOFILTER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOFILTER_g", ref val, Marshal.ReadInt32)) { H5E_NOFILTER_g = val; } } return H5E_NOFILTER_g.GetValueOrDefault(); } }
        public static hid_t H5E_CALLBACK { get { if (!H5E_CALLBACK_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CALLBACK_g", ref val, Marshal.ReadInt32)) { H5E_CALLBACK_g = val; } } return H5E_CALLBACK_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANAPPLY { get { if (!H5E_CANAPPLY_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANAPPLY_g", ref val, Marshal.ReadInt32)) { H5E_CANAPPLY_g = val; } } return H5E_CANAPPLY_g.GetValueOrDefault(); } }
        public static hid_t H5E_SETLOCAL { get { if (!H5E_SETLOCAL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SETLOCAL_g", ref val, Marshal.ReadInt32)) { H5E_SETLOCAL_g = val; } } return H5E_SETLOCAL_g.GetValueOrDefault(); } }
        public static hid_t H5E_NOENCODER { get { if (!H5E_NOENCODER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NOENCODER_g", ref val, Marshal.ReadInt32)) { H5E_NOENCODER_g = val; } } return H5E_NOENCODER_g.GetValueOrDefault(); } }
        public static hid_t H5E_CANTFILTER { get { if (!H5E_CANTFILTER_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_CANTFILTER_g", ref val, Marshal.ReadInt32)) { H5E_CANTFILTER_g = val; } } return H5E_CANTFILTER_g.GetValueOrDefault(); } }
        public static hid_t H5E_SYSERRSTR { get { if (!H5E_SYSERRSTR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_SYSERRSTR_g", ref val, Marshal.ReadInt32)) { H5E_SYSERRSTR_g = val; } } return H5E_SYSERRSTR_g.GetValueOrDefault(); } }
        public static hid_t H5E_OPENERROR { get { if (!H5E_OPENERROR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_OPENERROR_g", ref val, Marshal.ReadInt32)) { H5E_OPENERROR_g = val; } } return H5E_OPENERROR_g.GetValueOrDefault(); } }
        public static hid_t H5E_NONE_MINOR { get { if (!H5E_NONE_MINOR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5E_NONE_MINOR_g", ref val, Marshal.ReadInt32)) { H5E_NONE_MINOR_g = val; } } return H5E_NONE_MINOR_g.GetValueOrDefault(); } }

        #endregion

        /// <summary>
        /// Value for the default error stack
        /// </summary>
        public const hid_t DEFAULT = 0;

        /// <summary>
        /// Different kinds of error information
        /// </summary>
        public enum type_t
        {
            MAJOR,
            MINOR
        }

        /// <summary>
        /// Information about an error; element of error stack
        /// </summary>

        public struct error_t
        {
            /// <summary>
            /// class ID
            /// </summary>
            hid_t cls_id;

            /// <summary>
            /// major error ID
            /// </summary>
            hid_t maj_num;

            /// <summary>
            /// minor error ID
            /// </summary>
            hid_t min_num;

            /// <summary>
            /// line in file where error occurs
            /// </summary>
            uint line;

            /// <summary>
            /// function in which error occurred
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            string func_name;

            /// <summary>
            /// file in which error occurred
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            string file_name;

            /// <summary>
            /// optional supplied description
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            string desc;
        };

        /// <summary>
        /// Error stack traversal callback function pointers
        /// </summary>

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t auto_t
        (
        hid_t estack,
        IntPtr client_data
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t walk_t
        (
        uint n,
        ref error_t err_desc,
        IntPtr client_data
        );

        /// <summary>
        /// Error stack traversal direction
        /// </summary>
        public enum direction_t
        {
            /// <summary>
            /// begin deep, end at API function [value = 0]
            /// </summary>
            H5E_WALK_UPWARD = 0,
            /// <summary>
            /// begin at API function, end deep [value = 1]
            /// </summary>
            H5E_WALK_DOWNWARD = 1
        }

        /// <summary>
        /// Determines type of error stack.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-AutoIsV2
        /// </summary>
        /// <param name="estack_id">The error stack identifier</param>
        /// <param name="is_stack">A flag indicating which error stack typedef
        /// the specified error stack conforms to.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eauto_is_v2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t auto_is_v2
            (hid_t estack_id, ref uint is_stack);

        /// <summary>
        /// Clears the specified error stack or the error stack for the current
        /// thread.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-Clear2
        /// </summary>
        /// <param name="estack_id">Error stack identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eclear2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t clear(hid_t estack_id);

        /// <summary>
        /// Closes an error message identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-CloseMsg
        /// </summary>
        /// <param name="msg_id">Error message identifier.</param>
        /// <returns>Returns a non-negative value on success; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eclose_msg",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t close_msg(hid_t msg_id);

        /// <summary>
        /// Closes object handle for error stack.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-CloseStack
        /// </summary>
        /// <param name="estack_id">Error stack identifier.</param>
        /// <returns>Returns a non-negative value on success; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eclose_stack",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t close_stack(hid_t estack_id);

        /// <summary>
        /// Add major error message to an error class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-CreateMsg
        /// </summary>
        /// <param name="cls">Error class identifier.</param>
        /// <param name="msg_type">The type of the error message.</param>
        /// <param name="msg">Major error message.</param>
        /// <returns>Returns a message identifier on success; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ecreate_msg",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create_msg
            (hid_t cls, type_t msg_type,
            [MarshalAs(UnmanagedType.LPStr)]string msg);

        /// <summary>
        /// Creates a new empty error stack.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-CreateStack
        /// </summary>
        /// <returns>Returns an error stack identifier on success; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ecreate_stack",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create_stack();

        /// <summary>
        /// Returns the settings for the automatic error stack traversal
        /// function and its data.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-GetAuto2
        /// </summary>
        /// <param name="estack_id">Error stack identifier.
        /// <code>H5E_DEFAULT</code> indicates the current stack.</param>
        /// <param name="func">The function currently set to be called upon an
        /// error condition.</param>
        /// <param name="client_data">Data currently set to be passed to the
        /// error function.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eget_auto2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_auto
            (hid_t estack_id, auto_t func /*out*/,
            ref IntPtr client_data /*out*/);

        /// <summary>
        /// Retrieves error class name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-GetClassName
        /// </summary>
        /// <param name="class_id">Error class identifier.</param>
        /// <param name="name">The name of the class to be queried.</param>
        /// <param name="size">The length of class name to be returned by
        /// this function.</param>
        /// <returns>Returns non-negative value as on success; otherwise
        /// returns negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eget_class_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_class_name(
            hid_t class_id, StringBuilder name, size_t size);

        /// <summary>
        /// Returns copy of current error stack.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-GetCurrentStack
        /// </summary>
        /// <returns>Returns an error stack identifier on success; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eget_current_stack",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_current_stack();

        /// <summary>
        /// Retrieves an error message.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-GetMsg
        /// </summary>
        /// <param name="msg_id">Idenfier for error message to be queried.</param>
        /// <param name="msg_type">The type of the error message.</param>
        /// <param name="mesg">Error message buffer.</param>
        /// <param name="size">The length of error message to be returned by
        /// this function.</param>
        /// <returns>Returns the size of the error message in bytes on success;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eget_msg",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_msg(
            hid_t msg_id, ref type_t msg_type, StringBuilder msg, size_t size);

        /// <summary>
        /// Retrieves the number of error messages in an error stack.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-GetNum
        /// </summary>
        /// <param name="estack_id">Error stack identifier.</param>
        /// <returns>Returns a non-negative value on success; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eget_num",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern ssize_t get_num(hid_t estack_id);

        /// <summary>
        /// Deletes specified number of error messages from the error stack.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-Pop
        /// </summary>
        /// <param name="estack_id">Error stack identifier.</param>
        /// <param name="count">The number of error messages to be deleted from
        /// the top of error stack.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Epop",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t pop(hid_t estack_id, size_t count);

        /// <summary>
        /// Prints the specified error stack in a default manner.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-Print2
        /// </summary>
        /// <param name="estack_id">Identifier of the error stack to be printed.</param>
        /// <param name="stream">File pointer, or stderr if NULL.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eprint2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t print
            (hid_t estack_id, /* FILE * */ IntPtr stream);

        /* herr_t H5Epush2( hid_t estack_id, const char *file, const char *func, unsigned line, hid_t class_id, hid_t major_id, hid_t minor_id, const char *msg, ...) */

        /// <summary>
        /// Registers a client library or application program to the HDF5 error
        /// API.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-RegisterClass
        /// </summary>
        /// <param name="cls_name">Name of the error class.</param>
        /// <param name="lib_name">Name of the client library or application to
        /// which the error class belongs.</param>
        /// <param name="version">Version of the client library or application
        /// to which the error class belongs. A NULL can be passed in.</param>
        /// <returns>Returns a class identifier on success; otherwise returns a
        /// negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eregister_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t register_class
            ([MarshalAs(UnmanagedType.LPStr)]string cls_name,
            [MarshalAs(UnmanagedType.LPStr)]string lib_name,
            [MarshalAs(UnmanagedType.LPStr)]string version);

        /// <summary>
        /// Turns automatic error printing on or off.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-SetAuto2
        /// </summary>
        /// <param name="estack_id">Error stack identifier.</param>
        /// <param name="func">Function to be called upon an error condition.</param>
        /// <param name="client_data">Data passed to the error function.</param>
        /// <returns>Returns a non-negative value on success; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eset_auto2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_auto
            (hid_t estack_id, auto_t func, IntPtr client_data);

        /// <summary>
        /// Replaces the current error stack.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-SetCurrentStack
        /// </summary>
        /// <param name="estack_id">Error stack identifier.</param>
        /// <returns>Returns a non-negative value on success; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eset_current_stack",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_current_stack(hid_t estack_id);

        /// <summary>
        /// Removes an error class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-UnregisterClass
        /// </summary>
        /// <param name="class_id">Error class identifier.</param>
        /// <returns>Returns a non-negative value on success; otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Eunregister_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t unregister_class(hid_t class_id);

        /// <summary>
        /// Walks the specified error stack, calling the specified function.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5E.html#Error-Walk2
        /// </summary>
        /// <param name="estack_id">Error stack identifier.</param>
        /// <param name="direction">Direction in which the error stack is to be
        /// walked.</param>
        /// <param name="func">Function to be called for each error encountered.</param>
        /// <param name="client_data">Data to be passed with
        /// <paramref name="func"/>.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ewalk2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t walk
            (hid_t estack_id, direction_t direction, walk_t func,
            IntPtr client_data);
    }
}