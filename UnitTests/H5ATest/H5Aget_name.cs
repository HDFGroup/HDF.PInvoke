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
        public void H5Aget_nameTest1()
        {
            size_t buf_size = IntPtr.Zero;
            ssize_t size = IntPtr.Zero;
            hid_t att = H5A.create(m_v2_test_file, "H5Aget_name",
                H5T.IEEE_F64LE, m_space_scalar);
            Assert.IsTrue(att >= 0);

            // pretend we don't know the size
            size = H5A.get_name(att, buf_size, (StringBuilder) null);
            Assert.IsTrue(size.ToInt32() == 11);
            buf_size = new IntPtr(size.ToInt32() + 1);
            StringBuilder nameBuilder = new StringBuilder(buf_size.ToInt32());
            size = H5A.get_name(att, buf_size, nameBuilder);
            Assert.IsTrue(size.ToInt32() == 11);
            string name = nameBuilder.ToString();
            // names should match
            Assert.AreEqual("H5Aget_name", name);

            // read a truncated version
            buf_size = new IntPtr(3);
            nameBuilder = new StringBuilder(3);
            size = H5A.get_name(att, buf_size, nameBuilder);
            Assert.IsTrue(size.ToInt32() == 11);
            name = nameBuilder.ToString();
            // names won't match
            Assert.AreNotEqual("H5Aget_name", name);
            Assert.AreEqual("H5", name);

            Assert.IsTrue(H5A.close(att) >= 0);
        }

        [TestMethod]
        public void H5Aget_nameTest2()
        {
            Assert.IsFalse(H5A.get_name(Utilities.RandomInvalidHandle(),
                IntPtr.Zero, (StringBuilder) null).ToInt32() >= 0);

            Assert.IsFalse(H5A.get_name(Utilities.RandomInvalidHandle(),
                IntPtr.Zero, (byte[]) null).ToInt32() >= 0);
        }

        [TestMethod]
        public void H5Aget_nameTest3()
        {
            byte[] name = Encoding.UTF8.GetBytes(
                String.Join(":", m_utf8strings));
            byte[] name_buf = new byte [name.Length + 1];
            Array.Copy(name, name_buf, name.Length);
            hid_t att = H5A.create(m_v0_test_file, name_buf, H5T.IEEE_F64BE,
                m_space_scalar, m_acpl);
            Assert.IsTrue(att >= 0);

            ssize_t buf_size = H5A.get_name(att, IntPtr.Zero, (byte[])null)+1;
            Assert.IsTrue(buf_size.ToInt32() > 1);
            byte[] buf = new byte[buf_size.ToInt32()];
            Assert.IsTrue(H5A.get_name(att, buf_size, buf).ToInt32() >= 0);

            for (int i = 0; i < buf.Length; ++i)
            {
                Assert.IsTrue(name_buf[i] == buf[i]);
            }

            Assert.IsTrue(H5A.close(att) >= 0);
        }

        [TestMethod]
        public void H5Aget_nameTest4()
        {
            byte[] name = Encoding.UTF8.GetBytes(
                String.Join(":", m_utf8strings));
            byte[] name_buf = new byte[name.Length + 1];
            Array.Copy(name, name_buf, name.Length);
            hid_t att = H5A.create(m_v2_test_file, name_buf, H5T.IEEE_F64BE,
                m_space_scalar, m_acpl);
            Assert.IsTrue(att >= 0);

            ssize_t buf_size = H5A.get_name(att, IntPtr.Zero, (byte[])null) + 1;
            Assert.IsTrue(buf_size.ToInt32() > 1);
            byte[] buf = new byte[buf_size.ToInt32()];
            Assert.IsTrue(H5A.get_name(att, buf_size, buf).ToInt32() >= 0);

            for (int i = 0; i < buf.Length; ++i)
            {
                Assert.IsTrue(name_buf[i] == buf[i]);
            }

            Assert.IsTrue(H5A.close(att) >= 0);
        }
    }
}