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
        public void H5Adelete_by_idxTest1()
        {
            // use test-local files, because we don't know what's out there
            // at the class level, create two attributes in each file

            hid_t att = H5A.create(m_v0_test_file, "DNA", H5T.IEEE_F32BE,
                m_space_null);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            att = H5A.create(m_v2_test_file, "DNA", H5T.IEEE_F32BE,
               m_space_null);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            att = H5A.create(m_v0_test_file, "DSA", H5T.IEEE_F32BE,
                m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            att = H5A.create(m_v2_test_file, "DSA", H5T.IEEE_F32BE,
               m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            // we have two attributes, delete the one in first position twice
            Assert.IsTrue(H5A.delete_by_idx(m_v0_test_file, ".",
                H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 0) >= 0);
            Assert.IsTrue(H5A.delete_by_idx(m_v0_test_file, ".",
                H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 0) >= 0);

            // we have two attributes, first delete the one in second position
            // then the one in first position
            Assert.IsTrue(H5A.delete_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 1) >= 0);
            Assert.IsTrue(H5A.delete_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 0) >= 0);
        }

        [TestMethod]
        public void H5Adelete_by_idxTest2()
        {
            Assert.IsFalse(
                H5A.delete_by_idx(Utilities.RandomInvalidHandle(), ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 10)
                >= 0);
            Assert.IsFalse(
                H5A.delete_by_idx(m_v0_class_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 1024)
                >= 0);
        }
    }
}