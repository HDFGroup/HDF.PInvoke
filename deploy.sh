#!/bin/bash

# Push NuGet packages.
rm -f "bin/Release/*.symbols.nupkg"
dotnet nuget push -s https://api.nuget.org/v3/index.json -k $NUGETKEY 'bin/Release/*.nupkg' || true
