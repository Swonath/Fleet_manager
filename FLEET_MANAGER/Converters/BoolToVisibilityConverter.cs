using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FLEET_MANAGER.Converters
{
    /// <summary>
    /// Convertisseur pour transformer une valeur booleenne ou un objet en Visibility
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = false;

            if (value is bool boolValue)
            {
                isVisible = boolValue;
            }
            else if (value is string stringValue)
            {
                isVisible = !string.IsNullOrEmpty(stringValue);
            }
            else
            {
                // Pour les objets : visible si non null
                isVisible = value != null;
            }

            // Si le parametre est "Invert", on inverse la logique
            bool invert = parameter is string param && param.Equals("Invert", StringComparison.OrdinalIgnoreCase);

            if (invert)
                isVisible = !isVisible;

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return visibility == Visibility.Visible;

            return false;
        }
    }
}
