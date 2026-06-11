using System.Windows;
using System.Windows.Input;

namespace TiendaRopaCano.Presentation.Views.Inventario
{
    public partial class MovimientoInventarioDialog : Window
    {
        public MovimientoInventarioDialog()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
