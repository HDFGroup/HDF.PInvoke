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
        public void H5resize_memoryTest1()
        {
            IntPtr size = new IntPtr(1024 * 1024);

            // uninitialized allocation
            IntPtr ptr = H5.allocate_memory(size, 0);
            Assert.IsFalse(ptr == IntPtr.Zero);

            // reallocate
            size = new IntPtr(1024 * 1024 * 10);
            IntPtr ptr1 = H5.resize_memory(ptr, size);
            Assert.IsFalse(ptr1 == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr1) >= 0);

            // reallocate from NULL -> allocation
            ptr = H5.resize_memory(IntPtr.Zero, size);
            Assert.IsFalse(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);

            // reallocate to size zero -> free
            ptr = H5.allocate_memory(size, 1);
            Assert.IsFalse(ptr == IntPtr.Zero);
            size = new IntPtr(0);
            ptr1 = H5.resize_memory(ptr, size);
            Assert.IsTrue(ptr1 == IntPtr.Zero);

            // H5resize_memory(NULL, 0)	Returns NULL (undefined in C standard).
            size = new IntPtr(0);
            Assert.IsTrue(H5.resize_memory(IntPtr.Zero, size) == IntPtr.Zero);
        }

        [TestMethod]
        public void H5resize_memoryTest2()
        {
            IntPtr size = new IntPtr(1024 * 1024);

            // uninitialized allocation
            IntPtr ptr = H5.allocate_memory(size, 0);
            Assert.IsFalse(ptr == IntPtr.Zero);

            // reallocate
            size = new IntPtr(1024 * 1024 * 10);
            IntPtr ptr1 = Marshal.ReAllocHGlobal(ptr, size);
            Assert.IsFalse(ptr1 == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr1) >= 0);

            // reallocate to size zero -> free
            ptr = H5.allocate_memory(size, 1);
            Assert.IsFalse(ptr == IntPtr.Zero);
            size = new IntPtr(0);
            ptr1 = Marshal.ReAllocHGlobal(ptr, size);
            Assert.IsFalse(ptr1 == IntPtr.Zero);

            // H5resize_memory(NULL, 0)	Returns NULL (undefined in C standard).
            size = new IntPtr(0);
            //Assert.IsTrue(Marshal.ReAllocHGlobal(IntPtr.Zero, size) == IntPtr.Zero);
        }

        [TestMethod]
        [ExpectedException(typeof(OutOfMemoryException))]
        public void H5resize_memoryTest3()
        {
            IntPtr size = new IntPtr(1024 * 1024);

            // reallocate from NULL -> allocation
            IntPtr ptr = Marshal.ReAllocHGlobal(IntPtr.Zero, size);
            Assert.IsFalse(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);
        }

        [TestMethod]
        [ExpectedException(typeof(OutOfMemoryException))]
        public void H5resize_memoryTest4()
        {
            // H5resize_memory(NULL, 0)	Returns NULL (undefined in C standard).
            IntPtr size = new IntPtr(0);
            Assert.IsTrue(
                Marshal.ReAllocHGlobal(IntPtr.Zero, size) == IntPtr.Zero);
        }
    }
}