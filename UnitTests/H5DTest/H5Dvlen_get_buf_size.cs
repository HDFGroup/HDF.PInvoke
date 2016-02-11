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
        public void H5Dvlen_get_buf_sizeTest1()
        {
            // write a VLEN dataset

            hid_t vlen = H5T.vlen_create(H5T.NATIVE_INT);
            Assert.IsTrue(vlen >= 0);

            hsize_t[] dims = { 10 };
            hid_t space = H5S.create_simple(1, dims, null);
            Assert.IsTrue(space >= 0);

            
            hid_t dset = H5D.create(m_v0_test_file, "vlen", vlen, space);
            Assert.IsTrue(space >= 0);
            hid_t dset1 = H5D.create(m_v2_test_file, "vlen", vlen, space);
            Assert.IsTrue(space >= 0);

            H5T.hvl_t[] wdata = new H5T.hvl_t[dims[0]];
            GCHandle[] whndl = new GCHandle[wdata.Length];
            int[][] jagged = new int[dims[0]][];
            for (int i = 0; i < wdata.Length; ++i)
            {
                jagged[i] = new int[i + 1];
                whndl[i] = GCHandle.Alloc(jagged[i], GCHandleType.Pinned);
                wdata[i].len = new IntPtr(i + 1);
                wdata[i].p = whndl[i].AddrOfPinnedObject();
            }

            GCHandle wdata_hndl = GCHandle.Alloc(wdata, GCHandleType.Pinned);
            Assert.IsTrue(H5D.write(dset, vlen, H5S.ALL, H5S.ALL, H5P.DEFAULT,
                wdata_hndl.AddrOfPinnedObject()) >= 0);
            Assert.IsTrue(H5D.write(dset1, vlen, H5S.ALL, H5S.ALL, H5P.DEFAULT,
                wdata_hndl.AddrOfPinnedObject()) >= 0);
            wdata_hndl.Free();

            for (int i = 0; i < wdata.Length; ++i)
            {
                whndl[i].Free();
            }

            hsize_t size = 0;
            Assert.IsTrue(H5S.select_all(space) >= 0);
            Assert.IsTrue(
                H5D.vlen_get_buf_size(dset, vlen, space, ref size) >= 0);
            Assert.IsTrue(size == 220);  // (1 + 2 + ... + 10) x sizeof(int)

            Assert.IsTrue(
                H5D.vlen_get_buf_size(dset1, vlen, space, ref size) >= 0);
            Assert.IsTrue(size == 220);  // (1 + 2 + ... + 10) x sizeof(int)

            Assert.IsTrue(H5D.close(dset1) >= 0);
            Assert.IsTrue(H5D.close(dset) >= 0);
            Assert.IsTrue(H5T.close(vlen) >= 0);
            Assert.IsTrue(H5S.close(space) >= 0);
        }
    }
}