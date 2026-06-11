using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using TiendaRopaCano.Presentacion.Auxiliares;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserControl? _vistaActual;

        [ObservableProperty]
        private string _tituloSeccion = "Bienvenido";

        [ObservableProperty]
        private string _nombreUsuario = GestorSesion.UsuarioActual?.NombreCompleto ?? "Usuario";

        [ObservableProperty]
        private string _rolUsuario = GestorSesion.UsuarioActual?.Rol?.Nombre ?? "Sin Rol";

        public bool EsAdministrador => GestorSesion.EsAdministrador;

        public MainViewModel()
        {
            // Al iniciar sesión, cargar automáticamente el módulo de Ventas
            NavegarVentas();
        }

        [ObservableProperty]
        private bool _menuVentasActivo;

        [ObservableProperty]
        private bool _menuInventarioActivo;

        [ObservableProperty]
        private bool _menuUsuariosActivo;

        [ObservableProperty]
        private bool _menuCategoriasActivo;

        [ObservableProperty]
        private bool _menuReportesActivo;

        [RelayCommand]
        private void NavegarVentas()
        {
            ResetearMenu();
            MenuVentasActivo = true;
            TituloSeccion = "Ventas";
            VistaActual = new Views.Ventas.VentasView();
        }

        [RelayCommand]
        private void NavegarInventario()
        {
            ResetearMenu();
            MenuInventarioActivo = true;
            TituloSeccion = "Inventario";
            VistaActual = new Views.Inventario.InventarioView();
        }

        [RelayCommand]
        private void NavegarUsuarios()
        {
            if (!GestorSesion.EsAdministrador)
            {
                MessageBox.Show("No tiene permisos para acceder a esta sección.",
                    "Acceso Denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ResetearMenu();
            MenuUsuariosActivo = true;
            TituloSeccion = "Usuarios";
            VistaActual = new Views.Usuarios.UsuariosView();
        }

        [RelayCommand]
        private void NavegarCategorias()
        {
            ResetearMenu();
            MenuCategoriasActivo = true;
            TituloSeccion = "Categorías";
            VistaActual = new Views.Categorias.CategoriaView();
        }

        [RelayCommand]
        private void NavegarReportes()
        {
            if (!GestorSesion.EsAdministrador)
            {
                MessageBox.Show("No tiene permisos para acceder a esta sección.",
                    "Acceso Denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ResetearMenu();
            MenuReportesActivo = true;
            TituloSeccion = "Reportes y Ganancias";
            VistaActual = new Views.Reportes.ReportesView();
        }

        [RelayCommand]
        private void CerrarSesion()
        {
            var resultado = MessageBox.Show("¿Desea cerrar sesión?",
                "Cerrar Sesión", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                GestorSesion.CerrarSesion();

                var loginView = new Views.LoginView();
                loginView.Show();

                // Close all MainView instances
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window is Views.MainView)
                    {
                        window.Close();
                        break;
                    }
                }

                System.Windows.Application.Current.MainWindow = loginView;
            }
        }

        private void ResetearMenu()
        {
            MenuVentasActivo = false;
            MenuInventarioActivo = false;
            MenuUsuariosActivo = false;
            MenuCategoriasActivo = false;
            MenuReportesActivo = false;
        }
    }
}
