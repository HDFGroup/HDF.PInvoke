##### 1.10.6.1
* Fixed the documentation link.

##### 1.10.6.0
* Updated native dependencies (HDF5 1.10.6) 

##### 1.10.5.2
* Bugfix for https://github.com/HDFGroup/HDF.PInvoke/issues/161.

##### 1.10.5.1
* Bugfix for https://github.com/HDFGroup/HDF.PInvoke/issues/154.

##### 1.8.21.1
* Bugfix for https://github.com/HDFGroup/HDF.PInvoke/issues/154.

##### 1.8.21.0
* Updated native dependencies (HDF5 1.8.21) 

##### 1.10.5.0
* Updated native dependencies (HDF5 1.10.5) 
* New chunk info functions H5Dget_chunk[info,info_by_coord], H5Dget_num_chunks
* New object header minimization functions H5[F,P][g,s]et_dset_no_attrs_hint

##### 1.10.4.0
* Updated native dependencies (HDF5 1.10.4) 

##### 1.10.3.0
* Updated native dependencies (HDF5 1.10.3) 
* API versioning for H5O[get_info*,visit*]
* Moved H5DO[read,write]_chunk to H5D[read,write]_chunk
* General performance improvements (native library)

##### 1.10.2.0
* Updated native dependencies (HDF5 1.10.2) 
* VDS H5P.[g,s]et_virtual_prefix
* Fixed H5F.get_file_image() declaration (thanks polivbr)

##### 1.8.20.0
* Updated native dependencies (HDF5 1.8.20) 

##### 1.10.1.0
* Updated native dependencies (HDF5 1.10.1) 
* New functions for metadata cache image handling
* MDC H5P.set_evict_on_close
* New functions for file space management (page buffering, paged aggr.)

##### 1.8.19.0
* Updated native dependencies (HDF5 1.8.19) 
* New H5PL functions
* H5DOread_chunk
* H5Dget_chunk_storage_size

##### 1.8.18.0
* Updated native dependencies (HDF5 1.8.18) 

##### 1.10.0.4
* Added missing predefined datatypes (INTEL, MIPS, VAX)
* Added the CMake configuration files for building the native dependnecies
* The native dependencies included were built as RelWithDebugInfo (PDBs are included)

##### 1.8.17.8
* Added missing predefined datatypes (INTEL, MIPS, VAX)
* Added the CMake configuration files for building the native dependnecies
* The native dependencies included were built as RelWithDebugInfo (PDBs are included)
