$renames = @{
    "TiendaRopaCano.Data\Repositorios\AlertaRepository.cs" = "AlertaRepositorio.cs"
    "TiendaRopaCano.Data\Repositorios\IAlertaRepository.cs" = "IAlertaRepositorio.cs"
    "TiendaRopaCano.Data\Repositorios\ILogErrorRepository.cs" = "ILogErrorRepositorio.cs"
    "TiendaRopaCano.Data\Repositorios\LogErrorRepository.cs" = "LogErrorRepositorio.cs"
    "TiendaRopaCano.Data\Repositorios\IReporteRepository.cs" = "IReporteRepositorio.cs"
    "TiendaRopaCano.Data\Repositorios\ReporteRepository.cs" = "ReporteRepositorio.cs"
    "TiendaRopaCano.Data\Repositorios\IRolRepository.cs" = "IRolRepositorio.cs"
    "TiendaRopaCano.Data\Repositorios\RolRepository.cs" = "RolRepositorio.cs"

    "TiendaRopaCano.Application\Services\CategoriaService.cs" = "CategoriaServicio.cs"
    "TiendaRopaCano.Application\Services\ICategoriaService.cs" = "ICategoriaServicio.cs"
    "TiendaRopaCano.Application\Services\IPdfService.cs" = "IPdfServicio.cs"
    "TiendaRopaCano.Application\Services\IProductoService.cs" = "IProductoServicio.cs"
    "TiendaRopaCano.Application\Services\IReporteService.cs" = "IReporteServicio.cs"
    "TiendaRopaCano.Application\Services\IRolService.cs" = "IRolServicio.cs"
    "TiendaRopaCano.Application\Services\IUsuarioService.cs" = "IUsuarioServicio.cs"
    "TiendaRopaCano.Application\Services\IVentaService.cs" = "IVentaServicio.cs"
    "TiendaRopaCano.Application\Services\PdfService.cs" = "PdfServicio.cs"
    "TiendaRopaCano.Application\Services\ProductoService.cs" = "ProductoServicio.cs"
    "TiendaRopaCano.Application\Services\ReporteService.cs" = "ReporteServicio.cs"
    "TiendaRopaCano.Application\Services\RolService.cs" = "RolServicio.cs"
    "TiendaRopaCano.Application\Services\UsuarioService.cs" = "UsuarioServicio.cs"
    "TiendaRopaCano.Application\Services\VentaService.cs" = "VentaServicio.cs"

    "TiendaRopaCano.Application\Helpers\PasswordHasher.cs" = "EncriptadorContrasena.cs"
    "TiendaRopaCano.Presentation\Helpers\SessionManager.cs" = "GestorSesion.cs"
}

foreach ($key in $renames.Keys) {
    if (Test-Path $key) {
        Rename-Item -Path $key -NewName $renames[$key]
        Write-Host "Renamed $key to $($renames[$key])"
    }
}
