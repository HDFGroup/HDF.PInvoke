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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5RTest
    {
        [TestMethod]
        public void H5Rget_nameTest1()
        {
            byte[] path =
                Encoding.UTF8.GetBytes(String.Join("/", m_utf8strings));
            // make room for the trailling \0
            byte[] name = new byte [path.Length + 1];
            Array.Copy(path, name, path.Length);

            Assert.IsTrue(
                H5G.close(H5G.create(m_v0_test_file, path, m_lcpl_utf8)) >= 0);

            byte[] refer = new byte [H5R.OBJ_REF_BUF_SIZE];
            GCHandle hnd = GCHandle.Alloc(refer, GCHandleType.Pinned);

            Assert.IsTrue(
                H5R.create(hnd.AddrOfPinnedObject(), m_v0_test_file, name,
                H5R.type_t.OBJECT, -1) >= 0);

            ssize_t size = H5R.get_name(m_v0_test_file, H5R.type_t.OBJECT,
                hnd.AddrOfPinnedObject(), (byte[])null, IntPtr.Zero);
            Assert.IsTrue(size.ToInt32() == name.Length);

            // size does not include the trailling \0
            byte[] buf = new byte[size.ToInt32()+1];
            size = H5R.get_name(m_v0_test_file, H5R.type_t.OBJECT,
                hnd.AddrOfPinnedObject(), buf, new IntPtr(buf.Length));
            Assert.IsTrue(size.ToInt32() == name.Length);

            hnd.Free();

            // we need to account for the leading "/", which was not included
            // in path
            for (int i = 0; i < name.Length; ++i)
            {
                Assert.IsTrue(name[i] == buf[i+1]);
            }
        }
    }
}
