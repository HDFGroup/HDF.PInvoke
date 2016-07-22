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

using hbool_t = System.UInt32;
using herr_t = System.Int32;
using hsize_t = System.UInt64;


#if HDF5_VER1_10

using hid_t = System.Int64;

namespace UnitTests
{
    public partial class H5SWMRTest
    {
        [TestMethod]
        public void H5Pget_chunk_optsTestSWMR1()
        {
            hid_t dcpl = H5P.create(H5P.DATASET_CREATE);
            Assert.IsTrue(dcpl >= 0);

            // without chunking, H5Pset_chunk_opts will throw an error
            hsize_t[] dims = { 4711 };
            Assert.IsTrue(H5P.set_chunk(dcpl, 1, dims) >= 0);

            uint opts = H5D.DONT_FILTER_PARTIAL_CHUNKS;
            Assert.IsTrue(H5P.set_chunk_opts(dcpl, opts) >= 0);

            opts = 4711;
            Assert.IsTrue(H5P.get_chunk_opts(dcpl, ref opts) >= 0);

            Assert.IsTrue(opts == H5D.DONT_FILTER_PARTIAL_CHUNKS);
            
            Assert.IsTrue(H5P.close(dcpl) >= 0);
        }
    }
}

#endif