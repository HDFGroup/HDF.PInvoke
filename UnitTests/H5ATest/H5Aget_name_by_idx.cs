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
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

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
        public void H5Aget_name_by_idxTest1()
        {
            hid_t att = H5A.create(m_v2_test_file, "H5Aget_name",
                H5T.IEEE_F64LE, m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);
            att = H5A.create(m_v2_test_file, "H5Aget_name_by_idx",
                H5T.STD_I16LE, m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            size_t buf_size = IntPtr.Zero;
            ssize_t size = IntPtr.Zero;
            StringBuilder nameBuilder = new StringBuilder(19);
            buf_size = new IntPtr(19);
            size = H5A.get_name_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE,
                0, nameBuilder, buf_size);
            Assert.IsTrue(size.ToInt32() == 11);
            string name = nameBuilder.ToString();
            // names should match
            Assert.AreEqual("H5Aget_name", name);

            nameBuilder.Clear();
            size = H5A.get_name_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE,
                1, nameBuilder, buf_size);
            Assert.IsTrue(size.ToInt32() == 18);
            name = nameBuilder.ToString();
            // names should match
            Assert.AreEqual("H5Aget_name_by_idx", name);

            // read a truncated version
            buf_size = new IntPtr(3);
            nameBuilder = new StringBuilder(3);
            size = H5A.get_name_by_idx(m_v2_test_file, ".",
                H5.index_t.NAME, H5.iter_order_t.NATIVE,
                1, nameBuilder, buf_size);
            Assert.IsTrue(size.ToInt32() == 18);
            name = nameBuilder.ToString();
            // names won't match
            Assert.AreNotEqual("H5Aget_name_by_idx", name);
            Assert.AreEqual("H5", name);
        }

        [TestMethod]
        public void H5Aget_name_by_idxTest2()
        {
            Assert.IsFalse(H5A.get_name_by_idx(Utilities.RandomInvalidHandle(),
                ".", H5.index_t.NAME, H5.iter_order_t.NATIVE,
                0, null, IntPtr.Zero).ToInt32() >= 0);
        }

        [TestMethod]
        public void H5Aget_name_by_idxTest3()
        {
            hid_t att = H5A.create(m_v0_test_file, "H5Aget_name",
                H5T.IEEE_F64LE, m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);
            att = H5A.create(m_v0_test_file, "H5Aget_name_by_idx",
                H5T.STD_I16LE, m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            byte[] name = Encoding.UTF8.GetBytes(
                String.Join(":", m_utf8strings));
            byte[] name_buf = new byte[name.Length + 1];
            Array.Copy(name, name_buf, name.Length);
            att = H5A.create(m_v0_test_file, name_buf, H5T.IEEE_F64BE,
                m_space_scalar, m_acpl);
            Assert.IsTrue(att >= 0);

            ssize_t buf_size = H5A.get_name(att, IntPtr.Zero, (byte[])null) + 1;
            Assert.IsTrue(buf_size.ToInt32() > 1);
            byte[] buf = new byte[buf_size.ToInt32()];
            Assert.IsTrue(
                H5A.get_name_by_idx(m_v0_test_file,
                Encoding.ASCII.GetBytes("."), H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 2, buf, buf_size).ToInt32() >= 0);

            for (int i = 0; i < buf.Length; ++i)
            {
                Assert.IsTrue(name_buf[i] == buf[i]);
            }

            Assert.IsTrue(H5A.close(att) >= 0);
        }

        [TestMethod]
        public void H5Aget_name_by_idxTest4()
        {
            hid_t att = H5A.create(m_v2_test_file, "H5Aget_name",
                H5T.IEEE_F64LE, m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);
            att = H5A.create(m_v2_test_file, "H5Aget_name_by_idx",
                H5T.STD_I16LE, m_space_scalar);
            Assert.IsTrue(att >= 0);
            Assert.IsTrue(H5A.close(att) >= 0);

            byte[] name = Encoding.UTF8.GetBytes(
                String.Join(":", m_utf8strings));
            byte[] name_buf = new byte[name.Length + 1];
            Array.Copy(name, name_buf, name.Length);
            att = H5A.create(m_v2_test_file, name_buf, H5T.IEEE_F64BE,
                m_space_scalar, m_acpl);
            Assert.IsTrue(att >= 0);

            ssize_t buf_size = H5A.get_name(att, IntPtr.Zero, (byte[])null) + 1;
            Assert.IsTrue(buf_size.ToInt32() > 1);
            byte[] buf = new byte[buf_size.ToInt32()];
            Assert.IsTrue(
                H5A.get_name_by_idx(m_v2_test_file,
                Encoding.ASCII.GetBytes("."), H5.index_t.NAME,
                H5.iter_order_t.NATIVE, 2, buf, buf_size).ToInt32() >= 0);

            for (int i = 0; i < buf.Length; ++i)
            {
                Assert.IsTrue(name_buf[i] == buf[i]);
            }

            Assert.IsTrue(H5A.close(att) >= 0);
        }
    }
}