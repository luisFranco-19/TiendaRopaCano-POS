using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TiendaRopaCano.Presentation.Helpers
{
    /// <summary>
    /// Convierte un valor booleano a un estado de visibilidad en WPF (true = Visible, false = Collapsed).
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convierte un valor booleano en un objeto <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">El valor booleano de origen.</param>
        /// <param name="targetType">El tipo del destino del enlace.</param>
        /// <param name="parameter">Parámetro opcional. Si se envía "Invert", invierte el resultado del booleano antes de convertir.</param>
        /// <param name="culture">La cultura que se usará en el convertidor.</param>
        /// <returns><see cref="Visibility.Visible"/> si es verdadero; de lo contrario, <see cref="Visibility.Collapsed"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = false;

            if (value is bool b)
            {
                boolValue = b;
            }
            else if (value is string s)
            {
                boolValue = !string.IsNullOrWhiteSpace(s);
            }
            else if (value != null)
            {
                boolValue = true;
            }

            if (parameter?.ToString() == "Invert")
                boolValue = !boolValue;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Realiza la conversión inversa de un objeto <see cref="Visibility"/> a un valor booleano.
        /// </summary>
        /// <param name="value">El valor de visibilidad de origen.</param>
        /// <param name="targetType">El tipo al que se va a convertir.</param>
        /// <param name="parameter">El parámetro que se va a usar.</param>
        /// <param name="culture">La cultura que se usará en el convertidor.</param>
        /// <returns><c>true</c> si el estado de visibilidad es Visible; de lo contrario, <c>false</c>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return visibility == Visibility.Visible;

            return false;
        }
    }
}
