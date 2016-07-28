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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hbool_t = System.UInt32;
using herr_t = System.Int32;
using hsize_t = System.UInt64;

#if HDF5_VER1_10

using hid_t = System.Int64;

namespace UnitTests
{
    public partial class H5SWMRTest
    {
        [TestMethod]
        public void H5Odisable_mdc_flushesTestSWMR1()
        {
            hid_t grp = H5G.create(m_v3_test_file_swmr, "/A/B/C", m_lcpl);
            Assert.IsTrue(grp >= 0);
            Assert.IsTrue(H5O.disable_mdc_flushes(grp) >= 0);
            Assert.IsTrue(H5G.flush(grp) >= 0);
            Assert.IsTrue(H5G.close(grp) >= 0);
        }

        [TestMethod]
        public void H5Odisable_mdc_flushesTestSWMR2()
        {
            hid_t grp = H5G.create(m_v3_test_file_no_swmr, "/A/B/C", m_lcpl);
            Assert.IsTrue(grp >= 0);
            Assert.IsTrue(H5O.disable_mdc_flushes(grp) >= 0);
            Assert.IsTrue(H5G.flush(grp) >= 0);
            Assert.IsTrue(H5G.close(grp) >= 0);
        }

        [TestMethod]
        public void H5Odisable_mdc_flushesTestSWMR3()
        {
            hid_t grp = H5G.create(m_v3_test_file_swmr, "/A/B/C", m_lcpl);
            Assert.IsTrue(grp >= 0);

            hbool_t flag = 11;
            Assert.IsTrue(H5O.are_mdc_flushes_disabled(grp, ref flag) >= 0);
            Console.WriteLine(flag);
            Assert.IsTrue(flag == 0);

            Assert.IsTrue(H5O.disable_mdc_flushes(grp) >= 0);

            Assert.IsTrue(H5O.are_mdc_flushes_disabled(grp, ref flag) >= 0);
            Assert.IsTrue(flag > 0);

            Assert.IsTrue(H5G.flush(grp) >= 0);

            Assert.IsTrue(H5G.close(grp) >= 0);
        }
    }
}

#endif