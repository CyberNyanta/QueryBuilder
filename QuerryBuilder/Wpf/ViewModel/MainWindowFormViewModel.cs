using BuilderBL;
using BuilderBL.SQLDesigner;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Wpf.DataModel;
using Wpf.DataModel.Entity;



namespace Wpf.ViewModel
{
    partial class MainWindowFormViewModel : Notifier
    {
        public string SqlQuerry {
            get { return _builder?.Sql; }
            
        }

        private bool _canExecute = false;

        private Users _currentUser;
        private string _firstname;

        #region QueryBuilder
        static QueryBuilder _builder;

        public ObservableCollection<QueryField> QueryList
        {
            get { return _builder?.QueryFields; }
        }


        public void AddField(object item)
        {

            var dataColumn = item as Entry;
            if (dataColumn != null)
            {
                AddColumn(_builder.DataTables[ dataColumn.Parent].Columns[dataColumn.Name]);
            }
            var dataTable = item as Group;
            if (dataTable != null && dataTable.SubGroups.Count == 0)
            {
                AddTable(_builder.DataTables[dataTable.Name]);
            }
        }
        void AddTable(DataTable dataTable)
        {
            var field = new QueryField(dataTable);
            _builder.QueryFields.Add(field);
            OnPropertyChanged("QueryList");
            OnPropertyChanged("SqlQuerry");
        }

        void AddColumn(DataColumn dc)
        {
            var field = new QueryField(dc);
            _builder.QueryFields.Add(field);
            OnPropertyChanged("QueryList");
            OnPropertyChanged("SqlQuerry");
        }


        #endregion
        public string FirstName
        {
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
            var schema = new DbSchema(connString);
            _builder = new QueryBuilder(schema);
            var newGroup = new Group { Name = dbName, SubGroups = new List<Group>(), Entries = new List<Entry>() };

            if (schema != null)
            {
                foreach (var dt in schema.GetTableEntities(schema))
                {
                    var temp = new Group { Name = dt.Key, SubGroups = new List<Group>(), Entries = new List<Entry>() };
                    foreach (var entry in dt.Value)
                    {
                        temp.Entries.Add(new Entry() { Name = entry, Parent = dt.Key });
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
