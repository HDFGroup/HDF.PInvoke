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
using System.Text;
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

        #region Flag definitions for H5LTopen_file_image()

        /// <summary>
        /// Open image for read-write.
        /// </summary>
        public const uint H5LT_FILE_IMAGE_OPEN_RW = 0x0001u;

        /// <summary>
        /// The HDF5 lib won't copy user supplied image buffer.
        /// The same image is open with the core driver.
        /// </summary>
        public const uint H5LT_FILE_IMAGE_DONT_COPY = 0x0002u;

        /// <summary>
        /// The HDF5 lib won't deallocate user supplied image buffer.
        /// The user application is reponsible for doing so.
        /// </summary>
        public const uint H5LT_FILE_IMAGE_DONT_RELEASE = 0x0004u;

        public const uint H5LT_FILE_IMAGE_ALL = 0x0007u;

        #endregion

        public enum lang_t
        {
            /// <summary>
            /// this is the first
            /// </summary>
            LANG_ERR = -1,

            /// <summary>
            /// DDL (Data Definition Language).
            /// </summary>
            DDL = 0,

            /// <summary>
            /// C language.
            /// </summary>
            C = 1,

            /// <summary>
            /// FORTRAN language.
            /// </summary>
            FORTRAN = 2,

            /// <summary>
            /// this is the last
            /// </summary>
            NO_LANG = 3
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

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_char
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_char",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_char(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* sbyte* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_uchar
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_uchar",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_uchar(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* byte* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_short
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_short",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_short(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* short* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_ushort
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_ushort",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_ushort(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* ushort* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_int
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_int",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_int(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* int* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_uint
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_uint",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_uint(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* uint* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_long
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_long",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_long(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* long* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_ulong
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_ulong",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_ulong(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* ulong* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_long_long
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_long_long",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_long_long(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* long* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_float
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_float",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_float(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* float* */ buffer, hsize_t size);

        /// <summary>
        /// Creates and writes an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTset_attribute_double
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to create the attribute within.</param>
        /// <param name="obj_name">The name of the object to attach the attribute.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="buffer">Buffer with data to be written to the attribute.</param>
        /// <param name="size">
        /// The size of the 1D array (one in the case of a scalar attribute).
        /// This value is used by <c>H5Screate_simple</c> to create the dataspace.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTset_attribute_double",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t set_attribute_double(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* float* */ buffer, hsize_t size);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="mem_type_id">Identifier of the memory datatype.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, hid_t mem_type_id, IntPtr /* void* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_string
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_string",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_string(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* sbyte* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_char
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_char",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_char(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* sbyte* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_uchar
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_uchar",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_uchar(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* byte* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_short
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_short",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_short(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* short* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_ushort
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_ushort",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_ushort(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* ushort* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_int
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_int",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_int(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* int* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_uint
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_uint",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_uint(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* uint* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_long
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_long",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_long(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* long* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_long_long
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_long_long",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_long_long(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* long* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_ulong
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_ulong",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_ulong(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* ulong* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_float
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_float",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_float(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* float* */ data);

        /// <summary>
        /// Reads an attribute from disk.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_double
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="data">Buffer with data.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_double",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_double(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* double* */ data);

        /// <summary>
        /// Determines whether an attribute exists.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTfind_attribute
        /// </summary>
        /// <param name="loc_id">Identifier of the object to which the attribute is expected to be attached.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <returns>Returns 1 if the attribute exists; returns 0 otherwise.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTfind_attribute",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t find_attribute(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string attr_name);

        /// <summary>
        /// Gets the dimensionality of an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_ndims
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="rank">The dimensionality of the attribute.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_ndims",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_ndims(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, out int rank);

        /// <summary>
        /// Gets the dimensionality of an attribute.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTget_attribute_info
        /// </summary>
        /// <param name="loc_id">Identifier of the object (dataset or group) to read the attribute from.</param>
        /// <param name="obj_name">The name of the object that the attribute is attached to.</param>
        /// <param name="attr_name">The attribute name.</param>
        /// <param name="dims">The dimensions of the attribute.</param>
        /// <param name="type_class">The class identifier. To a list of the HDF5 class types please refer to the Datatype Interface API (H5T) help.</param>
        /// <param name="type_size">The size of the datatype in bytes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTget_attribute_info",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t get_attribute_info(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string obj_name,
            [MarshalAs(UnmanagedType.LPStr)] string attr_name, IntPtr /* hsize_t* */ dims, out H5T.class_t type_class, out size_t type_size);

        /// <summary>
        /// Creates an HDF5 datatype given a text description.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTtext_to_dtype
        /// </summary>
        /// <param name="text">A character string containing a DDL definition of the datatype to be created.</param>
        /// <param name="lang_type">The language used to describe the datatype. The only currently supported language is <see cref="lang_t.DDL"/>.</param>
        /// <returns>Returns the datatype identifier(non-negative) if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTtext_to_dtype",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern hid_t text_to_dtype([MarshalAs(UnmanagedType.LPStr)] string text, lang_t lang_type);

        /// <summary>
        /// Creates a text description of an HDF5 datatype.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5LT.html#H5LTdtype_to_text
        /// </summary>
        /// <param name="datatype">Identifier of the datatype to be converted.</param>
        /// <param name="str">Buffer for the text description of the datatype.</param>
        /// <param name="lang_type">The language used to describe the datatype. The currently supported language is <see cref="lang_t.DDL"/>.</param>
        /// <param name="len">The size of buffer needed to store the text description.</param>
        /// <returns>Returns non-negative if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5LTdtype_to_text",
             CallingConvention = CallingConvention.Cdecl),
         SecuritySafeCritical]
        public static extern herr_t dtype_to_text(hid_t datatype, [MarshalAs(UnmanagedType.LPStr)] StringBuilder str, lang_t lang_type, out size_t len);
    }
}
