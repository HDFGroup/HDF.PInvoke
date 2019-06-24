#!/bin/bash

$VERSION="2.42.2"

$ErrorActionPreference="Stop"

Push-Location $PSScriptRoot

if (-not (Test-Path docfx)) {
    New-Item -ItemType Directory docfx
    Push-Location docfx
    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile nuget.exe
    ./nuget.exe install docfx.console -ExcludeVersion -Version $VERSION
    Pop-Location
}

./docfx/docfx.console/tools/docfx.exe

Pop-Location
