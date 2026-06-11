using System.Windows.Controls;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views.Ventas
{
    public partial class VentasView : UserControl
    {
        public VentasView()
        {
            InitializeComponent();

            var db = App.BaseDeDatos;
            var ventaRepo = new VentaRepository(db);
            var productoRepo = new ProductoRepository(db);
            var alertaRepo = new AlertaRepository(db);
            var logRepo = new LogErrorRepository(db);
            
            var ventaService = new VentaService(ventaRepo, productoRepo, logRepo);
            var productoService = new ProductoService(productoRepo, logRepo, alertaRepo);

            var pdfService = new PdfService();
            DataContext = new VentasViewModel(ventaService, productoService, pdfService);
        }
    }
}
