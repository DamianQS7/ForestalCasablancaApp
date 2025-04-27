using System.Globalization;

namespace BosquesNalcahue.Converters
{
    class EmptyStringToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            string alternativeMessage = parameter as string ?? "Sin observaciones."; // Default message if no parameter is provided

            return string.IsNullOrWhiteSpace(input) ? alternativeMessage : input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
