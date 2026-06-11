using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Presentacion.Auxiliares;
using TiendaRopaCano.Presentation.Views;
using TiendaRopaCano.Dominio.Entidades;

using System.Linq;

namespace TiendaRopaCano.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel para controlar la lógica de la vista de Inicio de Sesión (LoginView).
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Indica si se debe mostrar el enlace para registrar el primer usuario.
        /// </summary>
        [ObservableProperty]
        private bool _mostrarRegistroLink;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LoginViewModel"/> inyectando el servicio de usuarios.
        /// </summary>
        /// <param name="usuarioService">Servicio para la autenticación y validación de usuarios.</param>
        public LoginViewModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            _ = InicializarAsync();
        }

        private async Task InicializarAsync()
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerTodosAsync();
                MostrarRegistroLink = !usuarios.Any();
            }
            catch
            {
                MostrarRegistroLink = false;
            }
        }

        /// <summary>
        /// Respalda el nombre de usuario ingresado por el usuario en la interfaz.
        /// </summary>
        [ObservableProperty]
        private string _nombreUsuario = string.Empty;

        /// <summary>
        /// Respalda la contraseña ingresada por el usuario en la interfaz.
        /// </summary>
        [ObservableProperty]
        private string _contrasena = string.Empty;

        /// <summary>
        /// Respalda el mensaje de error a mostrar en caso de fallos de autenticación.
        /// </summary>
        [ObservableProperty]
        private string _mensajeError = string.Empty;

        /// <summary>
        /// Respalda el estado de carga o progreso de la autenticación.
        /// </summary>
        [ObservableProperty]
        private bool _isLoading;

        /// <summary>
        /// Determina si se muestra o se oculta la contraseña en texto plano.
        /// </summary>
        [ObservableProperty]
        private bool _mostrarContrasena;

        /// <summary>
        /// Valida las credenciales ingresadas, inicia sesión en el sistema y abre la ventana principal.
        /// </summary>
        [RelayCommand]
        private async Task IngresarAsync()
        {
            MensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                MensajeError = "Ingrese usuario y contraseña.";
                return;
            }

            try
            {
                IsLoading = true;

                var usuario = await _usuarioService.ObtenerPorCredencialesAsync(NombreUsuario, Contrasena);

                if (usuario == null)
                {
                    MensajeError = "Credenciales incorrectas.";
                    return;
                }

                GestorSesion.UsuarioActual = usuario;

                // Abrir ventana principal
                var mainView = new MainView();
                mainView.Show();

                // Cerrar ventana de login
                System.Windows.Application.Current.MainWindow?.Close();
                System.Windows.Application.Current.MainWindow = mainView;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al iniciar sesión: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
