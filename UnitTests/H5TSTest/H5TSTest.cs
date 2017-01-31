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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    [TestClass]
    public partial class H5TSTest
    {
        [ClassInitialize()]
        public static void ClassInit(TestContext testContext)
        {
            hid_t fapl = H5P.create(H5P.FILE_ACCESS);
            Assert.IsTrue(fapl >= 0);
            Assert.IsTrue(
                H5P.set_libver_bounds(fapl, H5F.libver_t.LATEST) >= 0);
            m_shared_file_id = H5F.create(m_shared_file_name, H5F.ACC_TRUNC,
                H5P.DEFAULT, fapl);
            Assert.IsTrue(H5P.close(fapl) >= 0);
        }

        [TestInitialize()]
        public void Init()
        {
            Utilities.DisableErrorPrinting();
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Assert.IsTrue(H5F.close(m_shared_file_id) >= 0);
            File.Delete(m_shared_file_name);
        }

        private static readonly string m_shared_file_name = Path.GetTempFileName();

        private static hid_t m_shared_file_id = -1;

        private Thread Thread1;

        private Thread Thread2;

        private Thread Thread3;
        
        private Thread Thread4;
    }
}