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

namespace QueryBuilderMVC.Controllers
{
    public class WorkflowController : Controller
    {
        // GET: Workflow
        private IUnitOfWorkFactory repository = new UnitOfWorkFactory();
        private ApplicationUser CurrentUser;
        private ProjectService serviceProject;
        private UserService serviceUser;
        private ProjectViewModel model = new ProjectViewModel();
        private ConnectionDbService serviceConnection;

        // GET: Product
        public WorkflowController(IUnitOfWorkFactory Repository)
        {
            this.repository = Repository;
        }

        [HttpGet]
        public ActionResult List(string id = null)
        {
            ProjectViewModel model = new ProjectViewModel();
            serviceConnection = new ConnectionDbService(repository);
            serviceProject = new ProjectService(repository);
            serviceUser = new UserService(repository);
            var conn = new ConnectionViewModel();
            CurrentUser = serviceUser.GetUserByID(User.Identity.GetUserId());
            if (id != null)
            {
                model.idCurrentProject = Convert.ToInt32(id);
                Project currentProject = serviceProject.GetProjects().First(a => a.ProjectID == model.idCurrentProject);
                model.Name = currentProject.ProjectName;
                model.Description = currentProject.ProjectDescription;
                var connection = serviceConnection.GetConnectionDB(model.idCurrentProject).FirstOrDefault();
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
            model.Projects = serviceProject.GetUserProjects(CurrentUser);

            return View(model);

        }


        [HttpPost]
        public ActionResult List(ProjectViewModel _model, string action)
        {
            serviceProject = new ProjectService(repository);
            serviceUser = new UserService(repository);
            serviceConnection = new ConnectionDbService(repository);
            CurrentUser = serviceUser.GetUserByID(User.Identity.GetUserId());


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
                    serviceProject.SaveProject(newProject);
                }

                model.Projects = serviceProject.GetUserProjects(CurrentUser);

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
                    serviceConnection.SaveConnection(newConnection);

                    ;
                }
                model.Projects = serviceProject.GetUserProjects(CurrentUser);

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
                    serviceProject.SaveProject(newProject);
                

                model.Projects = serviceProject.GetUserProjects(CurrentUser);

                return View("List", model);
            }


            else
            {
                ProjectViewModel model = new ProjectViewModel
                {
                    Projects = serviceProject.GetUserProjects(CurrentUser),
                    _ConnectionDb = serviceConnection.GetConnectionDB(_model.idCurrentProject)

                };

                return View("List", model);
            }






        }

    }
}