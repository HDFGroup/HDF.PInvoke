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
using hsize_t = System.UInt64;
using size_t = System.IntPtr;

namespace HDF.PInvoke
{
    public unsafe sealed class H5FD
    {
        static H5FD() { H5.open(); }

        /// <summary>
        /// Define enum for the source of file image callbacks
        /// </summary>
        public enum file_image_op_t
        {
            NO_OP,
            PROPERTY_LIST_SET,
            PROPERTY_LIST_COPY,
            PROPERTY_LIST_GET,
            PROPERTY_LIST_CLOSE,
            FILE_OPEN,
            FILE_RESIZE,
            FILE_CLOSE
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr image_malloc_t
        (size_t size, file_image_op_t file_image_op, IntPtr udata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr image_memcpy_t
        (IntPtr dest, IntPtr src, size_t size, file_image_op_t file_image_op,
        IntPtr udata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr image_realloc_t
        (IntPtr ptr, size_t size, file_image_op_t file_image_op, IntPtr udata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t image_free_t
        (IntPtr ptr, file_image_op_t file_image_op, IntPtr udata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr udata_copy_t(IntPtr udata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t udata_free_t(IntPtr udata);

        /// <summary>
        /// Define structure to hold file image callbacks
        /// </summary>
        public struct file_image_callbacks_t
        {
            public image_malloc_t image_malloc;

            public image_memcpy_t image_memcpy;

            public image_realloc_t image_realloc;

            public image_free_t image_free;

            public udata_copy_t udata_copy;

            public udata_free_t udata_free;

            public IntPtr udata;
        }
    }
}
