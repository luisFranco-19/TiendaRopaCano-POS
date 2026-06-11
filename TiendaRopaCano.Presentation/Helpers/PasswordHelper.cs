using System.Windows;
using System.Windows.Controls;

namespace TiendaRopaCano.Presentation.Helpers
{
    /// <summary>
    /// Proporciona propiedades adjuntas (attached properties) para permitir el enlace de datos (data binding) bidireccional
    /// con la propiedad Password de un control <see cref="PasswordBox"/> en WPF, lo cual no es soportado nativamente por razones de seguridad.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Propiedad adjunta para enlazar la contraseña del PasswordBox a un ViewModel.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
                typeof(string), typeof(PasswordHelper),
                new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// Propiedad adjunta para indicar si el PasswordHelper debe suscribirse a los eventos del PasswordBox.
        /// </summary>
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
                typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
                typeof(PasswordHelper));

        /// <summary>
        /// Establece el valor de la propiedad adjunta Attach en el objeto especificado.
        /// </summary>
        /// <param name="dp">El control WPF destino.</param>
        /// <param name="value">Valor a establecer.</param>
        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        /// <summary>
        /// Obtiene el valor de la propiedad adjunta Attach del objeto especificado.
        /// </summary>
        /// <param name="dp">El control WPF origen.</param>
        /// <returns>El valor booleano actual.</returns>
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        /// <summary>
        /// Obtiene el valor de la contraseña adjunta en el objeto especificado.
        /// </summary>
        /// <param name="dp">El control WPF origen.</param>
        /// <returns>La contraseña en formato de cadena.</returns>
        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Establece la contraseña adjunta en el objeto especificado.
        /// </summary>
        /// <param name="dp">El control WPF destino.</param>
        /// <param name="value">La contraseña a establecer.</param>
        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
                if (!GetIsUpdating(passwordBox))
                {
                    passwordBox.Password = (string)e.NewValue;
                }
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (sender is not PasswordBox passwordBox)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetIsUpdating(passwordBox, true);
                SetPassword(passwordBox, passwordBox.Password);
                SetIsUpdating(passwordBox, false);
            }
        }
    }
}
