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

using H5O_msg_crt_idx_t = System.UInt32;

using ssize_t = System.IntPtr;

#if HDF5_VER1_10
using hid_t = System.Int64;
#else
using hid_t = System.Int32;
#endif

namespace HDF.PInvoke
{
    public unsafe sealed class H5A
    {
        static H5A() { H5.open(); }

        /// <summary>
        /// Information struct for attribute
        /// (for H5Aget_info/H5Aget_info_by_idx)
        /// </summary>
        public struct info_t
        {
            /// <summary>
            /// Indicate if creation order is valid
            /// </summary>
            public hbool_t corder_valid;
            /// <summary>
            /// Creation order
            /// </summary>
            public H5O_msg_crt_idx_t corder;
            /// <summary>
            /// Character set of attribute name
            /// </summary>
            public H5T.cset_t cset;
            /// <summary>
            /// Size of raw data
            /// </summary>
            public hsize_t data_size;
        };

        /// <summary>
        /// Delegate for H5Aiterate2() callbacks
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Iterate2
        /// </summary>
        /// <param name="location_id">The location identifier for the group or
        /// dataset being iterated over</param>
        /// <param name="attr_name">The name of the current object attribute.</param>
        /// <param name="ainfo">The attribute’s <code>info</code>struct</param>
        /// <param name="op_data">A pointer referencing operator data passed
        /// to <code>iterate</code></param>
        /// <returns>Valid return values from an operator and the resulting
        /// H5Aiterate2 and op behavior are as follows: Zero causes the iterator
        /// to continue, returning zero when all attributes have been processed.
        /// A positive value causes the iterator to immediately return that
        /// positive value, indicating short-circuit success. The iterator can
        /// be restarted at the next attribute, as indicated by the return
        /// value of <code>n</code>. A negative value causes the iterator to
        /// immediately return that value, indicating failure. The iterator can
        /// be restarted at the next attribute, as indicated by the return value
        /// of <code>n</code>.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate herr_t operator_t
            (hid_t location_id, IntPtr attr_name, ref info_t ainfo,
            IntPtr op_data);

        /// <summary>
        /// Closes the specified attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Close
        /// </summary>
        /// <param name="attr_id">Attribute to release access to.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aclose",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t close(hid_t attr_id);

        /// <summary>
        /// Creates an attribute attached to a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Create2
        /// </summary>
        /// <param name="loc_id">Location or object identifier</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="type_id">Attribute datatype identifier</param>
        /// <param name="space_id">Attribute dataspace identifier</param>
        /// <param name="acpl_id">Attribute creation property list identifier</param>
        /// <param name="aapl_id">Attribute access property list identifier</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Acreate2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t create
            (hid_t loc_id, byte[] attr_name, hid_t type_id, hid_t space_id,
            hid_t acpl_id = H5P.DEFAULT, hid_t aapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates an attribute attached to a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Create2
        /// </summary>
        /// <param name="loc_id">Location or object identifier</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="type_id">Attribute datatype identifier</param>
        /// <param name="space_id">Attribute dataspace identifier</param>
        /// <param name="acpl_id">Attribute creation property list identifier</param>
        /// <param name="aapl_id">Attribute access property list identifier</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Acreate2",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t create
            (hid_t loc_id, string attr_name, hid_t type_id, hid_t space_id,
            hid_t acpl_id = H5P.DEFAULT, hid_t aapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates an attribute attached to a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-CreateByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name, relative to <paramref name="loc_id"/>,
        /// of object that attribute is to be attached to</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="type_id">Attribute datatype identifier</param>
        /// <param name="space_id">Attribute dataspace identifier</param>
        /// <param name="acpl_id">Attribute creation property list identifier</param>
        /// <param name="aapl_id">Attribute access property list identifier</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Acreate_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t create_by_name
            (hid_t loc_id, byte[] obj_name, byte[] attr_name, hid_t type_id,
            hid_t space_id, hid_t acpl_id = H5P.DEFAULT,
            hid_t aapl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Creates an attribute attached to a specified object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-CreateByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name, relative to <paramref name="loc_id"/>,
        /// of object that attribute is to be attached to</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="type_id">Attribute datatype identifier</param>
        /// <param name="space_id">Attribute dataspace identifier</param>
        /// <param name="acpl_id">Attribute creation property list identifier</param>
        /// <param name="aapl_id">Attribute access property list identifier</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Acreate_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t create_by_name
            (hid_t loc_id, string obj_name, string attr_name, hid_t type_id,
            hid_t space_id, hid_t acpl_id = H5P.DEFAULT,
            hid_t aapl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Deletes an attribute from a specified location.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Delete
        /// </summary>
        /// <param name="loc_id">Identifier of the dataset, group, or named
        /// datatype to have the attribute deleted from.</param>
        /// <param name="name">Name of the attribute to delete.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Adelete",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete(hid_t loc_id, byte[] name);

        /// <summary>
        /// Deletes an attribute from a specified location.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Delete
        /// </summary>
        /// <param name="loc_id">Identifier of the dataset, group, or named
        /// datatype to have the attribute deleted from.</param>
        /// <param name="name">Name of the attribute to delete.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Adelete",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete(hid_t loc_id, string name);

        /// <summary>
        /// Deletes an attribute from an object according to index order.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-DeleteByIdx
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location, from
        /// which attribute is to be removed</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which to iterate over index</param>
        /// <param name="n">Offset within index</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Adelete_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete_by_idx
            (hid_t loc_id, byte[] obj_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Deletes an attribute from an object according to index order.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-DeleteByIdx
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location, from
        /// which attribute is to be removed</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which to iterate over index</param>
        /// <param name="n">Offset within index</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Adelete_by_idx",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete_by_idx
            (hid_t loc_id, string obj_name, H5.index_t idx_type,
            H5.iter_order_t order, hsize_t n, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Removes an attribute from a specified location.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-DeleteByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location, from
        /// which attribute is to be removed</param>
        /// <param name="attr_name">Name of attribute to delete</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Adelete_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete_by_name
            (hid_t loc_id, byte[] obj_name, byte[] attr_name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Removes an attribute from a specified location.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-DeleteByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location, from
        /// which attribute is to be removed</param>
        /// <param name="attr_name">Name of attribute to delete</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Adelete_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t delete_by_name
            (hid_t loc_id, string obj_name, string attr_name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Determines whether an attribute with a given name exists on an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Exists
        /// </summary>
        /// <param name="obj_id">Object identifier</param>
        /// <param name="attr_name">Attribute name</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, or 0 (zero), for <code>FALSE</code>. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aexists",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists(hid_t obj_id, byte[] attr_name);

        /// <summary>
        /// Determines whether an attribute with a given name exists on an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Exists
        /// </summary>
        /// <param name="obj_id">Object identifier</param>
        /// <param name="attr_name">Attribute name</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, or 0 (zero), for <code>FALSE</code>. Otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aexists",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists(hid_t obj_id, string attr_name);

        /// <summary>
        /// Determines whether an attribute with a given name exists on an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-ExistsByName
        /// </summary>
        /// <param name="loc_id">Location identifier</param>
        /// <param name="obj_name">Object name</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, or 0 (zero), for <code>FALSE</code>. Otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aexists_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists_by_name
            (hid_t loc_id, byte[] obj_name, byte[] attr_name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Determines whether an attribute with a given name exists on an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-ExistsByName
        /// </summary>
        /// <param name="loc_id">Location identifier</param>
        /// <param name="obj_name">Object name</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>When successful, returns a positive value, for
        /// <code>TRUE</code>, or 0 (zero), for <code>FALSE</code>. Otherwise
        /// returns a negative value.</returns>
        /// <remarks>ANSI strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aexists_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static htri_t exists_by_name
            (hid_t loc_id, string obj_name, string attr_name,
            hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Gets an attribute creation property list identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetCreatePlist
        /// </summary>
        /// <param name="attr_id">Identifier of the attribute.</param>
        /// <returns>Returns an identifier for the attribute’s creation property
        /// list if successful. Otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_create_plist",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t get_create_plist(hid_t attr_id);

        /// <summary>
        /// Retrieves attribute information, by attribute identifier.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetInfo
        /// </summary>
        /// <param name="attr_id">Attribute identifier</param>
        /// <param name="ainfo">Attribute information struct</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_info",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info(hid_t attr_id, ref info_t ainfo);

        /// <summary>
        /// Retrieves attribute information, by attribute index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Index traversal order</param>
        /// <param name="n">Attribute’s position in index</param>
        /// <param name="ainfo">Struct containing returned attribute
        /// information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_info_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, byte[] obj_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n,
            ref info_t ainfo, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves attribute information, by attribute index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetInfoByIdx
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Index traversal order</param>
        /// <param name="n">Attribute’s position in index</param>
        /// <param name="ainfo">Struct containing returned attribute
        /// information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_info_by_idx",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_idx
            (hid_t loc_id, string obj_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n,
            ref info_t ainfo, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves attribute information, by attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetInfoByName
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="ainfo">Struct containing returned attribute
        /// information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_info_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_name
            (hid_t loc_id, byte[] obj_name, byte[] attr_name,
            ref info_t ainfo, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Retrieves attribute information, by attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetInfoByName
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="attr_name">Attribute name</param>
        /// <param name="ainfo">Struct containing returned attribute
        /// information</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_info_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t get_info_by_name
            (hid_t loc_id, string obj_name, string attr_name,
            ref info_t ainfo, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Gets an attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetName
        /// </summary>
        /// <param name="attr_id">Identifier of the attribute.</param>
        /// <param name="size">The size of the buffer to store the name
        /// in.</param>
        /// <param name="name">Buffer to store name in.</param>
        /// <returns>Returns the length of the attribute's name, which may be
        /// longer than <code>buf_size</code>, if successful. Otherwise returns
        /// a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name(
            hid_t attr_id, size_t size, [In][Out]byte[] name);

        /// <summary>
        /// Gets an attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetName
        /// </summary>
        /// <param name="attr_id">Identifier of the attribute.</param>
        /// <param name="size">The size of the buffer to store the name
        /// in.</param>
        /// <param name="name">Buffer to store name in.</param>
        /// <returns>Returns the length of the attribute's name, which may be
        /// longer than <code>buf_size</code>, if successful. Otherwise returns
        /// a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name(
            hid_t attr_id, size_t size, [In][Out]StringBuilder name);

        /// <summary>
        /// Gets an attribute name, by attribute index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetNameByIdx
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Index traversal order</param>
        /// <param name="n">Attribute’s position in index</param>
        /// <param name="name">Attribute name</param>
        /// <param name="size">Size, in bytes, of attribute name</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns attribute name size, in bytes, if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_name_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name_by_idx
            (hid_t loc_id, byte[] obj_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n,
            [In][Out]byte[] name, size_t size, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Gets an attribute name, by attribute index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetNameByIdx
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Index traversal order</param>
        /// <param name="n">Attribute’s position in index</param>
        /// <param name="name">Attribute name</param>
        /// <param name="size">Size, in bytes, of attribute name</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns attribute name size, in bytes, if successful;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_name_by_idx",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name_by_idx
            (hid_t loc_id, string obj_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n,
            [In][Out]StringBuilder name, size_t size, hid_t lapl_id = H5P.DEFAULT);
        
        
        /// <summary>
        /// Gets an attribute name, by attribute index position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetNameByIdx
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Index traversal order</param>
        /// <param name="n">Attribute’s position in index</param>
        /// <param name="name">Attribute name</param>
        /// <param name="size">Size, in bytes, of attribute name</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns attribute name size, in bytes, if successful;
        /// otherwise returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_name_by_idx",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static ssize_t get_name_by_idx(hid_t loc_id, string obj_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n, IntPtr name,
            size_t size, hid_t lapl_id);

        /// <summary>
        /// Gets a copy of the dataspace for an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetSpace
        /// </summary>
        /// <param name="attr_id">Identifier of an attribute.</param>
        /// <returns>Returns attribute dataspace identifier if successful;
        /// otherwise returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_space",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t get_space(hid_t attr_id);

        /// <summary>
        /// Returns the amount of storage required for an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetStorageSize
        /// </summary>
        /// <param name="attr_id">Identifier of the attribute to query.</param>
        /// <returns>Returns the amount of storage size allocated for the
        /// attribute; otherwise returns 0 (zero).</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_storage_size",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hsize_t get_storage_size(hid_t attr_id);

        /// <summary>
        /// Returns the amount of storage required for an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-GetType
        /// </summary>
        /// <param name="attr_id">Identifier of an attribute.</param>
        /// <returns>Returns a datatype identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aget_type",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t get_type(hid_t attr_id);

        /// <summary>
        /// Calls user-defined function for each attribute on an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Iterate2
        /// </summary>
        /// <param name="obj_id">Identifier for object to which attributes are
        /// attached; may be group, dataset, or named datatype.</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which to iterate over index</param>
        /// <param name="n">Initial and returned offset within index</param>
        /// <param name="op">User-defined function to pass each attribute to</param>
        /// <param name="op_data">User data to pass through to and to be
        /// returned by iterator operator function</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value. Further note that this function returns
        /// the return value of the last operator if it was non-zero, which
        /// can be a negative value, zero if all attributes were processed, or
        /// a positive value indicating short-circuit success
        /// </returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aiterate2",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t iterate
            (hid_t obj_id, H5.index_t idx_type, H5.iter_order_t order,
            ref hsize_t n, operator_t op, IntPtr op_data);

        /// <summary>
        /// Calls user-defined function for each attribute on an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-IterateByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which to iterate over index</param>
        /// <param name="n">Initial and returned offset within index</param>
        /// <param name="op">User-defined function to pass each attribute to</param>
        /// <param name="op_data">User data to pass through to and to be
        /// returned by iterator operator function</param>
        /// <param name="lapd_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value. Further note that this function returns
        /// the return value of the last operator if it was non-zero, which can
        /// be a negative value, zero if all attributes were processed, or a
        /// positive value indicating short-circuit success</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aiterate_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t iterate_by_name(hid_t loc_id,
            byte[] obj_name, H5.index_t idx_type, H5.iter_order_t order,
            ref hsize_t n, operator_t op, IntPtr op_data,
            hid_t lapd_id = H5P.DEFAULT);

        /// <summary>
        /// Calls user-defined function for each attribute on an object.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-IterateByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Order in which to iterate over index</param>
        /// <param name="n">Initial and returned offset within index</param>
        /// <param name="op">User-defined function to pass each attribute to</param>
        /// <param name="op_data">User data to pass through to and to be
        /// returned by iterator operator function</param>
        /// <param name="lapd_id">Link access property list</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value. Further note that this function returns
        /// the return value of the last operator if it was non-zero, which can
        /// be a negative value, zero if all attributes were processed, or a
        /// positive value indicating short-circuit success</returns>
        /// <remarks>ANSI strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aiterate_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t iterate_by_name(hid_t loc_id,
            string obj_name, H5.index_t idx_type, H5.iter_order_t order,
            ref hsize_t n, operator_t op, IntPtr op_data,
            hid_t lapd_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an attribute for an object specified by object identifier
        /// and attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Open
        /// </summary>
        /// <param name="obj_id">Identifer for object to which attribute is
        /// attached</param>
        /// <param name="attr_name">Name of attribute to open</param>
        /// <param name="aapl_id">Attribute access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aopen",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open
            (hid_t obj_id, byte[] attr_name, hid_t aapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an attribute for an object specified by object identifier
        /// and attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Open
        /// </summary>
        /// <param name="obj_id">Identifer for object to which attribute is
        /// attached</param>
        /// <param name="attr_name">Name of attribute to open</param>
        /// <param name="aapl_id">Attribute access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aopen",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open
            (hid_t obj_id, string attr_name, hid_t aapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an attribute for an object specified by attribute index
        /// position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-OpenByIdx
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Index traversal order</param>
        /// <param name="n">Attribute’s position in index</param>
        /// <param name="aapl_id">Attribute access property list</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aopen_by_idx",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open_by_idx
            (hid_t loc_id, byte[] obj_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n,
            hid_t aapl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an attribute for an object specified by attribute index
        /// position.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-OpenByIdx
        /// </summary>
        /// <param name="loc_id">Location of object to which attribute is
        /// attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to location</param>
        /// <param name="idx_type">Type of index</param>
        /// <param name="order">Index traversal order</param>
        /// <param name="n">Attribute’s position in index</param>
        /// <param name="aapl_id">Attribute access property list</param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aopen_by_idx",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open_by_idx
            (hid_t loc_id, string obj_name,
            H5.index_t idx_type, H5.iter_order_t order, hsize_t n,
            hid_t aapl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an attribute for an object by object name and attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-OpenByName
        /// </summary>
        /// <param name="loc_id">Location from which to find object to which
        /// attribute is attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to <paramref name="loc_id"/></param>
        /// <param name="attr_name">Name of attribute to open</param>
        /// <param name="aapl_id">Attribute access property list </param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aopen_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open_by_name
            (hid_t loc_id, byte[] obj_name, byte[] attr_name,
            hid_t aapl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Opens an attribute for an object by object name and attribute name.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-OpenByName
        /// </summary>
        /// <param name="loc_id">Location from which to find object to which
        /// attribute is attached</param>
        /// <param name="obj_name">Name of object to which attribute is
        /// attached, relative to <paramref name="loc_id"/></param>
        /// <param name="attr_name">Name of attribute to open</param>
        /// <param name="aapl_id">Attribute access property list </param>
        /// <param name="lapl_id">Link access property list</param>
        /// <returns>Returns an attribute identifier if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aopen_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static hid_t open_by_name
            (hid_t loc_id, string obj_name, string attr_name,
            hid_t aapl_id = H5P.DEFAULT, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Reads an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Read
        /// </summary>
        /// <param name="attr_id">Identifier of an attribute to read.</param>
        /// <param name="type_id"> Identifier of the attribute datatype
        /// (in memory).</param>
        /// <param name="buf">Buffer for data to be read.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Aread",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t read
            (hid_t attr_id, hid_t type_id, IntPtr buf);

        /// <summary>
        /// Renames an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Rename
        /// </summary>
        /// <param name="loc_id">Location of the attribute.</param>
        /// <param name="old_name">Name of the attribute to be changed.</param>
        /// <param name="new_name">New name for the attribute.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.S</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Arename",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t rename
            (hid_t loc_id, byte[] old_name, byte[] new_name);

        /// <summary>
        /// Renames an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Rename
        /// </summary>
        /// <param name="loc_id">Location of the attribute.</param>
        /// <param name="old_name">Name of the attribute to be changed.</param>
        /// <param name="new_name">New name for the attribute.</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Arename",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t rename
            (hid_t loc_id, string old_name, string new_name);

        /// <summary>
        /// Renames an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-RenameByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location, whose
        /// attribute is to be renamed</param>
        /// <param name="old_attr_name">Prior attribute name</param>
        /// <param name="new_attr_name">New attribute name</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Arename_by_name",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t rename_by_name
            (hid_t loc_id, byte[] obj_name, byte[] old_attr_name, 
            byte[] new_attr_name, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Renames an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-RenameByName
        /// </summary>
        /// <param name="loc_id">Location or object identifier; may be dataset
        /// or group</param>
        /// <param name="obj_name">Name of object, relative to location, whose
        /// attribute is to be renamed</param>
        /// <param name="old_attr_name">Prior attribute name</param>
        /// <param name="new_attr_name">New attribute name</param>
        /// <param name="lapl_id">Link access property list identifier</param>
        /// <returns>Returns a non-negative value if successful; otherwise
        /// returns a negative value.</returns>
        /// <remarks>ASCII strings ONLY!</remarks>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Arename_by_name",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t rename_by_name
            (hid_t loc_id, string obj_name, string old_attr_name,
            string new_attr_name, hid_t lapl_id = H5P.DEFAULT);

        /// <summary>
        /// Writes data to an attribute.
        /// See https://www.hdfgroup.org/HDF5/doc/RM/RM_H5A.html#Annot-Write
        /// </summary>
        /// <param name="attr_id">Identifier of an attribute to write.</param>
        /// <param name="mem_type_id">Identifier of the attribute datatype
        /// (in memory).</param>
        /// <param name="buf">Data to be written.</param>
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Awrite",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public extern static herr_t write
            (hid_t attr_id, hid_t mem_type_id, IntPtr buf);
    }
}
