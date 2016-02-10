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
        public void H5Dget_typeTest1()
        {
            hsize_t[] dims = {1024, 2048};
            hid_t space = H5S.create_simple(3, dims, null);

            hid_t dset = H5D.create(m_v0_test_file, "dset", H5T.STD_I16LE,
                space);
            Assert.IsTrue(dset >= 0);
            hid_t type = H5D.get_type(dset);
            Assert.IsTrue(type >= 0);
            Assert.IsTrue(H5T.equal(type, H5T.STD_I16LE) > 0);
            Assert.IsTrue(H5T.close(type) >= 0);
            Assert.IsTrue(H5D.close(dset) >= 0);

            dset = H5D.create(m_v2_test_file, "dset", H5T.STD_I16LE,
                space);
            Assert.IsTrue(dset >= 0);
            type = H5D.get_type(dset);
            Assert.IsTrue(type >= 0);
            Assert.IsTrue(H5T.equal(type, H5T.STD_I16LE) > 0);
            Assert.IsTrue(H5T.close(type) >= 0);
            Assert.IsTrue(H5D.close(dset) >= 0);

            Assert.IsTrue(H5S.close(space) >= 0);
        }

        [TestMethod]
        public void H5Dget_typeTest2()
        {
            Assert.IsFalse(H5D.get_type(Utilities.RandomInvalidHandle()) >= 0);
        }
    }
}