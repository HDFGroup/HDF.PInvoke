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
using hsize_t = System.UInt64;

namespace HDF.PInvoke
{
    public unsafe sealed class H5Z
    {
        /// <summary>
        /// Filter IDs
        /// </summary>
        public enum filter_t
        {
            /// <summary>
            /// no filter [value = -1]
            /// </summary>
            FILTER_ERROR = -1,
            /// <summary>
            /// reserved indefinitely [value = 0]
            /// </summary>
            FILTER_NONE = 0,
            /// <summary>
            /// deflation like gzip ]value = 1]
            /// </summary>
            FILTER_DEFLATE = 1,
            /// <summary>
            /// shuffle the data [value = 2]
            /// </summary>
            FILTER_SHUFFLE = 2,
            /// <summary>
            /// fletcher32 checksum of EDC [value = 3]
            /// </summary>
            FILTER_FLETCHER32 = 3,
            /// <summary>
            /// szip compression [value = 4]
            /// </summary>
            FILTER_SZIP = 4,
            /// <summary>
            /// nbit compression [value = 5]
            /// </summary>
            FILTER_NBIT = 5,
            /// <summary>
            /// scale+offset compression [value = 6]
            /// </summary>
            FILTER_SCALEOFFSET = 6,
            /// <summary>
            /// filter ids below this value are reserved for library use [value = 256]
            /// </summary>
            FILTER_RESERVED = 256,
            /// <summary>
            /// maximum filter id [value = 65535]
            /// </summary>
            FILTER_MAX = 65535
        }

        /// <summary>
        /// Values to decide if EDC is enabled for reading data
        /// </summary>
        public enum EDC_t
        {
            /// <summary>
            /// error value
            /// </summary>
            H5Z_ERROR_EDC = -1,
            H5Z_DISABLE_EDC = 0,
            H5Z_ENABLE_EDC = 1,
            H5Z_NO_EDC = 2
        }
    }
}
