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

using herr_t = System.Int32;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5TTest
    {
        [TestMethod]
        public void H5Tget_tagTest1()
        {
            hid_t dtype = H5T.create(H5T.class_t.OPAQUE, new IntPtr(1024));
            Assert.IsTrue(dtype >= 0);

            Assert.IsTrue(
                H5T.set_tag(dtype, "Mary had a little lamb...") >= 0);

            IntPtr tag = H5T.get_tag(dtype);
            Assert.IsTrue(tag.ToInt64() >= 0);

            Assert.IsTrue(Marshal.PtrToStringAnsi(tag)
                == "Mary had a little lamb...");

            Assert.IsTrue(H5.free_memory(tag) >= 0);

            Assert.IsTrue(H5T.close(dtype) >= 0);
        }

        [TestMethod]
        public void H5Tget_tagTest2()
        {
            IntPtr tag = H5T.get_tag(Utilities.RandomInvalidHandle());
            Assert.IsTrue(tag == IntPtr.Zero);
        }
    }
}