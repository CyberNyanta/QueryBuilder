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

        private IUnitOfWorkFactory repository;
        private ApplicationUser CurrentUser;
        private ProjectService serviceProject;
        private UserService serviceUser;
        // GET: Product
        public WorkflowController(IUnitOfWorkFactory Repository)
        {
            this.repository = Repository;
        }
        [HttpGet]
        public ActionResult List()
        {
            serviceProject = new ProjectService(repository);
            serviceUser = new UserService(repository);
            CurrentUser = serviceUser.GetUserByID(User.Identity.GetUserId());

            var s = repository.GetUnitOfWork();
               var model = new ProjectViewModel
                {
                   
                    Projects = serviceProject.GetUserProjects(CurrentUser)

                };
                return View(model);

        }

        [HttpPost]
        public ActionResult List(ProjectViewModel _model, string action)
        {
            serviceProject = new ProjectService(repository);
            serviceUser = new UserService(repository);
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

                ProjectViewModel model = new ProjectViewModel
                {
                    Projects = serviceProject.GetUserProjects(CurrentUser)

                };
                return View("List", model);
            }
            else if(action== "Create new connection")
            {
                if (ModelState.IsValid)
                {
                    var newConnection = new ConnectionDB
                    {
                        ConnectionName = _model.ConnectionDb.ConnectionName,
                        //ConnectionOwner = 

                    };

                }
                ProjectViewModel model = new ProjectViewModel
                {
                    Projects = serviceProject.GetUserProjects(CurrentUser)

                };
                return View("List", model);
            }
            else
            {
                ProjectViewModel model = new ProjectViewModel
                {
                    Projects = serviceProject.GetUserProjects(CurrentUser)

                };

                return View("List", model);
            }






        }

    }
}