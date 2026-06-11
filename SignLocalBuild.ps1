# Script para crear certificado auto-firmado y firmar localmente los binarios compilados
# Debe ejecutarse en una consola de PowerShell con privilegios de Administrador.

$certName = "CN=TiendaRopaCanoLocalDev"
$dllPath = "C:\Users\admin\Desktop\Tienda de Ropa\TiendaRopaCano\TiendaRopaCano.Presentation\bin\Debug\net10.0-windows\TiendaRopaCano.Presentation.dll"
$exePath = "C:\Users\admin\Desktop\Tienda de Ropa\TiendaRopaCano\TiendaRopaCano.Presentation\bin\Debug\net10.0-windows\TiendaRopaCano.Presentation.exe"

# 1. Verificar si se ejecuta como Administrador
$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Warning "Por favor, ejecuta este script en una ventana de PowerShell como ADMINISTRADOR."
    Exit
}

# 2. Buscar o crear el certificado
$cert = Get-ChildItem -Path Cert:\LocalMachine\My | Where-Object { $_.Subject -like "*$certName*" } | Select-Object -First 1

if ($cert -eq $null) {
    Write-Host "Creando certificado de firma de código local..." -ForegroundColor Cyan
    $cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject $certName -CertStoreLocation "Cert:\LocalMachine\My" -NotAfter (Get-Date).AddYears(5)
    
    # Importar en 'Entidades de certificación de raíz de confianza'
    Write-Host "Instalando certificado en Raíz de Confianza..." -ForegroundColor Cyan
    $rootStore = New-Object System.Security.Cryptography.X509Certificates.X509Store("Root", "LocalMachine")
    $rootStore.Open("ReadWrite")
    $rootStore.Add($cert)
    $rootStore.Close()
    
    # Importar en 'Publicadores de confianza'
    Write-Host "Instalando certificado en Publicadores de Confianza..." -ForegroundColor Cyan
    $pubStore = New-Object System.Security.Cryptography.X509Certificates.X509Store("TrustedPublisher", "LocalMachine")
    $pubStore.Open("ReadWrite")
    $pubStore.Add($cert)
    $pubStore.Close()
    
    Write-Host "Certificado creado e instalado exitosamente.`n" -ForegroundColor Green
} else {
    Write-Host "Certificado local existente encontrado." -ForegroundColor Green
}

# 3. Firmar los archivos binarios
if (Test-Path $dllPath) {
    Write-Host "Firmando DLL: $dllPath" -ForegroundColor Cyan
    Set-AuthenticodeSignature -FilePath $dllPath -Certificate $cert
} else {
    Write-Warning "No se encontró el DLL en la ruta especificada. Compila el proyecto en Visual Studio primero."
}

if (Test-Path $exePath) {
    Write-Host "Firmando EXE: $exePath" -ForegroundColor Cyan
    Set-AuthenticodeSignature -FilePath $exePath -Certificate $cert
}
