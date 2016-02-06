using BuilderBL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.DataModel.Entity;
using Wpf.View;
using Wpf.ViewModel.Command;



namespace Wpf.ViewModel
{
   partial class MainWindowFormViewModel:Notifier
    {      
       public string SqlQuerry { get; set; }

       private bool _canExecute=false;

       private Users _currentUser;
       private string _firstname;

       public string FirstName {
            get { return _firstname; }
            set
            {
                _firstname = value;
                OnPropertyChanged("FirstName");
            }
        }
       

        public ObservableCollection<Group> List
        {
            get { return MainWindowData.UserConnections; }
            set
            {
                MainWindowData.UserConnections = value;
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
                MainWindowData.UserConnections.Add(newGroup);
            }
        }

        #region ICommand members


        public Users CurrentUser
        {
            get
            {
                return _currentUser;
            }

            set
            {
                _currentUser = value;
                CanExecute = true;
            }
        }

        public bool CanExecute
        {
            get
            {
                return _canExecute;
            }

            set
            {
                _canExecute = value;
            }
        }

        private void EditProject_CommandExecute()
        {
        }
        #endregion
    }
}
