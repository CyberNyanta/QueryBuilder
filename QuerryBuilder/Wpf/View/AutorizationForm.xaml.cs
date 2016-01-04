using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.View
{
    /// <summary>
    /// Interaction logic for AutorizationForm.xaml
    /// </summary>
    public partial class AutorizationForm : Window
    {
        public AutorizationForm()
        {
           InitializeComponent();
        }

        public static implicit operator UserControl(AutorizationForm v)
        {
            throw new NotImplementedException();
        }
    }
}
