using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FLEET_MANAGER.Converters
{
    /// <summary>
    /// Convertisseur pour transformer une valeur booléenne en Visibility
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // Si le paramètre est "Invert", on inverse la logique
                bool invert = parameter is string param && param.Equals("Invert", StringComparison.OrdinalIgnoreCase);
                
                if (invert)
                    boolValue = !boolValue;

                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return visibility == Visibility.Visible;

            return false;
        }
    }
}
