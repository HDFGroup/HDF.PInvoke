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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using herr_t = System.Int32;
using hsize_t = System.UInt64;

#if HDF5_VER1_10

using hid_t = System.Int64;

namespace UnitTests
{
    public partial class H5SWMRTest
    {
        [TestMethod]
        public void H5Pset_object_flush_cbTestSWMR1()
        {
            hid_t fapl = H5P.create(H5P.FILE_ACCESS);
            Assert.IsTrue(fapl >= 0);

            H5F.flush_cb_t cb = flush_func;

            Assert.IsTrue(
                H5P.set_object_flush_cb(fapl, cb, IntPtr.Zero) >= 0);
            
            Assert.IsTrue(H5P.close(fapl) >= 0);
        }

        [TestMethod]
        public void H5Pset_object_flush_cbTestSWMR2()
        {
            hid_t fapl = H5P.create(H5P.FILE_ACCESS);
            Assert.IsTrue(fapl >= 0);

            H5F.flush_cb_t cb = flush_func;

            Assert.IsTrue(
                H5P.set_object_flush_cb(fapl, cb, IntPtr.Zero) >= 0);

            H5F.flush_cb_t check_cb = null;

            IntPtr check_ptr = new IntPtr(4711);

            Assert.IsTrue(
                H5P.get_object_flush_cb(fapl, ref check_cb,
                ref check_ptr) >= 0);

            Assert.IsTrue(check_cb == cb);

            Assert.IsTrue(check_ptr == IntPtr.Zero);

            Assert.IsTrue(H5P.close(fapl) >= 0);
        }
    }
}

#endif