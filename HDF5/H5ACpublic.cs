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

using hbool_t = System.UInt32;
using size_t = System.IntPtr;

namespace HDF.PInvoke
{
    public unsafe sealed class H5AC
    {
        public const int H5AC__CURR_CACHE_CONFIG_VERSION = 1;

        public const int H5AC__MAX_TRACE_FILE_NAME_LEN = 1024;

        public enum metadata_write_strategy_t : int
        {
            H5AC_METADATA_WRITE_STRATEGY__PROCESS_0_ONLY = 0,
            H5AC_METADATA_WRITE_STRATEGY__DISTRIBUTED = 1
        }


        /// <summary>
        /// Cache configuration struct used by H5F.[get,set]_mdc_config()
        /// </summary>
        public struct cache_config_t
        {
            /* general configuration fields: */
            int version;

            hbool_t rpt_fcn_enabled;

            hbool_t open_trace_file;

            hbool_t close_trace_file;

            fixed char trace_file_name[H5AC__MAX_TRACE_FILE_NAME_LEN + 1];

            hbool_t evictions_enabled;
            
            hbool_t set_initial_size;
            
            size_t initial_size;
            
            double min_clean_fraction;
            
            size_t max_size;
            
            size_t min_size;
            
            long epoch_length;

            /* size increase control fields: */
            H5C.cache_incr_mode incr_mode;

            double lower_hr_threshold;

            double increment;

            hbool_t apply_max_increment;

            size_t max_increment;

            H5C.cache_flash_incr_mode flash_incr_mode;

            double flash_multiple;

            double flash_threshold;

            /* size decrease control fields: */
            H5C.cache_decr_mode decr_mode;

            double upper_hr_threshold;

            double decrement;

            hbool_t apply_max_decrement;

            size_t max_decrement;

            int epochs_before_eviction;

            hbool_t apply_empty_reserve;

            double empty_reserve;


            /* parallel configuration fields: */
            int dirty_bytes_threshold;

            int metadata_write_strategy;
        }
    }
}
