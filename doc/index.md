# HDF.PInvoke.NETStandard

[![Build Status](https://travis-ci.org/surban/HDF.PInvoke.svg?branch=master)](https://travis-ci.org/surban/HDF.PInvoke)
[![Build status](https://ci.appveyor.com/api/projects/status/k9f3fqys0hwdvxnu?svg=true)](https://ci.appveyor.com/project/surban/hdf-pinvoke)

## .NET Standard 2.0 port

This is an unofficial port of [HDF.PInvoke from the HDF group](https://github.com/HDFGroup/HDF.PInvoke) to .NET Standard 2.0 by [Sebastian Urban](mailto:surban@surban.net).
It has been tested on Linux, MacOS and Microsoft Windows.

The source code is available at <https://github.com/surban/HDF.PInvoke>.
Please use this GitHub repository to report issues and send pull requests.

## Multithreaded operation

All HDF5 operations are thread-safe as described in the [official document](https://support.hdfgroup.org/HDF5/faq/threadsafe.html),


## What it is (not)

HDF.PInvoke is a collection of [PInvoke](https://en.wikipedia.org/wiki/Platform_Invocation_Services)
signatures for the [HDF5 C-API](https://www.hdfgroup.org/HDF5/doc/RM/RM_H5Front.html).
It's practically *code-free*, which means we can blame all the bugs on Microsoft or [The HDF Group](https://www.hdfgroup.org/) :smile:

It is **not** a high-level .NET interface for HDF5. "It's the [GCD](https://en.wikipedia.org/wiki/Greatest_common_divisor)
of .NET bindings for HDF5, not the [LCM](https://en.wikipedia.org/wiki/Least_common_multiple)." :bowtie:

## Quick Install

To install the latest HDF.PInvoke 1.10 for .NET core, run the following command inside your project directory.

```
dotnet add package HDF.PInvoke.NETStandard
```

You can also grab the NuGet package from <https://www.nuget.org/packages/HDF.PInvoke.NETStandard>.

## Prerequisites

The ``HDF.PInvoke.dll`` managed assembly depends on the following native libraries (32-bit and 64-bit):
- HDF5 core API, ``hdf5.dll``, ``libhdf5.so`` or ``libhdf5.dylib``
- HDF5 high-level APIs, ``hdf5_hl.dll``, ``libhdf5_hl.so`` or ``libhdf5_hl.dylib``
- Gzip compression, ``zlib1.dll`` (Windows only)
- The C-runtime of the Visual Studio version used to build the former, e.g., ``msvcrXXX.dll`` for Visual Studio 2017 (Windows only)

### Linux

Everything you need should be included in the NuGet package.

### Mac OS

Everything you need should be included in the NuGet package.

### Microsoft Windows

All native dependencies, built with [thread-safety enabled](https://support.hdfgroup.org/HDF5/faq/threadsafe.html),
are included in the NuGet package.
You additionally require the Visual Studio C-runtime, which is available from Microsoft as [Visual C++ Redistributable Packages for Visual Studio 2017](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads). 
In the unlikely event that they aren't already installed on your system, go get 'em!
(See [this link](https://msdn.microsoft.com/en-us/library/ms235299.aspx) for the rationale behind not distributing the Visual Studio C-runtime in the NuGet package.)

## License

HDF.PInvoke is part of [HDF5](https://www.hdfgroup.org/HDF5/). It is subject to
the *same* terms and conditions as HDF5. Please review <https://support.hdfgroup.org/ftp/HDF5/releases/COPYING>
for the details. If you have any questions, please [contact the HDF group](http://www.hdfgroup.org/about/contact.html).

## Supporting HDF.PInvoke

The best way to support HDF.PInvoke is to contribute to it either by reporting
bugs, writing documentation (e.g., the [cookbook](https://github.com/HDFGroup/HDF.PInvoke/wiki/Cookbook)),
or sending pull requests.

***
