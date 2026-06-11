using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using TiendaRopaCano.Datos.Contexto;

namespace TiendaRopaCano.Presentation
{
    public partial class App : System.Windows.Application
    {
        public static ConfiguracionBaseDatos BaseDeDatos { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Establecer cultura Nicaragua (C$ - Córdobas) para todo el sistema (UI y Reportes)
            var cultura = new System.Globalization.CultureInfo("es-NI");
            cultura.NumberFormat.CurrencySymbol = "C$";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultura;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultura;

            // Configurar WPF para que los enlaces de datos (Bindings) respeten la cultura configurada
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(cultura.IetfLanguageTag)));

            base.OnStartup(e);

            // Seleccionar todo el texto al hacer foco en cualquier TextBox de la app
            EventManager.RegisterClassHandler(
                typeof(TextBox),
                UIElement.GotFocusEvent,
                new RoutedEventHandler((s, _) => (s as TextBox)?.SelectAll()));

            // Pasar el foco al presionar Enter en cualquier TextBox (que no sea multilinea)
            EventManager.RegisterClassHandler(
                typeof(TextBox),
                UIElement.KeyDownEvent,
                new KeyEventHandler(TextBox_KeyDown));

            // Ruta donde se guarda el archivo .db
            string rutaDb = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TiendaRopa.db"
            );

            string connectionString = $"Data Source={rutaDb}";

            BaseDeDatos = new ConfiguracionBaseDatos(connectionString);

            // Esto crea las tablas la primera vez que arranca
            BaseDeDatos.InicializarBaseDeDatos();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox textBox)
            {
                if (!textBox.AcceptsReturn)
                {
                    e.Handled = true;
                    textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }
    }
}
