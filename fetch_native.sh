#!/bin/bash

NATIVE_SRC="$(cat native_src.txt)"

set -e
pushd $(dirname "${BASH_SOURCE[0]}")/native/HDF5_1.10
wget -q $NATIVE_SRC/hdf5_linux.zip
wget -q $NATIVE_SRC/hdf5_windows.zip
unzip hdf5_linux.zip
unzip hdf5_windows.zip
rm -f hdf5_linux.zip hdf5_windows.zip
popd

