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
using System.IO;
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
    [TestClass]
    public partial class H5SWMRTest
    {
        [ClassInitialize()]
        public static void ClassInit(TestContext testContext)
        {
#if HDF5_VER1_10

            // create test files which persists across file tests
            m_v3_class_file = Utilities.H5TempFileSWMR(ref m_v3_class_file_name);
            Assert.IsTrue(m_v3_class_file >= 0);

            m_lcpl = H5P.create(H5P.LINK_CREATE);
            Assert.IsTrue(H5P.set_create_intermediate_group(m_lcpl, 1) >= 0);

            m_lcpl_utf8 = H5P.copy(m_lcpl);
            Assert.IsTrue(
                H5P.set_char_encoding(m_lcpl_utf8, H5T.cset_t.UTF8) >= 0);

#endif      
        }

        [TestInitialize()]
        public void Init()
        {
#if HDF5_VER1_10
            m_v3_test_file_no_swmr =
                Utilities.H5TempFileNoSWMR(ref m_v3_test_file_name_no_swmr);
            Assert.IsTrue(m_v3_test_file_no_swmr >= 0);

            m_v3_test_file_swmr =
                Utilities.H5TempFileSWMR(ref m_v3_test_file_name_swmr);
            Assert.IsTrue(m_v3_test_file_swmr >= 0);
#endif
        }

        [TestCleanup()]
        public void Cleanup()
        {
#if HDF5_VER1_10
            // close the test-local files
            Assert.IsTrue(H5F.close(m_v3_test_file_no_swmr) >= 0);
            File.Delete(m_v3_test_file_name_no_swmr);
            Assert.IsTrue(H5F.close(m_v3_test_file_swmr) >= 0);
            File.Delete(m_v3_test_file_name_swmr);
#endif
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
#if HDF5_VER1_10
            Assert.IsTrue(H5P.close(m_lcpl) >= 0);
            Assert.IsTrue(H5P.close(m_lcpl_utf8) >= 0);

            // close the global test files
            Assert.IsTrue(H5F.close(m_v3_class_file) >= 0);
            File.Delete(m_v3_class_file_name);
#endif      
        }

#if HDF5_VER1_10
        
        private static hid_t m_v3_class_file = -1;

        private static string m_v3_class_file_name;

        private static hid_t m_lcpl;

        private static hid_t m_lcpl_utf8;

        private hid_t m_v3_test_file_no_swmr = -1;

        private string m_v3_test_file_name_no_swmr;

        private hid_t m_v3_test_file_swmr = -1;

        private string m_v3_test_file_name_swmr;

#endif
    }
}