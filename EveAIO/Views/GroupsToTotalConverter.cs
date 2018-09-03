namespace EveAIO.Views
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows.Data;

    public class GroupsToTotalConverter : IValueConverter
    {
        public GroupsToTotalConverter()
        {
            Class7.RIuqtBYzWxthF();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ReadOnlyObservableCollection<object>))
            {
                return "";
            }
            decimal d = 0M;
            foreach (object local1 in (ReadOnlyObservableCollection<object>) value)
            {
                d = decimal.op_Increment(d);
            }
            return ("(" + d + ")");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
            value;
    }
}

