$files = Get-ChildItem -Path "c:\Users\admin\Desktop\Tienda de Ropa\TiendaRopaCano\TiendaRopaCano.Presentation" -Filter "*.xaml" -Recurse

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    
    # Backgrounds
    $content = $content -replace '#13141F', '#F4F6F8'
    $content = $content -replace '#1A1C2E', '#FFFFFF'
    $content = $content -replace '#1E2035', '#F8F9FA'
    $content = $content -replace '#20223A', '#E9ECEF'
    $content = $content -replace '#14162180', '#F3F4F6'
    $content = $content -replace '#16182480', '#FFFFFF'
    
    # Borders and UI Elements
    $content = $content -replace '#252840', '#E5E7EB'
    $content = $content -replace '#3D4059', '#E2E8F0'
    
    # Text Colors
    $content = $content -replace '#5A5F8A', '#6B7280'
    $content = $content -replace '#8B8FA3', '#4B5563'
    $content = $content -replace '#E0E0E0', '#111827'
    
    # Accents
    $content = $content -replace '#7C83FD', '#0B2447'
    $content = $content -replace '#A78BFA', '#1E3A8A'
    $content = $content -replace '#4ADE80', '#15803D'
    $content = $content -replace '#EF4444', '#DC2626'
    
    Set-Content -Path $file.FullName -Value $content -Encoding UTF8
}
