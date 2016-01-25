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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hid_t = System.Int32;
using hsize_t = System.UInt64;
using hssize_t = System.Int64;

namespace UnitTests
{
    public partial class H5FTest
    {
        [TestMethod]
        public void FileCreationTest()
        {
            hsize_t size = 0;
            Assert.IsTrue(H5F.get_filesize(m_test_file, ref size) >= 0);

            // get access properties
            hid_t plist = H5F.get_access_plist(m_test_file);
            Assert.IsTrue(plist >= 0);
            Assert.IsTrue(H5P.close(plist) >= 0);

            // get creation properties
            plist = H5F.get_create_plist(m_test_file);
            Assert.IsTrue(plist >= 0);
            Assert.IsTrue(H5P.close(plist) >= 0);

            // flush the file
            Assert.IsTrue(H5F.flush(m_test_file,
                H5F.scope_t.SCOPE_GLOBAL) >= 0);

            // check 4 free space
            Assert.IsTrue(H5F.get_freespace(m_test_file) >= 0);

            // check file info
            H5F.info_t info = new H5F.info_t();
            Assert.IsTrue(H5F.get_info(m_test_file, ref info) >= 0);

            // check intent
            uint intent = 4711;
            Assert.IsTrue(H5F.get_intent(m_test_file, ref intent) >= 0);
            Assert.IsTrue(intent == H5F.ACC_RDWR);

            // check object count
            Assert.IsTrue(H5F.get_obj_count(m_test_file, H5F.OBJ_ALL) > 0);

            // retrieve object handles
            IntPtr buf = H5.allocate_memory(new IntPtr(10 * sizeof(hid_t)), 0);
            Assert.IsTrue(
                H5F.get_obj_ids(m_test_file, H5F.OBJ_ALL, new IntPtr(10),
                buf) > 0);
            Assert.IsTrue(H5.free_memory(buf) >= 0);

            // this is an HDF5 file
            Assert.IsTrue(H5F.is_hdf5(m_test_file_name) > 0);
        }
    }
}
