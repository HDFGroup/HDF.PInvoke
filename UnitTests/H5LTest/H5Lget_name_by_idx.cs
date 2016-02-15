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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hsize_t = System.UInt64;

using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5LTest
    {
        [TestMethod]
        public void H5Lget_name_by_idxTest1()
        {
            Assert.IsTrue(
                H5L.create_external(m_v0_class_file_name, "/", m_v0_test_file,
                "A", m_lcpl) >= 0);
            Assert.IsTrue(
                H5L.create_external(m_v0_class_file_name, "/", m_v0_test_file,
                "AB", m_lcpl) >= 0);
            Assert.IsTrue(
                H5L.create_external(m_v0_class_file_name, "/", m_v0_test_file,
                "ABC", m_lcpl) >= 0);

            size_t buf_size = IntPtr.Zero;
            ssize_t size = H5L.get_name_by_idx(m_v0_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 1, null, buf_size);
            Assert.IsTrue(size.ToInt32() == 2);
            buf_size = new IntPtr(size.ToInt32() + 1);
            StringBuilder nameBuilder = new StringBuilder(buf_size.ToInt32());
            size = H5L.get_name_by_idx(m_v0_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 1, nameBuilder,
                buf_size);
            Assert.IsTrue(nameBuilder.ToString() == "AB");

            Assert.IsTrue(
                H5L.create_external(m_v2_class_file_name, "/", m_v2_test_file,
                "A", m_lcpl) >= 0);
            Assert.IsTrue(
                H5L.create_external(m_v2_class_file_name, "/", m_v2_test_file,
                "AB", m_lcpl) >= 0);
            Assert.IsTrue(
                H5L.create_external(m_v2_class_file_name, "/", m_v2_test_file,
                "ABC", m_lcpl) >= 0);

            buf_size = IntPtr.Zero;
            size = H5L.get_name_by_idx(m_v2_test_file, ".", H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 1, null, buf_size);
            Assert.IsTrue(size.ToInt32() == 2);
            buf_size = new IntPtr(size.ToInt32() + 1);
            nameBuilder = new StringBuilder(buf_size.ToInt32());
            size = H5L.get_name_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, 1, nameBuilder,
                buf_size);
            Assert.IsTrue(nameBuilder.ToString() == "AB");
        }

        [TestMethod]
        public void H5Lget_name_by_idxTest2()
        {
            hid_t lcpl = H5P.copy(m_lcpl);
            Assert.IsTrue(lcpl >= 0);
            Assert.IsTrue(H5P.set_char_encoding(lcpl, H5T.cset_t.UTF8) >= 0);

            for (int i = 0; i < m_utf8strings.Length; ++i)
            {
                Assert.IsTrue(
                    H5L.create_external(m_v0_class_file_name, "/",
                    m_v0_class_file, m_utf8strings[i], lcpl) >= 0);
            }

            for (int i = 0; i < m_utf8strings.Length; ++i)
            {
                size_t buf_size = IntPtr.Zero;
                ssize_t size = H5L.get_name_by_idx(m_v0_test_file, ".",
                    H5.index_t.NAME, H5.iter_order_t.NATIVE, (hsize_t)i, null,
                    buf_size);
                buf_size = new IntPtr(size.ToInt32() + 1);
                StringBuilder nameBuilder = new StringBuilder(buf_size.ToInt32());
                size = H5L.get_name_by_idx(m_v0_test_file, ".",
                    H5.index_t.NAME, H5.iter_order_t.NATIVE, (hsize_t)i, nameBuilder,
                    buf_size);
            }
            
            Assert.IsTrue(H5P.close(lcpl) >= 0);
        }

    }
}