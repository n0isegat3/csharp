$assemblyFile = "C:\git\n0isegat3\csharp\105-MSSQL\n0iseSQLExec\bin\x64\Release\n0iseSQLExec.dll"

$stringBuilder = New-Object -Type System.Text.StringBuilder
$fileStream = [IO.File]::OpenRead($assemblyFile)

while (($byte = $fileStream.ReadByte()) -gt -1) {
    $stringBuilder.Append($byte.ToString("X2")) | Out-Null
}

$hexAssembly = '0x' + ($stringBuilder.ToString() -join "")

$hexOutPath = Join-Path $env:TEMP ('n0iseSQLExecDLL{0}.txt' -f (Get-Random))

$hexAssembly | Out-File -FilePath $hexOut

<#
$hexAssembly | Set-Clipboard
#>

$hexOutPath