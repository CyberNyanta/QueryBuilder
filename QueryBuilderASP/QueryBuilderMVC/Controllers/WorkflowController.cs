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

        public int PageSize = 3;
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
        public ViewResult List(ProjectViewModel _project)
        {
            var s = repository.GetUnitOfWork();

            if (ModelState.IsValid)
            {
                serviceProject.SaveProject(new Project { ProjectName = _project.Name, ProjectOwner = User.Identity.Name, ProjectDescription = _project.Description });
                //s.Projects.Create(new Project { ProjectName = _project.Name, ProjectOwner = User.Identity.Name, ProjectDescription = _project.Description });
                //s.Save();
               
            }
            ProjectViewModel model = new ProjectViewModel
            {
                Projects = serviceProject.GetUserProjects(CurrentUser)

            };
            return View("List", model);


        }

    }
}