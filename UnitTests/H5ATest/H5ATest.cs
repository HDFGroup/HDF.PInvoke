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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using herr_t = System.Int32;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    [TestClass]
    public partial class H5ATest
    {
        [ClassInitialize()]
        public static void ClassInit(TestContext testContext)
        {
            // create test files which persists across file tests
            m_v0_class_file = Utilities.H5TempFile(ref m_v0_class_file_name,
                H5F.libver_t.EARLIEST);
            Assert.IsTrue(m_v0_class_file >= 0);
            m_v2_class_file = Utilities.H5TempFile(ref m_v2_class_file_name);
            Assert.IsTrue(m_v2_class_file >= 0);

            m_space_null = H5S.create(H5S.class_t.NULL);
            Assert.IsTrue(m_space_null >= 0);
            m_space_scalar = H5S.create(H5S.class_t.SCALAR);
            Assert.IsTrue(m_space_scalar >= 0);

            m_acpl = H5P.create(H5P.ATTRIBUTE_CREATE);
            Assert.IsTrue(m_acpl >= 0);
            Assert.IsTrue(H5P.set_char_encoding(m_acpl, H5T.cset_t.UTF8) >= 0);
        }

        [TestInitialize()]
        public void Init()
        {
            Utilities.DisableErrorPrinting();

            // create test-local files
            m_v0_test_file = Utilities.H5TempFile(ref m_v0_test_file_name,
                H5F.libver_t.EARLIEST);
            Assert.IsTrue(m_v0_test_file >= 0);

            m_v2_test_file = Utilities.H5TempFile(ref m_v2_test_file_name);
            Assert.IsTrue(m_v2_test_file >= 0);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            // close the test-local files
            Assert.IsTrue(H5F.close(m_v0_test_file) >= 0);
            Assert.IsTrue(H5F.close(m_v2_test_file) >= 0);
            File.Delete(m_v0_test_file_name);
            File.Delete(m_v2_test_file_name);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Assert.IsTrue(H5P.close(m_acpl) >= 0);
            // close the global test files
            Assert.IsTrue(H5F.close(m_v0_class_file) >= 0);
            Assert.IsTrue(H5F.close(m_v2_class_file) >= 0);
            Assert.IsTrue(H5S.close(m_space_null) >= 0);
            Assert.IsTrue(H5S.close(m_space_scalar) >= 0);
            File.Delete(m_v0_class_file_name);
            File.Delete(m_v2_class_file_name);
        }

        private static string[] m_utf8strings = new string[] { "Ελληνικά", "日本語", "العربية", "экземпляр", "סקרן" };

        private static hid_t m_v0_class_file = -1;

        private static string m_v0_class_file_name;

        private static hid_t m_v2_class_file = -1;

        private static string m_v2_class_file_name;

        private hid_t m_v0_test_file = -1;

        private string m_v0_test_file_name;

        private hid_t m_v2_test_file = -1;

        private string m_v2_test_file_name;

        private static hid_t m_space_null = -1;

        private static hid_t m_space_scalar = -1;

        private static hid_t m_acpl = -1;

        // Callback for H5A.iterate and H5A.iterate_by_name
        // We expect an array list as op_data; add the attribute names to the
        // array list as we go
        public herr_t DelegateMethod
            (
            hid_t location_id,
            IntPtr attr_name,
            ref H5A.info_t ainfo,
            IntPtr op_data
            ) {
            GCHandle hnd = (GCHandle)op_data;
            ArrayList al = (hnd.Target as ArrayList);
            int len = 0;
            while (Marshal.ReadByte(attr_name, len) != 0) { ++len; }
            byte[] buf = new byte[len];
            Marshal.Copy(attr_name, buf, 0, len);
            al.Add(Encoding.UTF8.GetString(buf));
            return 0;
        }
    }
}