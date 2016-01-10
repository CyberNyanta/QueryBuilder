using System;
using System.Diagnostics;
using System.Windows;
using Wpf.DataModel;
using Wpf.DataModel.Entity;
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

            EntityManager manager = new EntityManager();
            Users user=manager.RegistrationUser("Dmitriy", "Fokasii", "fokas.dima@yandex.ru", "12345");

            Debug.WriteLine(user.FirstName);

            



            //var ff = new AutorizationForm();
            //var ss = new RegistrationForm();
            //var dd = new ConnectionDBForm();
            //dd.Show();
            //ss.Show();
            //ff.Show();
        }
    }
}
