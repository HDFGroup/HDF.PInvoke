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
using System.Text;

using hbool_t = System.UInt32;
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
        static H5T()
        {
            H5.open();
        }

        /// <summary>
        /// These are the various classes of datatypes.
        /// </summary>
        public enum class_t
        {
            NO_CLASS = -1,
            INTEGER = 0,
            FLOAT = 1,
            TIME = 2,
            STRING = 3,
            BITFIELD = 4,
            OPAQUE = 5,
            COMPOUND = 6,
            REFERENCE = 7,
            ENUM = 8,
            VLEN = 9,
            ARRAY = 10,
            NCLASSES
        }

        /// <summary>
        /// Byte orders
        /// </summary>
        public enum order_t
        {
            /// <summary>
            /// error
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// little endian
            /// </summary>
            LE = 0,
            /// <summary>
            /// big endian
            /// </summary>
            BE = 1,
            /// <summary>
            /// VAX mixed endian
            /// </summary>
            VAX = 2,
            /// <summary>
            /// Compound type with mixed member orders
            /// </summary>
            MIXED = 3,
            /// <summary>
            /// no particular order (strings, bits,..)
            /// </summary>
            ONE = 4
        }

        /// <summary>
        /// Types of integer sign schemes
        /// </summary>
        public enum sign_t
        {
            /// <summary>
            /// error
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// unsigned
            /// </summary>
            NONE = 0,
            /// <summary>
            /// two's complement
            /// </summary>
            SGN_2 = 1,
            NSGN = 2
        }


        /// <summary>
        /// Floating-point normalization schemes
        /// </summary>
        public enum norm_t
        {
            /// <summary>
            /// error
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// msb of mantissa isn't stored, always 1
            /// </summary>
            IMPLIED = 0,
            /// <summary>
            /// msb of mantissa is always 1
            /// </summary>
            MSBSET = 1,
            /// <summary>
            /// not normalized
            /// </summary>
            NONE = 2
        }


        /// <summary>
        /// Character set to use for text strings.
        /// </summary>
        public enum cset_t
        {
            /// <summary>
            /// error [value = -1].
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// US ASCII [value = 0].
            /// </summary>
            ASCII = 0,
            /// <summary>
            /// UTF-8 Unicode encoding [value = 1].
            /// </summary>
            UTF8 = 1,
            // reserved for later use [values = 2-15]
            [System.ComponentModel.Browsable(false)]
            [System.ComponentModel.EditorBrowsable(
                System.ComponentModel.EditorBrowsableState.Never)]
            RESERVED_2 = 2,
            RESERVED_3 = 3,
            RESERVED_4 = 4,
            RESERVED_5 = 5,
            RESERVED_6 = 6,
            RESERVED_7 = 7,
            RESERVED_8 = 8,
            RESERVED_9 = 9,
            RESERVED_10 = 10,
            RESERVED_11 = 11,
            RESERVED_12 = 12,
            RESERVED_13 = 13,
            RESERVED_14 = 14,
        }

        /// <summary>
        /// Number of character sets actually defined 
        /// </summary>
        public const cset_t NCSET = cset_t.RESERVED_2;

        /// <summary>
        /// Type of padding to use in character strings.
        /// </summary>
        public enum str_t
        {
            /// <summary>
            /// error
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// null terminate like in C
            /// </summary>
            NULLTERM = 0,
            /// <summary>
            /// pad with nulls
            /// </summary>
            NULLPAD = 1,
            /// <summary>
            /// pad with spaces like in Fortran
            /// </summary>
            SPACEPAD = 2,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_3 = 3,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_4 = 4,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_5 = 5,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_6 = 6,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_7 = 7,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_8 = 8,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_9 = 9,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_10 = 10,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_11 = 11,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_12 = 12,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_13 = 13,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_14 = 14,
            /// <summary>
            /// reserved for later use
            /// </summary>
            RESERVED_15 = 15
        }

        /// <summary>
        /// num H5T_str_t types actually defined
        /// </summary>
        public const str_t H5T_NSTR = str_t.RESERVED_3;

        /// <summary>
        /// Type of padding to use in other atomic types
        /// </summary>
        public enum pad_t
        {
            /// <summary>
            /// error
            /// </summary>
            ERROR = -1,
            /// <summary>
            /// always set to zero
            /// </summary>
            ZERO = 0,
            /// <summary>
            /// always set to one
            /// </summary>
            ONE = 1,
            /// <summary>
            /// set to background value
            /// </summary>
            BACKGROUND = 2,
            NPAD = 3
        }

        /// <summary>
        /// The exception type passed into the conversion callback function
        /// </summary>
        public enum conv_except_t
        {
            /// <summary>
            /// source value is greater than destination's range
            /// </summary>
            RANGE_HI = 0,
            /// <summary>
            /// source value is less than destination's range
            /// </summary>
            RANGE_LOW = 1,
            /// <summary>
            /// source value loses precision in destination
            /// </summary>
            PRECISION = 2,
            /// <summary>
            /// source value is truncated in destination
            /// </summary>
            TRUNCATE = 3,
            /// <summary>
            /// source value is positive infinity(floating number)
            /// </summary>
            PINF = 4,
            /// <summary>
            /// source value is negative infinity(floating number)
            /// </summary>
            NINF = 5,
            /// <summary>
            /// source value is NaN(floating number)
            /// </summary>
            NAN = 6
        }

        /// <summary>
        /// Commands sent to conversion functions
        /// </summary>
        public enum cmd_t
        {
            /// <summary>
            /// query and/or initialize private data
            /// </summary>
            INIT = 0,
            /// <summary>
            /// convert data from source to dest datatype
            /// </summary>
            CONV = 1,
            /// <summary>
            /// function is being removed from path
            /// </summary>
            FREE = 2
        }

        /// <summary>
        /// How is the `bkg' buffer used by the conversion function?
        /// </summary>
        public enum bkg_t
        {
            /// <summary>
            /// background buffer is not needed, send NULL
            /// </summary>
            NO = 0,
            /// <summary>
            /// bkg buffer used as temp storage only
            /// </summary>
            TEMP = 1,
            /// <summary>
            /// init bkg buf with data before conversion
            /// </summary>
            YES = 2
        }

        /// <summary>
        /// Type conversion client data
        /// </summary>
        public struct cdata_t
        {
            /// <summary>
            /// what should the conversion function do?
            /// </summary>
            public cmd_t command;
            /// <summary>
            /// is the background buffer needed?
            /// </summary>
            public bkg_t need_bkg;
            /// <summary>
            /// recalculate private data
            /// </summary>
            public hbool_t recalc;
            /// <summary>
            /// private data
            /// </summary>
            public IntPtr priv;
        }

        /// <summary>
        /// Conversion function persistence
        /// </summary>
        public enum pers_t
        {
            /// <summary>
            /// wild card
            /// </summary>
            DONTCARE = -1,
            /// <summary>
            /// hard conversion function
            /// </summary>
            HARD = 0,
            /// <summary>
            /// soft conversion function
            /// </summary>
            SOFT = 1
        }

        /// <summary>
        /// The order to retrieve atomic native datatype
        /// </summary>
        public enum direction_t
        {
            /// <summary>
            /// default direction is ascending
            /// </summary>
            DEFAULT = 0,
            /// <summary>
            /// in ascending order
            /// </summary>
            ASCEND = 1,
            /// <summary>
            /// in descending order
            /// </summary>
            DESCEND = 2
        }

        
        /// <summary>
        /// The return value from conversion callback function
        /// conv_except_func_t
        /// </summary>
        public enum conv_ret_t
        {
            /// <summary>
            /// abort conversion [value = -1]
            /// </summary>
            ABORT = -1,
            /// <summary>
            /// callback function failed to handle the exception [value = 0]
            /// </summary>
            UNHANDLED = 0,
            /// <summary>
            /// callback function handled the exception successfully [value = 1]
            /// </summary>
            HANDLED = 1
        }

        /// <summary>
        /// Variable Length Datatype struct in memory
        /// (This is only used for VL sequences, not VL strings, which are
        /// stored in byte[]'s)
        /// </summary>
        public struct hvl_t
        {
            /// <summary>
            /// Length of VL data (in base type units)
            /// </summary>
            public size_t len;
            /// <summary>
            /// Pointer to VL data
            /// </summary>
            public IntPtr p;
        }

        /// <summary>
        /// Indicate that a string is variable length (null-terminated in C,
        /// instead of fixed length)
        /// </summary>
        public static readonly IntPtr VARIABLE = new IntPtr(-1); 
        
        /// <summary>
        /// Maximum length of an opaque tag
        /// </summary>
        public const int OPAQUE_TAG_MAX = 256;
      
        /// <summary>
        /// Exception handler.  If an exception like overflow happenes during
        /// conversion, this function is called if it's registered through
        /// H5P.set_type_conv_cb.
        /// </summary>
        /// <param name="except_type"></param>
        /// <param name="src_id"></param>
        /// <param name="dst_id"></param>
        /// <param name="src_buf"></param>
        /// <param name="dst_buf"></param>
        /// <param name="user_data"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate conv_ret_t conv_except_func_t
        (conv_except_t except_type, hid_t src_id, hid_t dst_id,
        IntPtr src_buf, IntPtr dst_buf, IntPtr user_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t conv_t
        (hid_t src_id, hid_t dst_id, ref cdata_t cdata,
        size_t nelmts, size_t buf_stride, size_t bkg_stride, IntPtr buf,
        IntPtr bkg, hid_t dset_xfer_plist = H5P.DEFAULT);

        /// <summary>
        /// Creates an array datatype object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-ArrayCreate2
        /// </summary>
        /// <param name="base_type_id">Datatype identifier for the array base
        /// datatype.</param>
        /// <param name="rank">Rank of the array.</param>
        /// <param name="dims">Size of each array dimension.</param>
        /// <returns>Returns a valid datatype identifier if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tarray_create2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t array_create
            (hid_t base_type_id, uint rank,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]hsize_t[] dims);

        /// <summary>
        /// Releases a datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Close
        /// </summary>
        /// <param name="type_id">Identifier of datatype to release.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tclose",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t close(hid_t type_id);

        /// <summary>
        /// Commits a transient datatype, linking it into the file and creating
        /// a new named datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Commit2
        /// </summary>
        /// <param name="loc_id">Location identifier</param>
        /// <param name="name">Name given to committed datatype</param>
        /// <param name="dtype_id">Identifier of datatype to be committed and,
        /// upon function’s return, identifier for the committed datatype</param>
        /// <param name="lcpl_id">Link creation property list</param>
        /// <param name="tcpl_id">Datatype creation property list</param>
        /// <param name="tapl_id">Datatype access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tcommit2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t commit
            (hid_t loc_id, byte[] name, hid_t dtype_id,
            hid_t lcpl_id = H5P.DEFAULT, hid_t tcpl_id = H5P.DEFAULT,
            hid_t tapl_id = H5P.DEFAULT);

        /// <summary>
        /// Commits a transient datatype, linking it into the file and creating
        /// a new named datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Commit2
        /// </summary>
        /// <param name="loc_id">Location identifier</param>
        /// <param name="name">Name given to committed datatype</param>
        /// <param name="dtype_id">Identifier of datatype to be committed and,
        /// upon function’s return, identifier for the committed datatype</param>
        /// <param name="lcpl_id">Link creation property list</param>
        /// <param name="tcpl_id">Datatype creation property list</param>
        /// <param name="tapl_id">Datatype access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tcommit2",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t commit
            (hid_t loc_id, string name, hid_t dtype_id,
            hid_t lcpl_id = H5P.DEFAULT, hid_t tcpl_id = H5P.DEFAULT,
            hid_t tapl_id = H5P.DEFAULT);

        /// <summary>
        /// Commits a transient datatype to a file, creating a new named
        /// datatype, but does not link it into the file structure.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-CommitAnon
        /// </summary>
        /// <param name="loc_id">A file or group identifier specifying the file
        /// in which the new named datatype is to be created.</param>
        /// <param name="dtype_id">A datatype identifier.</param>
        /// <param name="tcpl_id">A datatype creation property list identifier.</param>
        /// <param name="tapl_id">A datatype access property list identifier.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tcommit_anon",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t commit_anon
            (hid_t loc_id, hid_t dtype_id, hid_t tcpl_id = H5P.DEFAULT,
            hid_t tapl_id = H5P.DEFAULT);

        /// <summary>
        /// Determines whether a datatype is a named type or a transient type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Committed
        /// </summary>
        /// <param name="dtype_id">Datatype identifier.</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, if the datatype has been committed, or 0 (zero),
        /// for <code>FALSE</code>, if the datatype has not been committed.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tcommitted",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t committed( hid_t dtype_id );

        /// <summary>
        /// Check whether the library’s default conversion is hard conversion.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-CompilerConv
        /// </summary>
        /// <param name="src_id">Identifier for the source datatype.</param>
        /// <param name="dst_id">Identifier for the destination datatype.</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, if the datatype has been committed, or 0 (zero),
        /// for <code>FALSE</code>, if the datatype has not been committed.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tcompiler_conv",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t compiler_conv(hid_t src_id, hid_t dst_id);

        /// <summary>
        /// Converts data from one specified datatype to another.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Convert
        /// </summary>
        /// <param name="src_type_id">Identifier for the source datatype.</param>
        /// <param name="dest_type_id">Identifier for the destination datatype.</param>
        /// <param name="nelmts">Size of array <paramref name="buf"/>.</param>
        /// <param name="buf">Array containing pre- and post-conversion values.</param>
        /// <param name="background">Optional background buffer.</param>
        /// <param name="plist_id">Dataset transfer property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tconvert",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t convert
            (hid_t src_type_id, hid_t dest_type_id, size_t nelmts,
            IntPtr buf, IntPtr background, hid_t plist_id = H5P.DEFAULT);

        /// <summary>
        /// Copies an existing datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Copy
        /// </summary>
        /// <param name="type_id">Identifier of datatype to copy.</param>
        /// <returns>Returns a datatype identifier if successful; otherwise
        /// returns a negative value</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tcopy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t copy(hid_t type_id);

        /// <summary>
        /// Creates a new datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Create
        /// </summary>
        /// <param name="cls">Class of datatype to create.</param>
        /// <param name="size">Size, in bytes, of the datatype being created</param>
        /// <returns>Returns datatype identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tcreate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t create( class_t cls, size_t size);

        /// <summary>
        /// Decode a binary object description of datatype and return a new
        /// object handle.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Decode
        /// </summary>
        /// <param name="buf">Buffer for the datatype object to be decoded.</param>
        /// <returns>Returns an object identifier (non-negative) if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tdecode",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t decode(byte[] buf);

        /// <summary>
        /// Determines whether a datatype contains any datatypes of the given
        /// datatype class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-DetectClass
        /// </summary>
        /// <param name="dtype_id">Datatype identifier.</param>
        /// <param name="dtype_class">Datatype class.</param>
        /// <returns>Returns <code>TRUE</code> or <code>FALSE</code> if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tdetect_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t detect_class
            (hid_t dtype_id, class_t dtype_class);

        /// <summary>
        /// Encode a datatype object description into a binary buffer.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Encode
        /// </summary>
        /// <param name="obj_id">Identifier of the object to be encoded.</param>
        /// <param name="buf">Buffer for the object to be encoded into. If the
        /// provided buffer is <code>NULL</code>, only the size of buffer
        /// needed is returned through <paramref name="nalloc"/>.</param>
        /// <param name="nalloc">The size of the buffer allocated or needed.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tencode",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t encode
            (hid_t obj_id, byte[] buf, ref size_t nalloc);

        /// <summary>
        /// Creates a new enumeration datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-EnumCreate
        /// </summary>
        /// <param name="dtype_id">Datatype identifier for the base datatype. 
        /// Must be an integer datatype.</param>
        /// <returns>Returns the datatype identifier for the new enumeration
        /// datatype if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tenum_create",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t enum_create(hid_t dtype_id);

        /// <summary>
        /// Inserts a new enumeration datatype member.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-EnumInsert
        /// </summary>
        /// <param name="dtype_id">Datatype identifier for the enumeration
        /// datatype.</param>
        /// <param name="name">Name of the new member.</param>
        /// <param name="value">Pointer to the value of the new member.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tenum_insert",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t enum_insert
            (hid_t dtype_id, string name, IntPtr value);

        /// <summary>
        /// Returns the symbol name corresponding to a specified member of an
        /// enumeration datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-EnumNameOf
        /// </summary>
        /// <param name="dtype_id">Enumeration datatype identifier.</param>
        /// <param name="value">Value of the enumeration datatype.</param>
        /// <param name="name">Buffer for output of the symbol name.</param>
        /// <param name="size">The capacity of the buffer, in bytes
        /// (characters).</param>
        /// <returns>Returns a non-negative value if successful. Otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tenum_nameof",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t enum_nameof
            (hid_t dtype_id, IntPtr value, [In][Out]StringBuilder name, size_t size);

        /// <summary>
        /// Returns the value corresponding to a specified member of an
        /// enumeration datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-EnumValueOf
        /// </summary>
        /// <param name="dtype_id">Enumeration datatype identifier.</param>
        /// <param name="name">Symbol name of the enumeration datatype.</param>
        /// <param name="value">Buffer for output of the value of the
        /// enumeration datatype.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tenum_valueof",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t enum_valueof
            (hid_t dtype_id, string name, IntPtr value);

        /// <summary>
        /// Determines whether two datatype identifiers refer to the same
        /// datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Equal
        /// </summary>
        /// <param name="type_id1">Identifier of datatype to compare.</param>
        /// <param name="type_id2">Identifier of datatype to compare.</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, if the datatype has been committed, or 0 (zero),
        /// for <code>FALSE</code>, if the datatype has not been committed.
        /// Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tequal",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t equal(hid_t type_id1, hid_t type_id2);

        /// <summary>
        /// Finds a conversion function.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Find
        /// </summary>
        /// <param name="src_id">Identifier for the source datatype.</param>
        /// <param name="dst_id">Identifier for the destination datatype.</param>
        /// <param name="pcdata">Pointer to type conversion data.</param>
        /// <returns>Returns a pointer to a suitable conversion function if
        /// successful. Otherwise returns <code>NULL</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tfind",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern conv_t find
            (hid_t src_id, hid_t dst_id, ref cdata_t pcdata);

#if HDF5_VER1_10

        /// <summary>
        /// Flushes all buffers associated with a committed datatype to disk.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Tflush.htm
        /// </summary>
        /// <param name="type_id">Identifier of the committed datatype to be
        /// flushed.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tflush",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t flush(hid_t type_id);

#endif

        /// <summary>
        /// Retrieves sizes of array dimensions.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetArrayDims2
        /// </summary>
        /// <param name="adtype_id">Array datatype identifier.</param>
        /// <param name="dims">Sizes of array dimensions.</param>
        /// <returns>Returns the non-negative number of dimensions of the array
        /// type if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_array_dims2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_array_dims
            (hid_t adtype_id, hsize_t[] dims);

        /// <summary>
        /// Returns the rank of an array datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetArrayNdims
        /// </summary>
        /// <param name="adtype_id">Array datatype identifier.</param>
        /// <returns>Returns the rank of the array if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_array_ndims",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_array_ndims(hid_t adtype_id);

        /// <summary>
        /// Returns the datatype class identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetClass
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns datatype class identifier if successful; otherwise
        /// <code>H5T_NO_CLASS</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern class_t get_class(hid_t dtype_id);

        /// <summary>
        /// Returns a copy of a datatype creation property list.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetCreatePlist
        /// </summary>
        /// <param name="dtype_id">Datatype identifier.</param>
        /// <returns>Returns a datatype property list identifier if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_create_plist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_create_plist(hid_t dtype_id);

        /// <summary>
        /// Retrieves the character set type of a string datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetCset
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns a valid character set type if successful;
        /// otherwise <code>H5T.cset_t.CSET_ERROR</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_cset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern cset_t get_cset(hid_t dtype_id);

        /// <summary>
        /// Retrieves the exponent bias of a floating-point type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetEbias
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns the bias if successful; otherwise 0.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_ebias",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern size_t get_ebias(hid_t dtype_id);

        /// <summary>
        /// Retrieves floating point datatype bit field information.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetFields
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <param name="spos">Pointer to location to return floating-point
        /// sign bit.</param>
        /// <param name="epos">Pointer to location to return exponent
        /// bit-position.</param>
        /// <param name="esize">Pointer to location to return size of exponent
        /// in bits.</param>
        /// <param name="mpos">Pointer to location to return mantissa
        /// bit-position.</param>
        /// <param name="msize">Pointer to location to return size of mantissa
        /// in bits.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_fields",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_fields
            (hid_t dtype_id, ref size_t spos, ref size_t epos,
            ref size_t esize, ref size_t mpos, ref size_t msize);

        /// <summary>
        /// Retrieves the internal padding type for unused bits in
        /// floating-point datatypes.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetInpad
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns a valid padding type if successful; otherwise
        /// <code>H5T.pad_t.ERROR</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_inpad",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern pad_t get_inpad(hid_t dtype_id);

        /// <summary>
        /// Returns datatype class of compound datatype member.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetMemberClass
        /// </summary>
        /// <param name="cdtype_id">Datatype identifier of compound object.</param>
        /// <param name="member_no">Compound type member number.</param>
        /// <returns>Returns the datatype class, a non-negative value, if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_member_class",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern class_t get_member_class
            (hid_t cdtype_id, uint member_no);

        /// <summary>
        /// Retrieves the index of a compound or enumeration datatype member.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetMemberIndex
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <param name="field_name">Name of the field or member whose index is
        /// to be retrieved.</param>
        /// <returns>Returns a valid field or member index if successful;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_member_index",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_member_index
            (hid_t dtype_id, string field_name);

        /// <summary>
        /// Retrieves the name of a compound or enumeration datatype member.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetMemberName
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <param name="field_idx">Zero-based index of the field or element
        /// whose name is to be retrieved.</param>
        /// <returns>Returns a pointer to a string allocated in unmanaged
        /// memory if successful; otherwise returns <code>NULL</code>.</returns>
        /// <remarks>The caller is responsible for freeing the allocated
        /// memory.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_member_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern IntPtr get_member_name
            (hid_t dtype_id, uint field_idx);

        /// <summary>
        /// Retrieves the offset of a field of a compound datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetMemberOffset
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <param name="memb_no">Number of the field whose offset is
        /// requested.</param>
        /// <returns></returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_member_offset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern size_t get_member_offset
            (hid_t dtype_id, uint memb_no);

        /// <summary>
        /// Returns the datatype of the specified member.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetMemberType
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <param name="field_idx">Field index (0-based) of the field type to
        /// retrieve.</param>
        /// <returns>Returns the identifier of a copy of the datatype of the
        /// field if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_member_type",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_member_type
            (hid_t dtype_id, uint field_idx);

        /// <summary>
        /// Returns the value of an enumeration datatype member.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetMemberValue
        /// </summary>
        /// <param name="dtype_id">Datatype identifier for the enumeration
        /// datatype.</param>
        /// <param name="memb_no">Number of the enumeration datatype member.</param>
        /// <param name="value">Pointer to a buffer for output of the value of
        /// the enumeration datatype member.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_member_value",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_member_value
            (hid_t dtype_id, uint memb_no, IntPtr value);

        /// <summary>
        /// Returns the native datatype of a specified datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetNativeType
        /// </summary>
        /// <param name="dtype_id">Datatype identifier for the dataset
        /// datatype.</param>
        /// <param name="direction">Direction of search.</param>
        /// <returns>Returns the native datatype identifier for the specified
        /// dataset datatype if successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_native_type",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_native_type
            (hid_t dtype_id, direction_t direction);

        /// <summary>
        /// Retrieves the number of elements in a compound or enumeration
        /// datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetNmembers
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns the number of elements if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_nmembers",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_nmembers(hid_t dtype_id);

        /// <summary>
        /// Retrieves mantissa normalization of a floating-point datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetNorm
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns a valid normalization type if successful;
        /// otherwise <code>H5T.norm_t.ERROR</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_norm",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern norm_t get_norm(hid_t dtype_id);

        /// <summary>
        /// Retrieves the bit offset of the first significant bit.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetOffset
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns an offset value if successful; otherwise returns a
        /// negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_offset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern int get_offset(hid_t dtype_id);

        /// <summary>
        /// Returns the byte order of an atomic datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetOrder
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns a byte order constant if successful; otherwise
        /// <code>H5T.order_t.ERROR</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_order",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern order_t get_order(hid_t dtype_id);

        /// <summary>
        /// Retrieves the padding type of the least and most-significant bit
        /// padding.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetPad
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <param name="lsb">Pointer to location to return least-significant
        /// bit padding type.</param>
        /// <param name="msb">Pointer to location to return most-significant
        /// bit padding type.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_pad",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t get_pad
            (hid_t dtype_id, ref pad_t lsb, ref pad_t msb);

        /// <summary>
        /// Returns the precision of an atomic datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetPrecision
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns the number of significant bits if successful;
        /// otherwise 0.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_precision",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern size_t get_precision(hid_t dtype_id);

        /// <summary>
        /// Retrieves the sign type for an integer type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetSign
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns a valid sign type if successful; otherwise
        /// <code>H5T.sign_t.ERROR</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_sign",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern sign_t get_sign(hid_t dtype_id);

        /// <summary>
        /// Returns the size of a datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetSize
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns the size of the datatype in bytes if successful;
        /// otherwise 0.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern size_t get_size(hid_t dtype_id);

        /// <summary>
        /// Retrieves the type of padding used for a string datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetStrpad
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to query.</param>
        /// <returns>Returns a valid string storage mechanism if successful;
        /// otherwise <code>H5T.str_t.ERROR</code>.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_strpad",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern str_t get_strpad(hid_t dtype_id);

        /// <summary>
        /// Returns the base datatype from which a datatype is derived.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetSuper
        /// </summary>
        /// <param name="dtype_id">Datatype identifier for the derived
        /// datatype.</param>
        /// <returns>Returns the datatype identifier for the base datatype if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_super",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t get_super(hid_t dtype_id);

        /// <summary>
        /// Gets the tag associated with an opaque datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-GetTag
        /// </summary>
        /// <param name="dtype_id">Datatype identifier for the opaque datatype.</param>
        /// <returns>Returns a pointer to a string allocated in unmanaged
        /// memory if successful; otherwise returns <code>NULL</code>.</returns>
        /// <remarks>The caller is responsible for freeing the allocated
        /// memory.</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tget_tag",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern IntPtr get_tag(hid_t dtype_id);

        /// <summary>
        /// Adds a new member to a compound datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Insert
        /// </summary>
        /// <param name="dtype_id">Identifier of compound datatype to modify.</param>
        /// <param name="name">Name of the field to insert.</param>
        /// <param name="offset">Offset in memory structure of the field to
        /// insert.</param>
        /// <param name="field_id">Datatype identifier of the field to insert.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tinsert",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t insert
            (hid_t dtype_id, string name, size_t offset, hid_t field_id);

        /// <summary>
        /// Determines whether datatype is a variable-length string.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-IsVariableString
        /// </summary>
        /// <param name="dtype_id">Datatype identifier.</param>
        /// <returns>Returns <code>TRUE</code> or <code>FALSE</code> if
        /// successful; otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tis_variable_str",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern htri_t is_variable_str(hid_t dtype_id);

        /// <summary>
        /// Locks a datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Lock
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to lock.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tlock",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t lock_datatype(hid_t dtype_id);

        /// <summary>
        /// Opens a committed (named) datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Open2
        /// </summary>
        /// <param name="loc_id">A file or group identifier.</param>
        /// <param name="name">A datatype name, defined within the file or
        /// group identified by <paramref name="loc_id"/>.</param>
        /// <param name="tapl_id">Datatype access property list identifier.</param>
        /// <returns>Returns a committed datatype identifier if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Topen2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t open
            (hid_t loc_id, byte[] name, hid_t tapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens a committed (named) datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Open2
        /// </summary>
        /// <param name="loc_id">A file or group identifier.</param>
        /// <param name="name">A datatype name, defined within the file or
        /// group identified by <paramref name="loc_id"/>.</param>
        /// <param name="tapl_id">Datatype access property list identifier.</param>
        /// <returns>Returns a committed datatype identifier if successful;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Topen2",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t open
            (hid_t loc_id, string name, hid_t tapl_id = H5P.DEFAULT);

        /// <summary>
        /// Recursively removes padding from within a compound datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Pack
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to modify.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tpack",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t pack(hid_t dtype_id);

#if HDF5_VER1_10

        /// <summary>
        /// Refreshes all buffers associated with a committed datatype.
        /// See https://www.hdfgroup.org/HDF5/docNewFeatures/FineTuneMDC/H5Trefresh.htm
        /// </summary>
        /// <param name="type_id">Identifier of the committed datatype to be
        /// refreshed.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Trefresh",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t H5Trefresh(hid_t type_id);

#endif

        /// <summary>
        /// Registers a conversion function.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Register
        /// </summary>
        /// <param name="type">Conversion function type</param>
        /// <param name="name">Name displayed in diagnostic output</param>
        /// <param name="src_id">Identifier of source datatype</param>
        /// <param name="dst_id">Identifier of destination datatype</param>
        /// <param name="func">Function to convert between source and
        /// destination datatypes</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tregister",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t register(pers_t type, string name,
            hid_t src_id, hid_t dst_id, conv_t func);

        /// <summary>
        /// Sets character set to be used in a string or character datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetCset
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to modify.</param>
        /// <param name="cset">Character set type.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_cset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_cset(hid_t dtype_id, cset_t cset);

        /// <summary>
        /// Sets the exponent bias of a floating-point type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetEbias
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="ebias">Exponent bias value.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_ebias",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_ebias(hid_t dtype_id, size_t ebias);

        /// <summary>
        /// Sets locations and sizes of floating point bit fields.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetFields
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="spos">Sign position, i.e., the bit offset of the
        /// floating-point sign bit.</param>
        /// <param name="epos">Exponent bit position.</param>
        /// <param name="esize">Size of exponent in bits.</param>
        /// <param name="mpos">Mantissa bit position.</param>
        /// <param name="msize">Size of mantissa in bits.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_fields",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_fields
            (hid_t dtype_id, size_t spos, size_t epos, size_t esize,
            size_t mpos, size_t msize);

        /// <summary>
        /// Sets interal bit padding of floating point numbers.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetInpad
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to modify.</param>
        /// <param name="inpad">Padding type.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_inpad",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_inpad(hid_t dtype_id, pad_t inpad);

        /// <summary>
        /// Sets the mantissa normalization of a floating-point datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetNorm
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="norm">Mantissa normalization type.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_norm",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_norm(hid_t dtype_id, norm_t norm);

        /// <summary>
        /// Sets the bit offset of the first significant bit.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetOffset
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="offset">Offset of first significant bit.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_offset",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_offset(hid_t dtype_id, size_t offset);

        /// <summary>
        /// Sets the byte order of a datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetOrder
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="order">Byte order.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_order",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_order(hid_t dtype_id, order_t order);

        /// <summary>
        /// Sets the least and most-significant bits padding types.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetPad
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="lsb">Padding type for least-significant bits.</param>
        /// <param name="msb">Padding type for most-significant bits.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_pad",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_pad
            (hid_t dtype_id, pad_t lsb, pad_t msb);

        /// <summary>
        /// Sets the precision of an atomic datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetPrecision
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="precision">Number of bits of precision for datatype.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_precision",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_precision
            (hid_t dtype_id, size_t precision);

        /// <summary>
        /// Sets the sign property for an integer type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetSign
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to set.</param>
        /// <param name="sign">Sign type.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_sign",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_sign(hid_t dtype_id, sign_t sign);

        /// <summary>
        /// Sets the total size for a datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetSize
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype for which the size is
        /// being changed</param>
        /// <param name="size">New datatype size in bytes or <code>VARIABLE</code></param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_size(hid_t dtype_id, size_t size);

        /// <summary>
        /// Defines the type of padding used for character strings.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetStrpad
        /// </summary>
        /// <param name="dtype_id">Identifier of datatype to modify.</param>
        /// <param name="strpad">String padding type.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_strpad",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_strpad(hid_t dtype_id, str_t strpad);

        /// <summary>
        /// Tags an opaque datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-SetTag
        /// </summary>
        /// <param name="dtype_id">Datatype identifier for the opaque datatype
        /// to be tagged.</param>
        /// <param name="tag">Descriptive ASCII string with which the opaque
        /// datatype is to be tagged.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks><paramref name="tag"/> is intended to provide a concise
        /// description; the maximum size is hard-coded in the HDF5 Library as
        /// 256 bytes </remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tset_tag",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t set_tag( hid_t dtype_id, string tag);

        /// <summary>
        /// Removes a conversion function.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-Unregister
        /// </summary>
        /// <param name="type">Conversion function type</param>
        /// <param name="name">Name displayed in diagnostic output.</param>
        /// <param name="src_id">Identifier of source datatype.</param>
        /// <param name="dst_id">Identifier of destination datatype.</param>
        /// <param name="func">Function to convert between source and
        /// destination datatypes.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tunregister",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t unregister
            (pers_t type, string name, hid_t src_id, hid_t dst_id, conv_t func);

        /// <summary>
        /// Creates a new variable-length array datatype.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5T.html#Datatype-VLCreate
        /// </summary>
        /// <param name="base_type_id">Base type of datatype to create.</param>
        /// <returns>Returns datatype identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tvlen_create",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern hid_t vlen_create(hid_t base_type_id);
    }
}
