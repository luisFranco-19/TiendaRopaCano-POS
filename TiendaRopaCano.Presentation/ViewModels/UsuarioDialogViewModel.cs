using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class UsuarioDialogViewModel : ObservableObject
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly bool _esEdicion;
        private string? _contrasenaOriginalHash;
        private bool _contrasenaModificada = false;

        public UsuarioDialogViewModel(IUsuarioService usuarioService, IRolService rolService, Usuario? usuario = null)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
            
            if (usuario != null)
            {
                _esEdicion = true;
                UsuarioId = usuario.UsuarioId;
                NombreCompleto = usuario.NombreCompleto;
                NombreUsuario = usuario.NombreUsuario;
                _contrasenaOriginalHash = usuario.Contrasena;
                Contrasena = string.Empty; // Leave empty for PasswordBox
                RolId = usuario.RolId;
                Activo = usuario.Activo;
                Titulo = "Editar Usuario";
                MostrarCheckboxActivo = false; // No mostrar en edición, usar botón CambiarEstado
                PlaceholderContrasena = "Dejar vacío para mantener actual";
            }
            else
            {
                _esEdicion = false;
                Titulo = "Nuevo Usuario";
                MostrarCheckboxActivo = false; // Nuevos usuarios siempre activos
                PlaceholderContrasena = "";
            }

            _ = CargarRolesAsync();
        }

        [ObservableProperty]
        private string _titulo;

        [ObservableProperty]
        private int _usuarioId;

        [ObservableProperty]
        private string _nombreCompleto = string.Empty;

        [ObservableProperty]
        private string _nombreUsuario = string.Empty;

        [ObservableProperty]
        private string _contrasena = string.Empty;

        [ObservableProperty]
        private int _rolId;

        [ObservableProperty]
        private ObservableCollection<Rol> _roles = new();

        [ObservableProperty]
        private Rol? _rolSeleccionado;

        [ObservableProperty]
        private bool _activo = true;

        [ObservableProperty]
        private bool _mostrarCheckboxActivo;

        [ObservableProperty]
        private string _placeholderContrasena = string.Empty;

        partial void OnContrasenaChanged(string value)
        {
            // Si el usuario escribió algo en el campo de contraseña, marcar como modificada
            if (_esEdicion && !string.IsNullOrEmpty(value))
            {
                _contrasenaModificada = true;
            }
        }

        private async Task CargarRolesAsync()
        {
            var roles = await _rolService.ObtenerTodosAsync();
            Roles = new ObservableCollection<Rol>(roles);
            
            if (_esEdicion)
            {
                RolSeleccionado = Roles.FirstOrDefault(r => r.RolId == RolId);
            }
        }

        [RelayCommand]
        private async Task GuardarAsync(Window window)
        {
            if (string.IsNullOrWhiteSpace(NombreCompleto) || 
                string.IsNullOrWhiteSpace(NombreUsuario) || 
                RolSeleccionado == null)
            {
                MessageBox.Show("Por favor complete todos los campos.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!_esEdicion && string.IsNullOrWhiteSpace(Contrasena))
            {
                MessageBox.Show("La contraseña es requerida para nuevos usuarios.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_esEdicion)
                {
                    var usuario = new Usuario
                    {
                        UsuarioId = UsuarioId,
                        NombreCompleto = NombreCompleto,
                        NombreUsuario = NombreUsuario,
                        RolId = RolSeleccionado.RolId,
                        Activo = Activo
                    };

                    // Solo actualizar la contraseña si el usuario la modificó
                    if (_contrasenaModificada && !string.IsNullOrWhiteSpace(Contrasena))
                    {
                        usuario.Contrasena = Contrasena; // Se hasheará en el servicio
                    }
                    else
                    {
                        // Mantener la contraseña original hasheada
                        usuario.Contrasena = _contrasenaOriginalHash ?? string.Empty;
                    }

                    var actualizado = await _usuarioService.ActualizarAsync(usuario);
                    if (!actualizado)
                    {
                        MessageBox.Show("No se pudo actualizar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    await _usuarioService.InsertarAsync(new Usuario
                    {
                        NombreCompleto = NombreCompleto,
                        NombreUsuario = NombreUsuario,
                        Contrasena = Contrasena,
                        RolId = RolSeleccionado.RolId,
                        Activo = true
                    });
                }

                window.DialogResult = true;
                window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Cancelar(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }
}
