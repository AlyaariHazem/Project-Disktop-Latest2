$ErrorActionPreference = 'Stop'

#
# Add-Migration
#

Register-TabExpansion Add-Migration @{
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Adds a new migration.

.DESCRIPTION
    Adds a new migration.

.PARAMETER Name
    The name of the migration.

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory. Defaults to "Migrations".

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Remove-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Add-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [string] $Name,
        [string] $OutputDir,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Namespace,
        [string] $Args)

    WarnIfEF6 'Add-Migration'

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'add', $Name, '--json'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($Namespace)
    {
        $params += '--namespace', $Namespace
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json
    Write-Host 'To undo this action, use Remove-Migration.'

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
       $dteProject.ProjectItems.AddFromFile($result.migrationFile) | Out-Null

       $dteProject.ProjectItems.AddFromFile($result.metadataFile) | Out-Null

       $dteProject.ProjectItems.AddFromFile($result.snapshotFile) | Out-Null
    }

    $DTE.ItemOperations.OpenFile($result.migrationFile) | Out-Null
    ShowConsole
}

#
# Bundle-Migration
#

Register-TabExpansion Bundle-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Creates an executable to update the database.

.DESCRIPTION
    Creates an executable to update the database.

.PARAMETER Output
    The path of executable file to create.

.PARAMETER Force
    Overwrite existing files.

.PARAMETER SelfContained
    Also bundle the .NET runtime so it doesn't need to be installed on the machine.

.PARAMETER TargetRuntime
    The target runtime to bundle for.

.PARAMETER Framework
    The target framework. Defaults to the first one in the project.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Script-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Bundle-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Output,
        [switch] $Force,
        [switch] $SelfContained,
        [string] $TargetRuntime,
        [string] $Framework,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Framework)
    {
        $Framework = GetProperty $dteStartupProject.Properties 'FriendlyTargetFramework'
    }

    $params = 'migrations', 'bundle', '--framework', $Framework

    if ($Output)
    {
        $params += '--output', $Output
    }

    if ($Force)
    {
        $params += '--force'
    }

    if ($SelfContained)
    {
        $params += '--self-contained'
    }

    if ($TargetRuntime)
    {
        $params += '--target-runtime', $TargetRuntime
    }

    $params += GetParams $Context

    EF $dteProject $dteStartupProject $params $Args
}

#
# Drop-Database
#

Register-TabExpansion Drop-Database @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Drops the database.

.DESCRIPTION
    Drops the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Update-Database
    about_EntityFrameworkCore
#>
function Drop-Database
{
    [CmdletBinding(PositionalBinding = $false, SupportsShouldProcess = $true, ConfirmImpact = 'High')]
    param(
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $info = Get-DbContext -Context $Context -Project $Project -StartupProject $StartupProject

    if ($PSCmdlet.ShouldProcess("database '$($info.databaseName)' on server '$($info.dataSource)'"))
    {
        $params = 'database', 'drop', '--force'
        $params += GetParams $Context

        EF $dteProject $dteStartupProject $params $Args -skipBuild
    }
}

#
# Enable-Migrations (Obsolete)
#

function Enable-Migrations
{
    WarnIfEF6 'Enable-Migrations'
    Write-Warning 'Enable-Migrations is obsolete. Use Add-Migration to start using Migrations.'
}

#
# Get-DbContext
#

Register-TabExpansion Get-DbContext @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Lists and gets information about available DbContext types.

.DESCRIPTION
    Lists and gets information about available DbContext types.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Get-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if ($PSBoundParameters.ContainsKey('Context'))
    {
       $params = 'dbcontext', 'info', '--json'
       $params += GetParams $Context

       # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
       return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json
    }
    else
    {
       $params = 'dbcontext', 'list', '--json'
       # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
       # NB: Write-Output is here to break the JSON array into individual items
       return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json | Write-Output | Format-Table -Property safeName -HideTableHeaders
    }
}

#
# Get-Migration
#

Register-TabExpansion Get-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Lists available migrations.

.DESCRIPTION
    Lists available migrations.

.PARAMETER Connection
    The connection string to the database. Defaults to the one specified in AddDbContext or OnConfiguring.

.PARAMETER NoConnect
    Don't connect to the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Add-Migration
    Remove-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Get-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Connection,
        [switch] $NoConnect,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'list', '--json'

    if ($Connection)
    {
        $params += '--connection', $Connection
    }

    if ($NoConnect)
    {
        $params += '--no-connect'
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json | Write-Output
}

#
# Remove-Migration
#

Register-TabExpansion Remove-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Removes the last migration.

.DESCRIPTION
    Removes the last migration.

.PARAMETER Force
    Revert the migration if it has been applied to the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Add-Migration
    Get-Migration
    about_EntityFrameworkCore
#>
function Remove-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [switch] $Force,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'remove', '--json'

    if ($Force)
    {
        $params += '--force'
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
        $files = $result.migrationFile, $result.metadataFile, $result.snapshotFile
        $files | ?{ $_ -ne $null } | %{
            $projectItem = GetProjectItem $dteProject $_
            if ($projectItem)
            {
                $projectItem.Remove()
            }
        }
    }
}

#
# Optimize-DbContext
#

Register-TabExpansion Optimize-DbContext @{
    Project = { GetProjects }
    StartupProject = { GetProjects }
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
}

<#
.SYNOPSIS
    Generates a compiled version of the model used by the DbContext.

.DESCRIPTION
    Generates a compiled version of the model used by the DbContext.

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Optimize-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $OutputDir,
        [string] $Namespace,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'dbcontext', 'optimize'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($Namespace)
    {
        $params += '--namespace', $Namespace
    }

    if ($Context)
    {
        $params += '--context', $Context
    }

    EF $dteProject $dteStartupProject $params $Args
}

#
# Scaffold-DbContext
#

Register-TabExpansion Scaffold-DbContext @{
    Provider = { param($x) GetProviders $x.Project }
    Project = { GetProjects }
    StartupProject = { GetProjects }
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
    ContextDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
}

<#
.SYNOPSIS
    Scaffolds a DbContext and entity types for a database.

.DESCRIPTION
    Scaffolds a DbContext and entity types for a database.

.PARAMETER Connection
    The connection string to the database.

.PARAMETER Provider
    The provider to use. (E.g. Microsoft.EntityFrameworkCore.SqlServer)

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory.

.PARAMETER ContextDir
    The directory to put the DbContext file in. Paths are relative to the project directory.

.PARAMETER Context
    The name of the DbContext. Defaults to the database name.

.PARAMETER Schemas
    The schemas of tables and views to generate entity types for. All tables and views in the schemas will be included in the model, even if they are not explicitly included with the 'Tables' parameter.

.PARAMETER Tables
    The tables and views to generate entity types for. Tables or views in a specific schema can be included using the 'schema.table' or 'schema.view' format.

.PARAMETER DataAnnotations
    Use attributes to configure the model (where possible). If omitted, only the fluent API is used.

.PARAMETER UseDatabaseNames
    Use table, view, sequence, and column names directly from the database.

.PARAMETER Force
    Overwrite existing files.

.PARAMETER NoOnConfiguring
    Don't generate DbContext.OnConfiguring.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER ContextNamespace
    The namespace of the DbContext class. Matches the directory by default.

.PARAMETER NoPluralize
    Don't use the pluralizer.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Scaffold-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [string] $Connection,
        [Parameter(Position = 1, Mandatory = $true)]
        [string] $Provider,
        [string] $OutputDir,
        [string] $ContextDir,
        [string] $Context,
        [string[]] $Schemas = @(),
        [string[]] $Tables = @(),
        [switch] $DataAnnotations,
        [switch] $UseDatabaseNames,
        [switch] $Force,
        [switch] $NoOnConfiguring,
        [string] $Project,
        [string] $StartupProject,
        [string] $Namespace,
        [string] $ContextNamespace,
        [switch] $NoPluralize,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'dbcontext', 'scaffold', $Connection, $Provider, '--json'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($ContextDir)
    {
        $params += '--context-dir', $ContextDir
    }

    if ($Context)
    {
        $params += '--context', $Context
    }

    if ($PSBoundParameters.ContainsKey('Namespace'))
    {
        $params += '--namespace', $Namespace
    }

    if ($PSBoundParameters.ContainsKey('ContextNamespace'))
    {
        $params += '--context-namespace', $ContextNamespace
    }

    $params += $Schemas | %{ '--schema', $_ }
    $params += $Tables | %{ '--table', $_ }

    if ($DataAnnotations)
    {
        $params += '--data-annotations'
    }

    if ($UseDatabaseNames)
    {
        $params += '--use-database-names'
    }

    if ($Force)
    {
        $params += '--force'
    }

    if ($NoOnConfiguring)
    {
        $params += '--no-onconfiguring'
    }

    if ($NoPluralize)
    {
        $params += '--no-pluralize'
    }

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
       $files = $result.entityTypeFiles + $result.contextFile
       $files | %{ $dteProject.ProjectItems.AddFromFile($_) | Out-Null }
    }

    $DTE.ItemOperations.OpenFile($result.contextFile) | Out-Null
    ShowConsole
}

#
# Script-DbContext
#

Register-TabExpansion Script-DbContext @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Generates a SQL script from the DbContext. Bypasses any migrations.

.DESCRIPTION
    Generates a SQL script from the DbContext. Bypasses any migrations.

.PARAMETER Output
    The file to write the result to.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Script-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Output,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Output)
    {
        $intermediatePath = GetIntermediatePath $dteProject
        if (![IO.Path]::IsPathRooted($intermediatePath))
        {
            $projectDir = GetProperty $dteProject.Properties 'FullPath'
            $intermediatePath = [IO.Path]::GetFullPath((Join-Path $projectDir $intermediatePath))
        }

        $scriptFileName = [IO.Path]::ChangeExtension([IO.Path]::GetRandomFileName(), '.sql')
        $Output = Join-Path $intermediatePath $scriptFileName
    }
    elseif (![IO.Path]::IsPathRooted($Output))
    {
        $Output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Output)
    }

    $params = 'dbcontext', 'script', '--output', $Output
    $params += GetParams $Context

    EF $dteProject $dteStartupProject $params $Args

    $DTE.ItemOperations.OpenFile($Output) | Out-Null
    ShowConsole
}

#
# Script-Migration
#

Register-TabExpansion Script-Migration @{
    From = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    To = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Generates a SQL script from migrations.

.DESCRIPTION
    Generates a SQL script from migrations.

.PARAMETER From
    The starting migration. Defaults to '0' (the initial database).

.PARAMETER To
    The target migration. Defaults to the last migration.

.PARAMETER Idempotent
    Generate a script that can be used on a database at any migration.

.PARAMETER NoTransactions
    Don't generate SQL transaction statements.

.PARAMETER Output
    The file to write the result to.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Update-Database
    Get-Migration
    about_EntityFrameworkCore
#>
function Script-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(ParameterSetName = 'WithoutTo', Position = 0)]
        [Parameter(ParameterSetName = 'WithTo', Position = 0, Mandatory = $true)]
        [string] $From,
        [Parameter(ParameterSetName = 'WithTo', Position = 1, Mandatory = $true)]
        [string] $To,
        [switch] $Idempotent,
        [switch] $NoTransactions,
        [string] $Output,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Output)
    {
        $intermediatePath = GetIntermediatePath $dteProject
        if (![IO.Path]::IsPathRooted($intermediatePath))
        {
            $projectDir = GetProperty $dteProject.Properties 'FullPath'
            $intermediatePath = [IO.Path]::GetFullPath((Join-Path $projectDir $intermediatePath))
        }

        $scriptFileName = [IO.Path]::ChangeExtension([IO.Path]::GetRandomFileName(), '.sql')
        $Output = Join-Path $intermediatePath $scriptFileName
    }
    elseif (![IO.Path]::IsPathRooted($Output))
    {
        $Output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Output)
    }

    $params = 'migrations', 'script', '--output', $Output

    if ($From)
    {
        $params += $From
    }

    if ($To)
    {
        $params += $To
    }

    if ($Idempotent)
    {
        $params += '--idempotent'
    }

    if ($NoTransactions)
    {
        $params += '--no-transactions'
    }

    $params += GetParams $Context


    EF $dteProject $dteStartupProject $params $Args

    $DTE.ItemOperations.OpenFile($Output) | Out-Null
    ShowConsole
}

#
# Update-Database
#

Register-TabExpansion Update-Database @{
    Migration = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Updates the database to a specified migration.

.DESCRIPTION
    Updates the database to a specified migration.

.PARAMETER Migration
    The target migration. If '0', all migrations will be reverted. Defaults to the last migration.

.PARAMETER Connection
    The connection string to the database. Defaults to the one specified in AddDbContext or OnConfiguring.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Script-Migration
    about_EntityFrameworkCore
#>
function Update-Database
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0)]
        [string] $Migration,
        [string] $Connection,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    WarnIfEF6 'Update-Database'

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'database', 'update'

    if ($Migration)
    {
        $params += $Migration
    }

    if ($Connection)
    {
        $params += '--connection', $Connection
    }

    $params += GetParams $Context


    EF $dteProject $dteStartupProject $params $Args
}

#
# (Private Helpers)
#

function GetProjects
{
    return Get-Project -All | %{ $_.ProjectName }
}

function GetProviders($projectName)
{
    if (!$projectName)
    {
        $projectName = (Get-Project).ProjectName
    }

    return Get-Package -ProjectName $projectName | %{ $_.Id }
}

function GetContextTypes($projectName, $startupProjectName)
{
    $project = GetProject $projectName
    $startupProject = GetStartupProject $startupProjectName $project

    $params = 'dbcontext', 'list', '--json'

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    $result = (EF $project $startupProject $params $null -skipBuild) -join "`n" | ConvertFrom-Json | Write-Output

    return $result | %{ $_.safeName }
}

function GetMigrations($context, $projectName, $startupProjectName)
{
    $project = GetProject $projectName
    $startupProject = GetStartupProject $startupProjectName $project

    $params = 'migrations', 'list', '--no-connect', '--json'
    $params += GetParams $context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    $result = (EF $project $startupProject $params $null -skipBuild) -join "`n" | ConvertFrom-Json | Write-Output

    return $result | %{ $_.safeName }
}

function WarnIfEF6($cmdlet)
{
    if (Get-Module 'EntityFramework6')
    {
        Write-Warning "Both Entity Framework Core and Entity Framework 6 are installed. The Entity Framework Core tools are running. Use 'EntityFramework6\$cmdlet' for Entity Framework 6."
    }
    elseif (Get-Module 'EntityFramework')
    {
        Write-Warning "Both Entity Framework Core and Entity Framework 6 are installed. The Entity Framework Core tools are running. Use 'EntityFramework\$cmdlet' for Entity Framework 6."
    }
}

function GetProject($projectName)
{
    if (!$projectName)
    {
        return Get-Project
    }

    return Get-Project $projectName
}

function GetStartupProject($name, $fallbackProject)
{
    if ($name)
    {
        return Get-Project $name
    }

    $startupProjectPaths = $DTE.Solution.SolutionBuild.StartupProjects
    if ($startupProjectPaths)
    {
        if ($startupProjectPaths.Length -eq 1)
        {
            $startupProjectPath = $startupProjectPaths[0]
            if (![IO.Path]::IsPathRooted($startupProjectPath))
            {
                $solutionPath = Split-Path (GetProperty $DTE.Solution.Properties 'Path')
                $startupProjectPath = [IO.Path]::GetFullPath((Join-Path $solutionPath $startupProjectPath))
            }

            $startupProject = GetSolutionProjects | ?{
                try
                {
                    $fullName = $_.FullName
                }
                catch [NotImplementedException]
                {
                    return $false
                }

                if ($fullName -and $fullName.EndsWith('\'))
                {
                    $fullName = $fullName.Substring(0, $fullName.Length - 1)
                }

                return $fullName -eq $startupProjectPath
            }
            if ($startupProject)
            {
                return $startupProject
            }

            Write-Warning "Unable to resolve startup project '$startupProjectPath'."
        }
        else
        {
            Write-Warning 'Multiple startup projects set.'
        }
    }
    else
    {
        Write-Warning 'No startup project set.'
    }

    Write-Warning "Using project '$($fallbackProject.ProjectName)' as the startup project."

    return $fallbackProject
}

function GetSolutionProjects()
{
    $projects = New-Object 'System.Collections.Stack'

    $DTE.Solution.Projects | %{
        $projects.Push($_)
    }

    while ($projects.Count)
    {
        $project = $projects.Pop();

        <# yield return #> $project

        if ($project.ProjectItems)
        {
            $project.ProjectItems | ?{ $_.SubProject } | %{
                $projects.Push($_.SubProject)
            }
        }
    }
}

function GetParams($context)
{
    $params = @()

    if ($context)
    {
        $params += '--context', $context
    }

    return $params
}

function ShowConsole
{
    $componentModel = Get-VSComponentModel
    $powerConsoleWindow = $componentModel.GetService([NuGetConsole.IPowerConsoleWindow])
    $powerConsoleWindow.Show()
}

function WriteErrorLine($message)
{
    try
    {
        # Call the internal API NuGet uses to display errors
        $componentModel = Get-VSComponentModel
        $powerConsoleWindow = $componentModel.GetService([NuGetConsole.IPowerConsoleWindow])
        $bindingFlags = [Reflection.BindingFlags]::Instance -bor [Reflection.BindingFlags]::NonPublic
        $activeHostInfo = $powerConsoleWindow.GetType().GetProperty('ActiveHostInfo', $bindingFlags).GetValue($powerConsoleWindow)
        $internalHost = $activeHostInfo.WpfConsole.Host
        $reportErrorMethod = $internalHost.GetType().GetMethod('ReportError', $bindingFlags, $null, [Exception], $null)
        $exception = New-Object Exception $message
        $reportErrorMethod.Invoke($internalHost, $exception)
    }
    catch
    {
        Write-Host $message -ForegroundColor DarkRed
    }
}

function EF($project, $startupProject, $params, $applicationArgs, [switch] $skipBuild)
{
    if (IsDocker $startupProject)
    {
        throw "Startup project '$($startupProject.ProjectName)' is a Docker project. Select an ASP.NET Core Web " +
            'Application as your startup project and try again.'
    }
    if (IsUWP $startupProject)
    {
        throw "Startup project '$($startupProject.ProjectName)' is a Universal Windows Platform app. This version of " +
            'the Entity Framework Core Package Manager Console Tools doesn''t support this type of project. For more ' +
            'information on using the EF Core Tools with UWP projects, see ' +
            'https://go.microsoft.com/fwlink/?linkid=858496'
    }

    Write-Verbose "Using project '$($project.ProjectName)'."
    Write-Verbose "Using startup project '$($startupProject.ProjectName)'."

    if (!$skipBuild)
    {
        Write-Host 'Build started...'

        # TODO: Only build startup project. Don't use BuildProject, you can't specify platform
        $solutionBuild = $DTE.Solution.SolutionBuild
        $solutionBuild.Build(<# WaitForBuildToFinish: #> $true)
        if ($solutionBuild.LastBuildInfo)
        {
            throw 'Build failed.'
        }

        Write-Host 'Build succeeded.'
    }

    $activeConfiguration = $startupProject.ConfigurationManager.ActiveConfiguration
    if ($activeConfiguration -eq $null)
    {
        throw "Unable to read project configuration settings of project '$($startupProject.ProjectName)' for the " +
            'active solution configuration. Try closing Package Manager Console and restarting Visual Studio. If the ' +
            'problem persists, use Help > Send Feedback > Report a Problem.'
    }

    $startupProjectDir = GetProperty $startupProject.Properties 'FullPath'
    $outputPath = GetProperty $activeConfiguration.Properties 'OutputPath'
    $targetDir = [IO.Path]::GetFullPath([IO.Path]::Combine($startupProjectDir, $outputPath))
    $startupTargetFileName = GetProperty $startupProject.Properties 'OutputFileName'
    $startupTargetPath = Join-Path $targetDir $startupTargetFileName
    $targetFrameworkMoniker = GetProperty $startupProject.Properties 'TargetFrameworkMoniker'
    $frameworkName = New-Object 'System.Runtime.Versioning.FrameworkName' $targetFrameworkMoniker
    $targetFramework = $frameworkName.Identifier

    if ($targetFramework -in '.NETFramework')
    {
        $platformTarget = GetPlatformTarget $startupProject
        if ($platformTarget -eq 'x86')
        {
            $exePath = Join-Path $PSScriptRoot 'net472\win-x86\ef.exe'
        }
        elseif ($platformTarget -eq 'ARM64')
        {
            $exePath = Join-Path $PSScriptRoot 'net472\win-arm64\ef.exe'
        }
        elseif ($platformTarget -in 'AnyCPU', 'x64')
        {
            $exePath = Join-Path $PSScriptRoot 'net472\any\ef.exe'
        }
        else
        {
            throw "Startup project '$($startupProject.ProjectName)' has an active platform of '$platformTarget'. Select " +
                'a different platform and try again.'
        }
    }
    elseif ($targetFramework -eq '.NETCoreApp')
    {
        $targetPlatformIdentifier = GetCpsProperty $startupProject 'TargetPlatformIdentifier'
        if ($targetPlatformIdentifier -and $targetPlatformIdentifier -ne 'Windows')
        {
            throw "Startup project '$($startupProject.ProjectName)' targets platform '$targetPlatformIdentifier'. The Entity Framework " +
                'Core Package Manager Console Tools don''t support this platform. See https://aka.ms/efcore-docs-pmc-tfms for more ' +
                'information.'
        }

        $exePath = (Get-Command 'dotnet').Path

        $startupTargetName = GetProperty $startupProject.Properties 'AssemblyName'
        $depsFile = Join-Path $targetDir ($startupTargetName + '.deps.json')
        $projectAssetsFile = GetCpsProperty $startupProject 'ProjectAssetsFile'
        $runtimeConfig = Join-Path $targetDir ($startupTargetName + '.runtimeconfig.json')
        $runtimeFrameworkVersion = GetCpsProperty $startupProject 'RuntimeFrameworkVersion'
        $efPath = Join-Path $PSScriptRoot 'netcoreapp2.0\any\ef.dll'

        $dotnetParams = 'exec', '--depsfile', $depsFile

        if ($projectAssetsFile)
        {
            # NB: Don't use Get-Content. It doesn't handle UTF-8 without a signature
            # NB: Don't use ReadAllLines. ConvertFrom-Json won't work on PowerShell 3.0
            $projectAssets = [IO.File]::ReadAllText($projectAssetsFile) | ConvertFrom-Json
            $projectAssets.packageFolders.psobject.Properties.Name | %{
                $dotnetParams += '--additionalprobingpath', $_.TrimEnd('\')
            }
        }

        if ([IO.File]::Exists($runtimeConfig))
        {
            $dotnetParams += '--runtimeconfig', $runtimeConfig
        }
        elseif ($runtimeFrameworkVersion)
        {
            $dotnetParams += '--fx-version', $runtimeFrameworkVersion
        }

        $dotnetParams += $efPath

        $params = $dotnetParams + $params
    }
    elseif ($targetFramework -eq '.NETStandard')
    {
        throw "Startup project '$($startupProject.ProjectName)' targets framework '.NETStandard'. There is no " +
            'runtime associated with this framework, and projects targeting it cannot be executed directly. To use ' +
            'the Entity Framework Core Package Manager Console Tools with this project, add an executable project ' +
            'targeting .NET Framework or .NET Core that references this project, and set it as the startup project; ' +
            'or, update this project to cross-target .NET Framework or .NET Core. For more information on using the ' +
            'EF Core Tools with .NET Standard projects, see https://go.microsoft.com/fwlink/?linkid=2034705'
    }
    else
    {
        throw "Startup project '$($startupProject.ProjectName)' targets framework '$targetFramework'. The Entity Framework Core Package " +
            'Manager Console Tools don''t support this framework. See https://aka.ms/efcore-docs-pmc-tfms for more information and examples.'
    }

    $projectDir = GetProperty $project.Properties 'FullPath'
    $targetFileName = GetProperty $project.Properties 'OutputFileName'
    $targetPath = Join-Path $targetDir $targetFileName
    $rootNamespace = GetProperty $project.Properties 'RootNamespace'
    $language = GetLanguage $project
    $nullable = GetNullable $project

    $params += '--verbose',
        '--no-color',
        '--prefix-output',
        '--assembly', $targetPath,
        '--project', $project.FullName,
        '--startup-assembly', $startupTargetPath,
        '--startup-project', $startupProject.FullName,
        '--project-dir', $projectDir,
        '--language', $language,
        '--configuration', $activeConfiguration.ConfigurationName,
        '--working-dir', $PWD.Path

    if (IsWeb $startupProject)
    {
        $params += '--data-dir', (Join-Path $startupProjectDir 'App_Data')
    }

    if ($rootNamespace -ne $null)
    {
        $params += '--root-namespace', $rootNamespace
    }

    if ($nullable -in 'enable', 'annotations')
    {
        $params += '--nullable'
    }

    $arguments = ToArguments $params
    if ($applicationArgs)
    {
        $arguments += ' -- '
        $arguments += $applicationArgs
    }

    $startInfo = New-Object 'System.Diagnostics.ProcessStartInfo' -Property @{
        FileName = $exePath;
        Arguments = $arguments;
        UseShellExecute = $false;
        CreateNoWindow = $true;
        RedirectStandardOutput = $true;
        StandardOutputEncoding = [Text.Encoding]::UTF8;
        RedirectStandardError = $true;
        WorkingDirectory = $startupProjectDir;
    }

    Write-Verbose "$exePath $arguments"

    $process = [Diagnostics.Process]::Start($startInfo)

    while (($line = $process.StandardOutput.ReadLine()) -ne $null)
    {
        $level = $null
        $text = $null

        $parts = $line.Split(':', 2)
        if ($parts.Length -eq 2)
        {
            $level = $parts[0]

            $i = 0
            $count = 8 - $level.Length
            while ($i -lt $count -and $parts[1][$i] -eq ' ')
            {
                $i++
            }

            $text = $parts[1].Substring($i)
        }

        switch ($level)
        {
            'error' { WriteErrorLine $text }
            'warn' { Write-Warning $text }
            'info' { Write-Host $text }
            'data' { Write-Output $text }
            'verbose' { Write-Verbose $text }
            default { Write-Host $line }
        }
    }

    $process.WaitForExit()

    if ($process.ExitCode)
    {
        while (($line = $process.StandardError.ReadLine()) -ne $null)
        {
            WriteErrorLine $line
        }

        exit
    }
}

function IsDocker($project)
{
    return $project.Kind -eq '{E53339B2-1760-4266-BCC7-CA923CBCF16C}'
}

function IsCpsProject($project)
{
    $hierarchy = GetVsHierarchy $project
    $isCapabilityMatch = [Microsoft.VisualStudio.Shell.PackageUtilities].GetMethod(
        'IsCapabilityMatch',
        [type[]]([Microsoft.VisualStudio.Shell.Interop.IVsHierarchy], [string]))

    return $isCapabilityMatch.Invoke($null, ($hierarchy, 'CPS'))
}

function IsWeb($project)
{
    $types = GetProjectTypes $project

    return $types -contains '{349C5851-65DF-11DA-9384-00065B846F21}'
}

function IsUWP($project)
{
    $types = GetProjectTypes $project

    return $types -contains '{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}'
}

function GetIntermediatePath($project)
{
    $intermediatePath = GetProperty $project.ConfigurationManager.ActiveConfiguration.Properties 'IntermediatePath'
    if ($intermediatePath)
    {
        return $intermediatePath
    }

    return GetMSBuildProperty $project 'IntermediateOutputPath'
}

function GetPlatformTarget($project)
{
    if (IsCpsProject $project)
    {
        $platformTarget = GetCpsProperty $project 'PlatformTarget'
        if ($platformTarget)
        {
            return $platformTarget
        }

        return GetCpsProperty $project 'Platform'
    }

    $platformTarget = GetProperty $project.ConfigurationManager.ActiveConfiguration.Properties 'PlatformTarget'
    if ($platformTarget)
    {
        return $platformTarget
    }

    # NB: For classic F# projects
    $platformTarget = GetMSBuildProperty $project 'PlatformTarget'
    if ($platformTarget)
    {
        return $platformTarget
    }

    return 'AnyCPU'
}

function GetLanguage($project)
{
    if (IsCpsProject $project)
    {
        return GetCpsProperty $project 'Language'
    }

    return GetMSBuildProperty $project 'Language'
}

function GetNullable($project)
{
    if (IsCpsProject $project)
    {
        return GetCpsProperty $project 'Nullable'
    }

    return GetMSBuildProperty $project 'Nullable'
}

function GetVsHierarchy($project)
{
    $solution = Get-VSService 'Microsoft.VisualStudio.Shell.Interop.SVsSolution' 'Microsoft.VisualStudio.Shell.Interop.IVsSolution'
    $hierarchy = $null
    $hr = $solution.GetProjectOfUniqueName($project.UniqueName, [ref] $hierarchy)
    [Runtime.InteropServices.Marshal]::ThrowExceptionForHR($hr)

    return $hierarchy
}

function GetProjectTypes($project)
{
    $hierarchy = GetVsHierarchy $project
    $aggregatableProject = Get-Interface $hierarchy 'Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject'
    if (!$aggregatableProject)
    {
        return $project.Kind
    }

    $projectTypeGuidsString = $null
    $hr = $aggregatableProject.GetAggregateProjectTypeGuids([ref] $projectTypeGuidsString)
    [Runtime.InteropServices.Marshal]::ThrowExceptionForHR($hr)

    return $projectTypeGuidsString.Split(';')
}

function GetProperty($properties, $propertyName)
{
    try
    {
        return $properties.Item($propertyName).Value
    }
    catch
    {
        return $null
    }
}

function GetCpsProperty($project, $propertyName)
{
    $browseObjectContext = Get-Interface $project 'Microsoft.VisualStudio.ProjectSystem.Properties.IVsBrowseObjectContext'
    $unconfiguredProject = $browseObjectContext.UnconfiguredProject
    $configuredProject = $unconfiguredProject.GetSuggestedConfiguredProjectAsync().Result
    $properties = $configuredProject.Services.ProjectPropertiesProvider.GetCommonProperties()

    return $properties.GetEvaluatedPropertyValueAsync($propertyName).Result
}

function GetMSBuildProperty($project, $propertyName)
{
    $msbuildProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.LoadedProjects |
        ? FullPath -eq $project.FullName

    return $msbuildProject.GetProperty($propertyName).EvaluatedValue
}

function GetProjectItem($project, $path)
{
    $fullPath = GetProperty $project.Properties 'FullPath'

    if ([IO.Path]::IsPathRooted($path))
    {
        $path = $path.Substring($fullPath.Length)
    }

    $itemDirectory = (Split-Path $path -Parent)

    $projectItems = $project.ProjectItems
    if ($itemDirectory)
    {
        $directories = $itemDirectory.Split('\')
        $directories | %{
            if ($projectItems)
            {
                $projectItems = $projectItems.Item($_).ProjectItems
            }
        }
    }

    if (!$projectItems)
    {
        return $null
    }

    $itemName = Split-Path $path -Leaf

    try
    {
        return $projectItems.Item($itemName)
    }
    catch [Exception]
    {
    }

    return $null
}

function ToArguments($params)
{
    $arguments = ''
    for ($i = 0; $i -lt $params.Length; $i++)
    {
        if ($i)
        {
            $arguments += ' '
        }

        if ($params[$i] -eq '')
        {
            $arguments += '""'

            continue
        }
        if (!$params[$i].Contains(' '))
        {
            $arguments += $params[$i]

            continue
        }

        $arguments += '"'

        $pendingBackslashs = 0
        for ($j = 0; $j -lt $params[$i].Length; $j++)
        {
            switch ($params[$i][$j])
            {
                '"'
                {
                    if ($pendingBackslashs)
                    {
                        $arguments += '\' * $pendingBackslashs * 2
                        $pendingBackslashs = 0
                    }
                    $arguments += '\"'
                }

                '\'
                {
                    $pendingBackslashs++
                }

                default
                {
                    if ($pendingBackslashs)
                    {
                        if ($pendingBackslashs -eq 1)
                        {
                            $arguments += '\'
                        }
                        else
                        {
                            $arguments += '\' * $pendingBackslashs * 2
                        }

                        $pendingBackslashs = 0
                    }

                    $arguments += $params[$i][$j]
                }
            }
        }

        if ($pendingBackslashs)
        {
            $arguments += '\' * $pendingBackslashs * 2
        }

        $arguments += '"'
    }

    return $arguments
}

# SIG # Begin signature block
# MIIn0AYJKoZIhvcNAQcCoIInwTCCJ70CAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCDclR8eEkdY1anW
# zpXCUEf71z2yrr7sX1LNgcKdMtjG66CCDYUwggYDMIID66ADAgECAhMzAAADri01
# UchTj1UdAAAAAAOuMA0GCSqGSIb3DQEBCwUAMH4xCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25p
# bmcgUENBIDIwMTEwHhcNMjMxMTE2MTkwODU5WhcNMjQxMTE0MTkwODU5WjB0MQsw
# CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9u
# ZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMR4wHAYDVQQDExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
# AQD0IPymNjfDEKg+YyE6SjDvJwKW1+pieqTjAY0CnOHZ1Nj5irGjNZPMlQ4HfxXG
# yAVCZcEWE4x2sZgam872R1s0+TAelOtbqFmoW4suJHAYoTHhkznNVKpscm5fZ899
# QnReZv5WtWwbD8HAFXbPPStW2JKCqPcZ54Y6wbuWV9bKtKPImqbkMcTejTgEAj82
# 6GQc6/Th66Koka8cUIvz59e/IP04DGrh9wkq2jIFvQ8EDegw1B4KyJTIs76+hmpV
# M5SwBZjRs3liOQrierkNVo11WuujB3kBf2CbPoP9MlOyyezqkMIbTRj4OHeKlamd
# WaSFhwHLJRIQpfc8sLwOSIBBAgMBAAGjggGCMIIBfjAfBgNVHSUEGDAWBgorBgEE
# AYI3TAgBBggrBgEFBQcDAzAdBgNVHQ4EFgQUhx/vdKmXhwc4WiWXbsf0I53h8T8w
# VAYDVR0RBE0wS6RJMEcxLTArBgNVBAsTJE1pY3Jvc29mdCBJcmVsYW5kIE9wZXJh
# dGlvbnMgTGltaXRlZDEWMBQGA1UEBRMNMjMwMDEyKzUwMTgzNjAfBgNVHSMEGDAW
# gBRIbmTlUAXTgqoXNzcitW2oynUClTBUBgNVHR8ETTBLMEmgR6BFhkNodHRwOi8v
# d3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2NybC9NaWNDb2RTaWdQQ0EyMDExXzIw
# MTEtMDctMDguY3JsMGEGCCsGAQUFBwEBBFUwUzBRBggrBgEFBQcwAoZFaHR0cDov
# L3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9jZXJ0cy9NaWNDb2RTaWdQQ0EyMDEx
# XzIwMTEtMDctMDguY3J0MAwGA1UdEwEB/wQCMAAwDQYJKoZIhvcNAQELBQADggIB
# AGrJYDUS7s8o0yNprGXRXuAnRcHKxSjFmW4wclcUTYsQZkhnbMwthWM6cAYb/h2W
# 5GNKtlmj/y/CThe3y/o0EH2h+jwfU/9eJ0fK1ZO/2WD0xi777qU+a7l8KjMPdwjY
# 0tk9bYEGEZfYPRHy1AGPQVuZlG4i5ymJDsMrcIcqV8pxzsw/yk/O4y/nlOjHz4oV
# APU0br5t9tgD8E08GSDi3I6H57Ftod9w26h0MlQiOr10Xqhr5iPLS7SlQwj8HW37
# ybqsmjQpKhmWul6xiXSNGGm36GarHy4Q1egYlxhlUnk3ZKSr3QtWIo1GGL03hT57
# xzjL25fKiZQX/q+II8nuG5M0Qmjvl6Egltr4hZ3e3FQRzRHfLoNPq3ELpxbWdH8t
# Nuj0j/x9Crnfwbki8n57mJKI5JVWRWTSLmbTcDDLkTZlJLg9V1BIJwXGY3i2kR9i
# 5HsADL8YlW0gMWVSlKB1eiSlK6LmFi0rVH16dde+j5T/EaQtFz6qngN7d1lvO7uk
# 6rtX+MLKG4LDRsQgBTi6sIYiKntMjoYFHMPvI/OMUip5ljtLitVbkFGfagSqmbxK
# 7rJMhC8wiTzHanBg1Rrbff1niBbnFbbV4UDmYumjs1FIpFCazk6AADXxoKCo5TsO
# zSHqr9gHgGYQC2hMyX9MGLIpowYCURx3L7kUiGbOiMwaMIIHejCCBWKgAwIBAgIK
# YQ6Q0gAAAAAAAzANBgkqhkiG9w0BAQsFADCBiDELMAkGA1UEBhMCVVMxEzARBgNV
# BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jv
# c29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMpTWljcm9zb2Z0IFJvb3QgQ2VydGlm
# aWNhdGUgQXV0aG9yaXR5IDIwMTEwHhcNMTEwNzA4MjA1OTA5WhcNMjYwNzA4MjEw
# OTA5WjB+MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UE
# BxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSgwJgYD
# VQQDEx9NaWNyb3NvZnQgQ29kZSBTaWduaW5nIFBDQSAyMDExMIICIjANBgkqhkiG
# 9w0BAQEFAAOCAg8AMIICCgKCAgEAq/D6chAcLq3YbqqCEE00uvK2WCGfQhsqa+la
# UKq4BjgaBEm6f8MMHt03a8YS2AvwOMKZBrDIOdUBFDFC04kNeWSHfpRgJGyvnkmc
# 6Whe0t+bU7IKLMOv2akrrnoJr9eWWcpgGgXpZnboMlImEi/nqwhQz7NEt13YxC4D
# dato88tt8zpcoRb0RrrgOGSsbmQ1eKagYw8t00CT+OPeBw3VXHmlSSnnDb6gE3e+
# lD3v++MrWhAfTVYoonpy4BI6t0le2O3tQ5GD2Xuye4Yb2T6xjF3oiU+EGvKhL1nk
# kDstrjNYxbc+/jLTswM9sbKvkjh+0p2ALPVOVpEhNSXDOW5kf1O6nA+tGSOEy/S6
# A4aN91/w0FK/jJSHvMAhdCVfGCi2zCcoOCWYOUo2z3yxkq4cI6epZuxhH2rhKEmd
# X4jiJV3TIUs+UsS1Vz8kA/DRelsv1SPjcF0PUUZ3s/gA4bysAoJf28AVs70b1FVL
# 5zmhD+kjSbwYuER8ReTBw3J64HLnJN+/RpnF78IcV9uDjexNSTCnq47f7Fufr/zd
# sGbiwZeBe+3W7UvnSSmnEyimp31ngOaKYnhfsi+E11ecXL93KCjx7W3DKI8sj0A3
# T8HhhUSJxAlMxdSlQy90lfdu+HggWCwTXWCVmj5PM4TasIgX3p5O9JawvEagbJjS
# 4NaIjAsCAwEAAaOCAe0wggHpMBAGCSsGAQQBgjcVAQQDAgEAMB0GA1UdDgQWBBRI
# bmTlUAXTgqoXNzcitW2oynUClTAZBgkrBgEEAYI3FAIEDB4KAFMAdQBiAEMAQTAL
# BgNVHQ8EBAMCAYYwDwYDVR0TAQH/BAUwAwEB/zAfBgNVHSMEGDAWgBRyLToCMZBD
# uRQFTuHqp8cx0SOJNDBaBgNVHR8EUzBRME+gTaBLhklodHRwOi8vY3JsLm1pY3Jv
# c29mdC5jb20vcGtpL2NybC9wcm9kdWN0cy9NaWNSb29DZXJBdXQyMDExXzIwMTFf
# MDNfMjIuY3JsMF4GCCsGAQUFBwEBBFIwUDBOBggrBgEFBQcwAoZCaHR0cDovL3d3
# dy5taWNyb3NvZnQuY29tL3BraS9jZXJ0cy9NaWNSb29DZXJBdXQyMDExXzIwMTFf
# MDNfMjIuY3J0MIGfBgNVHSAEgZcwgZQwgZEGCSsGAQQBgjcuAzCBgzA/BggrBgEF
# BQcCARYzaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9kb2NzL3ByaW1h
# cnljcHMuaHRtMEAGCCsGAQUFBwICMDQeMiAdAEwAZQBnAGEAbABfAHAAbwBsAGkA
# YwB5AF8AcwB0AGEAdABlAG0AZQBuAHQALiAdMA0GCSqGSIb3DQEBCwUAA4ICAQBn
# 8oalmOBUeRou09h0ZyKbC5YR4WOSmUKWfdJ5DJDBZV8uLD74w3LRbYP+vj/oCso7
# v0epo/Np22O/IjWll11lhJB9i0ZQVdgMknzSGksc8zxCi1LQsP1r4z4HLimb5j0b
# pdS1HXeUOeLpZMlEPXh6I/MTfaaQdION9MsmAkYqwooQu6SpBQyb7Wj6aC6VoCo/
# KmtYSWMfCWluWpiW5IP0wI/zRive/DvQvTXvbiWu5a8n7dDd8w6vmSiXmE0OPQvy
# CInWH8MyGOLwxS3OW560STkKxgrCxq2u5bLZ2xWIUUVYODJxJxp/sfQn+N4sOiBp
# mLJZiWhub6e3dMNABQamASooPoI/E01mC8CzTfXhj38cbxV9Rad25UAqZaPDXVJi
# hsMdYzaXht/a8/jyFqGaJ+HNpZfQ7l1jQeNbB5yHPgZ3BtEGsXUfFL5hYbXw3MYb
# BL7fQccOKO7eZS/sl/ahXJbYANahRr1Z85elCUtIEJmAH9AAKcWxm6U/RXceNcbS
# oqKfenoi+kiVH6v7RyOA9Z74v2u3S5fi63V4GuzqN5l5GEv/1rMjaHXmr/r8i+sL
# gOppO6/8MO0ETI7f33VtY5E90Z1WTk+/gFcioXgRMiF670EKsT/7qMykXcGhiJtX
# cVZOSEXAQsmbdlsKgEhr/Xmfwb1tbWrJUnMTDXpQzTGCGaEwghmdAgEBMIGVMH4x
# CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRt
# b25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01p
# Y3Jvc29mdCBDb2RlIFNpZ25pbmcgUENBIDIwMTECEzMAAAOuLTVRyFOPVR0AAAAA
# A64wDQYJYIZIAWUDBAIBBQCgga4wGQYJKoZIhvcNAQkDMQwGCisGAQQBgjcCAQQw
# HAYKKwYBBAGCNwIBCzEOMAwGCisGAQQBgjcCARUwLwYJKoZIhvcNAQkEMSIEIJ+o
# McPKsuvDS4gjYXMbiNlrAdr6XkJT91xgBjDZWYsxMEIGCisGAQQBgjcCAQwxNDAy
# oBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBhodHRwOi8vd3d3Lm1pY3Jvc29mdC5j
# b20wDQYJKoZIhvcNAQEBBQAEggEALyt4CNo9/o+aJ10PYOAi9PS3ddWotDiET0xB
# oOmYiH+ZV7TbCfeMvmNoY2azjqbFJj9xsTIn8v7vMsattiM04BLxnbm4+9ibSh0L
# f1/l9Z+D3aDgwdue/R2p6ZzUkQ/F6fsfp3fTfS2bqEsklT7xumIAGfQfG3NUAW82
# k6+8gvgY8rAqUNNt4dfObZeKYVLDOKjv26Jb4tXEO93Mhe2kEJW/OdWPB9AHBvzI
# hV04ZGGscPYfvHOAmnjP9MkuqUON1i3239Fs7F8ndCgYZMvOHQOk4W9UAjp7Vaou
# Vjr1tDo9TW4xCuuqHDn3mn7WduL2Gub2KzZh+68al5KvfzUdX6GCFyswghcnBgor
# BgEEAYI3AwMBMYIXFzCCFxMGCSqGSIb3DQEHAqCCFwQwghcAAgEDMQ8wDQYJYIZI
# AWUDBAIBBQAwggFZBgsqhkiG9w0BCRABBKCCAUgEggFEMIIBQAIBAQYKKwYBBAGE
# WQoDATAxMA0GCWCGSAFlAwQCAQUABCAzf3Ek8Tt66k1rfg5Zp8mvWJ6R5lG/Gqfu
# yONUQ3OD0QIGZpfcjQOaGBMyMDI0MDgwNjA1MzM0Ni40MDJaMASAAgH0oIHYpIHV
# MIHSMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
# UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMS0wKwYDVQQL
# EyRNaWNyb3NvZnQgSXJlbGFuZCBPcGVyYXRpb25zIExpbWl0ZWQxJjAkBgNVBAsT
# HVRoYWxlcyBUU1MgRVNOOkQwODItNEJGRC1FRUJBMSUwIwYDVQQDExxNaWNyb3Nv
# ZnQgVGltZS1TdGFtcCBTZXJ2aWNloIIRejCCBycwggUPoAMCAQICEzMAAAHcweCM
# wl9YXo4AAQAAAdwwDQYJKoZIhvcNAQELBQAwfDELMAkGA1UEBhMCVVMxEzARBgNV
# BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jv
# c29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAg
# UENBIDIwMTAwHhcNMjMxMDEyMTkwNzA2WhcNMjUwMTEwMTkwNzA2WjCB0jELMAkG
# A1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
# HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEtMCsGA1UECxMkTWljcm9z
# b2Z0IElyZWxhbmQgT3BlcmF0aW9ucyBMaW1pdGVkMSYwJAYDVQQLEx1UaGFsZXMg
# VFNTIEVTTjpEMDgyLTRCRkQtRUVCQTElMCMGA1UEAxMcTWljcm9zb2Z0IFRpbWUt
# U3RhbXAgU2VydmljZTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAIvI
# syA1sjg9kSKJzelrUWF5ShqYWL83amn3SE5JyIVPUC7F6qTcLphhHZ9idf21f0Ra
# GrU8EHydF8NxPMR2KVNiAtCGPJa8kV1CGvn3beGB2m2ltmqJanG71mAywrkKATYn
# iwKLPQLJ00EkXw5TSwfmJXbdgQLFlHyfA5Kg+pUsJXzqumkIvEr0DXPvptAGqkdF
# LKwo4BTlEgnvzeTfXukzX8vQtTALfVJuTUgRU7zoP/RFWt3WagahZ6UloI0FC8Xl
# BQDVDX5JeMEsx7jgJDdEnK44Y8gHuEWRDq+SG9Xo0GIOjiuTWD5uv3vlEmIAyR/7
# rSFvcLnwAqMdqcy/iqQPMlDOcd0AbniP8ia1BQEUnfZT3UxyK9rLB/SRiKPyHDlg
# 8oWwXyiv3+bGB6dmdM61ur6nUtfDf51lPcKhK4Vo83pOE1/niWlVnEHQV9NJ5/Db
# USqW2RqTUa2O2KuvsyRGMEgjGJA12/SqrRqlvE2fiN5ZmZVtqSPWaIasx7a0GB+f
# dTw+geRn6Mo2S6+/bZEwS/0IJ5gcKGinNbfyQ1xrvWXPtXzKOfjkh75iRuXourGV
# PRqkmz5UYz+R5ybMJWj+mfcGqz2hXV8iZnCZDBrrnZivnErCMh5Flfg8496pT0ph
# jUTH2GChHIvE4SDSk2hwWP/uHB9gEs8p/9Pe/mt9AgMBAAGjggFJMIIBRTAdBgNV
# HQ4EFgQU6HPSBd0OfEX3uNWsdkSraUGe3dswHwYDVR0jBBgwFoAUn6cVXQBeYl2D
# 9OXSZacbUzUZ6XIwXwYDVR0fBFgwVjBUoFKgUIZOaHR0cDovL3d3dy5taWNyb3Nv
# ZnQuY29tL3BraW9wcy9jcmwvTWljcm9zb2Z0JTIwVGltZS1TdGFtcCUyMFBDQSUy
# MDIwMTAoMSkuY3JsMGwGCCsGAQUFBwEBBGAwXjBcBggrBgEFBQcwAoZQaHR0cDov
# L3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9jZXJ0cy9NaWNyb3NvZnQlMjBUaW1l
# LVN0YW1wJTIwUENBJTIwMjAxMCgxKS5jcnQwDAYDVR0TAQH/BAIwADAWBgNVHSUB
# Af8EDDAKBggrBgEFBQcDCDAOBgNVHQ8BAf8EBAMCB4AwDQYJKoZIhvcNAQELBQAD
# ggIBANnrb8Ewr8eX/H1sKt3rnwTDx4AqgHbkMNQo+kUGwCINXS3y1GUcdqsK/R1g
# 6Tf7tNx1q0NpKk1JTupUJfHdExKtkuhHA+82lT7yISp/Y74dqJ03RCT4Q+8ooQXT
# MzxiewfErVLt8WefebncST0i6ypKv87pCYkxM24bbqbM/V+M5VBppCUs7R+cETiz
# /zEA1AbZL/viXtHmryA0CGd+Pt9c+adsYfm7qe5UMnS0f/YJmEEMkEqGXCzyLK+d
# h+UsFi0d4lkdcE+Zq5JNjIHesX1wztGVAtvX0DYDZdN2WZ1kk+hOMblUV/L8n1YW
# zhP/5XQnYl03AfXErn+1Eatylifzd3ChJ1xuGG76YbWgiRXnDvCiwDqvUJevVRY1
# qy4y4vlVKaShtbdfgPyGeeJ/YcSBONOc0DNTWbjMbL50qeIEC0lHSpL2rRYNVu3h
# sHzG8n5u5CQajPwx9PzpsZIeFTNHyVF6kujI4Vo9NvO/zF8Ot44IMj4M7UX9Za4Q
# wGf5B71x57OjaX53gxT4vzoHvEBXF9qCmHRgXBLbRomJfDn60alzv7dpCVQIuQ06
# 2nyIZKnsXxzuKFb0TjXWw6OFpG1bsjXpOo5DMHkysribxHor4Yz5dZjVyHANyKo0
# bSrAlVeihcaG5F74SZT8FtyHAW6IgLc5w/3D+R1obDhKZ21WMIIHcTCCBVmgAwIB
# AgITMwAAABXF52ueAptJmQAAAAAAFTANBgkqhkiG9w0BAQsFADCBiDELMAkGA1UE
# BhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAc
# BgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMpTWljcm9zb2Z0
# IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5IDIwMTAwHhcNMjEwOTMwMTgyMjI1
# WhcNMzAwOTMwMTgzMjI1WjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGlu
# Z3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
# cmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDCC
# AiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAOThpkzntHIhC3miy9ckeb0O
# 1YLT/e6cBwfSqWxOdcjKNVf2AX9sSuDivbk+F2Az/1xPx2b3lVNxWuJ+Slr+uDZn
# hUYjDLWNE893MsAQGOhgfWpSg0S3po5GawcU88V29YZQ3MFEyHFcUTE3oAo4bo3t
# 1w/YJlN8OWECesSq/XJprx2rrPY2vjUmZNqYO7oaezOtgFt+jBAcnVL+tuhiJdxq
# D89d9P6OU8/W7IVWTe/dvI2k45GPsjksUZzpcGkNyjYtcI4xyDUoveO0hyTD4MmP
# frVUj9z6BVWYbWg7mka97aSueik3rMvrg0XnRm7KMtXAhjBcTyziYrLNueKNiOSW
# rAFKu75xqRdbZ2De+JKRHh09/SDPc31BmkZ1zcRfNN0Sidb9pSB9fvzZnkXftnIv
# 231fgLrbqn427DZM9ituqBJR6L8FA6PRc6ZNN3SUHDSCD/AQ8rdHGO2n6Jl8P0zb
# r17C89XYcz1DTsEzOUyOArxCaC4Q6oRRRuLRvWoYWmEBc8pnol7XKHYC4jMYcten
# IPDC+hIK12NvDMk2ZItboKaDIV1fMHSRlJTYuVD5C4lh8zYGNRiER9vcG9H9stQc
# xWv2XFJRXRLbJbqvUAV6bMURHXLvjflSxIUXk8A8FdsaN8cIFRg/eKtFtvUeh17a
# j54WcmnGrnu3tz5q4i6tAgMBAAGjggHdMIIB2TASBgkrBgEEAYI3FQEEBQIDAQAB
# MCMGCSsGAQQBgjcVAgQWBBQqp1L+ZMSavoKRPEY1Kc8Q/y8E7jAdBgNVHQ4EFgQU
# n6cVXQBeYl2D9OXSZacbUzUZ6XIwXAYDVR0gBFUwUzBRBgwrBgEEAYI3TIN9AQEw
# QTA/BggrBgEFBQcCARYzaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9E
# b2NzL1JlcG9zaXRvcnkuaHRtMBMGA1UdJQQMMAoGCCsGAQUFBwMIMBkGCSsGAQQB
# gjcUAgQMHgoAUwB1AGIAQwBBMAsGA1UdDwQEAwIBhjAPBgNVHRMBAf8EBTADAQH/
# MB8GA1UdIwQYMBaAFNX2VsuP6KJcYmjRPZSQW9fOmhjEMFYGA1UdHwRPME0wS6BJ
# oEeGRWh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01p
# Y1Jvb0NlckF1dF8yMDEwLTA2LTIzLmNybDBaBggrBgEFBQcBAQROMEwwSgYIKwYB
# BQUHMAKGPmh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2kvY2VydHMvTWljUm9v
# Q2VyQXV0XzIwMTAtMDYtMjMuY3J0MA0GCSqGSIb3DQEBCwUAA4ICAQCdVX38Kq3h
# LB9nATEkW+Geckv8qW/qXBS2Pk5HZHixBpOXPTEztTnXwnE2P9pkbHzQdTltuw8x
# 5MKP+2zRoZQYIu7pZmc6U03dmLq2HnjYNi6cqYJWAAOwBb6J6Gngugnue99qb74p
# y27YP0h1AdkY3m2CDPVtI1TkeFN1JFe53Z/zjj3G82jfZfakVqr3lbYoVSfQJL1A
# oL8ZthISEV09J+BAljis9/kpicO8F7BUhUKz/AyeixmJ5/ALaoHCgRlCGVJ1ijbC
# HcNhcy4sa3tuPywJeBTpkbKpW99Jo3QMvOyRgNI95ko+ZjtPu4b6MhrZlvSP9pEB
# 9s7GdP32THJvEKt1MMU0sHrYUP4KWN1APMdUbZ1jdEgssU5HLcEUBHG/ZPkkvnNt
# yo4JvbMBV0lUZNlz138eW0QBjloZkWsNn6Qo3GcZKCS6OEuabvshVGtqRRFHqfG3
# rsjoiV5PndLQTHa1V1QJsWkBRH58oWFsc/4Ku+xBZj1p/cvBQUl+fpO+y/g75LcV
# v7TOPqUxUYS8vwLBgqJ7Fx0ViY1w/ue10CgaiQuPNtq6TPmb/wrpNPgkNWcr4A24
# 5oyZ1uEi6vAnQj0llOZ0dFtq0Z4+7X6gMTN9vMvpe784cETRkPHIqzqKOghif9lw
# Y1NNje6CbaUFEMFxBmoQtB1VM1izoXBm8qGCAtYwggI/AgEBMIIBAKGB2KSB1TCB
# 0jELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1Jl
# ZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEtMCsGA1UECxMk
# TWljcm9zb2Z0IElyZWxhbmQgT3BlcmF0aW9ucyBMaW1pdGVkMSYwJAYDVQQLEx1U
# aGFsZXMgVFNTIEVTTjpEMDgyLTRCRkQtRUVCQTElMCMGA1UEAxMcTWljcm9zb2Z0
# IFRpbWUtU3RhbXAgU2VydmljZaIjCgEBMAcGBSsOAwIaAxUAHDn/cz+3yRkIUCJf
# SbL3djnQEqaggYMwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGlu
# Z3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
# cmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDAN
# BgkqhkiG9w0BAQUFAAIFAOpcDgYwIhgPMjAyNDA4MDYxMDUwNDZaGA8yMDI0MDgw
# NzEwNTA0NlowdjA8BgorBgEEAYRZCgQBMS4wLDAKAgUA6lwOBgIBADAJAgEAAgE0
# AgH/MAcCAQACAhHYMAoCBQDqXV+GAgEAMDYGCisGAQQBhFkKBAIxKDAmMAwGCisG
# AQQBhFkKAwKgCjAIAgEAAgMHoSChCjAIAgEAAgMBhqAwDQYJKoZIhvcNAQEFBQAD
# gYEAe0BNo9CYSiTRRJMk7QRF8DIFxhirehltnQ0tOZzhS/ifNLDvAZIBRxCrYKtu
# dfA5sKE0ciFvwgRJiJZ6jU5vJRnX1Uy7BAMta4YvO+wqhyDQMvLpClco+JsrKW7G
# 61z0oSoEcoiCid6lkdWLGq4Og26+BEkgFTkBp6XJRHLHKMYxggQNMIIECQIBATCB
# kzB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
# UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQD
# Ex1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMAITMwAAAdzB4IzCX1hejgAB
# AAAB3DANBglghkgBZQMEAgEFAKCCAUowGgYJKoZIhvcNAQkDMQ0GCyqGSIb3DQEJ
# EAEEMC8GCSqGSIb3DQEJBDEiBCCYNwcUCMZ5ch1QooHApNPjhc54klelgp5GeY7v
# w458FzCB+gYLKoZIhvcNAQkQAi8xgeowgecwgeQwgb0EIFOnF4pq2UQ/jLypnOO5
# YvQ67QirEQsOFfZMvKXEgg03MIGYMIGApH4wfDELMAkGA1UEBhMCVVMxEzARBgNV
# BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jv
# c29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAg
# UENBIDIwMTACEzMAAAHcweCMwl9YXo4AAQAAAdwwIgQgUIEF8wwWol8k+EXKox7E
# AcO20YJnOMuuYZrHpgMYl/UwDQYJKoZIhvcNAQELBQAEggIAVZGR5FBTS/o8mNmO
# xxwK4IdRDwt9ICoDnj9JiNwjZC80R+Z1Fc5CTEXqWp3I6mtOpBCDxaYxIlEjRQxA
# yIHGFhaPns2ctEVGtXgyGC4UpusxOsKkMEFtAYZSAOY7k9wpekGzKbSgQtjaO0I2
# U5+hu06y4oworHI9tro4OQewQt4b/+5BTgqrmIfPcaV5P+cCN0NrnTCg0I2czdR/
# TO17SRv91VSPeC3FNFKqLIslPkG5FMUlwj5BF8I22NGsQfP6hZg0GMN1sQ89fk4f
# 5Q67yaTfDBroC6pu7Kp7mRYue3Y8/u6nUMKSDtzQTDTXy9RIcpUrhGYWeiy8KUea
# Jrh5fjabTQ0+AnjtFgFe5bCeGIQYBOk6x3yRj/Hw1SKz1RAbxXnYJ91IxOg0VGRt
# dwtT99m2vk+ILwqy5ZSN9PYbohf+7GjowIMVpp9u5NS4MxBnb/oloqezO6fIF065
# HRhyinFqml4l0MxxHvX9TG2DqvC6UzvR5op/OlRn5LE7vab1hhXT4mXj2SwbSnQ3
# b832jIjWSDATvkkTVVO/4MbGXGNBCBmv14nW2KNtCUA6x7x2NWrjX9aTNxzEvzmp
# IrdxEfwswPQTWu8VewFs9lNCskf9a9wHjyLa5lyoLpeUgarGPmqBooBuruAxXrUy
# kayxJ2tzGbLTtq6g3ubORD5DIfs=
# SIG # End signature block
