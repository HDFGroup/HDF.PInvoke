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

using hsize_t = System.UInt64;

#if HDF5_VER1_10
using hid_t = System.Int64;

namespace UnitTests
{
    public partial class H5PTest
    {
        [TestMethod]
        public void H5Pget_mdc_image_configTest1()
        {
            hid_t fapl = H5P.create(H5P.FILE_ACCESS);
            Assert.IsTrue(fapl >= 0);
            H5AC.cache_image_config_t conf =
                new H5AC.cache_image_config_t();
            conf.version = H5AC.CURR_CACHE_IMAGE_CONFIG_VERSION;
            IntPtr config_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(conf));
            Marshal.StructureToPtr((H5AC.cache_image_config_t)conf,
                config_ptr, false);
            Assert.IsTrue(H5P.get_mdc_image_config(fapl, config_ptr) >= 0);
            Assert.IsTrue(H5P.close(fapl) >= 0);
            Marshal.FreeHGlobal(config_ptr);
        }
    }
}

#endif