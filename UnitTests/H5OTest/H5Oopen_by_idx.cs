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

using hsize_t = System.UInt64;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5ATest
    {
        [TestMethod]
        public void H5Oopen_by_idxTest1()
        {
            Assert.IsTrue(
                H5G.close(H5G.create(m_v0_test_file, "A")) >= 0);
            Assert.IsTrue(
                H5G.close(H5G.create(m_v0_test_file, "AA")) >= 0);
            Assert.IsTrue(
                H5G.close(H5G.create(m_v0_test_file, "AAA")) >= 0);
            Assert.IsTrue(
                H5G.close(H5G.create(m_v0_test_file, "AAAA")) >= 0);

            hid_t obj = H5O.open_by_idx(m_v0_test_file, ".", H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 0);
            Assert.IsTrue(obj >= 0);
            Assert.IsTrue(H5O.close(obj) >= 0);

            obj = H5O.open_by_idx(m_v0_test_file, ".", H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 2);
            Assert.IsTrue(obj >= 0);
            Assert.IsTrue(H5O.close(obj) >= 0);

            Assert.IsTrue(
                H5G.close(H5G.create(m_v2_test_file, "A")) >= 0);
            Assert.IsTrue(
                H5G.close(H5G.create(m_v2_test_file, "AA")) >= 0);
            Assert.IsTrue(
                H5G.close(H5G.create(m_v2_test_file, "AAA")) >= 0);
            Assert.IsTrue(
                H5G.close(H5G.create(m_v2_test_file, "AAAA")) >= 0);

            obj = H5O.open_by_idx(m_v2_test_file, ".", H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 0);
            Assert.IsTrue(obj >= 0);
            Assert.IsTrue(H5O.close(obj) >= 0);

            obj = H5O.open_by_idx(m_v2_test_file, ".", H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 2);
            Assert.IsTrue(obj >= 0);
            Assert.IsTrue(H5O.close(obj) >= 0);
        }

        [TestMethod]
        public void H5Oopen_by_idxTest2()
        {
            Assert.IsFalse(
                H5O.open_by_idx(Utilities.RandomInvalidHandle(), ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE,
                44) >= 0);
            Assert.IsFalse(
                H5O.open_by_idx(m_v2_class_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE,
                hsize_t.MaxValue) >= 0);
        }
    }
}