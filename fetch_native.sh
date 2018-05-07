#!/bin/bash

NATIVE_SRC="$(cat native_src.txt)"

set -e
pushd $(dirname "${BASH_SOURCE[0]}")/native/HDF5_1.10
wget -q $NATIVE_SRC/hdf5_linux.zip -O hdf5_linux.zip
wget -q $NATIVE_SRC/hdf5_mac.zip -O hdf5_mac.zip
wget -q $NATIVE_SRC/hdf5_windows.zip -O hdf5_windows.zip
unzip -o hdf5_linux.zip
unzip -o hdf5_mac.zip
unzip -o hdf5_windows.zip
rm -f hdf5_linux.zip 
rm -f hdf5_mac.zip
rm -f hdf5_windows.zip
popd

