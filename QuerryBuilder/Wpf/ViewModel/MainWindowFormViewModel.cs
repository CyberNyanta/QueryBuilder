using BuilderBL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
<<<<<<< HEAD
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.DataModel.Entity;
using Wpf.View;
using Wpf.ViewModel.Command;
=======
using Wpf.DataModel.Entity;
>>>>>>> c02a5f93e328c3b085d11c7fb9f952d80c7f17d6

namespace Wpf.ViewModel
{
   partial class MainWindowFormViewModel:Notifier
    {
       private static ObservableCollection<Group> _list;
       
       public string SqlQuerry { get; set; }

<<<<<<< HEAD
       private Users _currentUser;

       private bool _canExecute=false;
=======
        private Users _currentUser;
       private string _firstname;
>>>>>>> c02a5f93e328c3b085d11c7fb9f952d80c7f17d6

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
            get { return _list; }
            set
            {
                _list = value;
                OnPropertyChanged("List");
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
