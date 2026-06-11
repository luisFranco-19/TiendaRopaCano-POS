$replacements = [ordered]@{
    "TiendaRopaCano.Domain.Entities" = "TiendaRopaCano.Dominio.Entidades"
    "TiendaRopaCano.Data.Repositories" = "TiendaRopaCano.Datos.Repositorios"
    "TiendaRopaCano.Data.Context" = "TiendaRopaCano.Datos.Contexto"
    "TiendaRopaCano.Application.Services" = "TiendaRopaCano.Aplicacion.Servicios"
    "TiendaRopaCano.Application.Helpers" = "TiendaRopaCano.Aplicacion.Auxiliares"
    "TiendaRopaCano.Presentation.Helpers" = "TiendaRopaCano.Presentacion.Auxiliares"

    "IAlertaRepository" = "IAlertaRepositorio"
    "AlertaRepository" = "AlertaRepositorio"
    "ICategoriaRepository" = "ICategoriaRepositorio"
    "CategoriaRepository" = "CategoriaRepositorio"
    "ILogErrorRepository" = "ILogErrorRepositorio"
    "LogErrorRepository" = "LogErrorRepositorio"
    "IProductoRepository" = "IProductoRepositorio"
    "ProductoRepository" = "ProductoRepositorio"
    "IReporteRepository" = "IReporteRepositorio"
    "ReporteRepository" = "ReporteRepositorio"
    "IRolRepository" = "IRolRepositorio"
    "RolRepository" = "RolRepositorio"
    "IUsuarioRepository" = "IUsuarioRepositorio"
    "UsuarioRepository" = "UsuarioRepositorio"
    "IVentaRepository" = "IVentaRepositorio"
    "VentaRepository" = "VentaRepositorio"

    "ICategoriaService" = "ICategoriaServicio"
    "CategoriaService" = "CategoriaServicio"
    "IProductoService" = "IProductoServicio"
    "ProductoService" = "ProductoServicio"
    "IUsuarioService" = "IUsuarioServicio"
    "UsuarioService" = "UsuarioServicio"
    "IVentaService" = "IVentaServicio"
    "VentaService" = "VentaServicio"
    "IRolService" = "IRolServicio"
    "RolService" = "RolServicio"
    "IPdfService" = "IPdfServicio"
    "PdfService" = "PdfServicio"
    "IReporteService" = "IReporteServicio"
    "ReporteService" = "ReporteServicio"

    "DatabaseConfig" = "ConfiguracionBaseDatos"
    "SessionManager" = "GestorSesion"
    "PasswordHasher" = "EncriptadorContrasena"
}

Get-ChildItem -Recurse -Filter *.cs | Where-Object { $_.FullName -notmatch "\\(bin|obj)\\" } | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $changed = $false
    foreach ($key in $replacements.Keys) {
        $escapedKey = [regex]::Escape($key)
        if ($content -match "\b$escapedKey\b" -or $content -match "$escapedKey") {
            $content = $content -replace "\b$escapedKey\b", $replacements[$key]
            $changed = $true
        }
    }
    if ($changed) {
        Set-Content $_.FullName $content -NoNewline
        Write-Host "Updated contents in: $($_.FullName)"
    }
}
