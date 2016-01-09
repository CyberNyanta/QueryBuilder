using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Wpf.ViewModel;

namespace Wpf.View
{
    /// <summary>
    /// Interaction logic for AutorizationForm.xaml
    /// </summary>
    public partial class AutorizationForm : Window
    {
        private ValidationData User;
        public AutorizationForm()
        {
           InitializeComponent();
            User = new ValidationData();
       this.DataContext = User;
        }
        
    }
}
