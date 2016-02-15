# Getting started with HDF.PInvoke

Currently, the bindings are tested and developed with HDF5 1.8.16. Once we've covered the 1.8 API, we'll incorporate the new HDF5 1.10 features (March?).

### Dependencies

HDF5 binaries can be obtained from [here](https://www.hdfgroup.org/HDF5/release/obtain5.html). The ``HDF.PInvoke.dll`` managed assemblies, located in ``bin\[Debug,Release]``, depend on the unmanaged DLLs ``hdf5.dll``, ``szip.dll``, and ``zlib.dll`` for the corresponding processor architecture.

When the assembly ``HDF.PInvoke.dll`` is loaded, the configuration file ``HDF.PInvoke.dll.config`` is searched for the key ``NativeDependenciesAbsolutePath``, whose value, if found, is added to the ``PATH`` environment variable. If this key is not specified in ``HDF.PInvoke.dll.config``, then the ``HDF.PInvoke.dll`` assembly detects the processor architecture (32- or 64-bit) of the hosting process and expects the unmanaged DLLs in the ``bin32`` or ``bin64`` subdirectories relative to its own location. For example, if ``HDF.PInvoke.dll`` lives in ``C:\bin``, it expects the unmanaged DLLs in ``C:\bin\bin32`` and ``C:\bin\bin64``.

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
