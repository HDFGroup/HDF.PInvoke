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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hid_t = System.Int32;
using hsize_t = System.UInt64;

namespace UnitTests
{
    public partial class H5FTest
    {
        [TestMethod]
        public void H5Fget_filesizeTest1()
        {
            hsize_t size = 0;
            Assert.IsTrue(H5F.get_filesize(m_v0_class_file, ref size) >= 0);
            Assert.IsTrue(H5F.get_filesize(m_v2_class_file, ref size) >= 0);
            Assert.IsTrue(H5F.get_filesize(m_v0_test_file, ref size) >= 0);
            Assert.IsTrue(size == 2144, String.Format("File size: {0}", size));
            Assert.IsTrue(H5F.get_filesize(m_v2_test_file, ref size) >= 0);
            Assert.IsTrue(size == 2096, String.Format("File size: {0}", size));
        }

        [TestMethod]
        public void H5Fget_filesizeTest2()
        {
            hsize_t size = 0;
            Assert.IsFalse(H5F.get_filesize(Utilities.RandomInvalidHandle(),
                ref size) >= 0);
        }
    }
}