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

using hid_t = System.Int32;
using hsize_t = System.UInt64;
using hssize_t = System.Int64;

namespace HDF.PInvoke
{
    public partial class H5FTest
    {
        [TestMethod]
        public void HDF5FileTest()
        {
            string fname = Path.GetTempFileName();
            // this is not an HDF5 file
            Assert.IsTrue(H5F.is_hdf5(fname) == 0);
        }

        [TestMethod]
        public void FileNameTest()
        {
            IntPtr buf = H5.allocate_memory(new IntPtr(256), 0);

            Assert.IsTrue(H5F.get_name(m_test_file, buf, new IntPtr(255)) >= 0);

            string name = Marshal.PtrToStringAnsi(buf);
            // names should match
            Assert.AreEqual(m_test_file_name, name);

            Assert.IsTrue(H5.free_memory(buf) >= 0);
        }
    }
}
