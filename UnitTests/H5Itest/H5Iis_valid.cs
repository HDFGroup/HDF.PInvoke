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
    public partial class H5ITest
    {
        [TestMethod]
        public void H5Iis_validTest1()
        {
            hid_t gid = H5G.create(m_v0_test_file, "A");
            Assert.IsTrue(gid > 0);
            Assert.IsTrue(H5I.is_valid(gid) > 0);
            Assert.IsTrue(H5G.close(gid) >= 0);
            Assert.IsTrue(H5I.is_valid(gid) == 0);

            gid = H5G.create(m_v2_test_file, "A");
            Assert.IsTrue(gid > 0);
            Assert.IsTrue(H5I.is_valid(gid) > 0);
            Assert.IsTrue(H5G.close(gid) >= 0);
            Assert.IsTrue(H5I.is_valid(gid) == 0);
        }

        [TestMethod]
        public void H5Iis_validTest2()
        {
            Assert.IsTrue(H5I.is_valid(Utilities.RandomInvalidHandle()) == 0);
        }
    }
}
