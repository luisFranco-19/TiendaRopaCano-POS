
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

---

## 🛠️ Alternativa por consola (CLI de .NET)
Si prefieres no usar la interfaz de Visual Studio para compilar, puedes ejecutar el sistema desde tu terminal de comandos estando en la carpeta raíz del proyecto:
```bash
dotnet run --project TiendaRopaCano.Presentation/TiendaRopaCano.Presentation.csproj
```

*Requisitos: Tener instalado el [SDK de .NET 10.0](https://dotnet.microsoft.com/download/dotnet/10.0) para Windows.*


