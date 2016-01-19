using BuilderBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.View;

namespace Wpf.ViewModel
{
   class MainWindowFormViewModel:Notifier
    {
        
        private static ObservableCollection<Group> _list;

        public ICommand ClickAutorizationCommand { get; set; }
        public ICommand ClickAddConnectionCommand { get; set; }
        public ICommand ClickCloseCommand { get; set; }
        public ICommand ClickNewProjectCommand { get; set; }
        public ICommand ClickSaveExcelCommand { get; set; }
        public ICommand ClickSaveTxtCommand { get; set; }
        public ICommand ClickSavePdfCommand { get; set; }
        public ICommand ClickSendQuerryToEmailCommand { get; set; }

        public string SqlQuerry { get; set; }

        public MainWindowFormViewModel()
        {
            ClickAutorizationCommand = new RelayCommand(arg => ClickMethodAutorization());
            ClickAddConnectionCommand = new RelayCommand(arg => ClickMethodAddConection());
            ClickNewProjectCommand = new RelayCommand(arg => ClickMethodAddProject());
            ClickSaveExcelCommand = new RelayCommand(arg => ClickMethodSaveExcel());
            ClickSaveTxtCommand = new RelayCommand(arg => ClickMethodSaveTxt());
            ClickSavePdfCommand = new RelayCommand(arg => ClickMethodSavePdf());
            ClickSendQuerryToEmailCommand = new RelayCommand(arg => ClickMethodSendQuerryToEmail());
            _list = new ObservableCollection<Group>();
        }

        private void ClickMethodSendQuerryToEmail()
        {
           var SendMailWindow = new Email();
           SendMailWindow.ShowDialog();

        }
       
        /// <summary>
        /// Метод команды, сохраняющий запрос в TXT
        /// </summary>
        private void ClickMethodSaveTxt()
        {
            SaveTXT(SqlQuerry);
        }

        public static void SaveTXT(string querry)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "ResultQuerry"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents |*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                using (var sw = new StreamWriter(filename))
                {
                    sw.WriteLine(querry);
                }
            }
        }

        /// <summary>
        /// Метод команды, сохраняющий результат запроса в PDF
        /// </summary>
        private void ClickMethodSavePdf()
        {
            DataTable cc = new DataTable();
            cc.Columns.Add("Column1");
            cc.Rows.Add("1");
            cc.Rows.Add("2");
            SavePDF(cc, "title");
        }

        public static void SavePDF(DataTable table, string title)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "ResultQuerry"; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "Text documents |*.pdf"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                var file = ServicesLib.DataTableToPdfExporter.CreateInstance();
                file.DataTableExport(table, filename, title);
            }
        }

        private void ClickMethodAddProject()
        {
            var windowProject = new CreateProjectForm();
            windowProject.ShowDialog();
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
        /// Метод команды, сохраняющий результат запроса в Эксель
        /// </summary>
        private void ClickMethodSaveExcel()
        {
            DataTable table = null;
            SaveExcel(table, "title");
        }
        /// <summary>
        /// Метод сохранения в Эксель. Принимает таблицу и заголовок.
        /// Вызывает диалоговое окно сохранения в файл
        /// </summary>
        /// <param name="table"></param>
        /// <param name="Title"></param>
        public static void SaveExcel(DataTable table, string Title)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "ResultQuerry"; // Default file name
            dlg.DefaultExt = ".xlsx"; // Default file extension
            dlg.Filter = "Text documents |*.xlsx"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                var file = ServicesLib.DataTableToExcelExporter.CreateInstance();
                file.DataTableExport(table, filename, Title);
            }
        }
        public ObservableCollection<Group> List
        {
            get { return _list; }
            set
            {
                _list = value;
            }
        }

        public static void UpdateTable(string connString, string dbName)
        {
            var _schema = new DbSchema(connString);
            var newGroup = new Group { Name =dbName , SubGroups = new List<Group>(), Entries = new List<Entry>() };

            if (_schema != null)
            {
                foreach (var dt in _schema.GetTableEntities(_schema))
                {
                    var temp = new Group { Name = dt.Key, SubGroups = new List<Group>(), Entries = new List<Entry>() };
                    foreach (var entry in dt.Value)
                    {
                        temp.Entries.Add(new Entry() { Name = entry });
                    }
                    newGroup.SubGroups.Add(temp);
                }
                _list.Add(newGroup);

            }
        }
    }
}
