################################################################################
# Expect a CSV file name as input. We care ONLY about the first column, which
# we assume contains the paroperty names.
################################################################################

param ([Parameter(Mandatory=$true)][string] $fileName)

################################################################################
# The copyright notice
################################################################################

$copyright = @"
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

"@

################################################################################
# The class preamble
################################################################################

$prefix = @"
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using hbool_t = System.Int32;
using herr_t = System.Int32;
using hsize_t = System.UInt64;
using htri_t = System.Int32;
using size_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed partial class H5T
    {
        static H5DLLImporter m_importer;
"@

################################################################################
# Dot the i's and cross the t's
################################################################################

$suffix = @"
    }
}
"@

################################################################################
# The property template
################################################################################

$template = @"

        static hid_t? PROPERTY_g;

        public static hid_t PROPERTY 
        {
            get
            {
                if (!PROPERTY_g.HasValue)
                {
                    hid_t val = -1;
                    if (m_importer.GetValue<hid_t>(Constants.DLLFileName,
                        "PROPERTY_g", ref val,
#if HDF5_VER1_10
                        Marshal.ReadInt64
#else
                        Marshal.ReadInt32
#endif
                        ))
                    {
                        PROPERTY_g = val;
                    }
                }
                return PROPERTY_g.GetValueOrDefault();
            }
        }
"@

function Make-Property
{
  param ([Parameter(Mandatory=$true)][string] $propertyName)
  $template -replace 'PROPERTY',$propertyName
}

################################################################################
# print the file to stdout
################################################################################

$copyright

$prefix

$props = Get-Content $fileName

foreach ($item in $props) {
    Make-Property $item.split(',')[0]
}

$suffix
