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

namespace UnitTests
{
    public partial class H5Test
    {
        [TestMethod]
        public void H5is_library_threadsafeTest1()
        {
            uint majnum = 0, minnum = 0, relnum = 0;
            Assert.IsTrue(
                H5.get_libversion(ref majnum, ref minnum, ref relnum) >= 0);
            Assert.IsTrue(majnum == 1);
            Assert.IsTrue(minnum >= 8);

            if ((minnum == 8 && relnum >= 16) || majnum >= 10)
            {
                uint is_ts = 0; 
                Assert.IsTrue(H5.is_library_threadsafe(ref is_ts) >= 0);
            }
        }
    }
}