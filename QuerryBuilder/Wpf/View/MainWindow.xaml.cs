﻿using System.Windows;
using Wpf.View;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var ff = new AutorizationForm();
            var ss = new RegistrationForm();
            ss.Show();
            ff.Show();
        }
    }
}
