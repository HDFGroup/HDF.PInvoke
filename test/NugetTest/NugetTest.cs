using HDF.PInvoke;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace NugetTest
{
    [TestClass]
    public class NugetTest
    {
        [TestMethod]
        public void CanAccessHdfLib()
        {
            var fileAccessProps = H5P.create(H5P.FILE_ACCESS);
            Debug.Assert(fileAccessProps >= 0);

            var retval = H5P.set_libver_bounds(fileAccessProps, H5F.libver_t.LATEST, H5F.libver_t.LATEST);
            Debug.Assert(retval >= 0);

            var filePath = Path.GetTempFileName();
            var handle = H5F.create(filePath, H5F.ACC_TRUNC, access_plist: fileAccessProps);
            Debug.Assert(handle >= 0);

            retval = H5F.close(handle);
            Debug.Assert(retval >= 0);

            File.Delete(filePath);
        }
    }
}
