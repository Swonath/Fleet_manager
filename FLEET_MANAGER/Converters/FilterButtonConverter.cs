using System.Globalization;
using System.Windows.Data;

namespace FLEET_MANAGER.Converters
{
    /// <summary>
    /// Convertisseur pour determiner si un bouton de filtre est actif
    /// </summary>
    public class FilterButtonConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is string currentFilter && values[1] is string buttonFilter)
            {
                return currentFilter == buttonFilter ? "Active" : null;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
