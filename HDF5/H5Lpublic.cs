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

using haddr_t = System.UInt64;
using hbool_t = System.UInt32;
using herr_t = System.Int32;
using hsize_t = System.UInt64;
using htri_t = System.Int32;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif


namespace HDF.PInvoke
{
    public unsafe sealed class H5L
    {
        static H5L() { H5.open(); }

        /// <summary>
        /// Maximum length of a link's name
        /// (encoded in a 32-bit unsigned integer: 4GB - 1)
        /// </summary>
        public const UInt32 MAX_LINK_NAME_LEN = unchecked((UInt32) (-1));

        /// <summary>
        /// Constant to indicate operation occurs on same location
        /// </summary>
        public const hid_t SAME_LOC = 0;

        /// <summary>
        /// Current version of the class_t struct
        /// </summary>
        public const int LINK_CLASS_T_VERS = 0;

        /// <summary>
        /// Link class types.
        /// Values less than 64 are reserved for the HDF5 library's internal
        /// use. Values 64 to 255 are for "user-defined" link class types;
        /// these types are defined by HDF5 but their behavior can be
        /// overridden by users. Users who want to create new classes of links
        /// should contact the HDF5 development team at hdfhelp@hdfgroup.org .
        /// These values can never change because they appear in HDF5 files.
        /// </summary>
        public enum type_t
        {
            /// <summary>
            /// Invalid link type id [value = -1]
            /// </summary>
            ERROR = (-1),
            /// <summary>
            /// Hard link id [value = 0]
            /// </summary>
            HARD = 0,
            /// <summary>
            /// Soft link id [value = 1]
            /// </summary>
            SOFT = 1,
            /// <summary>
            /// External link id [value = 64]
            /// </summary>
            EXTERNAL = 64,
            /// <summary>
            /// Maximum link type id [value = 255]
            /// </summary>
            MAX = 255
        }

        /// <summary>
        /// Maximum value link value for "built-in" link types
        /// </summary>
        public const type_t TYPE_BUILTIN_MAX = type_t.SOFT;
        /// <summary>
        /// Link ids at or above this value are "user-defined" link types.
        /// </summary>
        public const type_t TYPE_UD_MIN = type_t.EXTERNAL;

        /// <summary>
        /// Information struct for link (for H5Lget_info/H5Lget_info_by_idx)
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct info_t
        {
            /// <summary>
            /// Type of link
            /// </summary>
            public type_t type;
            /// <summary>
            /// Indicate if creation order is valid
            /// </summary>
            public hbool_t corder_valid;
            /// <summary>
            /// Creation order
            /// </summary>
            public Int64 corder;
            /// <summary>
            /// Character set of link name
            /// </summary>
            public H5T.cset_t cset;
            /// <summary>
            /// Address to which hard link points or
            /// size of a soft link or UD link value
            /// </summary>
            public u_t u;

            /* union -> same field offset, see H5Lpublic.h */
            [StructLayout(LayoutKind.Explicit)]
            public struct u_t
            {
                [FieldOffset(0)]
                public haddr_t address;
                [FieldOffset(0)]
                public size_t val_size;
            }
        }

        /// <summary>
        /// Link creation callback
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="loc_group"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <param name="lcpl_id"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t create_func_t
            (byte[] link_name, hid_t loc_group, IntPtr lnkdata,
            size_t lnkdata_size, hid_t lcpl_id);

        /// <summary>
        /// Link creation callback
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="loc_group"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <param name="lcpl_id"></param>
        /// <returns></returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t create_func_ascii_t
            (string link_name, hid_t loc_group, IntPtr lnkdata,
            size_t lnkdata_size, hid_t lcpl_id);

        /// <summary>
        /// Callback for when the link is moved
        /// </summary>
        /// <param name="new_name"></param>
        /// <param name="new_loc"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t move_func_t
        (byte[] new_name, hid_t new_loc, IntPtr lnkdata, size_t lnkdata_size);

        /// <summary>
        /// Callback for when the link is moved
        /// </summary>
        /// <param name="new_name"></param>
        /// <param name="new_loc"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <returns></returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t move_func_ascii_t
        (string new_name, hid_t new_loc, IntPtr lnkdata, size_t lnkdata_size);

        /// <summary>
        /// Callback for when the link is copied
        /// </summary>
        /// <param name="new_name"></param>
        /// <param name="new_loc"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t copy_func_t
        (byte[] new_name, hid_t new_loc, IntPtr lnkdata, size_t lnkdata_size);

        /// <summary>
        /// Callback for when the link is copied
        /// </summary>
        /// <param name="new_name"></param>
        /// <param name="new_loc"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <returns></returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t copy_func_ascii_t
        (string new_name, hid_t new_loc, IntPtr lnkdata, size_t lnkdata_size);

        /// <summary>
        /// Callback during link traversal
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="cur_group"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <param name="lapl_id"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t traverse_func_t
        (byte[] link_name, hid_t cur_group, IntPtr lnkdata,
        size_t lnkdata_size, hid_t lapl_id);

        /// <summary>
        /// Callback during link traversal
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="cur_group"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <param name="lapl_id"></param>
        /// <returns></returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t traverse_func_ascii_t
        (string link_name, hid_t cur_group, IntPtr lnkdata,
        size_t lnkdata_size, hid_t lapl_id);

        /// <summary>
        /// Callback for when the link is deleted
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="file"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t delete_func_t
        (byte[] link_name, hid_t file, IntPtr lnkdata, size_t lnkdata_size);

        /// <summary>
        /// Callback for when the link is deleted
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="file"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <returns></returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t delete_func_ascii_t
        (string link_name, hid_t file, IntPtr lnkdata, size_t lnkdata_size);

        /// <summary>
        /// Callback for querying the link
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <param name="buf"></param>
        /// <param name="buf_size"></param>
        /// <returns>Returns the size of the buffer needed</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ssize_t query_func_t
        (byte[] link_name, IntPtr lnkdata, size_t lnkdata_size,
        IntPtr buf, size_t buf_size);

        /// <summary>
        /// Callback for querying the link
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="lnkdata"></param>
        /// <param name="lnkdata_size"></param>
        /// <param name="buf"></param>
        /// <param name="buf_size"></param>
        /// <returns>Returns the size of the buffer needed</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate ssize_t query_func_ascii_t
        (string link_name, IntPtr lnkdata, size_t lnkdata_size,
        IntPtr buf, size_t buf_size);

        /// <summary>
        /// User-defined link types
        /// </summary>
        public struct class_t
        {
            /// <summary>
            /// Version number of this struct
            /// </summary>
            public int version;
            /// <summary>
            /// Link type ID
            /// </summary>
            public type_t id;
            /// <summary>
            /// Comment for debugging
            /// </summary>
            public string comment;
            /// <summary>
            /// Callback during link creation
            /// </summary>
            public create_func_t create_func;
            /// <summary>
            /// Callback after moving link
            /// </summary>
            public move_func_t move_func;
            /// <summary>
            /// Callback after copying link
            /// </summary>
            public copy_func_t copy_func;
            /// <summary>
            /// Callback during link traversal
            /// </summary>
            public traverse_func_t trav_func;
            /// <summary>
            /// Callback for link deletion
            /// </summary>
            public delete_func_t del_func;
            /// <summary>
            /// Callback for queries
            /// </summary>
            public query_func_t query_func;
        }

        /// <summary>
        /// Prototype for H5Literate/H5Literate_by_name() operator
        /// </summary>
        /// <param name="group">Group that serves as root of the iteration</param>
        /// <param name="name">Name of link, relative to <paramref name="group"/>,
        /// being examined at current step of the iteration</param>
        /// <param name="info"><code>H5L.info_t</code> struct containing
        /// information regarding that link</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application in processing the link</param>
        /// <returns>Zero causes the visit iterator to continue, returning zero
        /// when all group members have been processed. A positive value causes
        /// the visit iterator to immediately return that positive value,
        /// indicating short-circuit success. A negative value causes the visit
        /// iterator to immediately return that value, indicating failure.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t iterate_t
        (hid_t group, IntPtr name, ref info_t info, IntPtr op_data);

        /// <summary>
        /// Callback for external link traversal
        /// </summary>
        /// <param name="parent_file_name"></param>
        /// <param name="parent_group_name"></param>
        /// <param name="child_file_name"></param>
        /// <param name="child_object_name"></param>
        /// <param name="acc_flags"></param>
        /// <param name="fapl_id"></param>
        /// <param name="op_data"></param>
        /// <returns></returns>
        /// <remarks>File names MUST be ASCII strings.</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t elink_traverse_t
        (string parent_file_name, byte[] parent_group_name,
        string child_file_name, byte[] child_object_name,
        ref uint acc_flags, hid_t fapl_id, IntPtr op_data);

        /// <summary>
        /// Callback for external link traversal
        /// </summary>
        /// <param name="parent_file_name"></param>
        /// <param name="parent_group_name"></param>
        /// <param name="child_file_name"></param>
        /// <param name="child_object_name"></param>
        /// <param name="acc_flags"></param>
        /// <param name="fapl_id"></param>
        /// <param name="op_data"></param>
        /// <returns></returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public delegate herr_t elink_traverse_ascii_t
        (string parent_file_name, string parent_group_name,
        string child_file_name, string child_object_name,
        ref uint acc_flags, hid_t fapl_id, IntPtr op_data);

        /// <summary>
        /// Copies a link from one location to another.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Copy
        /// </summary>
        /// <param name="src_loc">Location identifier of the source link</param>
        /// <param name="src_name">Name of the link to be copied</param>
        /// <param name="dst_loc">Location identifier specifying the destination
        /// of the copy</param>
        /// <param name="dst_name">Name to be assigned to the new copy</param>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint="H5Lcopy",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t copy
            (hid_t src_loc, byte[] src_name, hid_t dst_loc, byte[] dst_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Copies a link from one location to another.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Copy
        /// </summary>
        /// <param name="src_loc">Location identifier of the source link</param>
        /// <param name="src_name">Name of the link to be copied</param>
        /// <param name="dst_loc">Location identifier specifying the destination
        /// of the copy</param>
        /// <param name="dst_name">Name to be assigned to the new copy</param>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcopy",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t copy
            (hid_t src_loc, string src_name, hid_t dst_loc, string dst_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates an external link, a soft link to an object in a different file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateExternal
        /// </summary>
        /// <param name="file_name">Name of the target file containing the
        /// target object</param>
        /// <param name="obj_name">Path within the target file to the target
        /// object</param>
        /// <param name="link_loc_id">File or group identifier where the new
        /// link is to be created</param>
        /// <param name="link_name">Name of the new link, relative to
        /// <paramref name="link_loc_id"/></param>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>The <paramref name="file_name"/> must be an ASCII string!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_external",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_external
            (string file_name, byte[] obj_name, hid_t link_loc_id,
            byte[] link_name, hid_t lcpl_id = H5P.DEFAULT,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates an external link, a soft link to an object in a different file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateExternal
        /// </summary>
        /// <param name="file_name">Name of the target file containing the
        /// target object</param>
        /// <param name="obj_name">Path within the target file to the target
        /// object</param>
        /// <param name="link_loc_id">File or group identifier where the new
        /// link is to be created</param>
        /// <param name="link_name">Name of the new link, relative to
        /// <paramref name="link_loc_id"/></param>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_external",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_external
            (string file_name, string obj_name, hid_t link_loc_id,
            string link_name, hid_t lcpl_id = H5P.DEFAULT,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a hard link to an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateHard
        /// </summary>
        /// <param name="cur_loc">The file or group identifier for the target
        /// object.</param>
        /// <param name="cur_name">Name of the target object, which must
        /// already exist.</param>
        /// <param name="dst_loc">The file or group identifier for the new link.</param>
        /// <param name="dst_name">The name of the new link.</param>
        /// <param name="lcpl_id">Link creation property list identifier.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_hard",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_hard
            (hid_t cur_loc, byte[] cur_name, hid_t dst_loc, byte[] dst_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a hard link to an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateHard
        /// </summary>
        /// <param name="cur_loc">The file or group identifier for the target
        /// object.</param>
        /// <param name="cur_name">Name of the target object, which must
        /// already exist.</param>
        /// <param name="dst_loc">The file or group identifier for the new link.</param>
        /// <param name="dst_name">The name of the new link.</param>
        /// <param name="lcpl_id">Link creation property list identifier.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings only!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_hard",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_hard
            (hid_t cur_loc, string cur_name, hid_t dst_loc, string dst_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a soft link to an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateSoft
        /// </summary>
        /// <param name="link_target">Path to the target object, which is not
        /// required to exist.</param>
        /// <param name="link_loc_id">The file or group identifier for the new
        /// link.</param>
        /// <param name="link_name">The name of the new link.</param>
        /// <param name="lcpl_id">Link creation property list identifier.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_soft",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_soft
            (byte[] link_target, hid_t link_loc_id, byte[] link_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a soft link to an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateSoft
        /// </summary>
        /// <param name="link_target">Path to the target object, which is not
        /// required to exist.</param>
        /// <param name="link_loc_id">The file or group identifier for the new
        /// link.</param>
        /// <param name="link_name">The name of the new link.</param>
        /// <param name="lcpl_id">Link creation property list identifier.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_soft",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_soft
            (string link_target, hid_t link_loc_id, string link_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a link of a user-defined type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateUD
        /// </summary>
        /// <param name="link_loc_id">Link location identifier</param>
        /// <param name="link_name">Link name</param>
        /// <param name="link_type">User-defined link class</param>
        /// <param name="udata">User-supplied link information</param>
        /// <param name="udata_size">Size of udata buffer</param>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_ud",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_ud
            (hid_t link_loc_id, byte[] link_name, type_t link_type,
            IntPtr udata, size_t udata_size,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates a link of a user-defined type.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-CreateUD
        /// </summary>
        /// <param name="link_loc_id">Link location identifier</param>
        /// <param name="link_name">Link name</param>
        /// <param name="link_type">User-defined link class</param>
        /// <param name="udata">User-supplied link information</param>
        /// <param name="udata_size">Size of udata buffer</param>
        /// <param name="lcpl_id">Link creation property list identifier</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lcreate_ud",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t create_ud
            (hid_t link_loc_id, string link_name, type_t link_type,
            IntPtr udata, size_t udata_size,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Removes a link from a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Delete
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group containing
        /// the object.</param>
        /// <param name="name">Name of the link to delete.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ldelete",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete(hid_t loc_id, byte[] name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Removes a link from a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Delete
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group containing
        /// the object.</param>
        /// <param name="name">Name of the link to delete.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ldelete",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete(hid_t loc_id, string name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Removes the n-th link in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-DeleteByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Index or field which determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ldelete_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete_by_idx
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Removes the n-th link in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-DeleteByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Index or field which determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Ldelete_by_idx",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Determine whether a link with the specified name exists in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Exists
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to query.</param>
        /// <param name="name">The name of the link to check.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns 1 or 0 if successful; otherwise returns a negative
        /// value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lexists",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists(hid_t loc_id, byte[] name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Determine whether a link with the specified name exists in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Exists
        /// </summary>
        /// <param name="loc_id">Identifier of the file or group to query.</param>
        /// <param name="name">The name of the link to check.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns 1 or 0 if successful; otherwise returns a negative
        /// value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lexists",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists(hid_t loc_id, string name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Returns information about a link.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetInfo
        /// </summary>
        /// <param name="loc_id">File or group identifier.</param>
        /// <param name="name">Name of the link for which information is being
        /// sought.</param>
        /// <param name="linfo">Buffer in which link information is returned.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info
            (hid_t loc_id, byte[] name, ref info_t linfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Returns information about a link.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetInfo
        /// </summary>
        /// <param name="loc_id">File or group identifier.</param>
        /// <param name="name">Name of the link for which information is being
        /// sought.</param>
        /// <param name="linfo">Buffer in which link information is returned.</param>
        /// <param name="lapl_id">Link access property list identifier.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_info",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info
            (hid_t loc_id, string name, ref info_t linfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves metadata for a link in a group, according to the order
        /// within a field or index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Index or field which determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="linfo">Buffer in which link value is returned</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_info_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t linfo,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves metadata for a link in a group, according to the order
        /// within a field or index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Index or field which determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="linfo">Buffer in which link value is returned</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_info_by_idx",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, ref info_t linfo /*out*/,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves name of the nth link in a group, according to the order
        /// within a specified field or index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetNameByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Index or field which determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="name">Buffer in which link value is returned</param>
        /// <param name="size">Size in bytes of <paramref name="name"/></param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns the size of the link name if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_name_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name_by_idx
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, [In][Out]byte[] name, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves name of the nth link in a group, according to the order
        /// within a specified field or index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetNameByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Index or field which determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="name">Buffer in which link value is returned</param>
        /// <param name="size">Size in bytes of <paramref name="name"/></param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns the size of the link name if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_name_by_idx",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, [In][Out]StringBuilder name, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves name of the nth link in a group, according to the order
        /// within a specified field or index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetNameByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Index or field which determines the order</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="name">Buffer in which link value is returned</param>
        /// <param name="size">Size in bytes of <paramref name="name"/></param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns the size of the link name if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_name_by_idx",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name_by_idx(hid_t loc_id, string group_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n,
            IntPtr name /*out*/, size_t size, hid_t lapl_id);        

        /// <summary>
        /// Returns the value of a symbolic link.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetVal
        /// </summary>
        /// <param name="loc_id">File or group identifier.</param>
        /// <param name="name">Link whose value is to be returned.</param>
        /// <param name="buf">The buffer to hold the returned link value.</param>
        /// <param name="size">Maximum number of characters of link value to be
        /// returned.</param>
        /// <param name="lapl_id">List access property list identifier.</param>
        /// <returns>Returns a non-negative value, with the link value in
        /// <paramref name="buf"/>, if successful. Otherwise returns a negative
        /// value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_val",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_val
            (hid_t loc_id, byte[] name, IntPtr buf, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Returns the value of a symbolic link.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetVal
        /// </summary>
        /// <param name="loc_id">File or group identifier.</param>
        /// <param name="name">Link whose value is to be returned.</param>
        /// <param name="buf">The buffer to hold the returned link value.</param>
        /// <param name="size">Maximum number of characters of link value to be
        /// returned.</param>
        /// <param name="lapl_id">List access property list identifier.</param>
        /// <returns>Returns a non-negative value, with the link value in
        /// <paramref name="buf"/>, if successful. Otherwise returns a negative
        /// value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_val",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_val
            (hid_t loc_id, string name, IntPtr buf, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves value of the nth link in a group, according to the order
        /// within an index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetValByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="buf">Pointer to buffer in which link value is returned</param>
        /// <param name="size">Size in bytes of <paramref name="group_name"/></param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_val_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_val_by_idx
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, IntPtr buf, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves value of the nth link in a group, according to the order
        /// within an index.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-GetValByIdx
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order within field or index</param>
        /// <param name="n">Link for which to retrieve information</param>
        /// <param name="buf">Pointer to buffer in which link value is returned</param>
        /// <param name="size">Size in bytes of <paramref name="group_name"/></param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lget_val_by_idx",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_val_by_idx
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, IntPtr buf, size_t size,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Determines whether a class of user-defined links is registered.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-IsRegistered
        /// </summary>
        /// <param name="id">User-defined link class identifier</param>
        /// <returns>Returns a positive value if the link class has been
        /// registered and zero if it is unregistered. Otherwise returns a
        /// negative value; this may mean that the identifier is not a valid
        /// user-defined class identifier.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lis_registered",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t is_registered(type_t id);

        /// <summary>
        /// Iterates through links in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Iterate
        /// </summary>
        /// <param name="grp_id">Identifier specifying subject group</param>
        /// <param name="idx_type">Type of index which determines the order</param>
        /// <param name="order">Order within index</param>
        /// <param name="idx">Iteration position at which to start</param>
        /// <param name="op">Callback function passing data regarding the link
        /// to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the link</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Literate",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t iterate
            (hid_t grp_id, H5.index_t idx_type, H5.iter_order_t order,
            ref hsize_t idx, iterate_t op, IntPtr op_data);

        /// <summary>
        /// Iterates through links in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-IterateByName
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Type of index which determines the order</param>
        /// <param name="order">Order within index</param>
        /// <param name="idx">Iteration position at which to start</param>
        /// <param name="op">Callback function passing data regarding the link
        /// to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the link</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Literate_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t iterate_by_name
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, ref hsize_t idx, iterate_t op,
            IntPtr op_data, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Iterates through links in a group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-IterateByName
        /// </summary>
        /// <param name="loc_id">File or group identifier specifying location
        /// of subject group</param>
        /// <param name="group_name">Name of subject group</param>
        /// <param name="idx_type">Type of index which determines the order</param>
        /// <param name="order">Order within index</param>
        /// <param name="idx">Iteration position at which to start</param>
        /// <param name="op">Callback function passing data regarding the link
        /// to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the link</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Literate_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t iterate_by_name
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, ref hsize_t idx, iterate_t op,
            IntPtr op_data, hid_t lapl_id = H5P.DEFAULT);
        
        /// <summary>
        /// Moves a link within an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Move
        /// </summary>
        /// <param name="src_loc">Original file or group identifier.</param>
        /// <param name="src_name">Original link name.</param>
        /// <param name="dst_loc">Destination file or group identifier.</param>
        /// <param name="dst_name">New link name.</param>
        /// <param name="lcpl_id">Link creation property list identifier to be
        /// associated with the new link.</param>
        /// <param name="lapl_id">Link access property list identifier to be
        /// associated with the new link.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lmove",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t move
            (hid_t src_loc, byte[] src_name, hid_t dst_loc, byte[] dst_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Moves a link within an HDF5 file.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Move
        /// </summary>
        /// <param name="src_loc">Original file or group identifier.</param>
        /// <param name="src_name">Original link name.</param>
        /// <param name="dst_loc">Destination file or group identifier.</param>
        /// <param name="dst_name">New link name.</param>
        /// <param name="lcpl_id">Link creation property list identifier to be
        /// associated with the new link.</param>
        /// <param name="lapl_id">Link access property list identifier to be
        /// associated with the new link.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lmove",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t move
            (hid_t src_loc, string src_name, hid_t dst_loc, string dst_name,
            hid_t lcpl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Registers a user-defined link class or changes behavior of an existing class.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Register
        /// </summary>
        /// <param name="cls">Pointer to a buffer containing the struct
        /// describing the user-defined link class</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lregister",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t register(ref class_t cls);

        /// <summary>
        /// Decodes external link information.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-UnpackELinkVal
        /// </summary>
        /// <param name="ext_linkval">Buffer containing external link
        /// information</param>
        /// <param name="link_size">Size, in bytes, of the
        /// <paramref name="ext_linkval"/> buffer</param>
        /// <param name="flags">External link flags, packed as a bitmap</param>
        /// <param name="filename">Returned filename</param>
        /// <param name="obj_path">Returned object path, relative to
        /// <paramref name="filename"/></param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lunpack_elink_val",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t unpack_elink_val
            (IntPtr ext_linkval, size_t link_size, ref uint flags,
            ref IntPtr filename, ref IntPtr obj_path);

        /// <summary>
        /// Unregisters a class of user-defined links.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Unregister
        /// </summary>
        /// <param name="id">User-defined link class identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lunregister",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t unregister(type_t id);
        
        /// <summary>
        /// Recursively visits all links starting from a specified group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-Visit
        /// </summary>
        /// <param name="grp_id">Identifier of the group at which the recursive
        /// iteration begins.</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the link
        /// to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the link</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lvisit",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit
            (hid_t grp_id, H5.index_t idx_type, H5.iter_order_t order,
            iterate_t op, IntPtr op_data);
        
        /// <summary>
        /// Recursively visits all links starting from a specified group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-VisitByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file or group</param>
        /// <param name="group_name">Name of the group, generally relative to
        /// <paramref name="loc_id"/>, that will serve as root of the iteration</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the link
        /// to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the link</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lvisit_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit_by_name
            (hid_t loc_id, byte[] group_name, H5.index_t idx_type,
            H5.iter_order_t order, iterate_t op, IntPtr op_data,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Recursively visits all links starting from a specified group.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5L.html#Link-VisitByName
        /// </summary>
        /// <param name="loc_id">Identifier of a file or group</param>
        /// <param name="group_name">Name of the group, generally relative to
        /// <paramref name="loc_id"/>, that will serve as root of the iteration</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which index is traversed</param>
        /// <param name="op">Callback function passing data regarding the link
        /// to the calling application</param>
        /// <param name="op_data">User-defined pointer to data required by the
        /// application for its processing of the link</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>On success, returns the return value of the first operator
        /// that returns a positive value, or zero if all members were
        /// processed with no operator returning non-zero. On failure, returns
        /// a negative value if something goes wrong within the library, or the
        /// first negative value returned by an operator.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Lvisit_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t visit_by_name
            (hid_t loc_id, string group_name, H5.index_t idx_type,
            H5.iter_order_t order, iterate_t op, IntPtr op_data,
            hid_t lapl_id = H5P.DEFAULT);
    }
}