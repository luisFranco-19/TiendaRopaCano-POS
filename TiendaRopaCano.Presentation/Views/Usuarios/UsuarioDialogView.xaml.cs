using System.Windows;
using System.Windows.Input;

namespace TiendaRopaCano.Presentation.Views.Usuarios
{
    /// <summary>
    /// Lógica de interacción para UsuarioDialogView.xaml
    /// </summary>
    public partial class UsuarioDialogView : Window
    {
        public UsuarioDialogView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
