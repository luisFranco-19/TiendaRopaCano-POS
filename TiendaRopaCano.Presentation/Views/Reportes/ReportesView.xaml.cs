using System.Windows.Controls;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views.Reportes
{
    public partial class ReportesView : UserControl
    {
        public ReportesView()
        {
            InitializeComponent();

            var db = App.BaseDeDatos;
            var reporteRepo = new ReporteRepository(db);
            var logRepo = new LogErrorRepository(db);
            var reporteService = new ReporteService(reporteRepo, logRepo);
            var pdfService = new PdfService();

            DataContext = new ReportesViewModel(reporteService, pdfService);
        }
    }
}
