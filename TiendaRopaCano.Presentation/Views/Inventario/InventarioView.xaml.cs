using System.Windows.Controls;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views.Inventario
{
    public partial class InventarioView : UserControl
    {
        public InventarioView()
        {
            InitializeComponent();

            var db = App.BaseDeDatos;
            var productoRepo = new ProductoRepository(db);
            var categoriaRepo = new CategoriaRepository(db);
            var alertaRepo = new AlertaRepository(db);
            var logRepo = new LogErrorRepository(db);
            var productoService = new ProductoService(productoRepo, logRepo, alertaRepo);
            var categoriaService = new CategoriaService(categoriaRepo, logRepo);
            var reporteRepo = new ReporteRepository(db);
            var reporteService = new ReporteService(reporteRepo, logRepo);

            var pdfService = new PdfService();

            DataContext = new InventarioViewModel(productoService, categoriaService, reporteService, pdfService);
        }
    }
}
