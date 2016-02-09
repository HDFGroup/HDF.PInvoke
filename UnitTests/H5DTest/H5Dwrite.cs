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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hid_t = System.Int32;

namespace UnitTests
{
    public partial class H5DTest
    {
        [TestMethod]
        public void H5DwriteTest1()
        {

            string utf8string
                = "Γαζέες καὶ μυρτιὲς δὲν θὰ βρῶ πιὰ στὸ χρυσαφὶ ξέφωτο";
            byte[] wdata = Encoding.UTF8.GetBytes(utf8string);
            
            hid_t dtype = H5T.create(H5T.class_t.STRING,
                new IntPtr(wdata.Length));
            Assert.IsTrue(H5T.set_cset(dtype, H5T.cset_t.UTF8) >= 0);
            Assert.IsTrue(H5T.set_strpad(dtype, H5T.str_t.SPACEPAD) >= 0);

            hid_t dset_v0 = H5D.create(m_v0_test_file, "dset", dtype,
                m_space_scalar);
            Assert.IsTrue(dset_v0 >= 0);

            hid_t dset_v2 = H5D.create(m_v2_test_file, "dset", dtype,
                m_space_scalar);
            Assert.IsTrue(dset_v2 >= 0);

            GCHandle hnd = GCHandle.Alloc(wdata, GCHandleType.Pinned);
            Assert.IsTrue(H5D.write(dset_v0, dtype, H5S.ALL,
                H5S.ALL, H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            Assert.IsTrue(H5D.write(dset_v2, dtype, H5S.ALL,
                H5S.ALL, H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            hnd.Free();

            Assert.IsTrue(H5T.close(dtype) >= 0);
            Assert.IsTrue(H5D.close(dset_v2) >= 0);
            Assert.IsTrue(H5D.close(dset_v0) >= 0);
        }

        [TestMethod]
        public void H5DwriteTest2()
        {
            System.Collections.Generic.List<string> utf8strings
                = new System.Collections.Generic.List<string>()
                { "Ελληνικά", "日本語", "العربية" };

            hid_t dtype = H5T.create(H5T.class_t.STRING, new IntPtr(-1));
            Assert.IsTrue(H5T.set_cset(dtype, H5T.cset_t.UTF8) >= 0);
            Assert.IsTrue(H5T.set_strpad(dtype, H5T.str_t.SPACEPAD) >= 0);

            //hid_t dspace = H5S.create_simple(1, new ulong[] { (ulong)utf8strings.Count }, null);
            hid_t dspace = H5S.create_simple(1, new ulong[] { (ulong)1 }, null);

            hid_t dset = H5D.create(m_v0_test_file, "dset", dtype, dspace);
            Assert.IsTrue(dset >= 0);

            byte[] vlenStringBytes = Encoding.UTF8.GetBytes(utf8strings[0]);

            GCHandle hnd = GCHandle.Alloc(vlenStringBytes, GCHandleType.Pinned);
            Assert.IsTrue(H5D.write(dset, dtype, H5S.ALL, H5S.ALL, H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            hnd.Free();

            Assert.IsTrue(H5D.close(dset) >= 0);
            Assert.IsTrue(H5S.close(dspace) >= 0);
            Assert.IsTrue(H5T.close(dtype) >= 0);
        }
    }
}