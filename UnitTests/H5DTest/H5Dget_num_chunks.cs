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

#if HDF5_VER1_10

        [TestMethod]
        public void H5Dget_num_chunksTest1()
        {
            hsize_t[] dims = { 10, 10 };
            hsize_t[] max_dims = { H5S.UNLIMITED, H5S.UNLIMITED };
            hid_t space = H5S.create_simple(2, dims, max_dims);

            hid_t dcpl = H5P.create(H5P.DATASET_CREATE);
            Assert.IsTrue(dcpl >= 0);
            hsize_t[] chunk = { 4, 4 };
            Assert.IsTrue(H5P.set_chunk(dcpl, 2, chunk) >= 0);
            Assert.IsTrue(H5P.set_alloc_time(dcpl, H5D.alloc_time_t.EARLY) >= 0);
            Assert.IsTrue(H5P.set_fill_time(dcpl, H5D.fill_time_t.ALLOC) >= 0);

            hid_t dset = H5D.create(m_v0_test_file, "Early Bird", H5T.IEEE_F32BE,
                space, H5P.DEFAULT, dcpl);
            Assert.IsTrue(dset >= 0);

            // This should work but doesn't:
            // Assert.IsTrue(H5D.get_num_chunks(dset, H5S.ALL, ref nchunks) >= 0);
            hid_t fspace = H5D.get_space(dset);
            Assert.IsTrue(fspace >= 0);
            Assert.IsTrue(H5S.select_all(fspace) >= 0);
            
            hsize_t nchunks = 0;
            Assert.IsTrue(H5D.get_num_chunks(dset, fspace, ref nchunks) >= 0);
            Assert.IsTrue(nchunks == 9);

            Assert.IsTrue(H5D.close(dset) >= 0);

            dset = H5D.create(m_v2_test_file, "Early Bird", H5T.IEEE_F32BE,
                space, H5P.DEFAULT, dcpl);
            Assert.IsTrue(dset >= 0);

            // This should work but doesn't:
            // Assert.IsTrue(H5D.get_num_chunks(dset, H5S.ALL, ref nchunks) >= 0);
            fspace = H5D.get_space(dset);
            Assert.IsTrue(fspace >= 0);
            Assert.IsTrue(H5S.select_all(fspace) >= 0);

            nchunks = 0;
            Assert.IsTrue(H5D.get_num_chunks(dset, fspace, ref nchunks) >= 0);
            Assert.IsTrue(nchunks == 9);

            Assert.IsTrue(H5D.close(dset) >= 0);
        }

#endif
    }
}