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

using hbool_t = System.UInt32;
using size_t = System.IntPtr;

namespace HDF.PInvoke
{
    public unsafe sealed class H5AC
    {
        static H5AC() { H5.open(); }

        public const int CURR_CACHE_CONFIG_VERSION = 1;

        public const int MAX_TRACE_FILE_NAME_LEN = 1024;

        public enum metadata_write_strategy_t : int
        {
            PROCESS_0_ONLY = 0,
            DISTRIBUTED = 1
        }


        /// <summary>
        /// Cache configuration struct used by H5F.[get,set]_mdc_config()
        /// </summary>
        public struct cache_config_t
        {
            /* general configuration fields: */
            public int version;

            public hbool_t rpt_fcn_enabled;

            public hbool_t open_trace_file;
            public hbool_t close_trace_file;
            public fixed char trace_file_name[MAX_TRACE_FILE_NAME_LEN + 1];

            public hbool_t evictions_enabled;

            public hbool_t set_initial_size;
            public size_t initial_size;

            public double min_clean_fraction;

            public size_t max_size;
            public size_t min_size;

            public long epoch_length;

            /* size increase control fields: */
            public H5C.cache_incr_mode incr_mode;

            public double lower_hr_threshold;

            public double increment;

            public hbool_t apply_max_increment;
            public size_t max_increment;

            public H5C.cache_flash_incr_mode flash_incr_mode;
            public double flash_multiple;
            public double flash_threshold;

            /* size decrease control fields: */
            public H5C.cache_decr_mode decr_mode;

            public double upper_hr_threshold;

            public double decrement;

            public hbool_t apply_max_decrement;
            public size_t max_decrement;

            public int epochs_before_eviction;

            public hbool_t apply_empty_reserve;
            public double empty_reserve;


            /* parallel configuration fields: */
            public int dirty_bytes_threshold;
            public int metadata_write_strategy;

            public cache_config_t(int cache_config_version)
            {
                version = cache_config_version;

                rpt_fcn_enabled = 0;

                open_trace_file = 0;
                close_trace_file = 0;

                evictions_enabled = 0;

                set_initial_size = 0;
                initial_size = IntPtr.Zero;

                min_clean_fraction = 0.0;

                max_size = IntPtr.Zero;
                min_size = IntPtr.Zero;

                epoch_length = 0;

                incr_mode = H5C.cache_incr_mode.OFF;

                lower_hr_threshold = 0.0;

                increment = 0.0;

                apply_max_increment = 0;
                max_increment = IntPtr.Zero;

                flash_incr_mode = H5C.cache_flash_incr_mode.OFF;
                flash_multiple = 0.0;
                flash_threshold = 0.0;

                decr_mode = H5C.cache_decr_mode.OFF;

                upper_hr_threshold = 0.0;

                decrement = 0.0;

                apply_max_decrement = 0;
                max_decrement = IntPtr.Zero;

                epochs_before_eviction = 0;

                apply_empty_reserve = 0;
                empty_reserve = 0.0;

                dirty_bytes_threshold = 0;
                metadata_write_strategy = 0;
            }
        }

        public const int CURR_CACHE_IMAGE_CONFIG_VERSION = 1;

        public const int CACHE_IMAGE__ENTRY_AGEOUT__NONE = -1;

        public const int CACHE_IMAGE__ENTRY_AGEOUT__MAX = 100;

        /// <summary>
        /// Cache image configuration struct used by
        /// H5F.[get,set]_mdc_image_config()
        /// </summary>
        public struct cache_image_config_t
        {
            public int version;

            public hbool_t generate_image;

            public hbool_t save_resize_status;

            public int entry_ageout;
        }
    }
}