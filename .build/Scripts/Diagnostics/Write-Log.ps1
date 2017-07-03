Param([String]$Level = 'Information',[Int32]$Indent = 0,[String]$Message,[Object[]]$Arguments)
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
}
Process
{
    $context = (Get-Variable -Scope:'Global' -Name:'LogContext').Value;
	$path = $context.Peek();
	$Message = $Message.PadLeft($Indent);
	$Message = $Message -f $Arguments;
    switch($Level)
	{
		Verbose {
			"[+] $Message" | Tee-Object -FilePath:$Path -Append | Write-Verbose -Verbose:$PSBoundParameters['Verbose']; 
			break;
		}
		Information {
			"[?] $Message" | Tee-Object -FilePath:$Path -Append | Write-Host;
			break;
		}
		Warning {
			"[*] $Message" | Tee-Object -FilePath:$Path -Append | Write-Warning;
			break;
		}
		Error {
			"[!] $Message" | Tee-Object -FilePath:$Path -Append | Write-Error;
			break;
		}		
		Enter
		{
			"[->] $Message" | Tee-Object -FilePath:$Path -Append | Write-Host;
		}
		Exit
		{
			"[<-] $Message" | Tee-Object -FilePath:$Path -Append | Write-Host;
		}
	}
}
End
{
    Pop-Location;
}