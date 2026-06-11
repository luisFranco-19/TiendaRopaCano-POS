using System.Windows;
using System.Windows.Input;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views
{
    public partial class RegistroView : Window
    {
        private readonly RegistroViewModel _viewModel;

        public RegistroView()
        {
            InitializeComponent();

            // Crear dependencias
            var db = App.BaseDeDatos;
            var usuarioRepo = new UsuarioRepository(db);
            var logRepo = new LogErrorRepository(db);
            var usuarioService = new UsuarioService(usuarioRepo, logRepo);

            _viewModel = new RegistroViewModel(usuarioService);
            DataContext = _viewModel;

            // Ya no se necesita sincronizar PasswordBox manualmente
            // porque ahora usamos PasswordHelper con attached properties en el XAML
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void VolverLogin_Click(object sender, MouseButtonEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
