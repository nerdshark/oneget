@{
RootModule = 'PSGet.psm1'
ModuleVersion = '0.5'
GUID = '1d73a601-4a6c-43c5-ba3f-619b18bbb404'
Author = 'Microsoft Corporation'
CompanyName = 'Microsoft Corporation'
Copyright = '© Microsoft Corporation. All rights reserved.'
PowerShellVersion = '3.0'
FormatsToProcess = 'PSGet.Format.ps1xml'
FunctionsToExport = @('Install-Module',
                      'Find-Module',
                      'Save-Module',
                      'Update-Module',
                      'Publish-Module', 
                      'Get-InstalledModule',
                      'Uninstall-Module',
                      'Get-PSRepository',
                      'Set-PSRepository',                      
                      'Register-PSRepository',
                      'Unregister-PSRepository')
VariablesToExport = "*"
AliasesToExport = @('inmo',
                    'fimo',
                    'upmo',
                    'pumo')
FileList = @('PSGet.psm1',
             'PSGet.Format.ps1xml',
             'PSGet.Resource.psd1')

# OneGet nest-loads this module in the community version. (-edge)
#RequiredModules = @('PackageManagement')

PrivateData = @{
                "PackageManagementProviders" = 'PSGet.psm1'
                "SupportedPowerShellGetFormatVersions" = @('1.x')
               }
HelpInfoURI = 'http://go.microsoft.com/fwlink/?LinkId=393271'
}

