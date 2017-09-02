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

        /// <summary>
        /// Creates and writes a dataset of type 'character' (<see cref="H5T.NATIVE_CHAR"/>).
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset_char
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">An array of the size of each dimension.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset_char",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset_char(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, int rank, [MarshalAs(UnmanagedType.LPArray)] hsize_t[] dims, IntPtr buffer);

        /// <summary>
        /// Creates and writes a dataset of type 'short signed integer' (<see cref="H5T.NATIVE_SHORT"/>).
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset_short
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">An array of the size of each dimension.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset_short",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset_short(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, int rank, [MarshalAs(UnmanagedType.LPArray)] hsize_t[] dims, IntPtr buffer);

        /// <summary>
        /// Creates and writes a dataset of type 'native signed integer' (<see cref="H5T.NATIVE_INT"/>).
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset_int
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">An array of the size of each dimension.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset_int",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset_int(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, int rank, [MarshalAs(UnmanagedType.LPArray)] hsize_t[] dims, IntPtr buffer);

        /// <summary>
        /// Creates and writes a dataset of type 'long signed integer' (<see cref="H5T.NATIVE_LONG"/>).
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset_long
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">An array of the size of each dimension.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset_long",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset_long(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, int rank, [MarshalAs(UnmanagedType.LPArray)] hsize_t[] dims, IntPtr buffer);

        /// <summary>
        /// Creates and writes a dataset of type 'native floating point' (<see cref="H5T.NATIVE_FLOAT"/>).
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset_float
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">An array of the size of each dimension.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset_float",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset_float(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, int rank, [MarshalAs(UnmanagedType.LPArray)] hsize_t[] dims, IntPtr buffer);

        /// <summary>
        /// Creates and writes a dataset of type 'native floating-point double' (<see cref="H5T.NATIVE_DOUBLE"/>).
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset_double
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">An array of the size of each dimension.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset_double",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset_double(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, int rank, [MarshalAs(UnmanagedType.LPArray)] hsize_t[] dims, IntPtr buffer);

        /// <summary>
        /// Creates and writes a dataset of type 'C string' (<see cref="H5T.C_S1"/>).
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTmake_dataset_string
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to create.</param>
        /// <param name="buffer">Buffer with data to be written to the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTmake_dataset_string",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t make_dataset_string(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Reads a dataset from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="type_id">Identifier of the datatype to use when reading the dataset.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, hid_t type_id, IntPtr buffer);

        /// <summary>
        /// Reads a dataset of type 'character' (<see cref="H5T.NATIVE_CHAR"/>) from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset_char
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset_char",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset_char(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Reads a dataset of type 'short signed integer' (<see cref="H5T.NATIVE_SHORT"/>) from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset_short
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset_short",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset_short(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Reads a dataset of type 'native signed integer' (<see cref="H5T.NATIVE_INT"/>) from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset_int
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset_int",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset_int(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Reads a dataset of type 'long signed integer' (<see cref="H5T.NATIVE_LONG"/>) from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset_long
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset_long",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset_long(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Reads a dataset of type 'native floating point' (<see cref="H5T.NATIVE_FLOAT"/>) from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset_float
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset_float",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset_float(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Reads a dataset of type 'native floating point double' (<see cref="H5T.NATIVE_DOUBLE"/>) from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset_double
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset_double",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset_double(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Reads a dataset of type 'C string' (<see cref="H5T.C_S1"/>) from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTread_dataset_string
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to read the dataset within.</param>
        /// <param name="dset_name">The name of the dataset to read.</param>
        /// <param name="buffer">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTread_dataset_string",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t read_dataset_string(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr buffer);

        /// <summary>
        /// Determines whether a dataset exists.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTfind_dataset
        /// </summary>
        /// <param name="loc_id">Identifier of the group containing the dataset.</param>
        /// <param name="dset_name">Dataset name.</param>
        /// <returns>Returns 1 if the dataset exists, returns 0 otherwise.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTfind_dataset",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t find_dataset(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name);

        /// <summary>
        /// Gets the dimensionality of a dataset.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_dataset_ndims
        /// </summary>
        /// <param name="loc_id">Identifier of the object to locate the dataset within.</param>
        /// <param name="dset_name">Dataset name.</param>
        /// <param name="rank">The dimensionality of the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_dataset_ndims",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_dataset_ndims(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, out int rank);

        /// <summary>
        /// Gets information about a dataset.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_dataset_info
        /// </summary>
        /// <param name="loc_id">Identifier of the object to locate the dataset within.</param>
        /// <param name="dset_name">Dataset name.</param>
        /// <param name="dims">The dimensions of the dataset.</param>
        /// <param name="class_id">The class identifier.</param>
        /// <param name="type_size">The size of the datatype in bytes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_dataset_info",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_dataset_info(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name, IntPtr /* hsize_t* */ dims, out H5T.class_t class_id, out size_t type_size);

        /// <summary>
        /// Creates and writes a string attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_string
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="attr_data">Buffer with data to be written to the attribute.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_string",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_string(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, [MarshalAs(UnmanagedType.LPStr)] string attr_data);
    }
}
