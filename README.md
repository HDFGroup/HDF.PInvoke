# Getting started with HDF.PInvoke

Currently, the bindings are tested and developed with HDF5 1.8.16. Once we've covered the 1.8 API, we'll incorporate the new HDF5 1.10 features (March?).

HDF5 binaries can be obtained from [here](https://www.hdfgroup.org/HDF5/release/obtain5.html). The ``HDF.PInvoke.dll`` assembly, located in ``bin\[x86,x64]\[Debug,Release]`` depends only on ``hdf5.dll``, ``szip.dll``, and ``zlib.dll``. Make sure they're in your ``PATH`` or sit in the same directory as the assembly.

![Visual Studio Solution](/images/HDF.PInvoke.jpg)
