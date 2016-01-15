using System;
using System.Windows;
using System.Windows.Input;
using Wpf.View;

namespace Wpf.ViewModel
{
    class MainWindowFormViewModel
    {
        public ICommand ClickAutorizationCommand { get; set; }
        public ICommand ClickAddConnectionCommand { get; set; }
        public ICommand ClickCloseCommand { get; set; }

        public MainWindowFormViewModel()
        {
            ClickAutorizationCommand = new RelayCommand(arg => ClickMethodAutorization());
            ClickAddConnectionCommand = new RelayCommand(arg => ClickMethodAddConection());

        }
        /// <summary>
        /// Метод команды, вызывающий форму авторизации
        /// </summary>
        private void ClickMethodAutorization()
        {
            var windowAutorizationForm = new AutorizationForm();
            windowAutorizationForm.ShowDialog();

        }
        /// <summary>
        /// Метод команды, вызывающий форму подключения к базе данных
        /// </summary>
        private void ClickMethodAddConection()
        {
            var windowConnectionDbForm = new ConnectionDbForm();
            windowConnectionDbForm.Show();
        }

        /// <summary>
        /// Метод загружающий последнее местоположение окна
        /// 
        /// ПОКА НИГДЕ НЕ ИСПОЛЬЗУЕТСЯ
        /// 
        /// </summary>
        /// <param name="win"></param>
        public static void LoadSettings(Window win)
        {
            // Для сохранение местоположения окна использовать 
            // строки кода ниже
            //Properties.Settings.Default.SaveWindow = win.RestoreBounds;
            //Properties.Settings.Default.Save();

            try
            {
                Rect bounds = Properties.Settings.Default.SaveWindow;
                win.Top = bounds.Top;
                win.Left = bounds.Left;


                if (win.SizeToContent == SizeToContent.Manual)
                {
                    win.Width = bounds.Width;
                    win.Height = bounds.Height;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
