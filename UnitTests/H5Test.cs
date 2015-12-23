using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

namespace UnitTests
{
    [TestClass]
    public class H5Test
    {
        [TestMethod]
        public void AllocationTest()
        {
            Assert.IsTrue(H5.open() >= 0);

            IntPtr size = new IntPtr(1024*1024);

            // uninitialized allocation
            IntPtr ptr = H5.allocate_memory(size, 0);
            Assert.IsFalse(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);

            // initialize with zeros
            ptr = H5.allocate_memory(size, 1);
            Assert.IsFalse(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);

            // size = 0 -> NULL return
            size = new IntPtr(0);
            ptr = H5.allocate_memory(size, 0);
            Assert.IsTrue(ptr == IntPtr.Zero);
            Assert.IsTrue(H5.free_memory(ptr) >= 0);

            Assert.IsTrue(H5.close() >= 0);
        }

        [TestMethod]
        public void FreeListTest()
        {
            Assert.IsTrue(H5.open() >= 0);

            Assert.IsTrue(
                H5.set_free_list_limits(-1, -1, -1, -1, -1, -1) >= 0);

            Assert.IsTrue(
                H5.set_free_list_limits(1024, -1, 4096, -1, -1, 1024) >= 0);

            Assert.IsTrue(H5.close() >= 0);
        }

        [TestMethod]
        public void OpenAndCloseTest()
        {
            Assert.IsTrue(H5.open() >= 0);
            Assert.IsTrue(H5.close() >= 0);
        }

        [TestMethod]
        public void ReallocationTest()
        {
            Assert.IsTrue(H5.open() >= 0);

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

            Assert.IsTrue(H5.garbage_collect() >= 0);

            Assert.IsTrue(H5.close() >= 0);
        }

        [TestMethod]
        public void VersionTest()
        {
            Assert.IsTrue(H5.open() >= 0);

            uint majnum = 0, minnum = 0, relnum = 0;
            Assert.IsTrue(
                H5.get_libversion(ref majnum, ref minnum, ref relnum) >= 0);
            Assert.IsTrue(majnum == 1);
            Assert.IsTrue(minnum == 8);

            if (relnum >= 16)
            {
                uint is_ts = 0;
                Assert.IsTrue(H5.is_library_threadsafe(ref is_ts) >= 0);
            }

            Assert.IsTrue(H5.check_version(majnum, minnum, relnum) >= 0);
            // The following should fail (?), but doesn't. (HDFFV-9637)
            //Assert.IsTrue(H5.check_version(0, 0, 0) >= 0);

            Assert.IsTrue(H5.close() >= 0);
        }
    }
}