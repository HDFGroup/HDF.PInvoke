#!/bin/bash

# Push NuGet packages.
rm -f "bin/Release/*.symbols.nupkg"
dotnet nuget push -s https://www.myget.org/F/coreports/api/v2/package -k $MYGETKEY 'bin/Release/*.nupkg' || true
