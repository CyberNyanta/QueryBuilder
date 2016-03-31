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

namespace QueryBuilderMVC.Controllers
{
    public class WorkflowController : Controller
    {
        // GET: Workflow

        private IUnitOfWorkFactory repository;

        public int PageSize = 3;
        // GET: Product
        public WorkflowController(IUnitOfWorkFactory Repository)
        {
            this.repository = Repository;
        }
        [HttpGet]
        public ActionResult List()
        {

                var s = repository.GetUnitOfWork();
               var model = new ProjectViewModel
                {
                    Projects = s.Projects.GetAll().Where(x => x.ProjectOwner == User.Identity.Name)

                };
                return View(model);

        }

        [HttpPost]
        public ViewResult List(ProjectViewModel _project)
        {
            var s = repository.GetUnitOfWork();

            if (ModelState.IsValid)
            {

                s.Projects.Create(new Project { ProjectName = _project.Name, ProjectOwner = User.Identity.Name, ProjectDescription = _project.Description });
                s.Save();
               
            }
            ProjectViewModel model = new ProjectViewModel
            {
                Projects = s.Projects.GetAll()

            };
            return View("List", model);


        }

    }
}