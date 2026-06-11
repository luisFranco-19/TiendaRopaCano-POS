using System.Windows;
using System.Windows.Controls;
using TiendaRopaCano.Presentation.ViewModels;

namespace TiendaRopaCano.Presentation.Views.Inventario
{
    public partial class ProductoDialogView : Window
    {
        public ProductoDialogView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

