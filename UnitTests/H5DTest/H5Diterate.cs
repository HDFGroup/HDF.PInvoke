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
    public partial class H5DTest
    {
        [TestMethod]
        public void H5DiterateTest1()
        {
            int[] buf = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            IntPtr count_ptr = Marshal.AllocHGlobal(sizeof(int));
            Marshal.WriteInt32(count_ptr, 0);
            
            hsize_t[] dims = { 10 };
            hid_t space = H5S.create_simple(1, dims, null);
            Assert.IsTrue(space >= 0);
            Assert.IsTrue(H5S.select_all(space) >= 0);

            GCHandle buf_hnd = GCHandle.Alloc(buf, GCHandleType.Pinned);
            
            H5D.operator_t cb = DelegateMethod;
            Assert.IsTrue(
                H5D.iterate(buf_hnd.AddrOfPinnedObject(), H5T.NATIVE_INT,
                space, cb, count_ptr) >= 0);

            buf_hnd.Free();

            int count = Marshal.ReadInt32(count_ptr);
            // expect the sum of the buffer elements
            Assert.IsTrue(count == 45);

            Assert.IsTrue(H5S.close(space) >= 0);
            Marshal.FreeHGlobal(count_ptr);
        }
    }
}