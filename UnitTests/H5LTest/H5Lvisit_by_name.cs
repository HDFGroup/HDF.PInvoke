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
        public void H5Lvisit_by_nameTest1()
        {
            Assert.IsTrue(H5G.create(m_v0_test_file, "A/B/C/D", m_lcpl) >= 0);
            Assert.IsTrue(
                H5L.create_hard(m_v0_test_file, "A/B/C/D", m_v0_test_file,
                "shortcut") >= 0);

            Assert.IsTrue(H5G.create(m_v2_test_file, "A/B/C/D", m_lcpl) >= 0);
            Assert.IsTrue(
                H5L.create_hard(m_v2_test_file, "A/B/C/D", m_v2_test_file,
                "shortcut") >= 0);
            
            ArrayList al = new ArrayList();
            GCHandle hnd = GCHandle.Alloc(al);
            IntPtr op_data = (IntPtr)hnd;
            // the callback is defined in H5LTest.cs
            H5L.iterate_t cb = DelegateMethod;

            Assert.IsTrue(H5L.visit_by_name(m_v0_test_file, "A",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, cb, op_data) >= 0);
            // we should have 3 elements in the array list
            Assert.IsTrue(al.Count == 3);

            Assert.IsTrue(H5L.visit_by_name(m_v2_test_file, "A",
                H5.index_t.NAME, H5.iter_order_t.NATIVE, cb, op_data) >= 0);
            // we should have 6 (3 + 3) elements in the array list
            Assert.IsTrue(al.Count == 6);

            hnd.Free();
        }

        [TestMethod]
        public void H5Lvisit_by_nameTest2()
        {
            string path = String.Join("/", m_utf8strings);
            Assert.IsTrue(H5G.create(m_v0_test_file,
                Encoding.UTF8.GetBytes(path), m_lcpl_utf8) >= 0);
            Assert.IsTrue(H5G.create(m_v2_test_file,
                Encoding.UTF8.GetBytes(path), m_lcpl_utf8) >= 0);
           
            ArrayList al = new ArrayList();
            GCHandle hnd = GCHandle.Alloc(al);
            IntPtr op_data = (IntPtr)hnd;
            // the callback is defined in H5LTest.cs
            H5L.iterate_t cb = DelegateMethod;

            Assert.IsTrue(H5L.visit_by_name(m_v0_test_file,
                Encoding.UTF8.GetBytes(m_utf8strings[0]), H5.index_t.NAME,
                H5.iter_order_t.NATIVE, cb, op_data) >= 0);
            // we should have 4 elements in the array list
            Assert.IsTrue(al.Count == 4);

            Assert.IsTrue(H5L.visit_by_name(m_v2_test_file,
                Encoding.UTF8.GetBytes(m_utf8strings[0]), H5.index_t.NAME,
                H5.iter_order_t.NATIVE, cb, op_data) >= 0);
            // we should have 8 (4 + 4) elements in the array list
            Assert.IsTrue(al.Count == 8);

            hnd.Free();
        }
    }
}