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
using hssize_t = System.Int64;
using htri_t = System.Int32;
using size_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed class H5S
    {
        static H5S() { H5.open(); }

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
        /// Different ways of combining selections
        /// </summary>
        public enum seloper_t
        {
            /// <summary>
            /// error
            /// </summary>
            NOOP = -1,
            /// <summary>
            /// Select "set" operation
            /// </summary>
            SET = 0,
            /// <summary>
            /// Binary "or" operation for hyperslabs
            /// (add new selection to existing selection)
            /// Original region:  AAAAAAAAAA
            /// New region:             BBBBBBBBBB
            /// A or B:           CCCCCCCCCCCCCCCC
            /// </summary>
            OR,
            /// <summary>
            /// Binary "and" operation for hyperslabs
            /// (only leave overlapped regions in selection)
            /// Original region:  AAAAAAAAAA
            /// New region:             BBBBBBBBBB
            /// A and B:                CCCC
            /// </summary>
            AND,
            /// <summary>
            /// Binary "xor" operation for hyperslabs
            /// (only leave non-overlapped regions in selection)
            /// Original region:  AAAAAAAAAA
            /// New region:             BBBBBBBBBB
            /// A xor B:          CCCCCC    CCCCCC
            /// </summary>
            XOR,
            /// <summary>
            /// Binary "not" operation for hyperslabs
            /// (only leave non-overlapped regions in original selection)
            /// Original region:  AAAAAAAAAA
            /// New region:             BBBBBBBBBB
            /// A not B:          CCCCCC
            /// </summary>
            NOTB,
            /// <summary>
            /// Binary "not" operation for hyperslabs
            /// (only leave non-overlapped regions in new selection)
            /// Original region:  AAAAAAAAAA
            /// New region:             BBBBBBBBBB
            /// B not A:                    CCCCCC
            /// </summary>
            NOTA,
            /// <summary>
            /// Append elements to end of point selection
            /// </summary>
            APPEND,
            /// <summary>
            /// Prepend elements to beginning of point selection
            /// </summary>
            PREPEND,
            /// <summary>
            /// Invalid upper bound on selection operations
            /// </summary>
            INVALID
        }

        /// <summary>
        /// Enumerated type for the type of selection
        /// </summary>
        public enum sel_type
        {
            /// <summary>
            /// Error
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// Nothing selected
            /// </summary>
            NONE = 0,
            /// <summary>
            /// Sequence of points selected
            /// </summary>
            POINTS = 1,
            /// <summary>
            /// "New-style" hyperslab selection defined
            /// </summary>
            HYPERSLABS = 2,
            /// <summary>
            /// Entire extent selected
            /// </summary>
            ALL = 3,
            N
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
            (int rank, 
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]hsize_t[] dims, 
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]hsize_t[] maxdims);

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
        public static extern hid_t create_simple(int rank, hsize_t* dims, hsize_t* maxdims);

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
        public static extern hid_t decode(byte[] buf);

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
            (hid_t obj_id,
            [MarshalAs(UnmanagedType.LPArray)][In, Out] byte[] buf,
            ref size_t nalloc);

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

#if HDF5_VER1_10
        /// <summary>
        /// Retrieves a regular hyperslab selection.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Sget_regular_hyperslab.htm
        /// </summary>
        /// <param name="space_id">The identifier of the dataspace.</param>
        /// <param name="start">Offset of the start of the regular hyperslab.</param>
        /// <param name="stride">Stride of the regular hyperslab.</param>
        /// <param name="count">Number of blocks in the regular hyperslab.</param>
        /// <param name="block">Size of a block in the regular hyperslab.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>If a hyperslab selection is originally regular, then
        /// becomes irregular through selection operations, and then becomes
        /// regular again, the final regular selection may be equivalent but
        /// not identical to the original regular selection.</remarks>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_regular_hyperslab",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t H5Sget_regular_hyperslab
            (hid_t space_id, hsize_t[] start, hsize_t[] stride,
            hsize_t[] count, hsize_t[] block);
#endif

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
        /// <remarks>The <code>start</code> and <code>end</code> buffers must
        /// be large enough to hold the dataspace rank number of coordinates.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sget_select_bounds",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_select_bounds
            (hid_t space_id, [In][Out]hsize_t[] start, [In][Out]hsize_t[] end);

        /// <summary>
        /// Gets the number of points in the current point selection.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectElemNPoints
        /// </summary>
        /// <param name="space_id">Identifier of dataspace to query.</param>
        /// <returns>Returns the number of points in the current dataspace
        /// point selection if successful. Otherwise returns a negative
        /// value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_elem_npoints",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hssize_t get_select_elem_npoints(hid_t space_id);

        /// <summary>
        /// Gets the list of points in a point selection.
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
            [In][Out]hsize_t[] buf);

        /// <summary>
        /// Gets the list of hyperslab blocks in a hyperslab selection.
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
            [In][Out]hsize_t[] buf);

        /// <summary>
        /// Get number of hyperslab blocks in a hyperslab selection.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectHyperNBlocks
        /// </summary>
        /// <param name="space_id">Identifier of dataspace to query.</param>
        /// <returns>Returns the number of hyperslab blocks in a hyperslab
        /// selection if successful. Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sget_select_hyper_nblocks",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hssize_t get_select_hyper_nblocks(hid_t space_id);

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
            (hid_t space_id, [In][Out]hsize_t[] dims, [In][Out]hsize_t[] maxdims);


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
        public static extern int get_simple_extent_dims (
            hid_t space_id, hsize_t* dims, hsize_t* maxdims);

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
        public static extern class_t get_simple_extent_type(hid_t space_id);

#if HDF5_VER1_10
        /// <summary>
        /// Determines whether a hyperslab selection is regular.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/VDS/H5Sis_regular_hyperslab.htm
        /// </summary>
        /// <param name="spaceid">The identifier of the dataspace.</param>
        /// <returns>Returns <code>TRUE</code> or <code>FALSE</code> for
        /// hyperslab selection if successful. Returns <code>FAIL</code>on
        /// error or when querying other selection types such as point
        /// selection.</returns>
        [DllImport(Constants.DLLFileName,
            EntryPoint = "H5Sis_regular_hyperslab",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t is_regular_hyperslab(hid_t spaceid);
#endif

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
        public static extern htri_t is_simple(hid_t space_id);

        /// <summary>
        /// Sets the offset of a simple dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-OffsetSimple
        /// </summary>
        /// <param name="space_id">The identifier for the dataspace object to
        /// reset.</param>
        /// <param name="offset">The offset at which to position the selection.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>The offset array must be the same number of elements as
        /// the number of dimensions for the dataspace. If the offset array is
        /// set to <code>NULL</code>, the offset for the dataspace is reset
        /// to 0.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Soffset_simple",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t offset_simple
            (hid_t space_id,
            [MarshalAs(UnmanagedType.LPArray)] hssize_t[] offset);

        /// <summary>
        /// Selects an entire dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectAll
        /// </summary>
        /// <param name="dspace_id">The identifier for the dataspace for which
        /// the selection is being made.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sselect_all",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t select_all(hid_t dspace_id);

        /// <summary>
        /// Selects array elements to be included in the selection for a
        /// dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectElements
        /// </summary>
        /// <param name="space_id">Identifier of the dataspace.</param>
        /// <param name="op">Operator specifying how the new selection is to be
        /// combined with the existing selection for the dataspace.</param>
        /// <param name="num_elements">Number of elements to be selected.</param>
        /// <param name="coord">A pointer to a buffer containing a serialized
        /// copy of a 2-dimensional array of zero-based values specifying the
        /// coordinates of the elements in the point selection.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sselect_elements",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t select_elements
            (hid_t space_id, seloper_t op, size_t num_elements,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] coord);

        /// <summary>
        /// Selects a hyperslab region to add to the current selected region.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectHyperslab
        /// </summary>
        /// <param name="space_id">Identifier of dataspace selection to modify</param>
        /// <param name="op">Operation to perform on current selection.</param>
        /// <param name="start">Offset of start of hyperslab</param>
        /// <param name="stride">Number of blocks included in hyperslab.</param>
        /// <param name="count">Hyperslab stride.</param>
        /// <param name="block">Size of block in hyperslab.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sselect_hyperslab",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t select_hyperslab
            (hid_t space_id, seloper_t op,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] start,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] stride,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] count,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] block);

        /// <summary>
        /// Selects a hyperslab region to add to the current selected region.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectHyperslab
        /// </summary>
        /// <param name="space_id">Identifier of dataspace selection to modify</param>
        /// <param name="op">Operation to perform on current selection.</param>
        /// <param name="start">Offset of start of hyperslab</param>
        /// <param name="stride">Number of blocks included in hyperslab.</param>
        /// <param name="count">Hyperslab stride.</param>
        /// <param name="block">Size of block in hyperslab.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sselect_hyperslab",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t select_hyperslab(hid_t space_id, seloper_t op, hsize_t* start, hsize_t* stride, hsize_t* count, hsize_t* block);

        /// <summary>
        /// Resets the selection region to include no elements.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectNone
        /// </summary>
        /// <param name="space_id">The identifier for the dataspace in which
        /// the selection is being reset.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sselect_none",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t select_none(hid_t space_id);

        /// <summary>
        /// Verifies that the selection is within the extent of the dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SelectValid
        /// </summary>
        /// <param name="space_id">Identifier for the dataspace being queried.</param>
        /// <returns>Returns a positive value, for <code>TRUE</code>, if the
        /// selection is contained within the extent or 0 (zero), for
        /// <code>FALSE</code>, if it is not. Returns a negative value on error
        /// conditions such as the selection or extent not being defined.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sselect_valid",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t select_valid(hid_t space_id);

        /// <summary>
        /// Removes the extent from a dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SetExtentNone
        /// </summary>
        /// <param name="space_id">The identifier for the dataspace from which
        /// the extent is to be removed.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sset_extent_none",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_extent_none(hid_t space_id);

        /// <summary>
        /// Sets or resets the size of an existing dataspace.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5S.html#Dataspace-SetExtentSimple
        /// </summary>
        /// <param name="space_id">Dataspace identifier.</param>
        /// <param name="rank">Rank, or dimensionality, of the dataspace.</param>
        /// <param name="current_size">Array containing current size of
        /// dataspace.</param>
        /// <param name="maximum_size">Array containing maximum size of
        /// dataspace.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Sset_extent_simple",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_extent_simple
            (hid_t space_id, int rank,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] current_size,
            [MarshalAs(UnmanagedType.LPArray)] hsize_t[] maximum_size);
    }
}