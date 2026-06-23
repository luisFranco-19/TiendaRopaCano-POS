using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;
using TiendaRopaCano.Presentacion.Auxiliares;
using TiendaRopaCano.Presentation.Views;

using System.Linq;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class RegistroViewModel : ObservableObject
    {
        private readonly IUsuarioService _usuarioService;

        [ObservableProperty]
        private bool _mostrarVolverLoginLink = true;

        public RegistroViewModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            _ = InicializarAsync();
        }

        private async Task InicializarAsync()
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerTodosAsync();
                MostrarVolverLoginLink = usuarios.Any();
            }
            catch
            {
                MostrarVolverLoginLink = true;
            }
        }

        [ObservableProperty]
        private string _nombreCompleto = string.Empty;

        [ObservableProperty]
        private string _nombreUsuario = string.Empty;

        [ObservableProperty]
        private string _contrasena = string.Empty;

        [ObservableProperty]
        private string _confirmarContrasena = string.Empty;

        [ObservableProperty]
        private int _rolSeleccionado = 1; // Por defecto: Administrador

        [ObservableProperty]
        private string _mensajeError = string.Empty;

        [ObservableProperty]
        private string _mensajeExito = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _mostrarContrasena;

        // Opciones de rol para el ComboBox (Solo Administrador para el primer registro)
        public List<KeyValuePair<int, string>> Roles { get; } = new()
        {
            new KeyValuePair<int, string>(1, "Administrador")
        };

        [RelayCommand]
        private async Task RegistrarAsync()
        {
            MensajeError = string.Empty;
            MensajeExito = string.Empty;

            // Validaciones
            if (string.IsNullOrWhiteSpace(NombreCompleto))
            {
                MensajeError = "Ingrese su nombre completo.";
                return;
            }

            if (string.IsNullOrWhiteSpace(NombreUsuario))
            {
                MensajeError = "Ingrese un nombre de usuario.";
                return;
            }

            if (NombreUsuario.Length < 3)
            {
                MensajeError = "El usuario debe tener al menos 3 caracteres.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Contrasena))
            {
                MensajeError = "Ingrese una contraseña.";
                return;
            }

            if (Contrasena.Length < 4)
            {
                MensajeError = "La contraseña debe tener al menos 4 caracteres.";
                return;
            }

            if (Contrasena != ConfirmarContrasena)
            {
                MensajeError = "Las contraseñas no coinciden.";
                return;
            }

            try
            {
                IsLoading = true;

                var nuevoUsuario = new Usuario
                {
                    NombreCompleto = NombreCompleto.Trim(),
                    NombreUsuario = NombreUsuario.Trim(),
                    Contrasena = Contrasena,
                    RolId = RolSeleccionado,
                    Activo = true,
                    Rol = new Rol 
                    { 
                        RolId = RolSeleccionado, 
                        Nombre = Roles.FirstOrDefault(r => r.Key == RolSeleccionado).Value ?? "Vendedor" 
                    }
                };

                var nuevoId = await _usuarioService.InsertarAsync(nuevoUsuario);
                nuevoUsuario.UsuarioId = nuevoId;

                // Establecer la sesión del usuario recién creado
                GestorSesion.UsuarioActual = nuevoUsuario;

                // Abrir el Dashboard (MainView)
                var mainView = new MainView();
                mainView.Show();

                // Cerrar la ventana actual de registro
                CerrarVentanaActual();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase))
                    MensajeError = "Ese nombre de usuario ya existe. Elija otro.";
                else
                    MensajeError = $"Error al registrar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CerrarVentanaActual()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window is RegistroView)
                {
                    window.Close();
                    break;
                }
            }
        }

        [RelayCommand]
        private void VolverAlLogin()
        {
            var loginView = new LoginView();
            loginView.Show();

            CerrarVentanaActual();
        }
    }
}
