using BuilderBL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wpf.DataModel.Entity;

namespace Wpf.ViewModel
{
   partial class MainWindowFormViewModel:Notifier
    {
       private static ObservableCollection<Group> _list;
       
       public string SqlQuerry { get; set; }

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
