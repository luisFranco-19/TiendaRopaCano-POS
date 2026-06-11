using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;
using TiendaRopaCano.Presentacion.Auxiliares;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class NuevaVentaViewModel : ObservableObject
    {
        private readonly IProductoService _productoService;
        private readonly IVentaService _ventaService;
        private List<Producto> _todosLosProductos = new();

        public NuevaVentaViewModel(IProductoService productoService, IVentaService ventaService)
        {
            _productoService = productoService;
            _ventaService = ventaService;
            _ = CargarProductosAsync();
        }

        [ObservableProperty]
        private ObservableCollection<Producto> _productosFiltrados = new();

        [ObservableProperty]
        private ObservableCollection<DetalleVenta> _carrito = new();

        [ObservableProperty]
        private string _filtroTexto = string.Empty;

        [ObservableProperty]
        private decimal _total;

        [ObservableProperty]
        private bool _isLoading;

        private async Task CargarProductosAsync()
        {
            try
            {
                IsLoading = true;
                var productos = await _productoService.ObtenerTodosAsync();
                _todosLosProductos = productos.Where(p => p.Activo && p.Stock > 0).ToList();
                FiltrarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnFiltroTextoChanged(string value)
        {
            FiltrarProductos();
        }

        private void FiltrarProductos()
        {
            if (string.IsNullOrWhiteSpace(FiltroTexto))
            {
                ProductosFiltrados = new ObservableCollection<Producto>(_todosLosProductos);
            }
            else
            {
                var filtrados = _todosLosProductos
                    .Where(p => p.Nombre.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                ProductosFiltrados = new ObservableCollection<Producto>(filtrados);
            }
        }

        [RelayCommand]
        private void AgregarAlCarrito(Producto producto)
        {
            if (producto == null) return;

            var itemExistente = Carrito.FirstOrDefault(d => d.ProductoId == producto.ProductoId);
            if (itemExistente != null)
            {
                if (itemExistente.Cantidad + 1 > producto.Stock)
                {
                    MessageBox.Show("No hay suficiente stock disponible.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                itemExistente.Cantidad++;
                ActualizarTotal();
            }
            else
            {
                Carrito.Add(new DetalleVenta
                {
                    ProductoId = producto.ProductoId,
                    Producto = producto,
                    Cantidad = 1,
                    PrecioUnitario = producto.Precio,
                    Subtotal = producto.Precio
                });
                ActualizarTotal();
            }
        }

        [RelayCommand]
        private void QuitarDelCarrito(DetalleVenta detalle)
        {
            if (detalle == null) return;
            Carrito.Remove(detalle);
            ActualizarTotal();
        }

        [RelayCommand]
        private void IncrementarCantidad(DetalleVenta detalle)
        {
            if (detalle == null || detalle.Producto == null) return;

            if (detalle.Cantidad + 1 > detalle.Producto.Stock)
            {
                MessageBox.Show("No hay suficiente stock disponible.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            detalle.Cantidad++;
            ActualizarTotal();
        }

        [RelayCommand]
        private void DecrementarCantidad(DetalleVenta detalle)
        {
            if (detalle == null) return;

            if (detalle.Cantidad > 1)
            {
                detalle.Cantidad--;
                ActualizarTotal();
            }
            else
            {
                QuitarDelCarrito(detalle);
            }
        }

        private void ActualizarTotal()
        {
            Total = Carrito.Sum(d => d.Subtotal);
        }

        [RelayCommand]
        private async Task FinalizarVentaAsync(Window window)
        {
            if (!Carrito.Any())
            {
                MessageBox.Show("El carrito está vacío.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmacion = MessageBox.Show($"¿Desea finalizar la venta por un total de {Total:C2}?", 
                "Confirmar Venta", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacion == MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;

                    var nuevaVenta = new Venta
                    {
                        UsuarioId = GestorSesion.UsuarioActual?.UsuarioId ?? 1,
                        Fecha = DateTime.Now,
                        Total = Total,
                        Detalles = Carrito.ToList()
                    };

                    int ventaId = await _ventaService.RegistrarVentaAsync(nuevaVenta);

                    if (ventaId > 0)
                    {
                        MessageBox.Show("Venta registrada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        window.DialogResult = true;
                        window.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar la venta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
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
