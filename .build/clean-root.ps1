param(
    [Parameter(Mandatory=$True)][string]$where
)

function Remove-BuildArtifacts
{
    [CmdletBinding()]
    param([parameter(ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true, Position=0, Mandatory=$true)]
        [string] $Where
    )
    $root = (Resolve-Path $Where).Path
    if(Test-Path $root)
    {
        $items = (Get-ChildItem $root -Include artifacts,bin,obj,packages,clientbin -Directory -Recurse)
        if($items -ne $null)
        {
            Write-Host "Removing Build Artifacts from $root" -ForegroundColor White
            Remove-Items $items
        }
    }
}

function Remove-LogFiles
{
     [CmdletBinding()]
    param([parameter(ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true, Position=0, Mandatory=$true)]
        [string] $Where
    )
    $path = (Resolve-Path $where).Path
    if(Test-Path $path)
    {
        $items = (Get-ChildItem $path -File -Recurse -Include *.log)
        if($items -ne $null)
        {
            Write-Host "Removing Log Files from $path" -ForegroundColor White
            Remove-Items $items
        }
    }
}

function Remove-AspNetArtifacts
{
    $where = Join-Path -Path $env:TEMP -ChildPath "Temporary ASP.NET Files\root"
    $path = (Resolve-Path $where).Path
    if(Test-Path $path)
    {
        $items = (Get-ChildItem $path -Directory)
        if($items -ne $null)
        {
            Write-Host "Removing Asp.Net Artifacts from $path" -ForegroundColor White
            Remove-Items $items
        }
    }
}

function Remove-Items
{
    [CmdletBinding()]
    param([parameter(ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true, Position=0, Mandatory=$true)]
        [string[]] $paths
    )
    foreach($item in $paths)
    {
        $ex = $null
        $i = 0
        $fullName = $item
        if(Test-Path $fullName)
        {
            do
            {
                if(Test-Path $fullName)
                {
                    $i++
                    Remove-Item $fullName -Force -Recurse -ErrorAction SilentlyContinue -ErrorVariable ex
                    if($i -le 5 -and ($ex -ne $null -and $ex.count -gt 0))
                    {
                        Start-Sleep -Milliseconds 250
                    }
                    elseif($ex -eq $null -or $ex.count -eq 0)
                    {
                        Write-Host "Removed '$fullName'" -ForegroundColor Gray
                    }
                }
                else
                {
                    break
                }
            }
            while(($ex -ne $null -and $ex.count -gt 0) -and $i -le 5)
        }
        if(($ex -ne $null -and $ex.count -gt 0))
        {
            $failed = $failed + ("$fullName`n`t`t$ex[0]")
        }
    }
    foreach($f in $failed)
    {
        Write-Host `t"Failed to Remove: '$f'" -ForegroundColor Magenta
    }

}

Remove-BuildArtifacts -Where $where
Remove-LogFiles -Where $where
Remove-AspNetArtifacts