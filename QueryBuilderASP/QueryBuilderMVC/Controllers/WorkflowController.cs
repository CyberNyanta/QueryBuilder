using QueryBuilder.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueryBuilder.DAL.Models;
using QueryBuilderMVC.Models;
using QueryBuilder.DAL.Repositories;
using QueryBuilder.DAL.Contracts;
using Microsoft.AspNet.Identity;
using QueryBuilder.Services.DbServices;

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
            //ProjectViewModel model = new ProjectViewModel();

            serviceProject = new ProjectService(repository);
            serviceUser = new UserService(repository);
            CurrentUser = serviceUser.GetUserByID(User.Identity.GetUserId());
            if (id != null)
            {
                model.idCurrentProject = Convert.ToInt32(id);

            }

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
            else if (action == "Create new connection")
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
                        //PasswordDB = _model.ConnectionDb.PasswordDB,
                        ServerName = _model.ConnectionDb.ServerName,

                    };
                    serviceConnection.SaveConnection(newConnection);

                    ;
                }
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