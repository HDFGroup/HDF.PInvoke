# HDF.PInvoke

[![Build Status](https://travis-ci.org/surban/HDF.PInvoke.svg?branch=master)](https://travis-ci.org/surban/HDF.PInvoke)
[![Build status](https://ci.appveyor.com/api/projects/status/k9f3fqys0hwdvxnu?svg=true)](https://ci.appveyor.com/project/surban/hdf-pinvoke)
[![Gitter](https://badges.gitter.im/HDFGroup/HDF.PInvoke.svg)](https://gitter.im/HDFGroup/HDF.PInvoke?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Google Group](https://groups.google.com/forum/my-groups-color.png)](https://groups.google.com/forum/#!forum/sharp-hdf5)

## .NET Core 2.0 port

This is an unofficial port of HDF.PInvoke to .NET Core 2.0 by [Sebastian Urban](mailto:surban@surban.net).
It has been tested on Linux, Mac OS X and Microsoft Windows.

## What it is (not)

HDF.PInvoke is a collection of [PInvoke](https://en.wikipedia.org/wiki/Platform_Invocation_Services)
signatures for the [HDF5 C-API](https://www.hdfgroup.org/HDF5/doc/RM/RM_H5Front.html).
It's practically *code-free*, which means we can blame all the bugs on Microsoft or [The HDF Group](https://www.hdfgroup.org/) :smile:

It is **not** a high-level .NET interface for HDF5. "It's the [GCD](https://en.wikipedia.org/wiki/Greatest_common_divisor)
of .NET bindings for HDF5, not the [LCM](https://en.wikipedia.org/wiki/Least_common_multiple)." :bowtie:

## Quick Install

To install the latest HDF.PInvoke 1.10 for .NET core, create a `NuGet.config` file with the following contents in your project directory.
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="CorePorts" value="https://www.myget.org/F/coreports/api/v3/index.json" />
  </packageSources>
</configuration>
```
Then, run the following command.
```
dotnet add package HDF.PInvoke 
```

# Prerequisites

The ``HDF.PInvoke.dll`` managed assembly depends on the following native libraries (32-bit and 64-bit):
- HDF5 core API, ``hdf5.dll``, ``libhdf5.so`` or ``libhdf5.dylib``
- HDF5 high-level APIs, ``hdf5_hl.dll``, ``libhdf5_hl.so`` or ``libhdf5_hl.dylib``
- Gzip compression, ``zlib.dll`` (Windows only)
- Szip compression, ``szip.dll`` (Windows only)
- The C-runtime of the Visual Studio version used to build the former, e.g., ``msvcr120.dll`` for Visual Studio 2013 (Windows only)

## Linux

Everything you need should be included in the NuGet package.

## Microsoft Windows

All native dependencies, built with [thread-safety enabled](https://support.hdfgroup.org/HDF5/faq/threadsafe.html),
are included in the NuGet package.
You additionally require the Visual Studio C-runtime, which is available from Microsoft as [Visual C++ Redistributable Packages for Visual Studio 2013](https://www.microsoft.com/en-us/download/details.aspx?id=40784). 
In the unlikely event that they aren't already installed on your system, go get 'em!
(See [this link](https://msdn.microsoft.com/en-us/library/ms235299.aspx) for the rationale behind not distributing the Visual Studio C-runtime in the NuGet package.)

## Mac OS

You need to install the native dependencies yourself.
This can be done using [Homebrew](https://brew.sh/) by executing the command
```
brew install hdf5
```
Currently some tests are failing on Mac OS and thus you might experience issues.

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
