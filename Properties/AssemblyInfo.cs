﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("HDF.PInvoke")]
[assembly: AssemblyDescription(".NET interop with native HDF5 libraries")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCompany("The HDF Group")]
[assembly: AssemblyProduct("HDF.PInvoke")]
[assembly: AssemblyCopyright("Copyright © 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
#if HDF5_VER1_10
[assembly: Guid("ae1ca912-0c0b-41a9-80c9-4054ec5a9d7c")]
#else
[assembly: Guid("161e6943-58a3-4643-bf68-113ee2b2df92")]
#endif

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if HDF5_VER1_10
[assembly: AssemblyVersion("1.10.0.0")]
[assembly: AssemblyFileVersion("1.10.0.2")]
#else
[assembly: AssemblyVersion("1.8.17.0")]
[assembly: AssemblyFileVersion("1.8.17.6")]
#endif