
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

# 📦 Guía Rápida de Instalación y Uso

Sigue estos 3 simples pasos para clonar, compilar y ejecutar el sistema en tu entorno local:

### 1. Clonar el repositorio
Abre una terminal (PowerShell, CMD o Git Bash) y ejecuta:
```bash
git clone https://github.com/luisFranco-19/TiendaRopaCano-POS.git
cd TiendaRopaCano-POS
```

### 2. Abrir en Visual Studio
* Haz doble clic en el archivo **`TiendaRopaCano.slnx`** para abrir la solución en Visual Studio (se recomienda Visual Studio 2022 o superior con soporte para .NET 10).

### 3. Ejecutar el proyecto (F5)
* Haz clic derecho sobre el proyecto **`TiendaRopaCano.Presentation`** en el Explorador de soluciones y selecciona **Establecer como proyecto de inicio**.
* Presiona **F5** (o haz clic en el botón de **Iniciar**). Visual Studio se encargará de restaurar todas las dependencias NuGet y compilar el proyecto automáticamente.

---

> [!TIP]
> **Primer Inicio (Base de datos limpia):**
> Al iniciar la aplicación por primera vez, al no detectar usuarios ni datos cargados, se abrirá automáticamente el formulario de **Registro**. El primer usuario creado tendrá asignado obligatoriamente el rol de **Administrador**. Una vez registrado, podrás acceder a todas las funciones y comenzar a crear categorías, productos y realizar ventas.

## 🛠️ Alternativa por consola (CLI de .NET)
Si prefieres no usar la interfaz de Visual Studio para compilar, puedes ejecutar el sistema desde tu terminal de comandos estando en la carpeta raíz del proyecto:
```bash
dotnet run --project TiendaRopaCano.Presentation/TiendaRopaCano.Presentation.csproj
```

---

## ⚙️ Guía de Instalación de .NET 10.0 SDK

Para poder compilar y ejecutar este proyecto sin inconvenientes, necesitas tener instalado el SDK de .NET 10. Sigue estos sencillos pasos:

1. **Descargar el Instalador:**
   * Entra al sitio oficial de descargas de Microsoft: [Descargar .NET 10.0](https://dotnet.microsoft.com/download/dotnet/10.0).
   * En la tabla de **SDK**, busca la fila de **Windows** y descarga el instalador **Installer (x64)**.
2. **Ejecutar la Instalación:**
   * Abre el archivo ejecutable recién descargado (ej: `dotnet-sdk-10.0.x-win-x64.exe`).
   * Sigue los pasos en pantalla del asistente de Microsoft (haz clic en *Instalar* y luego en *Cerrar* cuando termine).
3. **Verificar la Instalación:**
   * Abre una nueva terminal de comandos (CMD o PowerShell) y escribe:
     ```bash
     dotnet --version
     ```
   * Te debe devolver una versión que inicie con `10.0.x` (por ejemplo, `10.0.100`).

> [!IMPORTANT]
> **Compatibilidad con Visual Studio:**
> Para usar .NET 10, es necesario contar con **Visual Studio 2022 (versión 17.12 o superior)**. Si al abrir el proyecto en Visual Studio te salen errores de compatibilidad, abre el programa **Visual Studio Installer** en tu equipo y haz clic en **Actualizar** para poner al día tu entorno de desarrollo.



