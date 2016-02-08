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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using herr_t = System.Int32;
using hid_t = System.Int32;
using hsize_t = System.UInt64;

namespace UnitTests
{
    [TestClass]
    public partial class H5DTest
    {
        [ClassInitialize()]
        public static void ClassInit(TestContext testContext)
        {
            // create a test file which persists across group tests
            m_v0_class_file = Utilities.H5TempFile(
                H5F.libver_t.LIBVER_EARLIEST);
            Assert.IsTrue(m_v0_class_file >= 0);
            m_v2_class_file = Utilities.H5TempFile();
            Assert.IsTrue(m_v2_class_file >= 0);
            m_space_null = H5S.create(H5S.class_t.NULL);
            Assert.IsTrue(m_space_null >= 0);
            m_space_scalar = H5S.create(H5S.class_t.SCALAR);
            Assert.IsTrue(m_space_scalar >= 0);

            // create two datasets of the extended ASCII character set
            // store as H5T.FORTRAN_S1 -> space padding

            hsize_t[] dims = { 256 };
            unsafe
            {
                fixed (hsize_t* ptr = dims)
                {
                    hid_t space = H5S.create_simple(1, ptr, ptr);
                    m_v0_ascii_dset = H5D.create(m_v0_class_file, "ASCII",
                        H5T.FORTRAN_S1, space);
                    m_v2_ascii_dset = H5D.create(m_v2_class_file, "ASCII",
                        H5T.FORTRAN_S1, space);
                    Assert.IsTrue(H5S.close(space) >= 0);
                }
            }

            // we write from C and must provide null-terminated strings

            byte[] wdata = new byte[512];
            for (int i = 0; i < 256; ++i)
            {
                wdata[2*i] = (byte)i;
            }

            hid_t mem_type = H5T.copy(H5T.C_S1);
            Assert.IsTrue(H5T.set_size(mem_type, new IntPtr(2)) >= 0);

            GCHandle hnd = GCHandle.Alloc(wdata, GCHandleType.Pinned);
            Assert.IsTrue(H5D.write(m_v0_ascii_dset, mem_type, H5S.ALL,
                H5S.ALL, H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            Assert.IsTrue(H5D.write(m_v2_ascii_dset, mem_type, H5S.ALL,
                H5S.ALL, H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            hnd.Free();

            Assert.IsTrue(H5T.close(mem_type) >= 0);
        }

        [TestInitialize()]
        public void Init()
        {
            // create a test-local files
            m_v0_test_file = Utilities.H5TempFile(H5F.libver_t.LIBVER_EARLIEST);
            Assert.IsTrue(m_v0_test_file >= 0);
            m_v2_test_file = Utilities.H5TempFile();
            Assert.IsTrue(m_v2_test_file >= 0);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            // close the test-local files
            Assert.IsTrue(H5F.close(m_v0_test_file) >= 0);
            Assert.IsTrue(H5F.close(m_v2_test_file) >= 0);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            // close the sample datasets
            Assert.IsTrue(H5D.close(m_v2_ascii_dset) >= 0);
            Assert.IsTrue(H5D.close(m_v0_ascii_dset) >= 0);

            // close the global test files
            Assert.IsTrue(H5F.close(m_v0_class_file) >= 0);
            Assert.IsTrue(H5F.close(m_v2_class_file) >= 0);
            Assert.IsTrue(H5S.close(m_space_null) >= 0);
            Assert.IsTrue(H5S.close(m_space_scalar) >= 0);
        }

        private static hid_t m_v0_class_file = -1;

        private static hid_t m_v2_class_file = -1;

        private hid_t m_v0_test_file = -1;

        private hid_t m_v2_test_file = -1;

        private static hid_t m_space_null = -1;

        private static hid_t m_space_scalar = -1;

        private static hid_t m_v0_ascii_dset = -1;

        private static hid_t m_v2_ascii_dset = -1;
    }
}