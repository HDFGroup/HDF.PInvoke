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
        public void H5Dget_storage_sizeTest1()
        {
            hsize_t[] dims = {1024, 2048};
            hid_t space = H5S.create_simple(2, dims, null);

            hid_t dcpl = H5P.create(H5P.DATASET_CREATE);
            Assert.IsTrue(dcpl >= 0);
            Assert.IsTrue(
                H5P.set_alloc_time(dcpl, H5D.alloc_time_t.EARLY) >= 0);

            hid_t dset = H5D.create(m_v0_test_file, "dset", H5T.STD_I16LE,
                space, H5P.DEFAULT, dcpl);
            Assert.IsTrue(dset >= 0);
            Assert.IsTrue(H5D.get_storage_size(dset) == 4194304);
            Assert.IsTrue(H5D.close(dset) >= 0);

            dset = H5D.create(m_v2_test_file, "dset", H5T.STD_I16LE,
                space, H5P.DEFAULT, dcpl);
            Assert.IsTrue(dset >= 0);
            Assert.IsTrue(H5D.get_storage_size(dset) == 4194304);
            Assert.IsTrue(H5D.close(dset) >= 0);

            Assert.IsTrue(H5P.close(dcpl) >= 0);
            Assert.IsTrue(H5S.close(space) >= 0);
        }

        [TestMethod]
        public void H5Dget_storage_sizeTest2()
        {
            hid_t dset = H5D.create(m_v0_test_file, "dset", H5T.STD_I16LE,
                m_space_null);
            Assert.IsTrue(dset >= 0);
            hsize_t size = H5D.get_storage_size(dset);
            Assert.IsTrue(H5D.get_storage_size(dset) == 0);
            Assert.IsTrue(H5D.close(dset) >= 0);

            dset = H5D.create(m_v2_test_file, "dset", H5T.STD_I16LE,
                m_space_null);
            Assert.IsTrue(dset >= 0);
            size = H5D.get_storage_size(dset);
            Assert.IsTrue(H5D.get_storage_size(dset) == 0);
            Assert.IsTrue(H5D.close(dset) >= 0);
        }

        [TestMethod]
        public void H5Dget_storage_sizeTest3()
        {
            Assert.IsFalse(
                H5D.get_storage_size(Utilities.RandomInvalidHandle()) > 0);
        }
    }
}