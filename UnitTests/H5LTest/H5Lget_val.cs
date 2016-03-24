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
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using size_t = System.IntPtr;

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
        public void H5Lget_valTest1()
        {
            string sym_path = String.Join("/", m_utf8strings);
            byte[] bytes = Encoding.UTF8.GetBytes(sym_path);

            Assert.IsTrue(
                H5L.create_soft(bytes, m_v0_test_file,
                Encoding.ASCII.GetBytes("/A/B/C/D"), m_lcpl) >= 0);

            H5L.info_t info = new H5L.info_t();
            Assert.IsTrue(
                H5L.get_info(m_v0_test_file, "/A/B/C/D", ref info) >= 0);
            Assert.IsTrue(info.type == H5L.type_t.SOFT);
            Assert.IsTrue(info.corder_valid == 0);
            Assert.IsTrue(info.cset == H5T.cset_t.ASCII);
            Int32 size = info.u.val_size.ToInt32();
            Assert.IsTrue(size == 70);

            // the library appends a null terminator (weired!)
            Assert.IsTrue(size == bytes.Length + 1);

            byte[] buf = new byte[size];

            GCHandle hnd = GCHandle.Alloc(buf, GCHandleType.Pinned);
            Assert.IsTrue(H5L.get_val(m_v0_test_file, "/A/B/C/D",
                hnd.AddrOfPinnedObject(), new IntPtr(buf.Length)) >= 0);
            hnd.Free();

            for (int i = 0; i < buf.Length-1; ++i)
            {
                Assert.IsTrue(buf[i] == bytes[i]);
            }

            Assert.IsTrue(
                H5L.create_soft(bytes, m_v2_test_file,
                Encoding.ASCII.GetBytes("/A/B/C/D"), m_lcpl) >= 0);

            info = new H5L.info_t();
            Assert.IsTrue(
                H5L.get_info(m_v2_test_file, "/A/B/C/D", ref info) >= 0);
            Assert.IsTrue(info.type == H5L.type_t.SOFT);
            Assert.IsTrue(info.corder_valid == 0);
            Assert.IsTrue(info.cset == H5T.cset_t.ASCII);
            size = info.u.val_size.ToInt32();
            Assert.IsTrue(size == 70);

            // the library appends a null terminator
            Assert.IsTrue(size == bytes.Length + 1);

            buf = new byte[size-1];

            hnd = GCHandle.Alloc(buf, GCHandleType.Pinned);
            Assert.IsTrue(H5L.get_val(m_v2_test_file, "/A/B/C/D",
                hnd.AddrOfPinnedObject(), new IntPtr(buf.Length)) >= 0);
            hnd.Free();

            for (int i = 0; i < buf.Length - 1; ++i)
            {
                Assert.IsTrue(buf[i] == bytes[i]);
            }
        }
    }
}