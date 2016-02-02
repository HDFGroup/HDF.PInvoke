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
using hid_t = System.Int32;
using hsize_t = System.UInt64;
using hssize_t = System.Int64;
using htri_t = System.Int32;
using size_t = System.IntPtr;

namespace HDF.PInvoke
{
    public unsafe sealed class H5S
    {
        // Define atomic datatypes
        public const int ALL = 0;
        public const hsize_t UNLIMITED = unchecked((hsize_t)(-1));

        /// <summary>
        /// Define user-level maximum number of dimensions
        /// </summary>
        public const int MAX_RANK = 32;

        /// <summary>
        /// Different types of dataspaces
        /// </summary>
        public enum class_t
        {
            /// <summary>
            /// error [value = -1].
            /// </summary>
            NO_CLASS = -1,
            /// <summary>
            /// scalar variable [value = 0].
            /// </summary>
            SCALAR = 0,
            /// <summary>
            /// simple data space [value = 1].
            /// </summary>
            SIMPLE = 1,
            /// <summary>
            /// null data space [value = 2].
            /// </summary>
            NULL = 2
        }

       
        /// <summary>
        /// Enumerated type for the type of selection
        /// </summary>
        public enum sel_type
        {
            /// <summary>
            /// Error
            /// </summary>
            SEL_ERROR = -1,
            /// <summary>
            /// Nothing selected
            /// </summary>
            SEL_NONE = 0,
            /// <summary>
            /// Sequence of points selected
            /// </summary>
            SEL_POINTS = 1,
            /// <summary>
            /// "New-style" hyperslab selection defined
            /// </summary>
            SEL_HYPERSLABS = 2,
            /// <summary>
            /// Entire extent selected
            /// </summary>
            SEL_ALL = 3,
            SEL_N
        }

        /// <summary>
        /// Releases and terminates access to a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-Close
        /// </summary>
        /// <param name="space_id">Identifier of dataspace to release.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sclose",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t close(hid_t space_id);

        /// <summary>
        /// Creates an exact copy of a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-Copy
        /// </summary>
        /// <param name="space_id">Identifier of dataspace to copy.</param>
        /// <returns>Returns a dataspace identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Scopy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t copy(hid_t space_id);

        /// <summary>
        /// Creates a new dataspace of a specified type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-Create
        /// </summary>
        /// <param name="type">Type of dataspace to be created.</param>
        /// <returns>Returns a dataspace identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Screate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create(class_t type);

        /// <summary>
        /// Creates a new simple dataspace and opens it for access.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-CreateSimple
        /// </summary>
        /// <param name="rank">Number of dimensions of dataspace.</param>
        /// <param name="dims">Array specifying the size of each dimension.</param>
        /// <param name="maxdims">Array specifying the maximum size of each
        /// dimension.</param>
        /// <returns>Returns a dataspace identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Screate_simple",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create_simple
            (int rank, hsize_t* dims, hsize_t* maxdims);

        /// <summary>
        /// Decode a binary object description of data space and return a new
        /// object handle.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-Decode
        /// </summary>
        /// <param name="buf">Buffer for the data space object to be decoded.</param>
        /// <returns>Returns an object ID(non-negative) if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sdecode",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t decode(byte* buf);

        /// <summary>
        /// Encode a data space object description into a binary buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-Encode
        /// </summary>
        /// <param name="obj_id">Identifier of the object to be encoded.</param>
        /// <param name="buf">Buffer for the object to be encoded into. If the
        /// provided buffer is <code>NULL</code>, only the size of buffer
        /// needed is returned through <paramref name="nalloc"/>.</param>
        /// <param name="nalloc">The size of the allocated buffer or the size
        /// of the buffer needed.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sencode",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t encode
            (hid_t obj_id, [Out] byte[] buf, ref size_t nalloc);

        /// <summary>
        /// Copies the extent of a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-ExtentCopy
        /// </summary>
        /// <param name="dest_space_id">The identifier for the dataspace to
        /// which the extent is copied.</param>
        /// <param name="source_space_id">The identifier for the dataspace from
        /// which the extent is copied.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sextent_copy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t extent_copy
            (hid_t dest_space_id, hid_t source_space_id);

        /// <summary>
        /// Determines whether two dataspace extents are equal.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-ExtentEqual
        /// </summary>
        /// <param name="space1_id">First dataspace identifier.</param>
        /// <param name="space2_id">Second dataspace identifier.</param>
        /// <returns>Returns 1 if equal, 0 if unequal, if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sextent_equal",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t extent_equal
            (hid_t space1_id, hid_t space2_id);

        /// <summary>
        /// Gets the bounding box containing the current selection.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectBounds
        /// </summary>
        /// <param name="space_id">Identifier of dataspace to query.</param>
        /// <param name="start">Starting coordinates of the bounding box.</param>
        /// <param name="end">Ending coordinates of the bounding box, i.e.,
        /// the coordinates of the diagonally opposite corner.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sget_select_bounds",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_select_bounds
            (hid_t space_id, hsize_t[] start, hsize_t[] end);

        /// <summary>
        /// Gets the number of element points in the current selection.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectElemNPoints
        /// </summary>
        /// <param name="space_id">Identifier of dataspace to query.</param>
        /// <returns>Returns the number of element points in the current
        /// dataspace selection if successful. Otherwise returns a negative
        /// value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_elem_npoints",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hssize_t get_select_elem_npoints(hid_t space_id);

        /// <summary>
        /// Gets the list of element points currently selected.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectElemPointList
        /// </summary>
        /// <param name="space_id">Dataspace identifier of selection to query.</param>
        /// <param name="startpoint">Element point to start with.</param>
        /// <param name="numpoints">Number of element points to get.</param>
        /// <param name="buf">List of element points selected.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_elem_pointlist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_select_elem_pointlist
            (hid_t space_id, hsize_t startpoint, hsize_t numpoints,
            [Out] hsize_t[] buf);

        /// <summary>
        /// Gets the list of hyperslab blocks currently selected.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectHyperBlockList
        /// </summary>
        /// <param name="space_id">Dataspace identifier of selection to query.</param>
        /// <param name="startblock">Hyperslab block to start with.</param>
        /// <param name="numblocks">Number of hyperslab blocks to get.</param>
        /// <param name="buf">List of hyperslab blocks selected.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_hyper_blocklist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_select_hyper_blocklist
            (hid_t space_id, hsize_t startblock, hsize_t numblocks,
            [Out] hsize_t[] buf);

        /// <summary>
        /// Get number of hyperslab blocks.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectHyperNBlocks
        /// </summary>
        /// <param name="space_id">Identifier of dataspace to query.</param>
        /// <returns>Returns the number of hyperslab blocks in the current
        /// dataspace selection if successful. Otherwise returns a negative
        /// value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_hyper_nblocks",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hssize_t get_select_hyper_nblocks
            (hid_t space_id);

        /// <summary>
        /// Determines the number of elements in a dataspace selection.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectNpoints
        /// </summary>
        /// <param name="space_id">Dataspace identifier.</param>
        /// <returns>Returns the number of elements in the selection if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_npoints",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hssize_t get_select_npoints(hid_t space_id);

        /// <summary>
        /// Determines the type of the dataspace selection.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-GetSelectType
        /// </summary>
        /// <param name="space_id">Dataspace identifier.</param>
        /// <returns>Returns the dataspace selection type, a value of the
        /// enumerated datatype <code>H5S.sel_type</code>, if successful.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_type",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern sel_type get_select_type(hid_t space_id);

        /// <summary>
        /// Retrieves dataspace dimension size and maximum size.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-ExtentDims
        /// </summary>
        /// <param name="space_id">Identifier of the dataspace object to query</param>
        /// <param name="dims">Pointer to array to store the size of each dimension.</param>
        /// <param name="maxdims">Pointer to array to store the maximum size of each dimension.</param>
        /// <returns>Returns the number of dimensions in the dataspace if
        /// successful; otherwise returns a negative value.</returns>
        /// <remarks>Either or both of <paramref name="dims"/> and
        /// <paramref name="maxdims"/> may be <code>NULL</code>.</remarks>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_simple_extent_dims",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_simple_extent_dims
            (hid_t space_id, hsize_t* dims, hsize_t* maxdims);

        /// <summary>
        /// Determines the dimensionality of a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-ExtentNdims
        /// </summary>
        /// <param name="space_id">Identifier of the dataspace</param>
        /// <returns>Returns the number of dimensions in the dataspace if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_simple_extent_ndims",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_simple_extent_ndims(hid_t space_id);

        /// <summary>
        /// Determines the number of elements in a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-ExtentNpoints
        /// </summary>
        /// <param name="space_id">Identifier of the dataspace object to query</param>
        /// <returns>Returns the number of elements in the dataspace if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_simple_extent_npoints",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hssize_t get_simple_extent_npoints(hid_t space_id);

        /// <summary>
        /// Determines the current class of a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-ExtentType
        /// </summary>
        /// <param name="space_id">Dataspace identifier.</param>
        /// <returns>Returns a dataspace class name if successful; otherwise
        /// <code>H5S.class_t.NO_CLASS</code>.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_simple_extent_type",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern class_t H5Sget_simple_extent_type(hid_t space_id);

        /// <summary>
        /// Determines whether a dataspace is a simple dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-IsSimple
        /// </summary>
        /// <param name="space_id">Identifier of the dataspace to query</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, or 0 (zero), for <code>FALSE</code>. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sis_simple",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t H5Sis_simple(hid_t space_id);
    }
}