Param()
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
        Set-Variable -Scope:'Global' -Value:$context;
    }
    if($context.Length -gt 0)
    {
        $context.Pop() | Out-Null;
    }
}
End
{
    Pop-Location;
}