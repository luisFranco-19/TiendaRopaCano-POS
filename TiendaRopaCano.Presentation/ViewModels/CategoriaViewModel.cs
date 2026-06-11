using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class CategoriaViewModel : ObservableObject
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaViewModel(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
            _ = CargarCategoriasAsync();
        }

        [ObservableProperty]
        private ObservableCollection<Categoria> _categorias = new();

        [ObservableProperty]
        private Categoria? _categoriaSeleccionada;

        [ObservableProperty]
        private string _nombreNuevaCategoria = string.Empty;

        [ObservableProperty]
        private string _descripcionNuevaCategoria = string.Empty;

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private bool _isLoading;

        private async Task CargarCategoriasAsync()
        {
            try
            {
                IsLoading = true;
                var categorias = await _categoriaService.ObtenerTodasAsync();
                Categorias = new ObservableCollection<Categoria>(categorias);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GuardarCategoriaAsync()
        {
            if (string.IsNullOrWhiteSpace(NombreNuevaCategoria))
            {
                System.Windows.MessageBox.Show("El nombre de la categoría es obligatorio.", "Validación", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsLoading = true;
                if (IsEditing && CategoriaSeleccionada != null)
                {
                    CategoriaSeleccionada.Nombre = NombreNuevaCategoria;
                    CategoriaSeleccionada.Descripcion = DescripcionNuevaCategoria;
                    
                    var exito = await _categoriaService.ActualizarAsync(CategoriaSeleccionada);
                    if (exito)
                    {
                        System.Windows.MessageBox.Show("Categoría actualizada correctamente.", "Éxito");
                        LimpiarCampos();
                    }
                }
                else
                {
                    var nueva = new Categoria 
                    { 
                        Nombre = NombreNuevaCategoria, 
                        Descripcion = DescripcionNuevaCategoria 
                    };
                    
                    var id = await _categoriaService.InsertarAsync(nueva);
                    if (id > 0)
                    {
                        nueva.CategoriaId = id;
                        Categorias.Add(nueva);
                        System.Windows.MessageBox.Show("Categoría agregada correctamente.", "Éxito");
                        LimpiarCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al guardar: {ex.Message}", "Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void PrepararEdicion()
        {
            if (CategoriaSeleccionada == null) return;
            
            NombreNuevaCategoria = CategoriaSeleccionada.Nombre;
            DescripcionNuevaCategoria = CategoriaSeleccionada.Descripcion ?? string.Empty;
            IsEditing = true;
        }

        [RelayCommand]
        private void CancelarEdicion()
        {
            LimpiarCampos();
        }

        [RelayCommand]
        private async Task EliminarCategoriaAsync()
        {
            if (CategoriaSeleccionada == null) return;

            var result = System.Windows.MessageBox.Show($"¿Eliminar la categoría '{CategoriaSeleccionada.Nombre}'?", 
                "Confirmar", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;
                    var exito = await _categoriaService.EliminarAsync(CategoriaSeleccionada.CategoriaId);
                    if (exito)
                    {
                        Categorias.Remove(CategoriaSeleccionada);
                        System.Windows.MessageBox.Show("Categoría eliminada.");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"No se puede eliminar la categoría. Es posible que existan productos asociados a ella.\nDetalle: {ex.Message}", 
                        "Error al eliminar", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private void LimpiarCampos()
        {
            NombreNuevaCategoria = string.Empty;
            DescripcionNuevaCategoria = string.Empty;
            IsEditing = false;
            CategoriaSeleccionada = null;
        }
    }
}
