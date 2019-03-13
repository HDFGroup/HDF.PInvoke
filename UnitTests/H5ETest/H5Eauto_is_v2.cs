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

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5ETest
    {
        [TestMethod]
        public void H5Eauto_is_v2Test1()
        {
            hid_t est = H5E.create_stack();
            Assert.IsTrue(est >= 0);
            uint version = 0;
            Assert.IsTrue(H5E.auto_is_v2(est, ref version) >= 0);
            Assert.IsTrue(version == 1);
            Assert.IsTrue(H5E.close_stack(est) >= 0);
        }

        [TestMethod]
        public void H5Eauto_is_v2Test2()
        {
            uint version = 0;
            Assert.IsFalse(
                H5E.auto_is_v2(Utilities.RandomInvalidHandle(), ref version) >= 0);
        }
    }
}
