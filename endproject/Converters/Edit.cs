using System.Globalization;

namespace endproject.Converters;

public class Edit: IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return (bool)value ? "Edit" : "Add";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
