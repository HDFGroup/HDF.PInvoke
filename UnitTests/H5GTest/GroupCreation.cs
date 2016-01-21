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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hid_t = System.Int32;
using hsize_t = System.UInt64;
using hssize_t = System.Int64;

namespace UnitTests
{
    public partial class H5GTest
    {
        [TestMethod]
        public void GroupCreationTest()
        {
            hid_t gid = H5G.create(m_test_file, "A");
            Assert.IsTrue(gid > 0);

            hid_t gid1 = H5G.create(gid, "B");
            Assert.IsTrue(gid1 > 0);

            Assert.IsTrue(H5G.close(gid1) >= 0);
            Assert.IsTrue(H5G.close(gid) >= 0);
        }

        [TestMethod]
        public void GroupAnonCreationTest()
        {
            hid_t gid = H5G.create_anon(m_test_file);
            Assert.IsTrue(gid > 0);

            hid_t gid1 = H5G.create_anon(gid);
            Assert.IsTrue(gid1 > 0);

            Assert.IsTrue(H5G.close(gid1) >= 0);
            Assert.IsTrue(H5G.close(gid) >= 0);
        }

        [TestMethod]
        public void GroupCPLTest()
        {
            hid_t gid = H5G.create(m_test_file, "A");
            Assert.IsTrue(gid > 0);

            hid_t gcpl = H5G.get_create_plist(gid);
            Assert.IsTrue(gcpl > 0);

            hid_t gid1 = H5G.create_anon(gid);
            Assert.IsTrue(gid1 > 0);

            hid_t gcpl1 = H5G.get_create_plist(gid1);
            Assert.IsTrue(gcpl1 > 0);

            Assert.IsTrue(H5P.close(gcpl1) >= 0);
            Assert.IsTrue(H5P.close(gcpl) >= 0);
            Assert.IsTrue(H5G.close(gid1) >= 0);
            Assert.IsTrue(H5G.close(gid) >= 0);
        }
    }
}