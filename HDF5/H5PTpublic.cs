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
using uint32_t = System.UInt32;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    /// <summary>
    /// H5PT: HDF5 Packet Table.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class H5PT
    {
        static H5PT()
        {
            H5.open();
        }

        /// <summary>
        /// Creates a packet table to store fixed-length or variable-length packets.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTcreate
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the table within.</param>
        /// <param name="table_name">The name of the packet table to create.</param>
        /// <param name="dtype_id">The datatype of the packet.</param>
        /// <param name="chunk_size">
        /// Chunk size, in number of table entries per chunk.
        /// Packet table datasets use HDF5 chunked storage to allow them to grow.
        /// This value allows the user to set the size of a chunk. The chunk size affects performance.
        /// </param>
        /// <param name="plist_id">Identifier of the property list. Can be used to specify the compression of the packet table.</param>
        /// <returns>Returns an identifier for the new packet table or <see cref="H5I.H5I_INVALID_HID"/> on error.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTcreate",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create (hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string table_name, hid_t dtype_id, hsize_t chunk_size, hid_t plist_id);

        /// <summary>
        /// Creates a packet table to store fixed-length packets.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTcreate_fl
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to create the table within.</param>
        /// <param name="table_name">The name of the packet table to create.</param>
        /// <param name="dtype_id">The datatype of the packet.</param>
        /// <param name="chunk_size">
        /// Chunk size, in number of table entries per chunk.
        /// Packet table datasets use HDF5 chunked storage to allow them to grow.
        /// This value allows the user to set the size of a chunk. The chunk size affects performance.
        /// </param>
        /// <param name="compression">
        /// Compression level, a value of 0 through 9. Level 0 is faster but offers the least compression;
        /// level 9 is slower but offers maximum compression.
        /// A setting of -1 indicates that no compression is desired.
        /// </param>
        /// <returns>Returns an identifier for the new packet table or <see cref="H5I.H5I_INVALID_HID"/> on error.</returns>
        [Obsolete("Call H5PT.create() instead.")]
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTcreate_fl",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create_fl(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string table_name, hid_t dtype_id, hsize_t chunk_size, int compression);

        /// <summary>
        /// Opens an existing packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTopen
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group within which the packet table can be found.</param>
        /// <param name="dset_name">The name of the packet table to open.</param>
        /// <returns>Returns an identifier for the packet table, or <see cref="H5I.H5I_INVALID_HID"/> on error.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTopen",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t open(hid_t loc_id, [MarshalAs(UnmanagedType.LPStr)] string dset_name);

        /// <summary>
        /// Closes an open packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTclose
        /// </summary>
        /// <param name="table_id">Identifier of packet table to be closed.</param>
        /// <returns>Returns a non-negative value if successful, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTclose",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t close(hid_t table_id);

        /// <summary>
        /// Appends packets to the end of a packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTappend
        /// </summary>
        /// <param name="table_id">Identifier of packet table to which packets should be appended.</param>
        /// <param name="nrecords">Number of packets to be appended.</param>
        /// <param name="data">Buffer holding data to write.</param>
        /// <returns>Returns an identifier for the packet table, or <see cref="H5I.H5I_INVALID_HID"/> on error.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTappend",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t append(hid_t table_id, size_t nrecords, IntPtr data);

        /// <summary>
        /// Resets a packet table's index to the first packet.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTcreate_index
        /// </summary>
        /// <param name="table_id">Identifier of packet table whose index should be initialized.</param>
        /// <returns>Returns a non-negative value if successful, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTcreate_index",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t create_index(hid_t table_id);

        /// <summary>
        /// Sets a packet table's index.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTset_index
        /// </summary>
        /// <param name="table_id">Identifier of packet table whose index is to be set.</param>
        /// <param name="index">The packet to which the index should point.</param>
        /// <returns>Returns a non-negative value if successful, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTset_index",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_index(hid_t table_id, hsize_t index);

        /// <summary>
        /// Reads a number of packets from a packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTread_packets
        /// </summary>
        /// <param name="table_id">Identifier of packet table to read from.</param>
        /// <param name="start">Packet to start reading from.</param>
        /// <param name="nrecords">Number of packets to be read.</param>
        /// <param name="data">Buffer into which to read data.</param>
        /// <returns>Returns a non-negative value if successful, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTread_packets",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t read_packets(hid_t table_id, hsize_t start, size_t nrecords, IntPtr data);

        /// <summary>
        /// Reads packets from a packet table starting at the current index.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTget_next
        /// </summary>
        /// <param name="table_id">Identifier of packet table to read from.</param>
        /// <param name="nrecords">Number of packets to be read.</param>
        /// <param name="data">Buffer into which to read data.</param>
        /// <returns>Returns a non-negative value if successful, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTget_next",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_next(hid_t table_id, size_t nrecords, IntPtr data);

        /// <summary>
        /// Returns the backend dataset of this packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTget_dataset
        /// </summary>
        /// <param name="table_id">Identifier of the packet table.</param>
        /// <returns>Returns a dataset identifier or <see cref="H5I.H5I_INVALID_HID"/> on error.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTget_dataset",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_dataset(hid_t table_id);

        /// <summary>
        /// Returns the backend datatype of this packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTget_type
        /// </summary>
        /// <param name="table_id">Identifier of the packet table.</param>
        /// <returns>Returns a datatype identifier or <see cref="H5I.H5I_INVALID_HID"/> on error.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTget_type",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_type(hid_t table_id);

        /// <summary>
        /// Returns the number of packets in a packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTget_num_packets
        /// </summary>
        /// <param name="table_id">Identifier of packet table to query.</param>
        /// <param name="nrecords">Number of packets in packet table.</param>
        /// <returns>Returns a non-negative value if successful, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTget_num_packets",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_num_packets(hid_t table_id, out hsize_t nrecords);

        /// <summary>
        /// Determines whether an identifier points to a packet table.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTis_valid
        /// </summary>
        /// <param name="table_id">Identifier to query.</param>
        /// <returns>Returns a non-negative value if <paramref name="table_id"/> is a valid packet table, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTis_valid",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t is_valid(hid_t table_id);

        /// <summary>
        /// Determines whether a packet table contains variable-length or fixed-length packets.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTis_varlen
        /// </summary>
        /// <param name="table_id">Packet table to query.</param>
        /// <returns>Returns 1 for a variable-length packet table, 0 for fixed-length, or a negative value on error.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTis_varlen",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t is_varlen(hid_t table_id);

        /// <summary>
        /// Releases memory allocated in the process of reading variable-length packets.
        /// See https://support.hdfgroup.org/HDF5/doc/HL/RM_H5PT.html#H5PTfree_vlen_buff
        /// </summary>
        /// <param name="table_id">Packet table whose memory should be freed.</param>
        /// <param name="bufflen">Size of <paramref name="buff"/>.</param>
        /// <param name="buff">Buffer that was used to read in variable-length packets.</param>
        /// <returns>Returns a non-negative value if successful, otherwise returns a negative value.</returns>
        [DllImport(Constants.HLDLLFileName,
             EntryPoint = "H5PTfree_vlen_buff",
             CallingConvention = CallingConvention.Cdecl),
         SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t free_vlen_buff(hid_t table_id, hsize_t bufflen, IntPtr buff);
    }
}
