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
        public void H5Lget_infoTest1()
        {
            Assert.IsTrue(
                H5L.create_external(m_v0_class_file_name, "/", m_v0_test_file,
                "A/B/C", m_lcpl) >= 0);

            H5L.info_t info = new H5L.info_t();
            Assert.IsTrue(H5L.get_info(m_v0_test_file, "A/B/C", ref info) >= 0);
            Assert.IsTrue(info.type == H5L.type_t.EXTERNAL);
            Assert.IsTrue(info.corder_valid == 0);
            Assert.IsTrue(info.cset == H5T.cset_t.ASCII);
            Assert.IsTrue(info.u.val_size.ToInt64() > 0);

            Assert.IsTrue(
                H5L.create_external(m_v2_class_file_name, "/", m_v2_test_file,
                "A/B/C", m_lcpl) >= 0);

            Assert.IsTrue(H5L.get_info(m_v2_test_file, "A/B/C", ref info) >= 0);
            Assert.IsTrue(info.type == H5L.type_t.EXTERNAL);
            Assert.IsTrue(info.corder_valid == 0);
            Assert.IsTrue(info.cset == H5T.cset_t.ASCII);
            Assert.IsTrue(info.u.val_size.ToInt64() > 0);
        }

        [TestMethod]
        public void H5Lget_infoTest2()
        {
            Assert.IsTrue(
                H5G.close(H5G.create(m_v0_test_file, "A/B/C/D", m_lcpl)) >= 0);
            H5L.info_t info = new H5L.info_t();
            Assert.IsTrue(H5L.get_info(m_v0_test_file, "A/B/C/D", ref info) >= 0);
            Assert.IsTrue(info.type == H5L.type_t.HARD);
            Assert.IsTrue(info.corder_valid == 0);
            Assert.IsTrue(info.cset == H5T.cset_t.ASCII);
            Assert.IsTrue(info.u.val_size.ToInt64() == 3896);

            Assert.IsTrue(
                H5G.close(H5G.create(m_v2_test_file, "A/B/C/D", m_lcpl)) >= 0);
            Assert.IsTrue(H5L.get_info(m_v2_test_file, "A/B/C/D", ref info) >= 0);
            Assert.IsTrue(info.type == H5L.type_t.HARD);
            Assert.IsTrue(info.corder_valid == 0);
            Assert.IsTrue(info.cset == H5T.cset_t.ASCII);
            Assert.IsTrue(info.u.val_size.ToInt64() == 636);
        }
    }
}