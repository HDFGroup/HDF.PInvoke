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

namespace HDF.PInvoke
{
    public unsafe sealed class H5T
    {
        /// <summary>
        /// Character set to use for text strings.
        /// </summary>
        public enum cset_t
        {
            /// <summary>
            /// error [value = -1].
            /// </summary>
            H5T_CSET_ERROR = -1,
            /// <summary>
            /// US ASCII [value = 0].
            /// </summary>
            H5T_CSET_ASCII = 0,
            /// <summary>
            /// UTF-8 Unicode encoding [value = 1].
            /// </summary>
            H5T_CSET_UTF8 = 1,
            // reserved for later use [values = 2-15]
            H5T_CSET_RESERVED_2 = 2,
            H5T_CSET_RESERVED_3 = 3,
            H5T_CSET_RESERVED_4 = 4,
            H5T_CSET_RESERVED_5 = 5,
            H5T_CSET_RESERVED_6 = 6,
            H5T_CSET_RESERVED_7 = 7,
            H5T_CSET_RESERVED_8 = 8,
            H5T_CSET_RESERVED_9 = 9,
            H5T_CSET_RESERVED_10 = 10,
            H5T_CSET_RESERVED_11 = 11,
            H5T_CSET_RESERVED_12 = 12,
            H5T_CSET_RESERVED_13 = 13,
            H5T_CSET_RESERVED_14 = 14,
            H5T_CSET_RESERVED_15 = 15
        }

        /// <summary>
        /// Number of character sets actually defined 
        /// </summary>
        public const cset_t H5T_NCSET = cset_t.H5T_CSET_RESERVED_2;
    }
}
