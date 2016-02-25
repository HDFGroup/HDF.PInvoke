$solutionDir = [System.IO.Path]::GetDirectoryName($dte.Solution.FullName) + "\"
$path = $installPath.Replace($solutionDir, "`$(SolutionDir)")

$NativeAssembliesDir = Join-Path $path "lib\native"
$x86 = $(Join-Path $NativeAssembliesDir "bin32\*.*")
$x64 = $(Join-Path $NativeAssembliesDir "bin64\*.*")

$HDFPostBuildCmd = "
if not exist `"`$(TargetDir)bin32`" md `"`$(TargetDir)bin32`"
xcopy /s /y `"$x86`" `"`$(TargetDir)bin32`"
if not exist `"`$(TargetDir)bin64`" md `"`$(TargetDir)bin64`"
xcopy /s /y `"$x64`" `"`$(TargetDir)bin64`""
