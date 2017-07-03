Param([String]$Title,[String]$Message,[HashTable]$Options,[Int32]$Default=0,[Switch]$Not)
Begin
{}
Process
{
    $items = @();
    $keys = @();
    foreach($option in ($Options.Keys | Sort-Object))
    {
        $items += New-Object System.Management.Automation.Host.ChoiceDescription $option
        $keys += $option;
    }
    $choice = $host.ui.PromptForChoice($Title,$Message,$items,$Default);
    $choice = $keys[$choice];
    foreach($key in $Options.Keys)
    {
        if(($choice -eq $key) -or ($Not -and -not ($choice -eq $key)))
        {
            Write-Output $Options[$key];
        }
    }
}
End
{}