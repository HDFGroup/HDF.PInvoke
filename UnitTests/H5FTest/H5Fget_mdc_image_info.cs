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

using haddr_t = System.UInt64;
using hsize_t = System.UInt64;

#if HDF5_VER1_10

namespace UnitTests
{
    public partial class H5FTest
    {
        [TestMethod]
        public void H5Fget_mdc_image_infoTest1()
        {
            haddr_t image_addr = 111;
            hsize_t image_size = 222;

            Assert.IsTrue(H5F.get_mdc_image_info(m_v2_class_file,
                ref image_addr,
                ref image_size) >= 0);
            Assert.IsTrue(image_addr == H5.HADDR_UNDEF);
            Assert.IsTrue(image_size == 0);
        }
    }
}

#endif