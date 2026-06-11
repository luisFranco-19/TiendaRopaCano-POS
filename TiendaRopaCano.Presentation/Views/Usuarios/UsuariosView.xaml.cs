using System.Windows.Controls;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views.Usuarios
{
    public partial class UsuariosView : UserControl
    {
        public UsuariosView()
        {
            InitializeComponent();

            var db = App.BaseDeDatos;
            var usuarioRepo = new UsuarioRepository(db);
            var rolRepo = new RolRepository(db);
            var logRepo = new LogErrorRepository(db);
            
            var usuarioService = new UsuarioService(usuarioRepo, logRepo);
            var rolService = new RolService(rolRepo);
            var pdfService = new PdfService();

            DataContext = new UsuariosViewModel(usuarioService, rolService, pdfService);
        }
    }
}
