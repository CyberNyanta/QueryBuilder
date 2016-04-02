using QueryBuilder.DAL.Infrastructure;
using System;
using System.Web.Mvc;
using QueryBuilder.DAL.Models;
using QueryBuilderMVC.Models;
using QueryBuilder.DAL.Contracts;
using Microsoft.AspNet.Identity;
using QueryBuilder.Services.DbServices;
using QueryBuilder.Utils;
using System.Linq;
using QueryBuilder.Services.Contracts;

namespace QueryBuilderMVC.Controllers
{
    public class WorkflowController : Controller
    {
        private IUnitOfWorkFactory _repository;
        private IProjectService _serviceProject;
        private IUserService _serviceUser;
        private IConnectionDbService _serviceConnection;

        private ProjectViewModel model = new ProjectViewModel();
        private ApplicationUser _currentUser;


        // GET: Product
        public WorkflowController(IUnitOfWorkFactory repository, IProjectService serviceProject, IUserService serviceUser, 
            IProjectsShareService serviceProjectShare, IConnectionDbService serviceConnection)
        {
            _repository = repository;
            _serviceProject = serviceProject;
            _serviceUser = serviceUser;
            _serviceConnection = serviceConnection;
        }

        [HttpGet]
        public ActionResult List(string id = null)
        {
            ProjectViewModel model = new ProjectViewModel();
          
            var conn = new ConnectionViewModel();
            _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            if (id != null)
            {
                model.idCurrentProject = Convert.ToInt32(id);
                Project currentProject = _serviceProject.GetProjects().First(a => a.ProjectID == model.idCurrentProject);
                model.Name = currentProject.ProjectName;
                model.Description = currentProject.ProjectDescription;
                var connection = _serviceConnection.GetConnectionDB(model.idCurrentProject).FirstOrDefault();
                if (connection != null)
                {
                    
                    conn.DatabaseName = connection.DatabaseName;
                    conn.LoginDB = connection.LoginDB;
                    conn.ServerName = connection.ServerName;
                   conn.ConnectionName = connection.ConnectionName;
                    conn.ConnectionOwner = connection.ConnectionOwner;

                }
            }
            model.ConnectionDb = conn;
            model.Projects = _serviceProject.GetUserProjects(_currentUser);

            return View(model);

        }


        [HttpPost]
        public ActionResult List(ProjectViewModel _model, string action)
        {
           _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            var conn = new ConnectionViewModel();



            if (action == "Create new project")
            {
                if (ModelState.IsValid)
                {
                    var newProject = new Project
                    {
                        ProjectName = _model.Name,
                        ProjectOwner = User.Identity.Name,
                        ProjectDescription = _model.Description
                    };
                    _serviceProject.SaveProject(newProject);
                }
                model.ConnectionDb = conn;

                model.Projects = _serviceProject.GetUserProjects(_currentUser);

                return View("List", model);
            }
            else if (action == "Save connection")
            {
                ///
                /// ПОКА так :(
                ///
                if (_model.ConnectionDb.DatabaseName != null && _model.ConnectionDb.ConnectionName != null &&
                    _model.ConnectionDb.LoginDB != null && _model.ConnectionDb.ServerName != null)
                {
                    var newConnection = new ConnectionDB
                    {
                        ConnectionName = _model.ConnectionDb.ConnectionName,
                        ConnectionOwner = _model.idCurrentProject,
                        DatabaseName = _model.ConnectionDb.DatabaseName,
                        LoginDB = _model.ConnectionDb.LoginDB,
                        PasswordDB = Scrambler.GetPassHash(_model.ConnectionDb.PasswordDB),
                        ServerName = _model.ConnectionDb.ServerName,

                    };
                    _serviceConnection.SaveConnection(newConnection);

                    ;
                }
                model.Projects = _serviceProject.GetUserProjects(_currentUser);

                return View("List", model);
            }


            else if (action == "Update project")
            {

                
                    var newProject = new Project
                    {
                        ProjectName = _model.Name,
                        ProjectID = _model.idCurrentProject,
                        ProjectOwner = User.Identity.Name,
                        ProjectDescription = _model.Description
                    };
                    _serviceProject.SaveProject(newProject);
                

                model.Projects = _serviceProject.GetUserProjects(_currentUser);

                return View("List", model);
            }


            else
            {
                ProjectViewModel model = new ProjectViewModel
                {
                    Projects = _serviceProject.GetUserProjects(_currentUser),
                    _ConnectionDb = _serviceConnection.GetConnectionDB(_model.idCurrentProject)

                };

                return View("List", model);
            }






        }

    }
}