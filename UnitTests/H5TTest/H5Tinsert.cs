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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hsize_t = System.UInt64;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5TTest
    {
        [TestMethod]
        public void H5TinsertTest1()
        {
            // a fixed-length string type
            hid_t fls = H5T.create(H5T.class_t.STRING, new IntPtr(16));
            Assert.IsTrue(fls >= 0);
            Assert.IsTrue(H5T.is_variable_str(fls) == 0);

            // a variable-length string type
            hid_t vls = H5T.create(H5T.class_t.STRING, H5T.VARIABLE);
            Assert.IsTrue(vls >= 0);
            Assert.IsTrue(H5T.is_variable_str(vls) > 0);

            // a key-value compound
            IntPtr size = new IntPtr(16 + IntPtr.Size);
            hid_t kvt = H5T.create(H5T.class_t.COMPOUND, size);
            Assert.IsTrue(H5T.insert(kvt, "key", IntPtr.Zero, fls) >= 0);
            Assert.IsTrue(H5T.insert(kvt, "value", new IntPtr(16), vls) >= 0);
            Assert.IsTrue(H5T.close(vls) >= 0);
            Assert.IsTrue(H5T.close(fls) >= 0);

            // create a key-value dataset (3 elements)

            hid_t fsp = H5S.create_simple(1, new hsize_t[] { 3 }, null);
            Assert.IsTrue(fsp >= 0);

            hid_t dset = H5D.create(m_v2_class_file, "KeyVal", kvt, fsp);
            Assert.IsTrue(dset >= 0);
            Assert.IsTrue(H5S.close(fsp) >= 0);

            // write a 3 elements

            string[] keys = new string[] {
                "Key0123456789ABC", "Key0123456789DEF", "Key0123456789GHI"};

            IntPtr[] values = new IntPtr[3];
            values[0] = Marshal.StringToHGlobalAnsi("I am a managed String!");
            values[1] = Marshal.StringToHGlobalAnsi("I am also a managed String!");
            values[2] = Marshal.StringToHGlobalAnsi("I am another managed String!");

            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            for (int i = 0; i < 3; ++i)
            {
                writer.Write(Encoding.ASCII.GetBytes(keys[i]));
                if (IntPtr.Size == 8)
                {
                    writer.Write(values[i].ToInt64());
                }
                else
                {
                    writer.Write(values[i].ToInt32());
                }   
            }

            byte[] wdata = ms.ToArray();             
            GCHandle hnd = GCHandle.Alloc(wdata, GCHandleType.Pinned);

            Assert.IsTrue(H5D.write(dset, kvt, H5S.ALL, H5S.ALL, H5P.DEFAULT,
                hnd.AddrOfPinnedObject()) >= 0);

            hnd.Free();

            // now read it back

            byte[] rdata = new byte[3*size.ToInt32()];
            hnd = GCHandle.Alloc(rdata, GCHandleType.Pinned);

            Assert.IsTrue(H5D.read(dset, kvt, H5S.ALL, H5S.ALL, H5P.DEFAULT,
                 hnd.AddrOfPinnedObject()) >= 0);
            
            hnd.Free();

            // check it out

            MemoryStream ms1 = new MemoryStream(rdata);
            BinaryReader reader = new BinaryReader(ms1);

            for (int i = 0; i < 3; ++i)
            {
                string k = Encoding.ASCII.GetString(reader.ReadBytes(16));
                Assert.IsTrue(k == keys[i]);
                IntPtr ptr = IntPtr.Zero;
                if (IntPtr.Size == 8)
                {
                    ptr = new IntPtr(reader.ReadInt64());
                }
                else
                {
                    ptr = new IntPtr(reader.ReadInt32());
                }   
                string v = Marshal.PtrToStringAnsi(ptr);
                Assert.IsTrue(v == Marshal.PtrToStringAnsi(values[i]));
                Marshal.FreeHGlobal(ptr);
                Marshal.FreeHGlobal(values[i]);
            }

            Assert.IsTrue(H5D.close(dset) >= 0);
            Assert.IsTrue(H5T.close(kvt) >= 0);
        }
    }
}