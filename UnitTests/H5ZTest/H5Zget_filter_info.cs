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

using herr_t = System.Int32;

namespace UnitTests
{
    public partial class H5ZTest
    {
        [TestMethod]
        public void H5Zget_filter_infoTest1()
        {
            uint filter_config = 0;
            Assert.IsTrue(H5Z.get_filter_info(H5Z.filter_t.DEFLATE,
                ref filter_config) >= 0);
            Assert.IsTrue((filter_config & H5Z.CONFIG_ENCODE_ENABLED) == 1);
            Assert.IsTrue((filter_config & H5Z.CONFIG_DECODE_ENABLED) == 2);
        }
    }
}
