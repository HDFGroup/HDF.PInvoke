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
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using herr_t = System.Int32;
using hsize_t = System.UInt64;

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
        public void H5Literate_by_nameTest1()
        {
            Assert.IsTrue(H5L.create_soft("this/is/a/soft/link",
                m_v0_test_file, "/A/A", m_lcpl) >= 0);
            Assert.IsTrue(H5L.create_soft("this/is/a/soft/link",
                m_v0_test_file, "/A/B", m_lcpl) >= 0);
            Assert.IsTrue(H5L.create_soft("this/is/a/soft/link",
                m_v0_test_file, "/A/C", m_lcpl) >= 0);
            
            ArrayList al = new ArrayList();
            GCHandle hnd = GCHandle.Alloc(al);
            IntPtr op_data = (IntPtr)hnd;
            hsize_t n = 0;
            // the callback is defined in H5LTest.cs
            H5L.iterate_t cb = DelegateMethod;
            Assert.IsTrue(
                H5L.iterate_by_name(m_v0_test_file, "A", H5.index_t.NAME,
                H5.iter_order_t.NATIVE, ref n, cb, op_data) >= 0);
            // we should have 3 elements in the array list
            Assert.IsTrue(al.Count == 3);

            Assert.IsTrue(H5L.create_soft("this/is/a/soft/link",
                m_v2_test_file, "A/A", m_lcpl) >= 0);
            Assert.IsTrue(H5L.create_soft("this/is/a/soft/link",
                m_v2_test_file, "A/B", m_lcpl) >= 0);
            Assert.IsTrue(H5L.create_soft("this/is/a/soft/link",
                m_v2_test_file, "A/C", m_lcpl) >= 0);

            n = 0;
            Assert.IsTrue(
                H5L.iterate_by_name(m_v2_test_file, "A", H5.index_t.NAME,
                H5.iter_order_t.NATIVE, ref n, cb, op_data) >= 0);
            // we should have 6 (3 + 3) elements in the array list
            Assert.IsTrue(al.Count == 6);

            hnd.Free();
        }

        [TestMethod]
        public void H5Literate_by_nameTest2()
        {
            ArrayList al = new ArrayList();
            GCHandle hnd = GCHandle.Alloc(al);
            IntPtr op_data = (IntPtr)hnd;
            hsize_t n = 0;
            // the callback is defined in H5ATest.cs
            H5L.iterate_t cb = DelegateMethod;

            Assert.IsFalse(
                H5L.iterate_by_name(Utilities.RandomInvalidHandle(), "A",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, ref n,
                cb, op_data) >= 0);

            hnd.Free();
        }

        [TestMethod]
        public void H5Literate_by_nameTest3()
        {
            for (int i = 0; i < m_utf8strings.Length; ++i)
            {
                Assert.IsTrue(H5L.create_soft(
                    Encoding.ASCII.GetBytes("this/is/a/soft/link"),
                    m_v0_test_file, Encoding.UTF8.GetBytes(String.Join("/",
                    "A", m_utf8strings[i])),
                    m_lcpl_utf8) >= 0);

                Assert.IsTrue(H5L.create_soft(
                    Encoding.ASCII.GetBytes("this/is/a/soft/link"),
                    m_v2_test_file, Encoding.UTF8.GetBytes(String.Join("/",
                    "A", m_utf8strings[i])),
                    m_lcpl_utf8) >= 0);
            }

            ArrayList al = new ArrayList();
            GCHandle hnd = GCHandle.Alloc(al);
            IntPtr op_data = (IntPtr)hnd;
            hsize_t n = 0;
            // the callback is defined in H5LTest.cs
            H5L.iterate_t cb = DelegateMethod;
            Assert.IsTrue(H5L.iterate_by_name(m_v0_test_file,
                Encoding.ASCII.GetBytes("A"), H5.index_t.NAME,
                H5.iter_order_t.NATIVE, ref n, cb, op_data) >= 0);
            Assert.IsTrue(al.Count == m_utf8strings.Length);

            n = 0;
            Assert.IsTrue(H5L.iterate_by_name(m_v2_test_file,
                Encoding.ASCII.GetBytes("A"), H5.index_t.NAME,
                H5.iter_order_t.NATIVE, ref n, cb, op_data) >= 0);
            Assert.IsTrue(al.Count == 2*m_utf8strings.Length);
            
            hnd.Free();
        }
    }
}