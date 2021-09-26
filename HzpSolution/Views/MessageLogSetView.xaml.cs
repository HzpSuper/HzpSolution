using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HzpSolution.Views
{
    /// <summary>
    /// MessageLogSetView.xaml 的交互逻辑
    /// </summary>
    public partial class MessageLogSetView : UserControl
    {
        public MessageLogSetView()
        {
            InitializeComponent();
        }
    }

    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "不可为空")
                : ValidationResult.ValidResult;
        }
    }
}
