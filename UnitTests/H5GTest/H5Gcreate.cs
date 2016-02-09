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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5GTest
    {
        [TestMethod]
        public void H5GcreateTest1()
        {
            hid_t gid = H5G.create(m_v0_test_file, "A");
            Assert.IsTrue(gid > 0);

            hid_t gid1 = H5G.create(gid, "B");
            Assert.IsTrue(gid1 > 0);

            Assert.IsTrue(H5G.close(gid1) >= 0);
            Assert.IsTrue(H5G.close(gid) >= 0);

            gid = H5G.create(m_v2_test_file, "A");
            Assert.IsTrue(gid > 0);

            gid1 = H5G.create(gid, "B");
            Assert.IsTrue(gid1 > 0);

            Assert.IsTrue(H5G.close(gid1) >= 0);
            Assert.IsTrue(H5G.close(gid) >= 0);
        }

        [TestMethod]
        public void H5GcreateTest2()
        {
            hid_t file = Utilities.RandomInvalidHandle();
            hid_t gid = H5G.create(file, "A");
            Assert.IsTrue(gid < 0);
        }

        [TestMethod]
        public void H5GcreateTest3()
        {
            hid_t lcpl = H5P.create(H5P.LINK_CREATE);
            Assert.IsTrue(lcpl >= 0);
            Assert.IsTrue(H5P.set_create_intermediate_group(lcpl, 1) >= 0);
            hid_t gid = H5G.create(m_v0_test_file, "A/B/C/D/E/F/G/H", lcpl);
            Assert.IsTrue(gid > 0);
            Assert.IsTrue(H5G.close(gid) >= 0);
            hid_t gid1 = H5G.create(m_v2_test_file, "A/B/C/D/E/F/G/H", lcpl);
            Assert.IsTrue(gid1 > 0);
            Assert.IsTrue(H5G.close(gid1) >= 0);
            Assert.IsTrue(H5P.close(lcpl) >= 0);
        }
    }
}
