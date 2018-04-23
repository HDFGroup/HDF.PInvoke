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

            var handle = H5F.create("test.h5", H5F.ACC_TRUNC);
            Debug.Assert(handle >= 0);

            var retval = H5F.close(handle);
            Debug.Assert(retval >= 0);

            File.Delete ("test.h5");

            Console.WriteLine("HDF5 NuGet package consumption test successful.");
        }
    }
}
