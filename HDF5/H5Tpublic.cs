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

using hbool_t = System.Int32;
using herr_t = System.Int32;
using hid_t = System.Int32;
using hsize_t = System.UInt64;
using htri_t = System.Int32;
using size_t = System.IntPtr;

namespace HDF.PInvoke
{
    public unsafe sealed class H5T
    {
        static H5DLLImporter m_importer;

        static H5T()
        {
            m_importer = H5DLLImporter.Create();
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
        /// Character set to use for text strings.
        /// </summary>
        public enum cset_t
        {
            /// <summary>
            /// error [value = -1].
            /// </summary>
            CSET_ERROR = -1,
            /// <summary>
            /// US ASCII [value = 0].
            /// </summary>
            CSET_ASCII = 0,
            /// <summary>
            /// UTF-8 Unicode encoding [value = 1].
            /// </summary>
            CSET_UTF8 = 1,
            // reserved for later use [values = 2-15]
            CSET_RESERVED_2 = 2,
            CSET_RESERVED_3 = 3,
            CSET_RESERVED_4 = 4,
            CSET_RESERVED_5 = 5,
            CSET_RESERVED_6 = 6,
            CSET_RESERVED_7 = 7,
            CSET_RESERVED_8 = 8,
            CSET_RESERVED_9 = 9,
            CSET_RESERVED_10 = 10,
            CSET_RESERVED_11 = 11,
            CSET_RESERVED_12 = 12,
            CSET_RESERVED_13 = 13,
            CSET_RESERVED_14 = 14,
            CSET_RESERVED_15 = 15
        }

        /// <summary>
        /// Number of character sets actually defined 
        /// </summary>
        public const cset_t NCSET = cset_t.CSET_RESERVED_2;

        /// <summary>
        /// The exception type passed into the conversion callback function
        /// </summary>
        public enum conv_except_t
        {
            /// <summary>
            /// source value is greater than destination's range
            /// </summary>
            CONV_EXCEPT_RANGE_HI = 0,
            /// <summary>
            /// source value is less than destination's range
            /// </summary>
            CONV_EXCEPT_RANGE_LOW = 1,
            /// <summary>
            /// source value loses precision in destination
            /// </summary>
            CONV_EXCEPT_PRECISION = 2,
            /// <summary>
            /// source value is truncated in destination
            /// </summary>
            CONV_EXCEPT_TRUNCATE = 3,
            /// <summary>
            /// source value is positive infinity(floating number)
            /// </summary>
            CONV_EXCEPT_PINF = 4,
            /// <summary>
            /// source value is negative infinity(floating number)
            /// </summary>
            CONV_EXCEPT_NINF = 5,
            /// <summary>
            /// source value is NaN(floating number)
            /// </summary>
            CONV_EXCEPT_NAN = 6
        }

        /// <summary>
        /// Commands sent to conversion functions
        /// </summary>
        public enum cmd_t
        {
            /// <summary>
            /// query and/or initialize private data
            /// </summary>
            H5T_CONV_INIT = 0,
            /// <summary>
            /// convert data from source to dest datatype
            /// </summary>
            H5T_CONV_CONV = 1,
            /// <summary>
            /// function is being removed from path
            /// </summary>
            H5T_CONV_FREE = 2
        }

        /// <summary>
        /// How is the `bkg' buffer used by the conversion function?
        /// </summary>
        public enum bkg_t
        {
            /// <summary>
            /// background buffer is not needed, send NULL
            /// </summary>
            H5T_BKG_NO = 0,
            /// <summary>
            /// bkg buffer used as temp storage only
            /// </summary>
            H5T_BKG_TEMP = 1,
            /// <summary>
            /// init bkg buf with data before conversion
            /// </summary>
            H5T_BKG_YES = 2
        }

        /// <summary>
        /// Type conversion client data
        /// </summary>
        public struct cdata_t
        {
            /// <summary>
            /// what should the conversion function do?
            /// </summary>
            cmd_t command;
            /// <summary>
            /// is the background buffer needed?
            /// </summary>
            bkg_t need_bkg;
            /// <summary>
            /// recalculate private data
            /// </summary>
            hbool_t recalc;
            /// <summary>
            /// private data
            /// </summary>
            IntPtr priv;
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
            CONV_ABORT = -1,
            /// <summary>
            /// callback function failed to handle the exception [value = 0]
            /// </summary>
            CONV_UNHANDLED = 0,
            /// <summary>
            /// callback function handled the exception successfully [value = 1]
            /// </summary>
            CONV_HANDLED = 1
        }
      
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


        #region native imported types caches
        /*
         * The IEEE floating point types in various byte orders.
         */
        static hid_t? H5T_IEEE_F32BE_g;
        static hid_t? H5T_IEEE_F32LE_g;
        static hid_t? H5T_IEEE_F64BE_g;
        static hid_t? H5T_IEEE_F64LE_g;

        /*
         * These are "standard" types.  For instance, signed (2's complement) and
         * uint integers of various sizes and byte orders.
         */
        static hid_t? H5T_STD_I8BE_g;
        static hid_t? H5T_STD_I8LE_g;
        static hid_t? H5T_STD_I16BE_g;
        static hid_t? H5T_STD_I16LE_g;
        static hid_t? H5T_STD_I32BE_g;
        static hid_t? H5T_STD_I32LE_g;
        static hid_t? H5T_STD_I64BE_g;
        static hid_t? H5T_STD_I64LE_g;
        static hid_t? H5T_STD_U8BE_g;
        static hid_t? H5T_STD_U8LE_g;
        static hid_t? H5T_STD_U16BE_g;
        static hid_t? H5T_STD_U16LE_g;
        static hid_t? H5T_STD_U32BE_g;
        static hid_t? H5T_STD_U32LE_g;
        static hid_t? H5T_STD_U64BE_g;
        static hid_t? H5T_STD_U64LE_g;
        static hid_t? H5T_STD_B8BE_g;
        static hid_t? H5T_STD_B8LE_g;
        static hid_t? H5T_STD_B16BE_g;
        static hid_t? H5T_STD_B16LE_g;
        static hid_t? H5T_STD_B32BE_g;
        static hid_t? H5T_STD_B32LE_g;
        static hid_t? H5T_STD_B64BE_g;
        static hid_t? H5T_STD_B64LE_g;
        static hid_t? H5T_STD_REF_OBJ_g;
        static hid_t? H5T_STD_REF_DSETREG_g;

        /*
         * Types which are particular to Unix.
         */
        static hid_t? H5T_UNIX_D32BE_g;
        static hid_t? H5T_UNIX_D32LE_g;
        static hid_t? H5T_UNIX_D64BE_g;
        static hid_t? H5T_UNIX_D64LE_g;

        /*
         * Types particular to the C language.  String types use `bytes' instead
         * of `bits' as their size.
         */
        static hid_t? H5T_C_S1_g;

        /*
         * Types particular to Fortran.
         */
        static hid_t? H5T_FORTRAN_S1_g;

        /*
         * The VAX floating point types (i.e. in VAX byte order)
         */
        static hid_t? H5T_VAX_F32_g;
        static hid_t? H5T_VAX_F64_g;

        /*
         * The predefined native types. These are the types detected by H5detect and
         * they violate the naming scheme a little.  Instead of a class name,
         * precision and byte order as the last component, they have a C-like type
         * name.  If the type begins with `U' then it is the uint version of the
         * integer type; other integer types are signed.  The type LLONG corresponds
         * to C's `long long' and LDOUBLE is `long double' (these types might be the
         * same as `LONG' and `DOUBLE' respectively).
         */
        static hid_t? H5T_NATIVE_SCHAR_g;
        static hid_t? H5T_NATIVE_UCHAR_g;
        static hid_t? H5T_NATIVE_SHORT_g;
        static hid_t? H5T_NATIVE_USHORT_g;
        static hid_t? H5T_NATIVE_INT_g;
        static hid_t? H5T_NATIVE_UINT_g;
        static hid_t? H5T_NATIVE_LONG_g;
        static hid_t? H5T_NATIVE_ULONG_g;
        static hid_t? H5T_NATIVE_LLONG_g;
        static hid_t? H5T_NATIVE_ULLONG_g;
        static hid_t? H5T_NATIVE_FLOAT_g;
        static hid_t? H5T_NATIVE_DOUBLE_g;
        static hid_t? H5T_NATIVE_LDOUBLE_g;
        static hid_t? H5T_NATIVE_B8_g;
        static hid_t? H5T_NATIVE_B16_g;
        static hid_t? H5T_NATIVE_B32_g;
        static hid_t? H5T_NATIVE_B64_g;
        static hid_t? H5T_NATIVE_OPAQUE_g;
        static hid_t? H5T_NATIVE_HADDR_g;
        static hid_t? H5T_NATIVE_HSIZE_g;
        static hid_t? H5T_NATIVE_HSSIZE_g;
        static hid_t? H5T_NATIVE_HERR_g;
        static hid_t? H5T_NATIVE_HBOOL_g;

        /* C9x integer types */
        static hid_t? H5T_NATIVE_INT8_g;
        static hid_t? H5T_NATIVE_UINT8_g;
        static hid_t? H5T_NATIVE_INT_LEAST8_g;
        static hid_t? H5T_NATIVE_UINT_LEAST8_g;
        static hid_t? H5T_NATIVE_INT_FAST8_g;
        static hid_t? H5T_NATIVE_UINT_FAST8_g;

        static hid_t? H5T_NATIVE_INT16_g;
        static hid_t? H5T_NATIVE_UINT16_g;
        static hid_t? H5T_NATIVE_INT_LEAST16_g;
        static hid_t? H5T_NATIVE_UINT_LEAST16_g;
        static hid_t? H5T_NATIVE_INT_FAST16_g;
        static hid_t? H5T_NATIVE_UINT_FAST16_g;

        static hid_t? H5T_NATIVE_INT32_g;
        static hid_t? H5T_NATIVE_UINT32_g;
        static hid_t? H5T_NATIVE_INT_LEAST32_g;
        static hid_t? H5T_NATIVE_UINT_LEAST32_g;
        static hid_t? H5T_NATIVE_INT_FAST32_g;
        static hid_t? H5T_NATIVE_UINT_FAST32_g;

        static hid_t? H5T_NATIVE_INT64_g;
        static hid_t? H5T_NATIVE_UINT64_g;
        static hid_t? H5T_NATIVE_INT_LEAST64_g;
        static hid_t? H5T_NATIVE_UINT_LEAST64_g;
        static hid_t? H5T_NATIVE_INT_FAST64_g;
        static hid_t? H5T_NATIVE_UINT_FAST64_g;
        #endregion

        #region native imported types
        /*
         * The IEEE floating point types in various byte orders.
         */
        public static hid_t IEEE_F32BE
        {
            get
            {
                if (!H5T_IEEE_F32BE_g.HasValue)
                {
                    hid_t val = -1;
                    if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_IEEE_F32BE_g", ref val, Marshal.ReadInt32))
                    {
                        H5T_IEEE_F32BE_g = val;
                    }
                }
                return H5T_IEEE_F32BE_g.GetValueOrDefault();
            }
        }
        public static hid_t IEEE_F32LE { get { if (!H5T_IEEE_F32LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_IEEE_F32LE_g", ref val, Marshal.ReadInt32)) { H5T_IEEE_F32LE_g = val; } } return H5T_IEEE_F32LE_g.GetValueOrDefault(); } }
        public static hid_t IEEE_F64BE { get { if (!H5T_IEEE_F64BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_IEEE_F64BE_g", ref val, Marshal.ReadInt32)) { H5T_IEEE_F64BE_g = val; } } return H5T_IEEE_F64BE_g.GetValueOrDefault(); } }
        public static hid_t IEEE_F64LE { get { if (!H5T_IEEE_F64LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_IEEE_F64LE_g", ref val, Marshal.ReadInt32)) { H5T_IEEE_F64LE_g = val; } } return H5T_IEEE_F64LE_g.GetValueOrDefault(); } }
        public static hid_t STD_I8BE { get { if (!H5T_STD_I8BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I8BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I8BE_g = val; } } return H5T_STD_I8BE_g.GetValueOrDefault(); } }
        public static hid_t STD_I8LE { get { if (!H5T_STD_I8LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I8LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I8LE_g = val; } } return H5T_STD_I8LE_g.GetValueOrDefault(); } }
        public static hid_t STD_I16BE { get { if (!H5T_STD_I16BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I16BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I16BE_g = val; } } return H5T_STD_I16BE_g.GetValueOrDefault(); } }
        public static hid_t STD_I16LE { get { if (!H5T_STD_I16LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I16LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I16LE_g = val; } } return H5T_STD_I16LE_g.GetValueOrDefault(); } }
        public static hid_t STD_I32BE { get { if (!H5T_STD_I32BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I32BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I32BE_g = val; } } return H5T_STD_I32BE_g.GetValueOrDefault(); } }
        public static hid_t STD_I32LE { get { if (!H5T_STD_I32LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I32LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I32LE_g = val; } } return H5T_STD_I32LE_g.GetValueOrDefault(); } }
        public static hid_t STD_I64BE { get { if (!H5T_STD_I64BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I64BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I64BE_g = val; } } return H5T_STD_I64BE_g.GetValueOrDefault(); } }
        public static hid_t STD_I64LE { get { if (!H5T_STD_I64LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_I64LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_I64LE_g = val; } } return H5T_STD_I64LE_g.GetValueOrDefault(); } }
        public static hid_t STD_U8BE { get { if (!H5T_STD_U8BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U8BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U8BE_g = val; } } return H5T_STD_U8BE_g.GetValueOrDefault(); } }
        public static hid_t STD_U8LE { get { if (!H5T_STD_U8LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U8LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U8LE_g = val; } } return H5T_STD_U8LE_g.GetValueOrDefault(); } }
        public static hid_t STD_U16BE { get { if (!H5T_STD_U16BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U16BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U16BE_g = val; } } return H5T_STD_U16BE_g.GetValueOrDefault(); } }
        public static hid_t STD_U16LE { get { if (!H5T_STD_U16LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U16LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U16LE_g = val; } } return H5T_STD_U16LE_g.GetValueOrDefault(); } }
        public static hid_t STD_U32BE { get { if (!H5T_STD_U32BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U32BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U32BE_g = val; } } return H5T_STD_U32BE_g.GetValueOrDefault(); } }
        public static hid_t STD_U32LE { get { if (!H5T_STD_U32LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U32LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U32LE_g = val; } } return H5T_STD_U32LE_g.GetValueOrDefault(); } }
        public static hid_t STD_U64BE { get { if (!H5T_STD_U64BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U64BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U64BE_g = val; } } return H5T_STD_U64BE_g.GetValueOrDefault(); } }
        public static hid_t STD_U64LE { get { if (!H5T_STD_U64LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_U64LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_U64LE_g = val; } } return H5T_STD_U64LE_g.GetValueOrDefault(); } }
        public static hid_t STD_B8BE { get { if (!H5T_STD_B8BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B8BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B8BE_g = val; } } return H5T_STD_B8BE_g.GetValueOrDefault(); } }
        public static hid_t STD_B8LE { get { if (!H5T_STD_B8LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B8LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B8LE_g = val; } } return H5T_STD_B8LE_g.GetValueOrDefault(); } }
        public static hid_t STD_B16BE { get { if (!H5T_STD_B16BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B16BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B16BE_g = val; } } return H5T_STD_B16BE_g.GetValueOrDefault(); } }
        public static hid_t STD_B16LE { get { if (!H5T_STD_B16LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B16LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B16LE_g = val; } } return H5T_STD_B16LE_g.GetValueOrDefault(); } }
        public static hid_t STD_B32BE { get { if (!H5T_STD_B32BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B32BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B32BE_g = val; } } return H5T_STD_B32BE_g.GetValueOrDefault(); } }
        public static hid_t STD_B32LE { get { if (!H5T_STD_B32LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B32LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B32LE_g = val; } } return H5T_STD_B32LE_g.GetValueOrDefault(); } }
        public static hid_t STD_B64BE { get { if (!H5T_STD_B64BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B64BE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B64BE_g = val; } } return H5T_STD_B64BE_g.GetValueOrDefault(); } }
        public static hid_t STD_B64LE { get { if (!H5T_STD_B64LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_B64LE_g", ref val, Marshal.ReadInt32)) { H5T_STD_B64LE_g = val; } } return H5T_STD_B64LE_g.GetValueOrDefault(); } }
        public static hid_t STD_REF_OBJ { get { if (!H5T_STD_REF_OBJ_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_REF_OBJ_g", ref val, Marshal.ReadInt32)) { H5T_STD_REF_OBJ_g = val; } } return H5T_STD_REF_OBJ_g.GetValueOrDefault(); } }
        public static hid_t STD_REF_DSETREG { get { if (!H5T_STD_REF_DSETREG_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_STD_REF_DSETREG_g", ref val, Marshal.ReadInt32)) { H5T_STD_REF_DSETREG_g = val; } } return H5T_STD_REF_DSETREG_g.GetValueOrDefault(); } }
        public static hid_t UNIX_D32BE { get { if (!H5T_UNIX_D32BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_UNIX_D32BE_g", ref val, Marshal.ReadInt32)) { H5T_UNIX_D32BE_g = val; } } return H5T_UNIX_D32BE_g.GetValueOrDefault(); } }
        public static hid_t UNIX_D32LE { get { if (!H5T_UNIX_D32LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_UNIX_D32LE_g", ref val, Marshal.ReadInt32)) { H5T_UNIX_D32LE_g = val; } } return H5T_UNIX_D32LE_g.GetValueOrDefault(); } }
        public static hid_t UNIX_D64BE { get { if (!H5T_UNIX_D64BE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_UNIX_D64BE_g", ref val, Marshal.ReadInt32)) { H5T_UNIX_D64BE_g = val; } } return H5T_UNIX_D64BE_g.GetValueOrDefault(); } }
        public static hid_t UNIX_D64LE { get { if (!H5T_UNIX_D64LE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_UNIX_D64LE_g", ref val, Marshal.ReadInt32)) { H5T_UNIX_D64LE_g = val; } } return H5T_UNIX_D64LE_g.GetValueOrDefault(); } }
        public static hid_t C_S1 { get { if (!H5T_C_S1_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_C_S1_g", ref val, Marshal.ReadInt32)) { H5T_C_S1_g = val; } } return H5T_C_S1_g.GetValueOrDefault(); } }
        public static hid_t FORTRAN_S1 { get { if (!H5T_FORTRAN_S1_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_FORTRAN_S1_g", ref val, Marshal.ReadInt32)) { H5T_FORTRAN_S1_g = val; } } return H5T_FORTRAN_S1_g.GetValueOrDefault(); } }
        public static hid_t VAX_F32 { get { if (!H5T_VAX_F32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_VAX_F32_g", ref val, Marshal.ReadInt32)) { H5T_VAX_F32_g = val; } } return H5T_VAX_F32_g.GetValueOrDefault(); } }
        public static hid_t VAX_F64 { get { if (!H5T_VAX_F64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_VAX_F64_g", ref val, Marshal.ReadInt32)) { H5T_VAX_F64_g = val; } } return H5T_VAX_F64_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_SCHAR { get { if (!H5T_NATIVE_SCHAR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_SCHAR_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_SCHAR_g = val; } } return H5T_NATIVE_SCHAR_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UCHAR { get { if (!H5T_NATIVE_UCHAR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UCHAR_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UCHAR_g = val; } } return H5T_NATIVE_UCHAR_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_SHORT { get { if (!H5T_NATIVE_SHORT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_SHORT_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_SHORT_g = val; } } return H5T_NATIVE_SHORT_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_USHORT { get { if (!H5T_NATIVE_USHORT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_USHORT_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_USHORT_g = val; } } return H5T_NATIVE_USHORT_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT { get { if (!H5T_NATIVE_INT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_g = val; } } return H5T_NATIVE_INT_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT { get { if (!H5T_NATIVE_UINT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_g = val; } } return H5T_NATIVE_UINT_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_LONG { get { if (!H5T_NATIVE_LONG_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_LONG_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_LONG_g = val; } } return H5T_NATIVE_LONG_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_ULONG { get { if (!H5T_NATIVE_ULONG_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_ULONG_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_ULONG_g = val; } } return H5T_NATIVE_ULONG_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_LLONG { get { if (!H5T_NATIVE_LLONG_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_LLONG_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_LLONG_g = val; } } return H5T_NATIVE_LLONG_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_ULLONG { get { if (!H5T_NATIVE_ULLONG_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_ULLONG_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_ULLONG_g = val; } } return H5T_NATIVE_ULLONG_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_FLOAT { get { if (!H5T_NATIVE_FLOAT_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_FLOAT_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_FLOAT_g = val; } } return H5T_NATIVE_FLOAT_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_DOUBLE { get { if (!H5T_NATIVE_DOUBLE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_DOUBLE_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_DOUBLE_g = val; } } return H5T_NATIVE_DOUBLE_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_LDOUBLE { get { if (!H5T_NATIVE_LDOUBLE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_LDOUBLE_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_LDOUBLE_g = val; } } return H5T_NATIVE_LDOUBLE_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_B8 { get { if (!H5T_NATIVE_B8_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_B8_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_B8_g = val; } } return H5T_NATIVE_B8_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_B16 { get { if (!H5T_NATIVE_B16_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_B16_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_B16_g = val; } } return H5T_NATIVE_B16_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_B32 { get { if (!H5T_NATIVE_B32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_B32_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_B32_g = val; } } return H5T_NATIVE_B32_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_B64 { get { if (!H5T_NATIVE_B64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_B64_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_B64_g = val; } } return H5T_NATIVE_B64_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_OPAQUE { get { if (!H5T_NATIVE_OPAQUE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_OPAQUE_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_OPAQUE_g = val; } } return H5T_NATIVE_OPAQUE_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_HADDR { get { if (!H5T_NATIVE_HADDR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_HADDR_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_HADDR_g = val; } } return H5T_NATIVE_HADDR_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_HSIZE { get { if (!H5T_NATIVE_HSIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_HSIZE_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_HSIZE_g = val; } } return H5T_NATIVE_HSIZE_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_HSSIZE { get { if (!H5T_NATIVE_HSSIZE_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_HSSIZE_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_HSSIZE_g = val; } } return H5T_NATIVE_HSSIZE_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_HERR { get { if (!H5T_NATIVE_HERR_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_HERR_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_HERR_g = val; } } return H5T_NATIVE_HERR_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_HBOOL { get { if (!H5T_NATIVE_HBOOL_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_HBOOL_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_HBOOL_g = val; } } return H5T_NATIVE_HBOOL_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT8 { get { if (!H5T_NATIVE_INT8_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT8_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT8_g = val; } } return H5T_NATIVE_INT8_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT8 { get { if (!H5T_NATIVE_UINT8_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT8_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT8_g = val; } } return H5T_NATIVE_UINT8_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_LEAST8 { get { if (!H5T_NATIVE_INT_LEAST8_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_LEAST8_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_LEAST8_g = val; } } return H5T_NATIVE_INT_LEAST8_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_LEAST8 { get { if (!H5T_NATIVE_UINT_LEAST8_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_LEAST8_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_LEAST8_g = val; } } return H5T_NATIVE_UINT_LEAST8_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_FAST8 { get { if (!H5T_NATIVE_INT_FAST8_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_FAST8_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_FAST8_g = val; } } return H5T_NATIVE_INT_FAST8_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_FAST8 { get { if (!H5T_NATIVE_UINT_FAST8_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_FAST8_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_FAST8_g = val; } } return H5T_NATIVE_UINT_FAST8_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT16 { get { if (!H5T_NATIVE_INT16_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT16_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT16_g = val; } } return H5T_NATIVE_INT16_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT16 { get { if (!H5T_NATIVE_UINT16_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT16_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT16_g = val; } } return H5T_NATIVE_UINT16_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_LEAST16 { get { if (!H5T_NATIVE_INT_LEAST16_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_LEAST16_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_LEAST16_g = val; } } return H5T_NATIVE_INT_LEAST16_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_LEAST16 { get { if (!H5T_NATIVE_UINT_LEAST16_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_LEAST16_g_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_LEAST16_g = val; } } return H5T_NATIVE_UINT_LEAST16_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_FAST16 { get { if (!H5T_NATIVE_INT_FAST16_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_FAST16_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_FAST16_g = val; } } return H5T_NATIVE_INT_FAST16_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_FAST16 { get { if (!H5T_NATIVE_UINT_FAST16_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_FAST16_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_FAST16_g = val; } } return H5T_NATIVE_UINT_FAST16_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT32 { get { if (!H5T_NATIVE_INT32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT32_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT32_g = val; } } return H5T_NATIVE_INT32_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT32 { get { if (!H5T_NATIVE_UINT32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT32_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT32_g = val; } } return H5T_NATIVE_UINT32_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_LEAST32 { get { if (!H5T_NATIVE_INT_LEAST32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_LEAST32_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_LEAST32_g = val; } } return H5T_NATIVE_INT_LEAST32_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_LEAST32 { get { if (!H5T_NATIVE_UINT_LEAST32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_LEAST32_g_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_LEAST32_g = val; } } return H5T_NATIVE_UINT_LEAST32_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_FAST32 { get { if (!H5T_NATIVE_INT_FAST32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_FAST32_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_FAST32_g = val; } } return H5T_NATIVE_INT_FAST32_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_FAST32 { get { if (!H5T_NATIVE_UINT_FAST32_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_FAST32_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_FAST32_g = val; } } return H5T_NATIVE_UINT_FAST32_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT64 { get { if (!H5T_NATIVE_INT64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT64_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT64_g = val; } } return H5T_NATIVE_INT64_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT64 { get { if (!H5T_NATIVE_UINT64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT64_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT64_g = val; } } return H5T_NATIVE_UINT64_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_LEAST64 { get { if (!H5T_NATIVE_INT_LEAST64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_LEAST64_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_LEAST64_g = val; } } return H5T_NATIVE_INT_LEAST64_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_LEAST64 { get { if (!H5T_NATIVE_UINT_LEAST64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_LEAST64_g_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_LEAST64_g = val; } } return H5T_NATIVE_UINT_LEAST64_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_INT_FAST64 { get { if (!H5T_NATIVE_INT_FAST64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_INT_FAST64_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_INT_FAST64_g = val; } } return H5T_NATIVE_INT_FAST64_g.GetValueOrDefault(); } }
        public static hid_t NATIVE_UINT_FAST64 { get { if (!H5T_NATIVE_UINT_FAST64_g.HasValue) { hid_t val = -1; if (m_importer.GetValue<hid_t>(Constants.DLLFileName, "H5T_NATIVE_UINT_FAST64_g", ref val, Marshal.ReadInt32)) { H5T_NATIVE_UINT_FAST64_g = val; } } return H5T_NATIVE_UINT_FAST64_g.GetValueOrDefault(); } }

        #endregion

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
            (hid_t base_type_id, uint rank, hsize_t[] dims);

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
            (hid_t obj_id, byte* buf, ref size_t nalloc);

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
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tenum_insert",
            CallingConvention = CallingConvention.Cdecl),
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
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tenum_nameof",
            CallingConvention = CallingConvention.Cdecl),
        SuppressUnmanagedCodeSecurity, SecuritySafeCritical]
        public static extern herr_t enum_nameof
            (hid_t dtype_id, IntPtr value, StringBuilder name, size_t size);

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
        [DllImport(Constants.DLLFileName, EntryPoint = "H5Tenum_valueof",
            CallingConvention = CallingConvention.Cdecl),
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
            (hid_t src_id, hid_t dst_id, ref cdata_t* pcdata);

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
            (hid_t adtype_id, hsize_t* dims);

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
    }
}
