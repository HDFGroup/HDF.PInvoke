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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using herr_t = System.Int32;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

#if HDF5_VER1_10

using hid_t = System.Int64;

namespace UnitTests
{
    public partial class H5VDSTest
    {
        [TestMethod]
        public void H5Pget_virtual_dsetnameTestVDS1()
        {
            hid_t vds = H5D.open(m_vds_class_file, "VDS");
            Assert.IsTrue(vds >= 0);

            hid_t dcpl = H5D.get_create_plist(vds);
            Assert.IsTrue(dcpl >= 0);

            IntPtr count = IntPtr.Zero;
            Assert.IsTrue(H5P.get_virtual_count(dcpl, ref count) >= 0);
            Assert.IsTrue(3 == count.ToInt32());

            string[] names = { "A", "B", "C" };

            for (int i = 0; i < count.ToInt32(); ++i)
            {
                size_t index = new ssize_t(i);
                ssize_t len = H5P.get_virtual_filename(dcpl, index, null,
                    IntPtr.Zero);
                Assert.IsTrue(len.ToInt32() > 0);
                StringBuilder name = new StringBuilder(len.ToInt32()+1);
                len = H5P.get_virtual_dsetname(dcpl, index, name, len+1);
                Assert.IsTrue(len.ToInt32() > 0);
                Assert.IsTrue(name.ToString() == names[i]);
            }

            Assert.IsTrue(H5P.close(dcpl) >= 0);
            Assert.IsTrue(H5D.close(vds) >= 0);
        }
    }
}

#endif