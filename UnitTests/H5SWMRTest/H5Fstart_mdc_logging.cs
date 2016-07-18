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
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hbool_t = System.UInt32;
using herr_t = System.Int32;


#if HDF5_VER1_10

using hid_t = System.Int64;

namespace UnitTests
{
    public partial class H5SWMRTest
    {
        [TestMethod]
        public void H5Fstart_mdc_loggingTestSWMR1()
        {
            hid_t fapl = H5P.create(H5P.FILE_ACCESS);
            Assert.IsTrue(fapl >= 0);
            Assert.IsTrue(
                H5P.set_libver_bounds(fapl, H5F.libver_t.LATEST) >= 0);

            hbool_t is_enabled = 1;
            string location = "mdc.log";
            hbool_t start_on_access = 0;

            Assert.IsTrue(
                H5P.set_mdc_log_options(fapl, is_enabled, location,
                start_on_access) >= 0);

            string fileName = Path.GetTempFileName();
            hid_t file = H5F.create(fileName, H5F.ACC_TRUNC, H5P.DEFAULT, fapl);
            Assert.IsTrue(file >= 0);

            Assert.IsTrue(H5F.start_mdc_logging(file) >= 0);

            hid_t group = H5G.create(file, "/A/B/C", m_lcpl);
            Assert.IsTrue(group >= 0);
            Assert.IsTrue(H5G.close(group) >= 0);

            Assert.IsTrue(H5F.stop_mdc_logging(file) >= 0);

            Assert.IsTrue(H5F.close(file) >= 0);

            Assert.IsTrue(H5P.close(fapl) >= 0);

            File.Delete("mdc.log");
        }
    }
}

#endif