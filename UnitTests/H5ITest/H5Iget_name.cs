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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using ssize_t = System.IntPtr;

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
        public void H5Iget_nameTest1()
        {
            hid_t gid = H5G.create(m_v0_test_file, "AAAAAAAAAAAAAAAAAAAAA");
            Assert.IsTrue(gid > 0);

            ssize_t buf_size = H5I.get_name(gid, (StringBuilder)null,
                IntPtr.Zero) + 1;
            Assert.IsTrue(buf_size.ToInt32() > 1);
            StringBuilder nameBuilder = new StringBuilder(buf_size.ToInt32());
            IntPtr size = H5I.get_name(gid, nameBuilder, buf_size);
            Assert.IsTrue(size.ToInt32() > 0);
            Assert.IsTrue(nameBuilder.ToString() == "/AAAAAAAAAAAAAAAAAAAAA");

            Assert.IsTrue(H5G.close(gid) >= 0);

            gid = H5G.create(m_v2_test_file, "AAAAAAAAAAAAAAAAAAAAA");
            Assert.IsTrue(gid > 0);

            buf_size = H5I.get_name(gid, (StringBuilder)null, IntPtr.Zero) + 1;
            Assert.IsTrue(buf_size.ToInt32() > 1);
            nameBuilder = new StringBuilder(buf_size.ToInt32());
            size = H5I.get_name(gid, nameBuilder, buf_size);
            Assert.IsTrue(size.ToInt32() > 0);
            Assert.IsTrue(nameBuilder.ToString() == "/AAAAAAAAAAAAAAAAAAAAA");

            Assert.IsTrue(H5G.close(gid) >= 0);
        }

        [TestMethod]
        public void H5Iget_nameTest2()
        {
            IntPtr size = H5I.get_name(Utilities.RandomInvalidHandle(), null,
                IntPtr.Zero);
            Assert.IsFalse(size.ToInt32() >= 0);
        }
    }
}
