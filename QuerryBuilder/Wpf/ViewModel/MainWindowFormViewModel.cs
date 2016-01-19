using BuilderBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string SqlQuerry { get; set; }

        public MainWindowFormViewModel()
        {
            ClickAutorizationCommand = new RelayCommand(arg => ClickMethodAutorization());
            ClickAddConnectionCommand = new RelayCommand(arg => ClickMethodAddConection());
            ClickNewProjectCommand = new RelayCommand(arg => ClickMethodAddProject());
            _list = new ObservableCollection<Group>();
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
