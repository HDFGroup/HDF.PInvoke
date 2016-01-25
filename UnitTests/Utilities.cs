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
using System.IO;

using HDF.PInvoke;

using hid_t = System.Int32;

namespace UnitTests
{
    class Utilities
    {
        /// <summary>
        /// Create a temporary HDF5 file and return a file handle.
        /// </summary>
        public static hid_t H5TempFile()
        {
            hid_t fapl = H5P.create(H5P.CLS_FILE_ACCESS);
            if (fapl < 0)
            {
                throw new ApplicationException("H5P.create failed.");
            }
            if (H5P.set_libver_bounds(fapl, H5F.libver_t.LIBVER_LATEST) < 0)
            {
                throw new ApplicationException("H5P.set_libver_bounds failed.");
            }
            string fname = Path.GetTempFileName();
            hid_t file = H5F.create(fname, H5F.ACC_TRUNC, H5P.DEFAULT, fapl);
            if (file < 0)
            {
                throw new ApplicationException("H5F.create failed.");
            }
            if (H5P.close(fapl) < 0)
            {
                throw new ApplicationException("H5P.close failed.");
            }
            return file;
        }

        /// <summary>
        /// Create a temporary HDF5 file and return a file handle.
        /// </summary>
        public static hid_t H5TempFile(ref string fileName)
        {
            hid_t fapl = H5P.create(H5P.CLS_FILE_ACCESS);
            if (fapl < 0)
            {
                throw new ApplicationException("H5P.create failed.");
            }
            if (H5P.set_libver_bounds(fapl, H5F.libver_t.LIBVER_LATEST) < 0)
            {
                throw new ApplicationException("H5P.set_libver_bounds failed.");
            }
            fileName = Path.GetTempFileName();
            hid_t file = H5F.create(fileName, H5F.ACC_TRUNC, H5P.DEFAULT, fapl);
            if (file < 0)
            {
                throw new ApplicationException("H5F.create failed.");
            }
            if (H5P.close(fapl) < 0)
            {
                throw new ApplicationException("H5P.close failed.");
            }
            return file;
        }

        /// <summary>
        /// Return a random INVALID handle.
        /// </summary>
        public static hid_t RandomInvalidHandle()
        {
            Random r = new Random();
            return r.Next(System.Int32.MinValue, -1);
        }
    }
}
