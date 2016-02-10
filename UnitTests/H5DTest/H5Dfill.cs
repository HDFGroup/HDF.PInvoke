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
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using herr_t = System.Int32;
using hsize_t = System.UInt64;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5DTest
    {
        [TestMethod]
        public void H5DfillTest1()
        {
            hsize_t[] dims = { 10 };
            hid_t space = H5S.create_simple(1, dims , null);
            Assert.IsTrue(H5S.select_all(space) >= 0);

            double[] v = new double[10];
            double fill = 1.0;

            GCHandle v_hnd = GCHandle.Alloc(v, GCHandleType.Pinned);
            GCHandle fill_hnd = GCHandle.Alloc(fill, GCHandleType.Pinned);
            Assert.IsTrue(
                H5D.fill(fill_hnd.AddrOfPinnedObject(), H5T.NATIVE_DOUBLE,
                v_hnd.AddrOfPinnedObject(), H5T.NATIVE_DOUBLE, space) >= 0);
            fill_hnd.Free();
            v_hnd.Free();

            for (int i = 0; i < v.Length; ++i)
            {
                Assert.IsTrue(v[i] == 1.0);
            }

            Assert.IsTrue(H5S.close(space) >= 0);
        }

        [TestMethod]
        public void H5DfillTest2()
        {
            hsize_t[] dims = { 5 };
            hid_t space = H5S.create_simple(1, dims, null);
            Assert.IsTrue(H5S.select_all(space) >= 0);

            double[] v = new double[5] { 0.0, 1.0, 2.0, 3.0, 4.0 };
            GCHandle v_hnd = GCHandle.Alloc(v, GCHandleType.Pinned);
            Assert.IsTrue(
                H5D.fill(IntPtr.Zero, H5T.NATIVE_DOUBLE,
                v_hnd.AddrOfPinnedObject(), H5T.NATIVE_DOUBLE, space) >= 0);
            v_hnd.Free();

            for (int i = 0; i < v.Length; ++i)
            {
                Assert.IsTrue(v[i] == 0.0);
            }

            Assert.IsTrue(H5S.close(space) >= 0);
        }
    }
}