Param([String]$Path)
Begin
{
    Push-Location -Path:(Split-Path -Path:$PSCommandPath -Parent);
}
Process
{
	$Path = $Path -f [DateTime]::Now;
    $initialized = Test-Path -Path:'variable:global:LogContext';
    $context = New-Object -TypeName:'System.Collections.Stack';
    if($initialized)
    {
     	$context = (Get-Variable -Scope:'Global' -Name:'LogContext').Value;
    }
    else
    {
        Set-Variable -Scope:'Global' -Name:'LogContext' -Value:$context;
    }
    $Path = $Path -f [DateTime]::Now;
    $context.Push($Path);
}
End
{
    Pop-Location;
}