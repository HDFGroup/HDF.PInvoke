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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5FTest
    {
        [TestMethod]
        public void H5Fget_obj_idsTest1()
        {
            IntPtr buf = H5.allocate_memory(new IntPtr(10 * sizeof(hid_t)), 0);
            
            Assert.IsTrue(
                H5F.get_obj_ids(m_v0_class_file, H5F.OBJ_ALL, new IntPtr(10),
                buf).ToInt32() > 0);
            Assert.IsTrue(
                H5F.get_obj_ids(m_v2_class_file, H5F.OBJ_ALL, new IntPtr(10),
                buf).ToInt32() > 0);

            Assert.IsTrue(H5.free_memory(buf) >= 0);
           
        }

        [TestMethod]
        public void H5Fget_obj_idsTest2()
        {
            IntPtr buf = H5.allocate_memory(new IntPtr(10 * sizeof(hid_t)), 0);
            Assert.IsFalse(
                H5F.get_obj_ids(Utilities.RandomInvalidHandle(),
                H5F.OBJ_ALL, new IntPtr(10), buf).ToInt32() > 0);
            Assert.IsTrue(H5.free_memory(buf) >= 0);
        }
    }
}