#
# Module manifest for module 'AdmPwd.PS'
#
# Generated by: Jiri Formacek - MSFT
#
# Generated on: 3.12.2012
#


@{

# Script module or binary module file associated with this manifest
RootModule = '.\AdmPwd.PS.dll'

# Version number of this module.
ModuleVersion = '6.3.1.0'

# ID used to uniquely identify this module
GUID = '11869C0C-4440-4560-AA89-9EEF0C0224FA'

# Author of this module
Author = 'Jiri Formacek - GreyCorbel Solutions'

# Company or vendor of this module
CompanyName = 'Microsoft | Services'

# Copyright statement for this module
Copyright = ''

# Description of the functionality provided by this module
Description = 'Provides cmdlets for configuration and usage of Local admin password management solution'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = ''

# Name of the Windows PowerShell host required by this module
PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
PowerShellHostVersion = ''

# Minimum version of the .NET Framework required by this module
DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module
CLRVersion = ''

# Processor architecture (None, X86, Amd64, IA64) required by this module
ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to importing this module
ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = @('AdmPwd.PS.format.ps1xml')

# Modules to import as nested modules of the module specified in ModuleToProcess
NestedModules = @()

# Functions to export from this module
FunctionsToExport = @()

# Cmdlets to export from this module
CmdletsToExport = @('Update-AdmPwdADSchema',
    'Get-AdmPwdPassword',
    'Reset-AdmPwdPassword',
    'Set-AdmPwdComputerSelfPermission',
    'Find-AdmPwdExtendedRights',
    'Set-AdmPwdAuditing',
    'Set-AdmPwdReadPasswordPermission',
    'Set-AdmPwdResetPasswordPermission'
)

# Variables to export from this module
VariablesToExport = @()

# Aliases to export from this module
AliasesToExport = @()

# List of all modules packaged with this module
ModuleList = @()

# List of all files packaged with this module
FileList = @('AdmPwd.PS.dll','AdmPwd.Utils.dll','AdmPwd.PS.format.ps1xml')

# Private data to pass to the module specified in ModuleToProcess
PrivateData = ''

}

