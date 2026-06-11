using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class VentasViewModel : ObservableObject
    {
        private readonly IVentaService _ventaService;
        private readonly IProductoService _productoService;
        private readonly IPdfService _pdfService;

        public VentasViewModel(IVentaService ventaService, IProductoService productoService, IPdfService pdfService)
        {
            _ventaService = ventaService;
            _productoService = productoService;
            _pdfService = pdfService;
            _ = CargarVentasAsync();
        }

        private List<Venta> _todasLasVentas = new();

        [ObservableProperty]
        private ObservableCollection<Venta> _ventas = new();

        [ObservableProperty]
        private Venta? _ventaSeleccionada;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _filtroTexto = string.Empty;

        [ObservableProperty]
        private DateTime _fechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        [ObservableProperty]
        private DateTime _fechaHasta = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
            DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

        private void AplicarFiltro()
        {
            if (string.IsNullOrWhiteSpace(FiltroTexto))
            {
                Ventas = new ObservableCollection<Venta>(_todasLasVentas);
            }
            else
            {
                var query = FiltroTexto.ToLower();
                var filtradas = _todasLasVentas.Where(v => 
                    v.VentaId.ToString().Contains(query) ||
                    (v.Usuario != null && v.Usuario.NombreCompleto.ToLower().Contains(query)) ||
                    v.Total.ToString().Contains(query) ||
                    (v.Detalles != null && v.Detalles.Any(d => d.Producto != null && d.Producto.Nombre.ToLower().Contains(query)))
                );
                Ventas = new ObservableCollection<Venta>(filtradas);
            }
        }

        partial void OnFiltroTextoChanged(string value)
        {
            AplicarFiltro();
        }

        [RelayCommand]
        private async Task CargarVentasAsync()
        {
            try
            {
                IsLoading = true;
                var lista = await _ventaService.ObtenerTodasAsync();
                _todasLasVentas = lista.ToList();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar ventas: {ex.Message}",
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task FiltrarPorFechaAsync()
        {
            try
            {
                IsLoading = true;
                var lista = await _ventaService.ObtenerPorFechaAsync(FechaDesde, FechaHasta);
                _todasLasVentas = lista.ToList();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al filtrar ventas: {ex.Message}",
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task NuevaVentaAsync()
        {
            var vm = new NuevaVentaViewModel(_productoService, _ventaService);
            var dialog = new Views.Ventas.NuevaVentaDialog
            {
                DataContext = vm,
                Owner = System.Windows.Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await CargarVentasAsync();
            }
        }

        [RelayCommand]
        private void ExportarPdf()
        {
            if (!Ventas.Any())
            {
                System.Windows.MessageBox.Show("No hay ventas para exportar.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Historial_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() == true)
                {
                    var pdfBytes = _pdfService.GenerarReporteHistorialVentas(Ventas, FechaDesde, FechaHasta);
                    System.IO.File.WriteAllBytes(sfd.FileName, pdfBytes);
                    System.Windows.MessageBox.Show("Historial de ventas exportado exitosamente a PDF.", "Éxito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void GenerarFacturaPdf()
        {
            if (VentaSeleccionada == null)
            {
                System.Windows.MessageBox.Show("Por favor, seleccione una venta de la lista.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Factura_{VentaSeleccionada.VentaId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() == true)
                {
                    var pdfBytes = _pdfService.GenerarFacturaVenta(VentaSeleccionada);
                    System.IO.File.WriteAllBytes(sfd.FileName, pdfBytes);
                    System.Windows.MessageBox.Show("Factura generada exitosamente en PDF.", "Éxito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar la factura: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
