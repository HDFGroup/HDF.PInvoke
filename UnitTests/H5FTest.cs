using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hid_t = System.Int32;
using hsize_t = System.UInt64;
using hssize_t = System.Int64;

namespace UnitTests
{
    [TestClass]
    public class H5FTest
    {
        [TestMethod]
        public void CreationTest()
        {
            Assert.IsTrue(H5.open() >= 0);

            string fname = Path.GetTempFileName();
            // this is not an HDF5 file
            Assert.IsTrue(H5F.is_hdf5(fname) == 0);

            hid_t fid = H5F.create
                (fname, H5F.ACC_TRUNC, H5P.H5P_DEFAULT, H5P.H5P_DEFAULT);
            Assert.IsTrue(fid > 0);

            hsize_t size = 0;
            Assert.IsTrue(H5F.get_filesize(fid, ref size) >= 0);

            // get access properties
            hid_t plist = H5F.get_access_plist(fid);
            Assert.IsTrue(plist >= 0);
            Assert.IsTrue(H5P.close(plist) >= 0);

            // get creation properties
            plist = H5F.get_create_plist(fid);
            Assert.IsTrue(plist >= 0);
            Assert.IsTrue(H5P.close(plist) >= 0);

            // flush the file
            Assert.IsTrue(H5F.flush(fid, H5F.scope_t.H5F_SCOPE_GLOBAL) >= 0);

            // check 4 free space
            Assert.IsTrue(H5F.get_freespace(fid) >= 0);

            // check file info
            H5F.info_t info = new H5F.info_t();
            Assert.IsTrue(H5F.get_info(fid, ref info) >= 0);

            // check intent
            uint intent = 4711;
            Assert.IsTrue(H5F.get_intent(fid, ref intent) >= 0);
            Assert.IsTrue(intent == H5F.ACC_RDWR);

            // check object count
            Assert.IsTrue(H5F.get_obj_count(fid, H5F.OBJ_ALL) > 0);

            // retrieve object handles
            IntPtr buf = H5.allocate_memory(new IntPtr(10*sizeof(hid_t)), 0);
            Assert.IsTrue(
                H5F.get_obj_ids(fid, H5F.OBJ_ALL, new IntPtr(10), buf) > 0);
            Assert.IsTrue(H5.free_memory(buf) >= 0);

            Assert.IsTrue(H5F.close(fid) >= 0);
            // this is an HDF5 file
            Assert.IsTrue(H5F.is_hdf5(fname) > 0);

            Assert.IsTrue(H5.close() >= 0);
        }

        [TestMethod]
        public void FileNameTest()
        {
            Assert.IsTrue(H5.open() >= 0);

            string fname = Path.GetTempFileName();
            
            hid_t fid = H5F.create
                (fname, H5F.ACC_TRUNC, H5P.H5P_DEFAULT, H5P.H5P_DEFAULT);
            Assert.IsTrue(fid > 0);

            IntPtr buf = H5.allocate_memory(new IntPtr(256), 0);

            Assert.IsTrue(H5F.get_name(fid, buf, new IntPtr(255)) >= 0);

            string name = Marshal.PtrToStringAnsi(buf);
            // names should match
            Assert.AreEqual(fname, name);

            Assert.IsTrue(H5.free_memory(buf) >= 0);
            Assert.IsTrue(H5F.close(fid) >= 0);
            Assert.IsTrue(H5.close() >= 0);
        }
    }
}