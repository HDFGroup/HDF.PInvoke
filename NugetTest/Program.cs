using System;
using System.Diagnostics;
using System.IO;

using HDF.PInvoke;

namespace NugetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting HDF5 NuGet package consumption test...");

            Console.WriteLine("H5P.ROOT: {0}", H5P.ROOT);

            var fileAccessProps = H5P.create(H5P.FILE_ACCESS);
            Debug.Assert(fileAccessProps >= 0);
    
            var retval = H5P.set_libver_bounds(fileAccessProps, H5F.libver_t.LATEST, H5F.libver_t.LATEST);
            Debug.Assert(retval >= 0);

            var handle = H5F.create("test.h5", H5F.ACC_TRUNC, access_plist: fileAccessProps);
            Debug.Assert(handle >= 0);

            retval = H5F.close(handle);
            Debug.Assert(retval >= 0);

            File.Delete ("test.h5");

            Console.WriteLine("HDF5 NuGet package consumption test successful.");
        }
    }
}
