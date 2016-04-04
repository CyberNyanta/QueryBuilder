using System;
using System.Web.Mvc;
using QueryBuilder.DAL.Models;
using QueryBuilderMVC.Models;
using QueryBuilder.DAL.Contracts;
using Microsoft.AspNet.Identity;
using QueryBuilder.Utils;
using System.Linq;
using AutoMapper;
using QueryBuilder.Services.Contracts;

namespace QueryBuilderMVC.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly IProjectService _serviceProject;
        private readonly IUserService _serviceUser;
        private readonly IConnectionDbService _serviceConnection;

        private readonly ProjectViewModel model = new ProjectViewModel();
        private ApplicationUser _currentUser;


        // GET: Product
        public WorkflowController(IProjectService serviceProject, IUserService serviceUser, 
            IProjectsShareService serviceProjectShare, IConnectionDbService serviceConnection)
        {
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
                model.IdCurrentProject = Convert.ToInt32(id);
                Project currentProject = _serviceProject.GetProjects().First(a => a.ProjectID == model.IdCurrentProject);
                model.Name = currentProject.ProjectName;
                model.Description = currentProject.ProjectDescription;
                var connection = _serviceConnection.GetConnectionDB(model.IdCurrentProject).FirstOrDefault();
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
        public ActionResult List(ProjectViewModel projectModel, string action)
        {
           _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            var conn = new ConnectionViewModel();

            if (action == "Create new project")
            {
                if (ModelState.IsValid)
                {
                    var newProject = Mapper.Map<ProjectViewModel, Project>(projectModel);
                    newProject.ProjectOwner = User.Identity.Name;
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
                if (projectModel.ConnectionDb.DatabaseName != null && projectModel.ConnectionDb.ConnectionName != null &&
                    projectModel.ConnectionDb.LoginDB != null && projectModel.ConnectionDb.ServerName != null)
                {
                    var newConnection = Mapper.Map<ConnectionViewModel, ConnectionDB>(projectModel.ConnectionDb);
                    newConnection.ConnectionOwner = projectModel.IdCurrentProject;
                   
                    _serviceConnection.SaveConnection(newConnection);
                }
                model.Projects = _serviceProject.GetUserProjects(_currentUser);

                return View("List", model);
            }

            else if (action == "Update project")
            {
                var newProject = Mapper.Map<ProjectViewModel, Project>(projectModel);
                newProject.ProjectOwner = User.Identity.Name;

                _serviceProject.SaveProject(newProject);
                
                model.Projects = _serviceProject.GetUserProjects(_currentUser);

                return View("List", model);
            }

            else
            {
                ProjectViewModel model = new ProjectViewModel
                {
                    Projects = _serviceProject.GetUserProjects(_currentUser),
                    _ConnectionDb = _serviceConnection.GetConnectionDB(projectModel.IdCurrentProject)

                };

                return View("List", model);
            }

        }

    }
}