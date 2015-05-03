using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MissAndCan
{
    /// <summary>
    /// Converter for output and input int in textbox
    /// </summary>
    [ValueConversion(typeof(string), typeof(int))]
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                int number;
                return Int32.TryParse(value as string, out number) ? number : 0;
            }
            else
            {
                return value;
            }
        }
    }
}
