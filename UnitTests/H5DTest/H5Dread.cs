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

namespace UnitTests
{
    public partial class H5DTest
    {
        [TestMethod]
        public void H5DreadTest1()
        {
            byte[] rdata = new byte[512];

            hid_t mem_type = H5T.copy(H5T.C_S1);
            Assert.IsTrue(H5T.set_size(mem_type, new IntPtr(2)) >= 0);

            GCHandle hnd = GCHandle.Alloc(rdata, GCHandleType.Pinned);
            Assert.IsTrue(H5D.read(m_v0_ascii_dset, mem_type, H5S.ALL,
                H5S.ALL, H5P.DEFAULT, hnd.AddrOfPinnedObject()) >= 0);
            for (int i = 0; i < 256; ++i)
            {
                // H5T.FORTRAN_S1 is space (= ASCII dec. 32) padded
                if (i != 32) 
                {
                    Assert.IsTrue(rdata[2 * i] == (byte)i);
                }
                else
                {
                    Assert.IsTrue(rdata[64] == (byte)0);
                }
                Assert.IsTrue(rdata[2 * i + 1] == (byte)0);
            }
            hnd.Free();

            Assert.IsTrue(H5T.close(mem_type) >= 0);
        }
    }
}