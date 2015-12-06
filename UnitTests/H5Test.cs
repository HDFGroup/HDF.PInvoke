using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

namespace UnitTests
{
    [TestClass]
    public class H5Test
    {
        [TestMethod]
        public void OpenAndCloseTest()
        {
            Assert.IsTrue(H5.open() >= 0);
            Assert.IsTrue(H5.close() >= 0);
        }

        [TestMethod]
        public void AllocationTest()
        {
            Assert.IsTrue(H5.open() >= 0);

            UIntPtr size = new UIntPtr(1024*1024);

            // uninitialized allocation
            IntPtr ptr = H5.allocate_memory(size, 0);
            Assert.IsFalse(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);

            // initialize with zeros
            ptr = H5.allocate_memory(size, 1);
            Assert.IsFalse(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);

            // size = 0 -> NULL return
            size = new UIntPtr(0);
            ptr = H5.allocate_memory(size, 0);
            Assert.IsTrue(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);

            Assert.IsTrue(H5.close() >= 0);
        }

        [TestMethod]
        public void ReallocationTest()
        {
            Assert.IsTrue(H5.open() >= 0);

            UIntPtr size = new UIntPtr(1024 * 1024);

            // uninitialized allocation
            IntPtr ptr = H5.allocate_memory(size, 0);
            Assert.IsFalse(ptr == IntPtr.Zero);

            // reallocate
            size = new UIntPtr(1024 * 1024 * 10);
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
            size = new UIntPtr(0);
            ptr1 = H5.resize_memory(ptr, size);
            Assert.IsTrue(ptr1 == IntPtr.Zero);

            // H5resize_memory(NULL, 0)	Returns NULL (undefined in C standard).
            size = new UIntPtr(0);
            Assert.IsTrue(H5.resize_memory(IntPtr.Zero, size) == IntPtr.Zero);

            Assert.IsTrue(H5.close() >= 0);
        }
    }
}