# target string
$Command = 'Start-Process calc.exe' 

# converting to Base64 encoded string
$Encoded = [convert]::ToBase64String([System.Text.encoding]::Unicode.GetBytes($command)) 

$Encoded

# running the encoded command with powershell
powershell.exe -encoded $Encoded