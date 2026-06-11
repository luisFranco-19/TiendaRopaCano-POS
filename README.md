
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

# 📦 Manual de Instalación y Clonación

## 💾 Clonar el Repositorio con Git

Sigue estos pasos para clonar el repositorio de forma segura utilizando Git desde tu terminal (PowerShell, Git Bash o Símbolo del Sistema):

1. **Abrir la terminal** en la carpeta donde deseas guardar el proyecto.
2. **Ejecutar el comando de clonación**:
   ```powershell
   git clone https://github.com/luisFranco-19/TiendaRopaCano-POS.git
   ```
3. **Acceder a la carpeta del proyecto**:
   ```powershell
   cd TiendaRopaCano-POS
   ```

---

## 🛠️ Restaurar Paquetes y Compilar

Una vez dentro de la carpeta del proyecto, ejecuta los siguientes comandos para restaurar las dependencias e iniciar la compilación:

```powershell
# Restaurar dependencias de NuGet del archivo de solución
dotnet restore TiendaRopaCano.slnx

# Compilar el proyecto en modo Debug
dotnet build TiendaRopaCano.slnx -c Debug
```

---

## 🚀 Ejecutar la Aplicación

Puedes iniciar la aplicación de dos maneras:

### Opción A: Usando Visual Studio (Recomendado)
1. Abre el archivo de solución `TiendaRopaCano.slnx` en Visual Studio.
2. En el Explorador de soluciones, haz clic derecho sobre el proyecto `TiendaRopaCano.Presentation` y selecciona **Establecer como proyecto de inicio** (Set as Startup Project).
3. Presiona **F5** o haz clic en el botón de **Iniciar** para ejecutar la aplicación con depuración.

### Opción B: Usando la CLI de .NET
Si estás en la consola de comandos, ejecuta:
```powershell
dotnet run --project TiendaRopaCano.Presentation\TiendaRopaCano.Presentation.csproj
```

> [!NOTE]
> La interfaz gráfica está basada en WPF (Windows Presentation Foundation), por lo que requiere un entorno de ejecución Windows compatible.

---

## 📝 Notas Adicionales

- **Versión de .NET**: El proyecto tiene como objetivo `net10.0-windows`. Asegúrate de tener instalado el SDK de .NET 10.0 o superior para evitar errores de compilación.
- **Base de Datos**: Se incluye una base de datos local SQLite preconfigurada para las pruebas de desarrollo.
- **Servicios de PDF**: Si la aplicación genera reportes en PDF, revisa las configuraciones de ruta y permisos en las clases de servicio.

---

## 🤝 Soporte y Contribuciones

Si tienes problemas con las credenciales de Git o requieres realizar un *push* al repositorio remoto, asegúrate de configurar tu identidad y credenciales en tu entorno local:
```powershell
git config --global user.name "Tu Nombre"
git config --global user.email "tu-correo@example.com"
```

