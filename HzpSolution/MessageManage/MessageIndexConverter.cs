using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace HzpSolution
{
    [ValueConversion(typeof(Int32), typeof(ListViewItem))]
    public class MessageIndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            ListBoxItem item = (ListBoxItem)value;
            return ItemsControl.ItemsControlFromItemContainer(item) is ListBox listView
                ? listView.ItemContainerGenerator.IndexFromContainer(item) + 1
                : -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
