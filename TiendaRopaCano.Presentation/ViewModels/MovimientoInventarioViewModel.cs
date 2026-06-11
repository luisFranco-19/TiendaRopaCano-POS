using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class MovimientoInventarioViewModel : ObservableObject
    {
        private readonly IProductoService _productoService;
        private readonly Producto _producto;

        public MovimientoInventarioViewModel(IProductoService productoService, Producto producto)
        {
            _productoService = productoService;
            _producto = producto;
            NombreProducto = producto.Nombre;
            StockActual = producto.Stock;
        }

        [ObservableProperty]
        private string _nombreProducto;

        [ObservableProperty]
        private int _stockActual;

        [ObservableProperty]
        private int _cantidad = 1;

        [ObservableProperty]
        private bool _esEntrada = true;

        [ObservableProperty]
        private bool _esSalida;

        [ObservableProperty]
        private string _motivo = string.Empty;

        [RelayCommand]
        private void Guardar(System.Windows.Window window)
        {
            if (Cantidad <= 0)
            {
                System.Windows.MessageBox.Show("La cantidad debe ser mayor a cero.", "Validación", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (EsSalida && Cantidad > StockActual)
            {
                System.Windows.MessageBox.Show("No hay suficiente stock para realizar esta salida.", "Validación", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // En el repositorio, ActualizarStockAsync hace Stock = Stock - Cantidad
            // Entonces:
            // Si es Salida: enviamos Cantidad (se resta)
            // Si es Entrada: enviamos -Cantidad (restar un negativo es sumar)
            int cantidadAEnviar = EsSalida ? Cantidad : -Cantidad;

            _ = ProcesarMovimientoAsync(cantidadAEnviar, window);
        }

        private async Task ProcesarMovimientoAsync(int cantidad, System.Windows.Window window)
        {
            try 
            {
                var exito = await _productoService.ActualizarStockAsync(_producto.ProductoId, cantidad);
                if (exito)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al procesar movimiento: {ex.Message}", "Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Cancelar(System.Windows.Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }
}
