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
using System.Security;

using haddr_t = System.UInt64;
using hbool_t = System.UInt32;
using herr_t = System.Int32;
using hid_t = System.Int32;
using hsize_t = System.UInt64;
using htri_t = System.Int32;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

namespace HDF.PInvoke
{
    public unsafe sealed class H5D
    {
        /// <summary>
        /// Values for the status of space allocation
        /// </summary>
        public enum space_status_t
        {
            H5D_SPACE_STATUS_ERROR = -1,
            H5D_SPACE_STATUS_NOT_ALLOCATED = 0,
            H5D_SPACE_STATUS_PART_ALLOCATED = 1,
            H5D_SPACE_STATUS_ALLOCATED = 2
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t gather_func_t
        (
        IntPtr dst_buf,
        size_t dst_buf_bytes_used,
        IntPtr op_data
        );

        /// <summary>
        /// Define the operator function pointer for H5Diterate()
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t operator_t
        (
        IntPtr      elem,
        hid_t       type_id,
        uint        ndim,
        ref hsize_t point,
        IntPtr      op_data
        );

        /// <summary>
        /// Define the operator function pointer for H5Dscatter()
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t scatter_func_t
        (
        ref IntPtr src_buf/*out*/,
        ref size_t src_buf_bytes_used/*out*/,
        IntPtr op_data
        );

        /// <summary>
        /// Closes the specified dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Close
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to close access to.
        /// </param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dclose",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t close(hid_t dset_id);

        /// <summary>
        /// Creates a new dataset and links it into the file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Create2
        /// </summary>
        /// <param name="loc_id">Location identifier</param>
        /// <param name="name">Dataset name</param>
        /// <param name="type_id">Datatype identifier</param>
        /// <param name="space_id">Dataspace identifier</param>
        /// <param name="lcpl_id">Link creation property list</param>
        /// <param name="dcpl_id">Dataset creation property list</param>
        /// <param name="dapl_id">Dataset access property list</param>
        /// <returns>Returns a dataset identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint="H5Dcreate2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create
            (hid_t loc_id, string name, hid_t type_id, hid_t space_id,
            hid_t lcpl_id = H5P.DEFAULT, hid_t dcpl_id = H5P.DEFAULT,
            hid_t dapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a dataset in a file without linking it into the file structure.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-CreateAnon
        /// </summary>
        /// <param name="file_id">Identifier of the file or group within which
        /// to create the dataset.</param>
        /// <param name="type_id">Identifier of the datatype to use when
        /// creating the dataset.</param>
        /// <param name="space_id">Identifier of the dataspace to use when
        /// creating the dataset.</param>
        /// <param name="dcpl_id">Dataset creation property list identifier.</param>
        /// <param name="dapl_id">Dataset access property list identifier.</param>
        /// <returns>Returns a dataset identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dcreate_anon",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create_anon
            (hid_t file_id, hid_t type_id, hid_t space_id,
            hid_t dcpl_id = H5P.DEFAULT, hid_t dapl_id = H5P.DEFAULT);

        /// <summary>
        /// Fills dataspace elements with a fill value in a memory buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Fill
        /// </summary>
        /// <param name="fill">Pointer to the fill value to be used.</param>
        /// <param name="fill_type">Fill value datatype identifier.</param>
        /// <param name="buf">Pointer to the memory buffer containing the
        /// selection to be filled.</param>
        /// <param name="buf_type">Datatype of dataspace elements to be filled.
        /// </param>
        /// <param name="space">Dataspace describing memory buffer and
        /// containing the selection to be filled.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dfill",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t fill
            (IntPtr fill, hid_t fill_type, IntPtr buf, hid_t buf_type,
            hid_t space);

        /// <summary>
        /// Gathers data from a selection within a memory buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Gather
        /// </summary>
        /// <param name="src_space_id">Identifier for the dataspace describing
        /// both the dimensions of the source buffer and the selection within
        /// the source buffer to gather data from.</param>
        /// <param name="src_buf">Source buffer which the data will be gathered
        /// from.</param>
        /// <param name="type_id"> Identifier for the datatype describing the
        /// data in both the source and definition buffers. This is only used
        /// to calculate the element size.</param>
        /// <param name="dst_buf_size">Size in bytes of
        /// <paramref name="dst_buf"/>.</param>
        /// <param name="dst_buf">Destination buffer where the gathered data
        /// will be placed.</param>
        /// <param name="op">Callback function which handles the gathered data.
        /// Optional if <paramref name="dst_buf"/> is large enough to hold all
        /// of the gathered data; required otherwise.</param>
        /// <param name="op_data">User-defined pointer to data required by
        /// <paramref name="op"/>.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dgather",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t gather
            (hid_t src_space_id, IntPtr src_buf, hid_t type_id,
            size_t dst_buf_size, IntPtr dst_buf, gather_func_t op,
            IntPtr op_data);

        /// <summary>
        /// Returns the dataset access property list associated with a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-GetAccessPlist
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to get access
        /// property list of.</param>
        /// <returns>Returns a dataset access property list identifier if
        /// Ssuccessful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dget_access_plist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_access_plist(hid_t dset_id);

        /// <summary>
        /// Returns an identifier for a copy of the dataset creation property
        /// list for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-GetCreatePlist
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to query.</param>
        /// <returns>Returns a dataset creation property list identifier if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dget_create_plist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_create_plist(hid_t dset_id);

        /// <summary>
        /// Returns dataset address in file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-GetOffset
        /// </summary>
        /// <param name="dset_id">Dataset identifier.</param>
        /// <returns>Returns the offset in bytes; otherwise returns
        /// <code>HADDR_UNDEF</code>, a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dget_offset",
           CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern haddr_t get_offset(hid_t dset_id);

        /// <summary>
        /// Returns an identifier for a copy of the dataspace for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-GetSpace
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to query.</param>
        /// <returns>Returns a dataspace identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dget_space",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_space(hid_t dset_id);

        /// <summary>
        /// Determines whether space has been allocated for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-GetSpaceStatus
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to query.</param>
        /// <param name="allocation">Space allocation status.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dget_space_status",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_space_status
            (hid_t dset_id, ref space_status_t allocation);

        /// <summary>
        /// Returns the amount of storage allocated for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-GetStorageSize
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to query.</param>
        /// <returns>Returns the amount of storage space, in bytes, allocated
        /// for the dataset, not counting metadata; otherwise returns 0 (zero).
        /// </returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dget_storage_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hsize_t get_storage_size(hid_t dset_id);

        /// <summary>
        /// Returns an identifier for a copy of the datatype for a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-GetType
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to query.</param>
        /// <returns>Returns a datatype identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dget_type",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_type(hid_t dset_id);

        /// <summary>
        /// Iterates over all selected elements in a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Iterate
        /// </summary>
        /// <param name="buf">Pointer to the buffer in memory containing the
        /// elements to iterate over</param>
        /// <param name="type_id">Datatype identifier for the elements stored
        /// in <code>buf</code></param>
        /// <param name="space_id">Dataspace identifier for <code>buf</code></param>
        /// <param name="op">Function pointer to the routine to be called for
        /// each element in buf iterated over</param>
        /// <param name="operator_data">Pointer to any user-defined data
        /// associated with the operation</param>
        /// <returns>Returns the return value of the last operator if it was
        /// non-zero, or zero if all elements have been processed. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Diterate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t iterate
            (IntPtr buf, hid_t type_id, hid_t space_id, operator_t op,
            object operator_data);

        /// <summary>
        /// Opens an existing dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Open2
        /// </summary>
        /// <param name="file_id">Location identifier</param>
        /// <param name="name">Dataset name</param>
        /// <param name="dapl_id">Dataset access property list</param>
        /// <returns>Returns a dataset identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dopen2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t open
            (hid_t file_id, string name, hid_t dapl_id = H5P.DEFAULT);

        /// <summary>
        /// Reads raw data from a dataset into a buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Read
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset read from.</param>
        /// <param name="mem_type_id">Identifier of the memory datatype.</param>
        /// <param name="mem_space_id">Identifier of the memory dataspace.</param>
        /// <param name="file_space_id">Identifier of the dataset's dataspace
        /// in the file.</param>
        /// <param name="plist_id">Identifier of a transfer property list for
        /// this I/O operation.</param>
        /// <param name="buf">Buffer to receive data read from file.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dread",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t read
            (hid_t dset_id, hid_t mem_type_id, hid_t mem_space_id,
            hid_t file_space_id, hid_t plist_id, IntPtr buf/*out*/);

        /// <summary>
        /// Scatters data into a selection within a memory buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Scatter
        /// </summary>
        /// <param name="op">Callback function which provides data to be
        /// scattered.</param>
        /// <param name="op_data">User-defined pointer to data required by op.</param>
        /// <param name="type_id">Identifier for the datatype describing the
        /// data in both the source and definition buffers. This is only used
        /// to calculate the element size.</param>
        /// <param name="dst_space_id">Identifier for the dataspace describing
        /// both the dimensions of the destination buffer and the selection
        /// within the destination buffer that data will be scattered to.</param>
        /// <param name="dst_buf">Destination buffer which the data will be
        /// scattered to.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dscatter",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t scatter
            (scatter_func_t op, IntPtr op_data, hid_t type_id,
            hid_t dst_space_id, IntPtr dst_buf);

        /// <summary>
        /// Changes the sizes of a dataset’s dimensions.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-SetExtent
        /// </summary>
        /// <param name="dset_id">Dataset identifier</param>
        /// <param name="size">Array containing the new magnitude of each
        /// dimension of the dataset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dset_extent",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_extent(hid_t dset_id, hsize_t* size);

        /// <summary>
        /// Determines the number of bytes required to store variable-length
        /// (VL) data.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-VLGetBuf
        /// </summary>
        /// <param name="dataset_id">Identifier of the dataset to query.</param>
        /// <param name="type_id">Datatype identifier.</param>
        /// <param name="space_id">Dataspace identifier.</param>
        /// <param name="size">The size in bytes of the memory buffer required
        /// to store the VL data.</param>
        /// <returns>Returns non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dvlen_get_buf_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t vlen_get_buf_size
            (hid_t dataset_id, hid_t type_id, hid_t space_id, ref hsize_t size);

        /// <summary>
        /// Reclaims variable-length (VL) datatype memory buffers.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-VLReclaim
        /// </summary>
        /// <param name="type_id">Identifier of the datatype.</param>
        /// <param name="space_id">Identifier of the dataspace.</param>
        /// <param name="plist_id">Identifier of the property list used to
        /// create the buffer.</param>
        /// <param name="buf">Pointer to the buffer to be reclaimed.</param>
        /// <returns>Returns non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dvlen_reclaim",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t vlen_reclaim
            (hid_t type_id, hid_t space_id, hid_t plist_id, IntPtr buf);

        /// <summary>
        /// Writes raw data from a buffer to a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5D.html#Dataset-Write
        /// </summary>
        /// <param name="dset_id">Identifier of the dataset to write to.</param>
        /// <param name="mem_type_id">Identifier of the memory datatype.</param>
        /// <param name="mem_space_id">Identifier of the memory dataspace.</param>
        /// <param name="file_space_id">Identifier of the dataset's dataspace
        /// in the file.</param>
        /// <param name="plist_id">Identifier of a transfer property list for
        /// this I/O operation.</param>
        /// <param name="buf">Buffer with data to be written to the file.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Dwrite",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t write
            (hid_t dset_id, hid_t mem_type_id, hid_t mem_space_id,
            hid_t file_space_id, hid_t plist_id, IntPtr buf);
    }
}
