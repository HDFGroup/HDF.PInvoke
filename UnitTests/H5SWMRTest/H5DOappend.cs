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
        public herr_t DOappend_func
            (hid_t dataset_id, hsize_t[] cur_dims, IntPtr op_data)
        {
            Assert.IsTrue(op_data.ToInt32() == 99);
            return 0;
        }

        [TestMethod]
        public void H5DOappendTestSWMR1()
        {
            hsize_t[] dims = {6, 0};
            hsize_t[] maxdims = {6, H5S.UNLIMITED};
            hsize_t[] chunk_dims = {2, 5};
            int[] cbuf = new int [6];

            hid_t dsp = H5S.create_simple(2, dims, maxdims);
            Assert.IsTrue(dsp >= 0);

            hid_t dcpl = H5P.create(H5P.DATASET_CREATE);
            Assert.IsTrue(dcpl >= 0);
            Assert.IsTrue(H5P.set_chunk(dcpl, 2, chunk_dims) >= 0);

            hsize_t[] boundary = { 0, 1 };

            hid_t dapl = H5P.create(H5P.DATASET_ACCESS);
            Assert.IsTrue(dapl >= 0);
            H5D.append_cb_t cb = DOappend_func;
            Assert.IsTrue(
                H5P.set_append_flush(dapl, 2, boundary, cb, new IntPtr(99)) >= 0);

            hid_t dst = H5D.create(m_v3_test_file_swmr, "dset",
                H5T.NATIVE_INT, dsp, H5P.DEFAULT, dcpl, dapl);
            Assert.IsTrue(dst >= 0);

            GCHandle hnd = GCHandle.Alloc(cbuf, GCHandleType.Pinned);
            
            for(int i = 0; i < 3; ++i) 
            {
                for(int j = 0; j < 6; ++j) {
                    cbuf[j] = ((i * 6) + (j + 1)) * -1;
                }
                Assert.IsTrue(
                    H5DO.append(dst, H5P.DEFAULT, 1, new IntPtr(1),
                    H5T.NATIVE_INT, hnd.AddrOfPinnedObject()) >= 0);
            }

            hnd.Free();

            Assert.IsTrue(H5D.close(dst) >= 0);
            Assert.IsTrue(H5P.close(dapl) >= 0);
            Assert.IsTrue(H5P.close(dcpl) >= 0);
            Assert.IsTrue(H5S.close(dsp) >= 0);
        }

        [TestMethod]
        public void H5DOappendTestSWMR2()
        {
            hsize_t[] dims = { 0 };
            hsize_t[] maxdims = { H5S.UNLIMITED };
            hsize_t[] chunk_dims = { 10 };
            uint[] cbuf = { 123, 456, 789 };

            hid_t dsp = H5S.create_simple(1, dims, maxdims);
            Assert.IsTrue(dsp >= 0);

            hid_t dcpl = H5P.create(H5P.DATASET_CREATE);
            Assert.IsTrue(dcpl >= 0);
            Assert.IsTrue(H5P.set_chunk(dcpl, 1, chunk_dims) >= 0);

            hid_t dst = H5D.create(m_v3_test_file_no_swmr, "dset1",
                H5T.NATIVE_UINT, dsp, H5P.DEFAULT, dcpl, H5P.DEFAULT);
            Assert.IsTrue(dst >= 0);

            GCHandle hnd = GCHandle.Alloc(cbuf, GCHandleType.Pinned);

            Assert.IsTrue(
                H5DO.append(dst, H5P.DEFAULT, 0, new IntPtr(3),
                H5T.NATIVE_UINT, hnd.AddrOfPinnedObject()) >= 0);
            
            hnd.Free();

            Assert.IsTrue(H5D.close(dst) >= 0);
            Assert.IsTrue(H5P.close(dcpl) >= 0);
            Assert.IsTrue(H5S.close(dsp) >= 0);
        }
    }
}

#endif