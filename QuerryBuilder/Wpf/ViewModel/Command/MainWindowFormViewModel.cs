using BuilderBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using QueryBuilder.DAL.Models;
using QueryBuilder.Services.Contracts;
using QueryBuilder.Services.DbServices;
using Wpf.DataModel;
using Wpf.DataModel.Entity;
using Wpf.View;
using Wpf.ViewModel.Command;

namespace Wpf.ViewModel
{
    partial class MainWindowFormViewModel
    {
        #region ICommand fields
        private ICommand _clickNewProjectCommand;
        private ICommand _clickSaveProjectCommand;
        private ICommand _clickOpenProjectCommand;
        private ICommand _editButton;
        private EntityManager entityManager;

        private readonly IProjectService _projectService;


        #endregion ICommand fields

        #region ICommand independt properties
        public ICommand ClickAutorizationCommand{ get; set; }
        public ICommand ClickAddConnectionCommand { get; set; }
        public ICommand ClickCloseCommand { get; set; }
        public ICommand ClickSaveExcelCommand { get; set; }
        public ICommand ClickSaveTxtCommand { get; set; }
        public ICommand ClickSavePdfCommand { get; set; }
        public ICommand ClickSendQuerryToEmailCommand { get; set; }
        public ICommand ClickBuildErModelCommand { get; set; }
        public ICommand ClickChangeQuerryCommand { get; set; }
		public ICommand ClickRefreshQuerryCommand { get; set; }
		public ICommand ClickDeleteRowCommand { get; set; }
		#endregion

		#region ICommand depenency properties
		public ICommand ClickNewProjectCommand
        {
            get
            {
                if (_clickNewProjectCommand == null)
                    _clickNewProjectCommand = new RelayCommand(arg => ClickMethodAddProject(), exec => CanExecute);
                return _clickNewProjectCommand;
            }
            set
            {
                _clickNewProjectCommand = value;
            }
        }

        public ICommand ClickOpenProjectCommand
        {
            get
            {
                if (_clickOpenProjectCommand == null)
                    _clickOpenProjectCommand = new RelayCommand(arg => ClickMethodOpenProject(), exec => CanExecute);
                return _clickOpenProjectCommand;
            }
            set
            {
                _clickOpenProjectCommand = value;
            }
        }

        public ICommand ClickSaveProjectCommand
        {
            get
            {
                if (_clickSaveProjectCommand == null)
                    _clickSaveProjectCommand = new RelayCommand(arg => ClickMethodSaveProject(), exec => CanExecute);
                return _clickSaveProjectCommand;
            }
            set
            {
                _clickNewProjectCommand = value;
            }
        }
        public ICommand EditButton
        {
            get
            {
                if (_editButton == null)
                    _editButton = new RelayCommand(arg => EditProject_CommandExecute(), exec => CanExecute);
                return _editButton;
            }

            set
            {
                _editButton = value;
            }
        }

        #endregion

        public ICommand ClickAddUserInProjectCommand { get; set; }

        
        #region ctor
        public MainWindowFormViewModel()
        {
            ClickAddUserInProjectCommand = new RelayCommand(arg => ClickMethodAddUserInProject());
            ClickBuildErModelCommand = new RelayCommand(arg => ClickMethodBuildErModel());
            ClickChangeQuerryCommand = new RelayCommand(arg => ClickMethodChangeQuerry());
            ClickAutorizationCommand = new RelayCommand(arg => ClickMethodAutorization());
            ClickAddConnectionCommand = new RelayCommand(arg => ClickMethodAddConection());
            ClickSaveExcelCommand = new RelayCommand(arg => ClickMethodSaveExcel());
            ClickSaveTxtCommand = new RelayCommand(arg => ClickMethodSaveTxt());
            ClickSavePdfCommand = new RelayCommand(arg => ClickMethodSavePdf());
            ClickSendQuerryToEmailCommand = new RelayCommand(arg => ClickMethodSendQuerryToEmail());
			ClickRefreshQuerryCommand = new RelayCommand(arg => ClickMethodRefreshQuerryCommand());
			ClickDeleteRowCommand = new RelayCommand(arg => ClickMethodDeleteRowCommand());

			MainWindowData.UserConnections = new ObservableCollection<Group>();
            CanExecute = false;
            _currentUser = new User();
            FirstName = "SignIn please";

            var servicesFactory = new ServicesFactory();
            _projectService = servicesFactory.GetProjectService();

            //_builder = new BuilderBL.SQLDesigner.QueryBuilder(new DbSchema());
            //SqlQuerry = MainWindowData.SqlQuerry;
            //MessageBox.Show("For using  all functionality of the application, you have to register or sign-in");
        }

        private void ClickMethodAddUserInProject()
        {
            if (MainWindowData.ProjectName != null)
            {
                var window = new AddUsersToEmailWindow();
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Create or open project");

            }
        }

        #endregion 
        private void ClickMethodSaveProject()
        {
            entityManager.SaveProject(MainWindowData.ProjectName, 
                MainWindowData.ProjectOwner, MainWindowData.DescriptionProject);
        }

        private void ClickMethodOpenProject()
        {
            _projectService.GetUserProjects(MainWindowData.CurrentUser);
        }


        private void ClickMethodBuildErModel()
        {
            try
            {
                var win = new GraphSharpDemo.MainWindow();
                win.Show();
            }
            catch (Exception)
            {

                MessageBox.Show("You don't have active connection to database");
            }

        }


        private void ClickMethodChangeQuerry()
        {
            MainWindowData.SqlQuerry = SqlQuerry;
        }

        private void ClickMethodSendQuerryToEmail()
        {
            
            var SendMailWindow = new Email();
            SendMailWindow.ShowDialog();

        }

		private void ClickMethodRefreshQuerryCommand()
		{
			OnPropertyChanged("SqlQuerry");
		}
		private void ClickMethodDeleteRowCommand()
		{
			QueryList.RemoveAt(QueryListSelectedIndexyList);
			OnPropertyChanged("QueryList");
			OnPropertyChanged("SqlQuerry");
		}

		/// <summary>
		/// Метод команды, сохраняющий запрос в TXT
		/// </summary>
		private void ClickMethodSaveTxt()
        {
            SaveTXT(MainWindowData.SqlQuerry);
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
            CurrentUser = MainWindowData.CurrentUser;

            if (MainWindowData.CurrentUser != null)
            {
                FirstName = MainWindowData.CurrentUser.FirstName;
                OnPropertyChanged("FirstName");
            }


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
    }
}
