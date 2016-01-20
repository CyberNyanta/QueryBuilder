using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.View;
using Wpf.ViewModel.Command;

namespace Wpf.ViewModel
{
    class CreateProjectFormViewModel
    {
        public string Name { get; set; }
        public List<string> MyConnectionList { get; set; }
        public string Summary { get; set; }
        private EntityManager entityManager;
        public List<string> UserConnection { get; set; }
        public ICommand AddConnectionCommand { get; set; }
        public ICommand CreateProjectCommand { get; set; }

        public CreateProjectFormViewModel()
        {
            entityManager = new EntityManager();
            AddConnectionCommand = new RelayCommand(arg => AddConnectionMethod());
            CreateProjectCommand = new RelayCommand(arg => CreateProjectMethod());
            foreach (var p in entityManager.GetUserConnections(entityManager.User))
            {
                UserConnection.Add(p.DatabaseName);
            }
           
        }

        private void CreateProjectMethod()
        {
            entityManager.SaveProject(Name, entityManager.User.Email, Summary);
        }


        private void AddConnectionMethod()
        {
            var window = new ConnectionDbForm();
            window.ShowDialog();
        }
    }
}
