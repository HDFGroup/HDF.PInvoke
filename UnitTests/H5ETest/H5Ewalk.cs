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
        public void H5EwalkTest1()
        {
            H5E.auto_t auto_cb = ErrorDelegateMethod;
            Assert.IsTrue(
                H5E.set_auto(H5E.DEFAULT, auto_cb, IntPtr.Zero) >= 0);

            H5E.walk_t walk_cb = WalkDelegateMethod;
            IntPtr client_data = IntPtr.Zero;
            Assert.IsTrue(
                H5E.walk(H5E.DEFAULT, H5E.direction_t.H5E_WALK_DOWNWARD,
                walk_cb, IntPtr.Zero) >= 0);
        }

        [TestMethod]
        public void H5EwalkTest2()
        {
            H5E.auto_t auto_cb = ErrorDelegateMethod;
            Assert.IsTrue(
                H5E.set_auto(H5E.DEFAULT, auto_cb, IntPtr.Zero) >= 0);

            H5E.walk_t walk_cb = WalkDelegateMethod;
            IntPtr client_data = IntPtr.Zero;

            Assert.IsTrue(
                H5E.push(H5E.DEFAULT, "hello.c", "sqrt", 77, H5E.ERR_CLS,
                H5E.NONE_MAJOR, H5E.NONE_MINOR, "Hello, World!") >= 0);

            Assert.IsTrue(
                H5E.push(H5E.DEFAULT, "hello.c", "sqr", 78, H5E.ERR_CLS,
                H5E.NONE_MAJOR, H5E.NONE_MINOR, "Hello, World!") >= 0);

            Assert.IsTrue(
                H5E.walk(H5E.DEFAULT, H5E.direction_t.H5E_WALK_DOWNWARD,
                walk_cb, IntPtr.Zero) >= 0);
        }

        public herr_t ErrorDelegateMethod
            (
            hid_t estack,
            IntPtr client_data
            )
        {
            return 0;
        }

        public herr_t WalkDelegateMethod
            (
            uint n,
            ref H5E.error_t err_desc,
            IntPtr client_data
            )
        {
            Assert.IsTrue(err_desc.line > 0);
            return 0;
        }
    }
}
