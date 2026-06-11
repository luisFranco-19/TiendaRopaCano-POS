<<<<<<< HEAD
# TiendaCano-POS 👗👔

Sistema integral de escritorio para la gestión comercial y administración de inventario diseñado para tiendas de ropa.

## 🚀 Características Principales
- **Módulo de Ventas (POS):** Procesamiento ágil de ventas, cálculo automático y facturación.
- **Control de Inventario:** Gestión de productos, categorías, alertas de stock mínimo y movimientos de mercancía.
- **Reportes y Analíticas:** Gráficos visuales de ventas y ganancias del mes en curso y exportación de reportes a PDF y CSV.
- **Seguridad y Acceso:** Control de sesiones de usuario diferenciado por roles (Administrador y Vendedor).

## 🛠️ Tecnologías Utilizadas
- **Lenguaje:** C# / .NET
- **Interfaz Gráfica:** WPF (Windows Presentation Foundation) con arquitectura MVVM
- **Componentes:** CommunityToolkit.Mvvm, LiveCharts (gráficos dinámicos) y librerías de generación de PDF.
- **Base de Datos:** SQLite / SQL Server local
=======
# TiendaRopaCano

Pequeña aplicación WPF para la gestión de una tienda de ropa.

## Manual de instalación

Requisitos previos

- Windows 10 u 11.
- SDK de .NET 10 (instala desde https://dotnet.microsoft.com/ cuando esté disponible) o versión compatible.
- Visual Studio 2022/2023 con la carga de trabajo de `.NET desktop development` (recomendado) o el CLI de `dotnet`.

Clonar el repositorio

```powershell
git clone https://github.com/luisFranco-19/TiendaRopaCano-POS.git
cd "TiendaRopaCano-POS"
```

Restaurar paquetes y compilar

```powershell
dotnet restore TiendaRopaCano.slnx
dotnet build TiendaRopaCano.slnx -c Debug
```

# TiendaCano-POS 👗👔

Sistema integral de escritorio para la gestión comercial y administración de inventario diseñado para tiendas de ropa.

## 🚀 Características Principales
- **Módulo de Ventas (POS):** Procesamiento ágil de ventas, cálculo automático y facturación.
- **Control de Inventario:** Gestión de productos, categorías, alertas de stock mínimo y movimientos de mercancía.
- **Reportes y Analíticas:** Gráficos visuales de ventas y ganancias del mes en curso y exportación de reportes a PDF y CSV.
- **Seguridad y Acceso:** Control de sesiones de usuario diferenciado por roles (Administrador y Vendedor).

## 🛠️ Tecnologías Utilizadas
- **Lenguaje:** C# / .NET
- **Interfaz Gráfica:** WPF (Windows Presentation Foundation) con arquitectura MVVM
- **Componentes:** CommunityToolkit.Mvvm, LiveCharts (gráficos dinámicos) y librerías de generación de PDF.
- **Base de Datos:** SQLite / SQL Server local

---

# Manual de instalación

Requisitos previos

- Windows 10 u 11.
- SDK de .NET 10 (instala desde https://dotnet.microsoft.com/ cuando esté disponible) o versión compatible.
- Visual Studio 2022/2023 con la carga de trabajo de `.NET desktop development` (recomendado) o el CLI de `dotnet`.

Clonar el repositorio

```powershell
git clone https://github.com/luisFranco-19/TiendaRopaCano-POS.git
cd "TiendaRopaCano-POS"
```

Restaurar paquetes y compilar

```powershell
dotnet restore TiendaRopaCano.slnx
dotnet build TiendaRopaCano.slnx -c Debug
```

Ejecutar la aplicación

- Usando Visual Studio: abre `TiendaRopaCano.slnx` y establece el proyecto `TiendaRopaCano.Presentation` como proyecto de inicio, luego ejecuta (F5).
- Usando CLI (puede abrir una ventana de la aplicación si el entorno soporta WPF):

```powershell
dotnet run --project TiendaRopaCano.Presentation\TiendaRopaCano.Presentation.csproj
```

Notas

- El proyecto TargetFramework es `net10.0-windows`; asegura tener el SDK adecuado.
- Si falla el `dotnet run` para WPF, usa Visual Studio para ejecutar la aplicación.
- Si usas autenticación o servicios externos (por ejemplo, generación de PDF), revisa las configuraciones en los archivos de configuración o en las clases de servicio.

Soporte

Si necesitas que yo haga el push del repositorio remoto desde esta copia local, autoriza las credenciales en tu entorno de Git (o proporciona un token) y ejecutaré los pasos para inicializar git, commitear y hacer push.
