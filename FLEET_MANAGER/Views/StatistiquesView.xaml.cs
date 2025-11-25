using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using FLEET_MANAGER.ViewModels;

namespace FLEET_MANAGER.Views
{
    /// <summary>
    /// Logique d'interaction pour StatistiquesView.xaml
    /// </summary>
    public partial class StatistiquesView : UserControl
    {
        public StatistiquesView()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// Convertisseur pour convertir un pourcentage en largeur pour les barres de progression
    /// </summary>
    public class PercentageToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal pourcentage)
            {
                // La largeur de référence est de 300 pixels (100%)
                // Ajustez cette valeur selon vos besoins
                return (double)pourcentage * 3; // 300px pour 100%
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
