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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

using herr_t = System.Int32;
using hsize_t = System.UInt64;
using size_t = System.IntPtr;
using hbool_t = System.UInt32;
using htri_t = System.Int32;

using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    /// <summary>
    /// H5LT: HDF5 Lite.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressUnmanagedCodeSecurity]
    public static class H5LT
    {
        static H5LT()
        {
            H5.open();
        }

        /// <summary>
        /// Determines whether an HDF5 path is valid and, optionally, whether the path resolves to an HDF5 object.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTpath_valid
        /// </summary>
        /// <param name="loc_id">An identifier of an object in the file.</param>
        /// <param name="path">The path to the object to check. Links in <paramref name="path"/> may be of any type.</param>
        /// <param name="check_object_valid">If TRUE, determine whether the final component of path resolves to an object; if FALSE, do not check.</param>
        /// <returns>
        /// Upon success:
        ///     If <paramref name="check_object_valid"/> is set to FALSE
        ///         Returns TRUE if the path is valid; otherwise returns FALSE.
        ///     If <paramref name="check_object_valid"/> is set to TRUE
        ///         Returns TRUE if the path is valid and resolves to an HDF5 object; otherwise returns FALSE.
        /// Upon error, returns a negative value. 
        /// </returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTpath_valid",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern htri_t path_valid(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string path, hbool_t check_object_valid);

        /// <summary>
        /// Opens an HDF5 file image in memory.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTopen_file_image
        /// </summary>
        /// <param name="buf_ptr">A pointer to the supplied initial image.</param>
        /// <param name="buf_size">Size of the supplied buffer.</param>
        /// <param name="flags">Flags specifying whether to open the image read-only or read/write, whether HDF5 is to take control of the buffer, and instruction regarding releasing the buffer.</param>
        /// <returns>Returns a file identifier if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTopen_file_image",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern hid_t open_file_image(IntPtr buf_ptr, size_t buf_size, uint flags);

        /// <summary>
        /// Creates and writes a dataset of a type <paramref name="type_id"/>.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">An array of the size of each dimension.</param>
        /// <param name="type_id">Identifier of the datatype to use when creating the dataset.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, int rank, [MarshalAs(UnmanagedType.LPArray)] hsize_t[] dims, hid_t type_id, IntPtr buffer);


    }
}
