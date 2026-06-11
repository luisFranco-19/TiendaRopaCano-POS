using System.Windows;
using System.Windows.Input;
using TiendaRopaCano.Aplicacion.Servicios;
using TiendaRopaCano.Datos.Contexto;
using TiendaRopaCano.Datos.Repositorios;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views
{
    public partial class LoginView : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();

            // Crear dependencias
            var db = App.BaseDeDatos;
            var usuarioRepo = new UsuarioRepository(db);
            var logRepo = new LogErrorRepository(db);
            var usuarioService = new UsuarioService(usuarioRepo, logRepo);

            _viewModel = new LoginViewModel(usuarioService);
            DataContext = _viewModel;

            Loaded += (s, e) => UsuarioTextBox.Focus();
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

        private void Registrarse_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var registroView = new RegistroView();
            registroView.Show();
            this.Close();
        }

        private void UsuarioTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ContrasenaBox.Focus();
                e.Handled = true;
            }
        }

        private void ContrasenaBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_viewModel.IngresarCommand.CanExecute(null))
                    _viewModel.IngresarCommand.Execute(null);
                e.Handled = true;
            }
        }
    }
}
