#!/bin/bash

# Push NuGet packages.
dotnet nuget push -s https://www.myget.org/F/coreports/api/v2/package -k $MYGETKEY 'bin/Release/*.nupkg' || true
