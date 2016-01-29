# Getting started with HDF.PInvoke

Currently, the bindings are tested and developed with HDF5 1.8.16. Once we've covered the 1.8 API, we'll incorporate the new HDF5 1.10 features (March?).

### Dependencies

HDF5 binaries can be obtained from [here](https://www.hdfgroup.org/HDF5/release/obtain5.html). The ``HDF.PInvoke.dll`` assemblies, located in ``bin\[x86,x64]\[Debug,Release]``, depend on ``hdf5.dll``, ``szip.dll``, and ``zlib.dll`` for the corresponding processor architecture. In order to prevent stray copies in your ``PATH`` being picked up at runtime, we recommend that you place them in those same directories alongside the assemblies.

The assemblies for the unit tests are located in ``UnitTests\bin\[x86,x64]\[Debug,Release]``. Save yourself some headaches and place the unmanaged DLLs there as well!

### Structure

Currently, the Visual Studio Solution has two projects, ``HDF.PInvoke`` and ``UnitTests``, which should be fairly self-explanatory. For each API (H5, H5A, etc.) there is one file, which contains the corresponding PInvoke declarations. They follow closely the native C-headers. Finally, there's one folder for each API in the ``UnitTests`` project with one file (of ``TestMethod`` definitions) for each API call.

![Visual Studio Solution](/images/HDF.PInvoke.jpg)

### How you can help

* Build it and run the tests
* Submit issues
* Submit pull requests addressing issues
* Make suggestions on how to better organize, document, and maintain the software
* Spread the word
* ...
