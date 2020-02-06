![AppVeyor Project status badge](https://ci.appveyor.com/api/projects/status/github/HDFGroup/HDF.PInvoke?branch=master&svg=true)
[![Gitter](https://badges.gitter.im/HDFGroup/HDF.PInvoke.svg)](https://gitter.im/HDFGroup/HDF.PInvoke?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Google Group](https://groups.google.com/forum/my-groups-color.png)](https://groups.google.com/forum/#!forum/sharp-hdf5)

# What it is (not)

HDF.PInvoke is a collection of [PInvoke](https://en.wikipedia.org/wiki/Platform_Invocation_Services)
signatures for the [HDF5 C-API](https://www.hdfgroup.org/HDF5/doc/RM/RM_H5Front.html).
It's practically *code-free*, which means we can blame all the bugs on Microsoft or [The HDF Group](https://www.hdfgroup.org/) :smile:

It is **not** a high-level .NET interface for HDF5. "It's the [GCD](https://en.wikipedia.org/wiki/Greatest_common_divisor)
of .NET bindings for HDF5, not the [LCM](https://en.wikipedia.org/wiki/Least_common_multiple)." :bowtie:

## Current Release Version(s)

| HDF5 Release Version                                                   | Assembly Version | Assembly File Version | Git Tag |
| ---------------------------------------------------------------------- | ---------------- | --------------------------------------------------------------- | ------- |
| [1.8.21](https://portal.hdfgroup.org/display/support/Downloads)  | 1.8.21.1         | [1.8.21.1](https://www.nuget.org/packages/HDF.PInvoke/1.8.21.1) | v1.8.21.1  |
| [1.10.6](https://portal.hdfgroup.org/display/support/Downloads) | 1.10.6.1         | [1.10.6.1](https://www.nuget.org/packages/HDF.PInvoke/1.10.6.1) | v1.10.6.1 |

[How "stuff" is versioned.](../../wiki/Versioning-and-Releases)

## Quick Install:

To install the latest HDF.PInvoke 1.8, run the following command in the
[Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)
```
    Install-Package HDF.PInvoke -Version 1.8.21.1
```
To install the latest HDF.PInvoke 1.10, run the following command in the
[Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)
```
    Install-Package HDF.PInvoke -Version 1.10.6.1
```

# Prerequisites

The ``HDF.PInvoke.dll`` managed assembly depends on the following native DLLs (32-bit and 64-bit):
- HDF5 core API, ``hdf5.dll``
- HDF5 high-level APIs, ``hdf5_hl.dll``
- Gzip compression, ``zlib.dll``
- Szip compression, ``szip.dll``
- The C-runtime of the Visual Studio version used to build the former, e.g., ``msvcr120.dll`` for Visual Studio 2013

All native dependencies, built with [thread-safety enabled](https://support.hdfgroup.org/HDF5/faq/threadsafe.html),
are included in the NuGet packages,
**except** the Visual Studio C-runtime, which is available from Microsoft as [Visual C++ Redistributable Packages for Visual Studio 2013](https://www.microsoft.com/en-us/download/details.aspx?id=40784). In the unlikely event that
they aren't already installed on your system, go get 'em!
(See [this link](https://msdn.microsoft.com/en-us/library/ms235299.aspx) for the rationale behind not
distributing the Visual Studio C-runtime in the NuGet package.)

## The DLL Resolution Process

On the first call to an ``H5*`` function, the application's configuration file
(e.g., ``YourApplication.exe.config``) is searched for the key ``NativeDependenciesAbsolutePath``,
whose value, if found, is added to the DLL-search path. If this key is not
specified in the application's config-file, then the ``HDF.PInvoke.dll`` assembly
detects the processor architecture (32- or 64-bit) of the hosting process and expects
to find the native DLLs in the ``bin32`` or ``bin64`` subdirectories, relative to its
location. For example, if ``HDF.PInvoke.dll`` lives in ``C:\bin``, it looks for
the native DLLs in ``C:\bin\bin32`` and ``C:\bin\bin64``.
Finally, the ``PATH`` environment variable of the running process is searched for other locations,
such as installed by the [HDF5 installers](https://www.hdfgroup.org/HDF5/).

# Two Major HDF5 Versions for the Price of One (Free)

The HDF Group currently maintains two major HDF5 release families, HDF5 1.8 and HDF5 1.10. The Visual Studio Solution is set up to build the `HDF.PInvoke.dll` .NET assemblies for the `"Any CPU"` platform in the `Debug` and `Release` configurations. Support for the HDF5 1.8 or 1.10 API is toggled via the `HDF5_VER1_10` conditional compilation symbol in the *Build* properties of the *HDF.PInvoke* and *UnitTest* projects.

# License

HDF.PInvoke is part of [HDF5](https://www.hdfgroup.org/HDF5/). It is subject to
the *same* terms and conditions as HDF5. Please review [COPYING](COPYING) or
[https://support.hdfgroup.org/ftp/HDF5/releases/COPYING](https://support.hdfgroup.org/ftp/HDF5/releases/COPYING)
for the details. If you have any questions, please [contact us](http://www.hdfgroup.org/about/contact.html).

# Supporting HDF.PInvoke

The best way to support HDF.Pinvoke is to contribute to it either by reporting
bugs, writing documentation (e.g., the [cookbook](https://github.com/HDFGroup/HDF.PInvoke/wiki/Cookbook)),
or sending pull requests.

***

![The HDF Group logo](https://github.com/HDFGroup/HDF.PInvoke/blob/master/images/The%20HDF%20Group.jpg)
