using System.Windows.Controls;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views.Categorias
{
    public partial class CategoriaView : UserControl
    {
        public CategoriaView()
        {
            InitializeComponent();

            var db = App.BaseDeDatos;
            var categoriaRepo = new CategoriaRepository(db);
            var logRepo = new LogErrorRepository(db);
            var categoriaService = new CategoriaService(categoriaRepo, logRepo);

            DataContext = new CategoriaViewModel(categoriaService);
        }
    }
}
