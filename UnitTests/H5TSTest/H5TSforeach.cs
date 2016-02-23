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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private const int NUM_FILES = 256;

        private struct TaskLocals
        {
            public hid_t handle;
            public int runningLength;
        }

        [TestMethod]
        public void H5TSforeachTest1()
        {
            // run only if we have a thread-safe build of the library
            hbool_t flag = 0;
            Assert.IsTrue(H5.is_library_threadsafe(ref flag) >= 0);
            if (flag > 0)
            {
                List<string> ls = new List<string>();
                for (int i = 0; i < NUM_FILES; ++i)
                {
                    ls.Add(Path.GetTempFileName());
                }

                var totalLength = 0;

                Parallel.ForEach<string, TaskLocals>
                    (ls,
                    () =>
                    {
                        TaskLocals tl = new TaskLocals();
                        tl.handle = -1;
                        tl.runningLength = 0;
                        return tl;
                    },
                    (name, loop, taskLocals) =>
                    {
                        // handle is "thread-local"
                        taskLocals.handle = H5F.create(name, H5F.ACC_TRUNC);
                        Assert.IsTrue(H5F.close(taskLocals.handle) >= 0);
                        Assert.IsTrue(H5F.is_hdf5(name) > 0);
                        File.Delete(name);
                        taskLocals.handle = -1;
                        taskLocals.runningLength += name.Length;
                        return taskLocals;
                    },
                     (taskLocals) =>
                     {
                         Interlocked.Add(ref totalLength, taskLocals.runningLength);
                     }
                    );

                Assert.IsTrue(totalLength > NUM_FILES);
            }
        }
    }
}