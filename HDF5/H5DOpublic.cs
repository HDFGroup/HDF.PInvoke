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

using herr_t = System.Int32;
using hsize_t = System.UInt64;
using size_t = System.IntPtr;
using uint32_t = System.UInt32;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed class H5DO
    {
        static H5DO() { H5.open(); }

        /// <summary>
        /// Reads a raw data chunk directly from a dataset in a file into a buffer.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_HDF5Optimized.html#H5DOread_chunk
        /// </summary>
        /// <param name="dset_id">Identifier for the dataset to be read</param>
        /// <param name="dxpl_id">Transfer property list identifier for this
        /// I/O operation</param>
        /// <param name="filter_mask">Mask for identifying the filters used
        /// with the chunk</param>
        /// <param name="offset">Logical position of the chunk’s first element
        /// in the dataspace</param>
        /// <param name="buf">Buffer containing the chunk read from the dataset</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName, EntryPoint = "H5DOread_chunk",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t read_chunk
            (hid_t dset_id, hid_t dxpl_id, ref hsize_t offset,
            ref uint32_t filter_mask, IntPtr buf);

        /// <summary>
        /// Writes a raw data chunk from a buffer directly to a dataset.
        /// See https://www.hdfgroup.org/HDF5/doc/HL/RM_HDF5Optimized.html
        /// </summary>
        /// <param name="dset_id">Identifier for the dataset to write to</param>
        /// <param name="dxpl_id">UNUSED</param>
        /// <param name="filter_mask">Mask for identifying the filters in use</param>
        /// <param name="offset">Logical position of the chunk’s first element
        /// in the dataspace</param>
        /// <param name="data_size">Size of the actual data to be written in
        /// bytes</param>
        /// <param name="buf">Buffer containing data to be written to the file</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName, EntryPoint = "H5DOwrite_chunk",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t write_chunk
            (hid_t dset_id, hid_t dxpl_id, uint32_t filter_mask,
            ref hsize_t offset, size_t data_size, IntPtr buf);

#if HDF5_VER1_10
        /// <summary>
        /// Appends data to a dataset along a specified dimension.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/SWMR/H5DOappend.htm
        /// </summary>
        /// <param name="dset_id">Dataset identifier.</param>
        /// <param name="dxpl_id">Dataset transfer property list identifier.</param>
        /// <param name="axis">Dimension number (0-based).</param>
        /// <param name="num_elem">Number of elements to add along the
        /// dimension.</param>
        /// <param name="memtype">Memory type identifier.</param>
        /// <param name="buf">Data buffer.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName, EntryPoint = "H5DOappend",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t append
            (hid_t dset_id, hid_t dxpl_id, uint axis,
            size_t num_elem, hid_t memtype, IntPtr buf);
#endif

    }
}
