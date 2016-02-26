# Getting started with HDF.PInvoke

We have fairly complete coverage of the HDF5 1.8.16 API, and declarations for HDF5 1.10 are under development.
The easiest way to get started with ``HDF.PInvoke`` is to use the [NuGet package](https://www.nuget.org/packages/HDF.PInvoke/):
```
Install-Package HDF.PInvoke
```
If you want to get your hands dirty, clone or fork the repo, and read on!

### How you can help

* Build it and run the tests
* Submit issues
* Submit pull requests addressing issues
* Make suggestions on how to better organize, document, and maintain the software
* Spread the word
* ...

### Dependencies

HDF5 binaries can be obtained from [here](https://www.hdfgroup.org/HDF5/release/obtain5.html). The ``HDF.PInvoke.dll`` managed assemblies, located in ``bin\[Debug,Release]``, depend on the unmanaged DLLs ``hdf5.dll``, ``hdf5_hl.dll``, ``szip.dll``, and ``zlib.dll`` for the corresponding processor architecture.

``H5.open()`` and ``H5.close()`` are the Alpha and Omega of the HDF5 library: ``H5.open()`` initializes the HDF5 library and should always be called near the beginning of your program - usually the very first line! (If you are dealing with a complex control flow with multiple entry points, there is no harm in calling it more than once.) ``H5.close()`` is the counterpart of ``H5.open()``. It will attempt to shut down the library, and release all associated resources. It is usally called automatically on application exit, but it's good practice to make it explicit and check for potential errors. *If the library cannot shut down cleanly, there's a good chance that there's a bug in your application or in HDF5, or both, and the risk of file corruption, unless your are only reading files, is non-negligible.*

Upon the first call to an ``H5*`` function (Remember ``H5.open()``?), the application's configuration file (e.g., ``YourApplication.exe.config``) is searched for the key ``NativeDependenciesAbsolutePath``, whose value, if found, is added to the DLL-search path. If this key is not specified in the application's config-file, then the ``HDF.PInvoke.dll`` assembly detects the processor architecture (32- or 64-bit) of the hosting process and expects the unmanaged DLLs in the ``bin32`` or ``bin64`` subdirectories relative to its own location. For example, if ``HDF.PInvoke.dll`` lives in ``C:\bin``, it expects the unmanaged DLLs in ``C:\bin\bin32`` and ``C:\bin\bin64``.

Changing the DLL-search path is done using the ``PATH`` variable of the running process. An attempt to modify the running processes ``PATH`` environment is made, if that fails, the native binaries will be loaded from their default places (as hopefully installed by the HDF5-Installer).

### Structure

Currently, the Visual Studio Solution has two projects, ``HDF.PInvoke`` and ``UnitTests``, which should be fairly self-explanatory. For each API (H5, H5A, etc.) there is one file, which contains the corresponding PInvoke declarations. They follow closely the native C-headers. Finally, there's one folder for each API in the ``UnitTests`` project with one file (of ``TestMethod`` definitions) for each API call.

![Visual Studio Solution](/images/HDF.PInvoke.jpg)
