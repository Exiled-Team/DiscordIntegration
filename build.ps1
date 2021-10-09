#! pwsh

param (
    [Switch]$BuildNuGet
)

$Projects = @(
    "DiscordIntegration"
)

function Execute {
    param (
        [string]$Cmd
    )

    foreach ($Project in $Projects) {
        Invoke-Expression ([string]::Join(' ', $Cmd, $Project, $args))
        CheckLastOperationStatus
    }
}

function CheckLastOperationStatus {
    if ($? -eq $false) {
        Exit 1
    }
}

function GetSolutionVersion {
    [XML]$PropsFile = Get-Content Cerberus.props
    $Version = $PropsFile.Project.PropertyGroup[2].Version
    $Version = $Version.'#text'.Trim()
    return $Version
}

# Restore projects
Execute 'dotnet restore'
# Build projects
Execute 'dotnet build'
# Build a NuGet package if needed
if ($BuildNuGet) {
    $Version = GetSolutionVersion
    $Year = [System.DateTime]::Now.ToString('yyyy')

    Write-Host "Generating NuGet package for version $Version"

    nuget pack Exiled/Exiled.nuspec -Version $Version -Properties Year=$Year
}
