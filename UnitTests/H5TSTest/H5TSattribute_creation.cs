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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hbool_t = System.UInt32;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace UnitTests
{
    public partial class H5TSTest
    {
        private void AttributeCreateProcedure()
        {
            string name = Thread.CurrentThread.Name;
            hid_t space = H5S.create(H5S.class_t.NULL);
            Assert.IsTrue(space >= 0);

            Assert.IsTrue(
                H5A.close(H5A.create(m_shared_file_id, name, H5T.STD_I32BE,
                space)) >= 0);

            Assert.IsTrue(H5S.close(space) >= 0);
        }

        [TestMethod]
        public void H5TSattribute_creationTest1()
        {
            // run only if we have a thread-safe build of the library
            hbool_t flag = 0;
            Assert.IsTrue(H5.is_library_threadsafe(ref flag) >= 0);
            if (flag > 0)
            {
                // Create the new Thread and use the FileCreateProcedure method
                Thread1 = new Thread(new ThreadStart(AttributeCreateProcedure));
                Thread1.Name = "Thread1";
                Thread2 = new Thread(new ThreadStart(AttributeCreateProcedure));
                Thread2.Name = "Thread2";
                Thread3 = new Thread(new ThreadStart(AttributeCreateProcedure));
                Thread3.Name = "Thread3";
                Thread4 = new Thread(new ThreadStart(AttributeCreateProcedure));
                Thread4.Name = "Thread4";

                // Start running the thread
                Thread4.Start();
                Thread2.Start();
                Thread1.Start();
                Thread3.Start();

                // Join the independent thread to this thread to wait until
                // AttributeCreateProcedure ends
                Thread1.Join();
                Thread2.Join();
                Thread3.Join();
                Thread4.Join();
            }
        }
    }
}
