using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;
using TiendaRopaCano.Presentacion.Auxiliares;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class UsuariosViewModel : ObservableObject
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IPdfService _pdfService;

        public UsuariosViewModel(IUsuarioService usuarioService, IRolService rolService, IPdfService pdfService)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
            _pdfService = pdfService;
            EsAdministrador = GestorSesion.EsAdministrador;

            if (EsAdministrador)
                _ = CargarUsuariosAsync();
        }

        [ObservableProperty]
        private ObservableCollection<Usuario> _usuarios = new();

        [ObservableProperty]
        private Usuario? _usuarioSeleccionado;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _esAdministrador;

        [ObservableProperty]
        private string _filtro = string.Empty;

        [ObservableProperty]
        private int _totalUsuarios;

        [ObservableProperty]
        private int _usuariosActivos;

        [ObservableProperty]
        private int _usuariosInactivos;

        private List<Usuario> _allUsuarios = new();

        partial void OnFiltroChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Usuarios = new ObservableCollection<Usuario>(_allUsuarios);
            }
            else
            {
                var lower = value.ToLower();
                var filtered = _allUsuarios.Where(u => 
                    u.NombreCompleto.ToLower().Contains(lower) || 
                    u.NombreUsuario.ToLower().Contains(lower) ||
                    (u.Rol?.Nombre ?? "").ToLower().Contains(lower));
                Usuarios = new ObservableCollection<Usuario>(filtered);
            }
        }

        [RelayCommand]
        private async Task CargarUsuariosAsync()
        {
            try
            {
                IsLoading = true;
                var lista = await _usuarioService.ObtenerTodosAsync();
                _allUsuarios = lista.ToList();
                Usuarios = new ObservableCollection<Usuario>(_allUsuarios);
                
                ActualizarEstadisticas();
                Filtro = string.Empty; // Reset filter
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ActualizarEstadisticas()
        {
            TotalUsuarios = _allUsuarios.Count;
            UsuariosActivos = _allUsuarios.Count(u => u.Activo);
            UsuariosInactivos = _allUsuarios.Count(u => !u.Activo);
        }

        [RelayCommand]
        private async Task AgregarUsuarioAsync()
        {
            var vm = new UsuarioDialogViewModel(_usuarioService, _rolService);
            var dialog = new Views.Usuarios.UsuarioDialogView
            {
                DataContext = vm,
                Owner = System.Windows.Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await CargarUsuariosAsync();
            }
        }

        [RelayCommand]
        private async Task EditarUsuarioAsync()
        {
            if (UsuarioSeleccionado == null)
            {
                System.Windows.MessageBox.Show("Seleccione un usuario para editar.",
                    "Editar Usuario", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var vm = new UsuarioDialogViewModel(_usuarioService, _rolService, UsuarioSeleccionado);
            var dialog = new Views.Usuarios.UsuarioDialogView
            {
                DataContext = vm,
                Owner = System.Windows.Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await CargarUsuariosAsync();
            }
        }

        [RelayCommand]
        private async Task EliminarUsuarioAsync()
        {
            if (UsuarioSeleccionado == null)
            {
                System.Windows.MessageBox.Show("Seleccione un usuario para eliminar.",
                    "Eliminar Usuario", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (UsuarioSeleccionado.UsuarioId == GestorSesion.UsuarioActual?.UsuarioId)
            {
                System.Windows.MessageBox.Show("No puede eliminar su propio usuario.",
                    "Operación no permitida", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (UsuarioSeleccionado.RolId == 1 || 
                UsuarioSeleccionado.Rol?.Nombre?.Equals("Administrador", StringComparison.OrdinalIgnoreCase) == true)
            {
                System.Windows.MessageBox.Show("No se puede eliminar ningún administrador.",
                    "Operación no permitida", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var resultado = System.Windows.MessageBox.Show(
                $"¿Está seguro de eliminar a '{UsuarioSeleccionado.NombreCompleto}'?",
                "Confirmar Eliminación", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (resultado == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;
                    var eliminado = await _usuarioService.EliminarAsync(UsuarioSeleccionado.UsuarioId);

                    if (eliminado)
                    {
                        var u = UsuarioSeleccionado;
                        Usuarios.Remove(u);
                        _allUsuarios.Remove(u);
                        UsuarioSeleccionado = null;
                        ActualizarEstadisticas();

                        System.Windows.MessageBox.Show("Usuario eliminado correctamente.",
                            "Éxito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error al eliminar: {ex.Message}",
                        "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        [RelayCommand]
        private async Task CambiarEstadoAsync()
        {
            if (UsuarioSeleccionado == null)
            {
                System.Windows.MessageBox.Show("Seleccione un usuario para cambiar su estado.",
                    "Cambiar Estado", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (UsuarioSeleccionado.UsuarioId == GestorSesion.UsuarioActual?.UsuarioId)
            {
                System.Windows.MessageBox.Show("No puede desactivar su propio usuario.",
                    "Operación no permitida", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var nuevoEstado = !UsuarioSeleccionado.Activo;
            var accion = nuevoEstado ? "activar" : "desactivar";
            var resultado = System.Windows.MessageBox.Show(
                $"¿Está seguro de {accion} a '{UsuarioSeleccionado.NombreCompleto}'?",
                "Confirmar Cambio de Estado", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (resultado != System.Windows.MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                var actualizado = await _usuarioService.CambiarEstadoAsync(UsuarioSeleccionado.UsuarioId, nuevoEstado);

                if (actualizado)
                {
                    await CargarUsuariosAsync();
                    System.Windows.MessageBox.Show($"Usuario {(nuevoEstado ? "activado" : "desactivado")} correctamente.",
                        "Éxito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cambiar estado: {ex.Message}",
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ExportarUsuariosPdf()
        {
            if (!_allUsuarios.Any())
            {
                System.Windows.MessageBox.Show("No hay usuarios para exportar.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Reporte_Usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() == true)
                {
                    var pdfBytes = _pdfService.GenerarReporteUsuarios(_allUsuarios);
                    System.IO.File.WriteAllBytes(sfd.FileName, pdfBytes);
                    System.Windows.MessageBox.Show("Reporte de usuarios exportado exitosamente.", "Éxito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
