using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.DataModel;
using Wpf.View;

namespace Wpf.ViewModel
{
    class CreateProjectFormViewModel
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        private EntityManager entityManager;
        public ICommand AddConnectionCommand { get; set; }
        public ICommand CreateProjectCommand { get; set; }

        public CreateProjectFormViewModel()
        {
            entityManager = new EntityManager();
            AddConnectionCommand = new RelayCommand(arg => AddConnectionMethod());
            CreateProjectCommand = new RelayCommand(arg => CreateProjectMethod());
        }

        private void CreateProjectMethod()
        {
            //entityManager.SaveProject();
        }


        private void AddConnectionMethod()
        {
            var window = new ConnectionDbForm();
            window.ShowDialog();
        }
    }
}
