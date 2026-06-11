using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class ProductoDialogViewModel : ObservableObject
    {
        private readonly IProductoService _productoService;
        private readonly Producto? _productoOriginal;

        public ProductoDialogViewModel(IProductoService productoService, ObservableCollection<Categoria> categorias, Producto? producto = null)
        {
            _productoService = productoService;
            Categorias = categorias;
            _productoOriginal = producto;

            if (producto != null)
            {
                IsEditing = true;
                Nombre = producto.Nombre;
                Descripcion = producto.Descripcion ?? string.Empty;
                Precio = producto.Precio;
                PrecioCompra = producto.PrecioCompra;
                Stock = producto.Stock;
                StockMinimo = producto.StockMinimo;
                CategoriaSeleccionada = producto.Categoria;
            }
            else
            {
                IsEditing = false;
                StockMinimo = 5; // Valor por defecto
            }
        }

        [ObservableProperty]
        private string _nombre = string.Empty;

        [ObservableProperty]
        private string _descripcion = string.Empty;

        [ObservableProperty]
        private decimal _precio;

        [ObservableProperty]
        private decimal _precioCompra;

        [ObservableProperty]
        private int _stock;

        [ObservableProperty]
        private int _stockMinimo;

        [ObservableProperty]
        private Categoria? _categoriaSeleccionada;

        [ObservableProperty]
        private ObservableCollection<Categoria> _categorias;

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private bool _isLoading;

        public bool OperacionExitosa { get; private set; }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                System.Windows.MessageBox.Show("El nombre del producto es obligatorio.", "Validación", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (CategoriaSeleccionada == null)
            {
                System.Windows.MessageBox.Show("Debe seleccionar una categoría.", "Validación", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsLoading = true;

                if (IsEditing && _productoOriginal != null)
                {
                    _productoOriginal.Nombre = Nombre;
                    _productoOriginal.Descripcion = Descripcion;
                    _productoOriginal.Precio = Precio;
                    _productoOriginal.PrecioCompra = PrecioCompra;
                    _productoOriginal.Stock = Stock;
                    _productoOriginal.StockMinimo = StockMinimo;
                    _productoOriginal.CategoriaId = CategoriaSeleccionada.CategoriaId;
                    _productoOriginal.Categoria = CategoriaSeleccionada;

                    var exito = await _productoService.ActualizarAsync(_productoOriginal);
                    if (exito)
                    {
                        OperacionExitosa = true;
                        CerrarVentana();
                    }
                }
                else
                {
                    var nuevo = new Producto
                    {
                        Nombre = Nombre,
                        Descripcion = Descripcion,
                        Precio = Precio,
                        PrecioCompra = PrecioCompra,
                        Stock = Stock,
                        StockMinimo = StockMinimo,
                        CategoriaId = CategoriaSeleccionada.CategoriaId,
                        Categoria = CategoriaSeleccionada
                    };

                    var id = await _productoService.InsertarAsync(nuevo);
                    if (id > 0)
                    {
                        nuevo.ProductoId = id;
                        OperacionExitosa = true;
                        CerrarVentana();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al guardar producto: {ex.Message}", "Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CerrarVentana()
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.DialogResult = true;
                    window.Close();
                    break;
                }
            }
        }
    }
}
