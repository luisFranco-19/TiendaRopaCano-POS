using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class InventarioViewModel : ObservableObject
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IReporteService _reporteService;
        private readonly IPdfService _pdfService;

        public InventarioViewModel(IProductoService productoService, ICategoriaService categoriaService, IReporteService reporteService, IPdfService pdfService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _reporteService = reporteService;
            _pdfService = pdfService;
            _ = CargarDatosAsync();
        }

        [ObservableProperty]
        private ObservableCollection<Producto> _productos = new();

        [ObservableProperty]
        private ObservableCollection<Categoria> _categorias = new();

        [ObservableProperty]
        private Producto? _productoSeleccionado;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _filtroTexto = string.Empty;

        private List<Producto> _todosLosProductos = new();

        partial void OnFiltroTextoChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Productos = new ObservableCollection<Producto>(_todosLosProductos);
            }
            else
            {
                var lower = value.ToLower();
                var filtrados = _todosLosProductos.Where(p => 
                    p.Nombre.ToLower().Contains(lower) || 
                    (p.Categoria?.Nombre ?? "").ToLower().Contains(lower));
                Productos = new ObservableCollection<Producto>(filtrados);
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                IsLoading = true;

                var productos = await _productoService.ObtenerTodosAsync();
                _todosLosProductos = productos.ToList();
                Productos = new ObservableCollection<Producto>(_todosLosProductos);
                FiltroTexto = string.Empty;

                var categorias = await _categoriaService.ObtenerTodasAsync();
                Categorias = new ObservableCollection<Categoria>(categorias);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar inventario: {ex.Message}",
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task RefrescarAsync()
        {
            await CargarDatosAsync();
        }

        [RelayCommand]
        private async Task AgregarProductoAsync()
        {
            var vm = new ProductoDialogViewModel(_productoService, Categorias);
            var dialog = new Views.Inventario.ProductoDialogView
            {
                DataContext = vm,
                Owner = System.Windows.Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await RefrescarAsync();
            }
        }

        [RelayCommand]
        private async Task EditarProductoAsync()
        {
            if (ProductoSeleccionado == null)
            {
                System.Windows.MessageBox.Show("Seleccione un producto para editar.",
                    "Editar Producto", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var vm = new ProductoDialogViewModel(_productoService, Categorias, ProductoSeleccionado);
            var dialog = new Views.Inventario.ProductoDialogView
            {
                DataContext = vm,
                Owner = System.Windows.Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await RefrescarAsync();
            }
        }

        [RelayCommand]
        private async Task EliminarProductoAsync()
        {
            if (ProductoSeleccionado == null)
            {
                System.Windows.MessageBox.Show("Seleccione un producto para eliminar.",
                    "Eliminar Producto", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var resultado = System.Windows.MessageBox.Show(
                $"¿Está seguro de eliminar '{ProductoSeleccionado.Nombre}'?",
                "Confirmar Eliminación", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (resultado == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;
                    var eliminado = await _productoService.EliminarAsync(ProductoSeleccionado.ProductoId);

                    if (eliminado)
                    {
                        Productos.Remove(ProductoSeleccionado);
                        ProductoSeleccionado = null;
                        System.Windows.MessageBox.Show("Producto eliminado correctamente.",
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
        private async Task AjustarStockAsync()
        {
            if (ProductoSeleccionado == null)
            {
                System.Windows.MessageBox.Show("Seleccione un producto para ajustar su inventario.",
                    "Ajuste de Inventario", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var vm = new MovimientoInventarioViewModel(_productoService, ProductoSeleccionado);
            var dialog = new Views.Inventario.MovimientoInventarioDialog
            {
                DataContext = vm,
                Owner = System.Windows.Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await RefrescarAsync();
            }
        }
        [RelayCommand]
        private void ExportarCsv()
        {
            if (!Productos.Any())
            {
                System.Windows.MessageBox.Show("No hay productos para exportar.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    FileName = $"Inventario_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() == true)
                {
                    var csvBytes = _reporteService.GenerarCsvInventario(Productos);
                    System.IO.File.WriteAllBytes(sfd.FileName, csvBytes);
                    System.Windows.MessageBox.Show("Inventario exportado exitosamente a CSV.", "Éxito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar CSV: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        [RelayCommand]
        private void ExportarPdf()
        {
            if (!Productos.Any())
            {
                System.Windows.MessageBox.Show("No hay productos para exportar.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Inventario_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() == true)
                {
                    var pdfBytes = _pdfService.GenerarReporteInventarioCompleto(Productos);
                    System.IO.File.WriteAllBytes(sfd.FileName, pdfBytes);
                    System.Windows.MessageBox.Show("Inventario exportado exitosamente a PDF.", "Éxito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
