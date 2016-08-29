################################################################################
# Create a partial class for the unmanaged global variables from a list of
# variable names. Write the class definition to standard output.
# Expect the API name [H5E, H5P, H5T] and a CSV file name as input. We care
# ONLY about the first column of the CSV file, which we assume contains the
# property names.
#
# Example:
#
#   .\Generate-H5globals.ps1 H5T ..\HDF5\H5Tglobals.csv ..\HDF5\H5Tgobal_aliases.csv > ..\HDF5\H5Tglobals.cs
#
################################################################################

param (
    [Parameter(Mandatory=$true)][ValidateSet('H5E','H5P','H5T')][string] $api,
    [Parameter(Mandatory=$true)][string] $fileName,
    [Parameter(Mandatory=$true)][string] $aliasName
)

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
    public unsafe sealed partial class API
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

        static hid_t? @INTERNAL@;

        public static hid_t @PROPERTY@ 
        {
            get
            {
                if (!@INTERNAL@.HasValue)
                {
                    hid_t val = -1;
                    if (m_importer.GetValue<hid_t>(Constants.DLLFileName,
                        "@SYMBOL@", ref val,
#if HDF5_VER1_10
                        Marshal.ReadInt64
#else
                        Marshal.ReadInt32
#endif
                        ))
                    {
                        @INTERNAL@ = val;
                    }
                }
                return @INTERNAL@.GetValueOrDefault();
            }
        }
"@

function Make-Property
{
  param (
      [Parameter(Mandatory=$true)][string] $propertyName)

    $property = $propertyName
    $internal = "$($api)_$($propertyName)_g"
    $symbol = "$($api)_$($propertyName)_g"

    if ($api -eq 'H5P')
    {
        if ($propertyName.StartsWith("CLS"))
        {
            $property = $propertyName.Substring(4)
        }
        $symbol = "$($api)_$($propertyName)_ID_g"
    }

    ($template -replace 'API',$api) `
        -replace '@PROPERTY@',$property `
        -replace '@INTERNAL@',$internal `
        -replace '@SYMBOL@',$symbol
  
}

$alias_template = @"

        public static hid_t @PROPERTY@ 
        {
            get
            {
                if (!@INTERNAL@.HasValue)
                {
                    hid_t val = -1;
                    if (m_importer.GetValue<hid_t>(Constants.DLLFileName,
                        "@SYMBOL@", ref val,
#if HDF5_VER1_10
                        Marshal.ReadInt64
#else
                        Marshal.ReadInt32
#endif
                        ))
                    {
                        @INTERNAL@ = val;
                    }
                }
                return @INTERNAL@.GetValueOrDefault();
            }
        }
"@

function Make-Alias
{
  param (
      [Parameter(Mandatory=$true)][string] $aliasName,
      [Parameter(Mandatory=$true)][string] $propertyName)

    $internal = "$($api)_$($propertyName)_g"
    $symbol = "$($api)_$($propertyName)_g"
    $alias = $aliasName

    ($alias_template -replace 'API',$api) `
        -replace '@PROPERTY@',$alias `
        -replace '@INTERNAL@',$internal `
        -replace '@SYMBOL@',$symbol
  
}

################################################################################
# print the file to stdout
################################################################################

$copyright

$prefix -replace 'API',$api

$props = Get-Content $fileName

foreach ($item in $props) {
    Make-Property $item.split(',')[0]
}

$aliases = Get-Content $aliasName

foreach ($item in $aliases) {
    $a = $item.split(',')
    Make-Alias $a[0] $a[1]
}

$suffix
