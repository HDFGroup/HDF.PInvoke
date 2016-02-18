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
        static H5DLLImporter m_importer;

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
                    if (m_importer.GetValue<hid_t>(Constants.DLLFileName,
                        "H5T_IEEE_F32BE_g", ref val,
#if HDF5_VER1_10
                        Marshal.ReadInt64
#else
                        Marshal.ReadInt32
#endif
                        ))
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
    }
}
