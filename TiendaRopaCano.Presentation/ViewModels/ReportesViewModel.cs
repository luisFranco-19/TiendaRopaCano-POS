using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Dominio.Entidades;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace TiendaRopaCano.Presentation.ViewModels
{
    public partial class ReportesViewModel : ObservableObject
    {
        private readonly IReporteService _reporteService;
        private readonly IPdfService _pdfService;

        public ReportesViewModel(IReporteService reporteService, IPdfService pdfService)
        {
            _reporteService = reporteService;
            _pdfService = pdfService;
            // Por defecto mostrar del primer al último día del mes actual
            FechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            FechaHasta = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            _ = CargarDatosAsync();
        }

        [ObservableProperty]
        private DateTime _fechaDesde;

        [ObservableProperty]
        private DateTime _fechaHasta;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<ReporteVentaDiaria> _ventasDiarias = new();

        [ObservableProperty]
        private ObservableCollection<Producto> _productosBajoStock = new();

        [ObservableProperty]
        private decimal _utilidadTotal;

        [ObservableProperty]
        private decimal _ventasTotales;

        [ObservableProperty]
        private int _cantidadVentas;

        [ObservableProperty]
        private ISeries[] _seriesVentas = Array.Empty<ISeries>();

        [ObservableProperty]
        private Axis[] _xAxes = new Axis[] { new Axis() };

        [ObservableProperty]
        private Axis[] _yAxes = new Axis[] { new Axis() };

        private async Task CargarDatosAsync()
        {
            try
            {
                IsLoading = true;
                
                var ventas = await _reporteService.ObtenerVentasDiariasAsync(FechaDesde, FechaHasta);
                VentasDiarias = new ObservableCollection<ReporteVentaDiaria>(ventas);
                
                UtilidadTotal = ventas.Sum(v => v.Utilidad);
                VentasTotales = ventas.Sum(v => v.TotalVentas);
                CantidadVentas = ventas.Sum(v => v.CantidadVentas);

                var values = ventas.Select(v => (double)v.TotalVentas).ToArray();
                var labels = ventas.Select(v => v.Fecha).ToArray();

                SeriesVentas = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = values,
                        Fill = new SolidColorPaint(new SKColor(124, 131, 253, 50)),
                        Stroke = new SolidColorPaint(new SKColor(124, 131, 253)) { StrokeThickness = 3 },
                        GeometrySize = 8,
                        GeometryStroke = new SolidColorPaint(new SKColor(124, 131, 253)) { StrokeThickness = 3 },
                        Name = "Ventas ($)"
                    }
                };

                XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = labels,
                        LabelsRotation = 45,
                        TextSize = 11,
                        LabelsPaint = new SolidColorPaint(new SKColor(139, 143, 163)),
                        SeparatorsPaint = new SolidColorPaint(new SKColor(37, 40, 64, 120))
                    }
                };

                YAxes = new Axis[]
                {
                    new Axis
                    {
                        TextSize = 11,
                        LabelsPaint = new SolidColorPaint(new SKColor(139, 143, 163)),
                        SeparatorsPaint = new SolidColorPaint(new SKColor(37, 40, 64, 120))
                    }
                };

                var bajoStock = await _reporteService.ObtenerProductosBajoStockAsync();
                ProductosBajoStock = new ObservableCollection<Producto>(bajoStock);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar reportes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void ExportarPdf()
        {
            if (!VentasDiarias.Any())
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() == true)
                {
                    var pdfBytes = _pdfService.GenerarReporteVentas(VentasDiarias, FechaDesde, FechaHasta, VentasTotales, UtilidadTotal, CantidadVentas);
                    System.IO.File.WriteAllBytes(sfd.FileName, pdfBytes);
                    MessageBox.Show("Reporte exportado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void ExportarStockBajoPdf()
        {
            if (!ProductosBajoStock.Any())
            {
                MessageBox.Show("No hay productos con stock bajo para exportar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Reporte_StockBajo_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() == true)
                {
                    var pdfBytes = _pdfService.GenerarReporteInventarioBajoStock(ProductosBajoStock);
                    System.IO.File.WriteAllBytes(sfd.FileName, pdfBytes);
                    MessageBox.Show("Reporte de stock bajo exportado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        [RelayCommand]
        private void ExportarCsv()
        {
            if (!VentasDiarias.Any())
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    FileName = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() == true)
                {
                    var csvBytes = _reporteService.GenerarCsvVentas(VentasDiarias);
                    System.IO.File.WriteAllBytes(sfd.FileName, csvBytes);
                    MessageBox.Show("Reporte exportado exitosamente a CSV.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
