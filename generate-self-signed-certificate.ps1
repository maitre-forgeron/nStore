# Source: https://stackoverflow.com/a/62060315
# Generate self-signed certificate to be used by IdentityServer.
# When using localhost - API cannot see the IdentityServer from within the docker-compose'd network.
# You have to run this script as Administrator (open Powershell by right click -> Run as Administrator).

$ErrorActionPreference = "Stop"

$rootCN = "storeCert"
$identityServerCNs = "nstore.identity", "localhost", "host.docker.internal"
$gatewayApiCNs = "nstore.gateway", "localhost", "host.docker.internal"
$cartingApiCNs = "cartingservice.api", "localhost", "host.docker.internal"
$catalogApiCNs = "catalogservice.api", "localhost", "host.docker.internal"
$storeWebCNs = "nstore.web", "localhost", "host.docker.internal"

function Get-Certificate($CN) {
    return Get-ChildItem -Path Cert:\CurrentUser\My -Recurse | Where-Object { $_.Subject -eq ("CN={0}" -f $CN) }
}

$alreadyExistingCertsRoot = Get-Certificate $rootCN
$alreadyExistingCertsIdentityServer = Get-Certificate $identityServerCNs[0]
$alreadyExistingCertsGatewayApi = Get-Certificate $gatewayApiCNs[0]
$alreadyExistingCertsCartingApi = Get-Certificate $cartingApiCNs[0]
$alreadyExistingCertsCatalogApi = Get-Certificate $catalogApiCNs[0]
$alreadyExistingCertsStoreWeb = Get-Certificate $storeWebCNs[0]

if ($alreadyExistingCertsRoot.Count -eq 1) {
    Write-Output "Skipping creating Root CA certificate as it already exists."
    $testRootCA = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsRoot[0]
} else {
    $testRootCA = New-SelfSignedCertificate -Subject $rootCN -KeyUsageProperty Sign -KeyUsage CertSign -CertStoreLocation Cert:\CurrentUser\My
}

if ($alreadyExistingCertsIdentityServer.Count -eq 1) {
    Write-Output "Skipping creating Identity Server certificate as it already exists."
    $identityServerCert = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsIdentityServer[0]
} else {
    $identityServerCert = New-SelfSignedCertificate -DnsName $identityServerCNs -Signer $testRootCA -CertStoreLocation Cert:\CurrentUser\My
}

if ($alreadyExistingCertsGatewayApi.Count -eq 1) {
    Write-Output "Skipping creating API certificate as it already exists."
    $gatewayApiCert = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsGatewayApi[0]
} else {
    $gatewayApiCert = New-SelfSignedCertificate -DnsName $gatewayApiCNs -Signer $testRootCA -CertStoreLocation Cert:\CurrentUser\My
}

if ($alreadyExistingCertsCartingApi.Count -eq 1) {
    Write-Output "Skipping creating API certificate as it already exists."
    $cartingApiCert = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsCartingApi[0]
} else {
    $cartingApiCert = New-SelfSignedCertificate -DnsName $cartingApiCNs -Signer $testRootCA -CertStoreLocation Cert:\CurrentUser\My
}

if ($alreadyExistingCertsCatalogApi.Count -eq 1) {
    Write-Output "Skipping creating API certificate as it already exists."
    $catalogApiCert = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsCatalogApi[0]
} else {
    $catalogApiCert = New-SelfSignedCertificate -DnsName $catalogApiCNs -Signer $testRootCA -CertStoreLocation Cert:\CurrentUser\My
}

if ($alreadyExistingCertsStoreWeb.Count -eq 1) {
    Write-Output "Skipping creating API certificate as it already exists."
    $storeWebCert = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsStoreWeb[0]
} else {
    $storeWebCert = New-SelfSignedCertificate -DnsName $storeWebCNs -Signer $testRootCA -CertStoreLocation Cert:\CurrentUser\My
}

$password = ConvertTo-SecureString -String "password" -Force -AsPlainText

$rootCertPathPfx = "./certs"
$identityServerCertPath = "./Services/Identity/NStore.Identity/certs"
$gatewayApiCertPath = "./Gateways/NStore.Gateway/certs"
$cartingApiCertPath = "./Services/Carting/CartingService.Api/certs"
$catalogApiCertPath = "./Services/Catalog/CatalogService.Api/certs"
$storeWebCertPath = "./Clients/NStore.Web/certs"

[System.IO.Directory]::CreateDirectory($rootCertPathPfx) | Out-Null
[System.IO.Directory]::CreateDirectory($identityServerCertPath) | Out-Null
[System.IO.Directory]::CreateDirectory($gatewayApiCertPath) | Out-Null
[System.IO.Directory]::CreateDirectory($cartingApiCertPath) | Out-Null
[System.IO.Directory]::CreateDirectory($catalogApiCertPath) | Out-Null
[System.IO.Directory]::CreateDirectory($storeWebCertPath) | Out-Null

Export-PfxCertificate -Cert $testRootCA -FilePath "$rootCertPathPfx/aspnetapp-root-cert.pfx" -Password $password | Out-Null
Export-PfxCertificate -Cert $identityServerCert -FilePath "$identityServerCertPath/aspnetapp-identity-server.pfx" -Password $password | Out-Null
Export-PfxCertificate -Cert $gatewayApiCert -FilePath "$gatewayApiCertPath/aspnetapp-gwy-api.pfx" -Password $password | Out-Null
Export-PfxCertificate -Cert $cartingApiCert -FilePath "$cartingApiCertPath/aspnetapp-carting-api.pfx" -Password $password | Out-Null
Export-PfxCertificate -Cert $catalogApiCert -FilePath "$catalogApiCertPath/aspnetapp-catalog-api.pfx" -Password $password | Out-Null
Export-PfxCertificate -Cert $storeWebCert -FilePath "$storeWebCertPath/aspnetapp-store-web.pfx" -Password $password | Out-Null

$rootCertPathCer = "certs/aspnetapp-root-cert.cer"
Export-Certificate -Cert $testRootCA -FilePath $rootCertPathCer -Type CERT | Out-Null

# Re-import the certificate to establish a valid context
$reImportedRootCA = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$reImportedRootCA.Import("$rootCertPathPfx/aspnetapp-root-cert.pfx", "password", [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::PersistKeySet)

$store = New-Object System.Security.Cryptography.X509Certificates.X509Store "Root", "CurrentUser"
$store.Open("ReadWrite")

if ($reImportedRootCA -eq $null) {
    Write-Error "The reImportedRootCA certificate object is null."
} else {
    Write-Output "reImportedRootCA certificate object is valid."

    $rootCertAlreadyTrusted = ($store.Certificates | Where-Object { $_.Subject -eq "CN=$rootCN" }).Count -eq 1

    if ($rootCertAlreadyTrusted -eq $false) {
        Write-Output "Adding the root CA certificate to the trust store."
        try {
            $store.Add($reImportedRootCA)
            Write-Output "The root CA certificate added to the trust store."
        } catch {
            Write-Error "Failed to add the root CA certificate to the trust store: $_"
        }
    } else {
        Write-Output "The root CA certificate is already trusted."
    }
}

$store.Close()



#Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass