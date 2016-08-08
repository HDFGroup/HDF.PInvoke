﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
using hid_t = System.Int32;

namespace HDF.PInvoke
{
    public static class Constants
    {
        public const string DLLFileName = "hdf5.dll";

        public const string HLDLLFileName = "hdf5_hl.dll";

        public const string DLL32bitPath = "bin32";
        
        public const string DLL64bitPath = "bin64";

        public const string MSVCRDllName = "msvcr120.dll";

        public const string MSVCRNotFoundErrorString =
            "This application requires the Visual C++ 2013 '{0}'" + 
            " Runtime to be installed on this computer." +
            " Please download and install it from" +
            " https://www.microsoft.com/en-US/download/details.aspx?id=40784"; 
    }
}
