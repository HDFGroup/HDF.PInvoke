$ErrorActionPreference = "Stop"

$NATIVE_SRC = Get-Content native_src.txt -Raw

pushd "$PSScriptRoot/native/HDF5_1.10"
Invoke-WebRequest "$NATIVE_SRC/hdf5_linux.zip" -OutFile hdf5_linux.zip
Invoke-WebRequest "$NATIVE_SRC/hdf5_mac.zip" -OutFile hdf5_linux.zip
Invoke-WebRequest "$NATIVE_SRC/hdf5_windows.zip" -OutFile hdf5_windows.zip
7z x hdf5_linux.zip
7z x hdf5_mac.zip
7z x hdf5_windows.zip
rm hdf5_linux.zip
rm hdf5_mac.zip
rm hdf5_windows.zip
popd

