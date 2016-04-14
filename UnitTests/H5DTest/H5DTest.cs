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
using hsize_t = System.UInt64;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    [TestClass]
    public partial class H5DTest
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

            // create two datasets of the extended ASCII character set
            // store as H5T.FORTRAN_S1 -> space padding

            hsize_t[] dims = { 256 };

            hid_t space = H5S.create_simple(1, dims, null);
            m_v0_ascii_dset = H5D.create(m_v0_class_file, "ASCII",
                H5T.FORTRAN_S1, space);
            m_v2_ascii_dset = H5D.create(m_v2_class_file, "ASCII",
                H5T.FORTRAN_S1, space);
            Assert.IsTrue(H5S.close(space) >= 0);

            // we write from C and must provide null-terminated strings

            byte[] wdata = new byte[512];
            for (int i = 0; i < 256; ++i)
            {
                wdata[2 * i] = (byte)i;
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

            // create UTF-8 encoded test datasets

            hid_t dtype = H5T.create(H5T.class_t.STRING, H5T.VARIABLE);
            Assert.IsTrue(H5T.set_cset(dtype, H5T.cset_t.UTF8) >= 0);
            Assert.IsTrue(H5T.set_strpad(dtype, H5T.str_t.SPACEPAD) >= 0);

            hid_t dspace = H5S.create_simple(1,
                new hsize_t[] { (hsize_t)m_utf8strings.Count }, null);

            m_v0_utf8_dset = H5D.create(m_v0_class_file, "UTF-8", dtype, dspace);
            Assert.IsTrue(m_v0_utf8_dset >= 0);
            m_v2_utf8_dset = H5D.create(m_v2_class_file, "UTF-8", dtype, dspace);
            Assert.IsTrue(m_v2_utf8_dset >= 0);

            GCHandle[] hnds = new GCHandle[m_utf8strings.Count];
            IntPtr[] wdata1 = new IntPtr[m_utf8strings.Count];

            for (int i = 0; i < m_utf8strings.Count; ++i)
            {
                hnds[i] = GCHandle.Alloc(
                    Encoding.UTF8.GetBytes((string)m_utf8strings[i]),
                    GCHandleType.Pinned);
                wdata1[i] = hnds[i].AddrOfPinnedObject();
            }

            hnd = GCHandle.Alloc(wdata1, GCHandleType.Pinned);
            Assert.IsTrue(H5D.write(m_v0_utf8_dset, dtype, H5S.ALL, H5S.ALL,
                H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            Assert.IsTrue(H5D.write(m_v2_utf8_dset, dtype, H5S.ALL, H5S.ALL,
                H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            hnd.Free();

            for (int i = 0; i < m_utf8strings.Count; ++i)
            {
                hnds[i].Free();
            }

            Assert.IsTrue(H5S.close(dspace) >= 0);
            Assert.IsTrue(H5T.close(dtype) >= 0);

#if HDF5_VER1_10

            createVDS();

#endif
        }

        [TestInitialize()]
        public void Init()
        {
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
            // close the sample datasets
            Assert.IsTrue(H5D.close(m_v2_ascii_dset) >= 0);
            Assert.IsTrue(H5D.close(m_v0_ascii_dset) >= 0);
            Assert.IsTrue(H5D.close(m_v2_utf8_dset) >= 0);
            Assert.IsTrue(H5D.close(m_v0_utf8_dset) >= 0);

            // close the global test files
            Assert.IsTrue(H5F.close(m_v0_class_file) >= 0);
            Assert.IsTrue(H5F.close(m_v2_class_file) >= 0);
            Assert.IsTrue(H5S.close(m_space_null) >= 0);
            Assert.IsTrue(H5S.close(m_space_scalar) >= 0);

            File.Delete(m_v0_class_file_name);
            File.Delete(m_v2_class_file_name);

#if HDF5_VER1_10

            cleanupVDS();

#endif
        }

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

        private static hid_t m_v0_ascii_dset = -1;

        private static hid_t m_v2_ascii_dset = -1;

        private static ArrayList m_utf8strings = new ArrayList() { "Ελληνικά", "日本語", "العربية", "экземпляр", "סקרן" };

        private static hid_t m_v0_utf8_dset = -1;

        private static hid_t m_v2_utf8_dset = -1;

#if HDF5_VER1_10

        private static hid_t m_vds_class_file = 1;

        private static string m_vds_class_file_name;

        private static hid_t m_a_class_file = 1;

        private static string m_a_class_file_name;

        private static hid_t m_b_class_file = 1;

        private static string m_b_class_file_name;

        private static hid_t m_c_class_file = 1;

        private static string m_c_class_file_name;

#endif

        // Callback for H5D.iterate
        // op_data is a pointer to a counter and we keep adding the elements
        private herr_t DelegateMethod
            (
            IntPtr elem,
            hid_t type_id,
            uint ndim,
            hsize_t[] point,
            IntPtr op_data
            )
        {
            int count = Marshal.ReadInt32(op_data) + Marshal.ReadInt32(elem);
            Marshal.WriteInt32(op_data, count);
            return 0;
        }

#if HDF5_VER1_10

        private static void createVDS()
        {
            // create files
            m_a_class_file = Utilities.H5TempFile(ref m_a_class_file_name,
                H5F.libver_t.LATEST, true);
            Assert.IsTrue(m_a_class_file >= 0);
            m_b_class_file = Utilities.H5TempFile(ref m_b_class_file_name,
                H5F.libver_t.LATEST, true);
            Assert.IsTrue(m_b_class_file >= 0);
            m_c_class_file = Utilities.H5TempFile(ref m_c_class_file_name,
                H5F.libver_t.LATEST, true);
            Assert.IsTrue(m_c_class_file >= 0);
            m_vds_class_file = Utilities.H5TempFile(ref m_vds_class_file_name);
            Assert.IsTrue(m_vds_class_file >= 0);

            //
            // create target datasets
            //
            hid_t dcpl = H5P.create(H5P.DATASET_CREATE);
            Assert.IsTrue(dcpl >= 0);
            int fill_value = 1;
            GCHandle hnd = GCHandle.Alloc(fill_value, GCHandleType.Pinned);
            Assert.IsTrue(H5P.set_fill_value(dcpl, H5T.NATIVE_INT,
                hnd.AddrOfPinnedObject()) >= 0);

            hsize_t[] dims = { 6 };
            hid_t src_dsp = H5S.create_simple(1, dims, null);
            
            // A
            fill_value = 1;
            hid_t a = H5D.create(m_a_class_file, "A", H5T.STD_I32LE, src_dsp);
            Assert.IsTrue(a >= 0);
            Assert.IsTrue(H5D.close(a) >= 0);
            // B
            fill_value = 2;
            hid_t b = H5D.create(m_b_class_file, "B", H5T.STD_I32LE, src_dsp);
            Assert.IsTrue(b >= 0);
            Assert.IsTrue(H5D.close(b) >= 0);
            // B
            fill_value = 3;
            hid_t c = H5D.create(m_c_class_file, "C", H5T.STD_I32LE, src_dsp);
            Assert.IsTrue(c >= 0);
            Assert.IsTrue(H5D.close(c) >= 0);
            
            //
            // create the VDS
            //
            fill_value = -1;
            hsize_t[] vds_dims = { 4, 6 };
            hid_t vds_dsp = H5S.create_simple(2, vds_dims, null);

            hsize_t[] start = { 0, 0 };
            hsize_t[] count = { 1, 1 };
            hsize_t[] block = { 1, 6 };

            start[0] = 0;
            Assert.IsTrue(H5S.select_hyperslab(vds_dsp, H5S.seloper_t.SET,
                start, null, count, block) >= 0);
            Assert.IsTrue(H5P.set_virtual(dcpl, vds_dsp,
                m_a_class_file_name, "A", src_dsp) >= 0);

            start[0] = 1;
            Assert.IsTrue(H5S.select_hyperslab(vds_dsp, H5S.seloper_t.SET,
                start, null, count, block) >= 0);
            Assert.IsTrue(H5P.set_virtual(dcpl, vds_dsp,
                m_b_class_file_name, "B", src_dsp) >= 0);

            start[0] = 2;
            Assert.IsTrue(H5S.select_hyperslab(vds_dsp, H5S.seloper_t.SET,
                start, null, count, block) >= 0);
            Assert.IsTrue(H5P.set_virtual(dcpl, vds_dsp,
                m_c_class_file_name, "C", src_dsp) >= 0);

            hid_t vds = H5D.create(m_vds_class_file, "VDS", H5T.STD_I32LE,
                vds_dsp, H5P.DEFAULT, dcpl, H5P.DEFAULT);
            Assert.IsTrue(vds >= 0);
            Assert.IsTrue(H5D.close(vds) >= 0);

            Assert.IsTrue(H5S.close(vds_dsp) >= 0);
            Assert.IsTrue(H5S.close(src_dsp) >= 0);
            Assert.IsTrue(H5P.close(dcpl) >= 0);

            hnd.Free();

            // close the satellite files
            Assert.IsTrue(H5F.close(m_a_class_file) >= 0);
            Assert.IsTrue(H5F.close(m_b_class_file) >= 0);
            Assert.IsTrue(H5F.close(m_c_class_file) >= 0);
        }

        private static void cleanupVDS()
        {
            Assert.IsTrue(H5F.close(m_vds_class_file) >= 0);

            File.Delete(m_vds_class_file_name);
            File.Delete(m_a_class_file_name);
            File.Delete(m_b_class_file_name);
            File.Delete(m_c_class_file_name);
        }

#endif

    }
}