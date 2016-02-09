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
        public void H5Aget_info_by_idxTest1()
        {
            H5A.info_t info = new H5A.info_t();
            hid_t att = H5A.create(m_v2_test_file, "A", H5T.IEEE_F64LE,
                m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);
            att = H5A.create(m_v2_test_file, "B", H5T.IEEE_F64LE,
                m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            Assert.IsTrue(H5A.get_info_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 0,
                ref info) >= 0);
            Assert.IsTrue(H5A.get_info_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 1,
                ref info) >= 0);
            
            att = H5A.create(m_v0_test_file, "A", H5T.IEEE_F64LE,
                m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);
            Assert.IsTrue(H5A.get_info_by_idx(m_v0_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 0,
                ref info) >= 0);

            Assert.IsFalse(H5A.get_info_by_idx(m_v0_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 1,
                ref info) >= 0);
        }

        [TestMethod]
        public void H5Aget_info_by_idxTest2()
        {
            H5A.info_t info = new H5A.info_t();
            Assert.IsFalse(
                H5A.get_info_by_idx(Utilities.RandomInvalidHandle(), ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 1024,
                ref info) >= 0);
        }
    }
}