using BuilderBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wpf.ViewModel
{
    class TreeViewHelper:Notifier
    {

        Dictionary<string, Dictionary<string, List<string>>> _dictionaryCollection;

        private ObservableCollection<Dictionary<string, Dictionary<string, List<string>>>> _list;
        
        private MainWindowFormViewModel MainWin { get; set; }

        public TreeViewHelper()
        {
            _list = new ObservableCollection<Dictionary<string, Dictionary<string, List<string>>>>();
        }

        public ObservableCollection<Dictionary<string, Dictionary<string, List<string>>>> List
        {
            get { return _list; }
            set
            {
                _list = value;
                OnPropertyChanged("List");
            }
        }

        private string _connString;

        public string ConnString
        {
            get{return _connString;}
            set{ _connString = value;}
        } 

        private Dictionary<String, List<String>> _dictionaryTables;

        public Dictionary<string, List<string>> DictionaryTables
        {
            get{ return _dictionaryTables;}
            set{ _dictionaryTables = value;}
        }

        private string _nameOfDB;

        public string NameOfDB
        {
            get{ return _nameOfDB; }

            set{ _nameOfDB = value; }
        }

        public void UpdateTable(string connString)
        {
            DbSchema temp = new DbSchema(connString);
            DictionaryTables = temp.GetTableEntities(temp);
            _dictionaryCollection = new Dictionary<string, Dictionary<string, List<string>>>();
            _dictionaryCollection.Add(connString, DictionaryTables);
            List.Add(_dictionaryCollection);
        }

    }

}

