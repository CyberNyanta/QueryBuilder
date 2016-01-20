using BuilderBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.View;

namespace Wpf.ViewModel
{
   partial class MainWindowFormViewModel:Notifier
    {
       private static ObservableCollection<Group> _list;
       
       public string SqlQuerry { get; set; }

        private Users _currentUser;

       public string FirstName { get; set; }
       

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
