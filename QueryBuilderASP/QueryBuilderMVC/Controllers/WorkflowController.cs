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
using System.Collections.Generic;

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

        //public ActionResult List()
        //{
        //    _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
        //    model.Projects = _serviceProject.GetUserProjects(_currentUser);
        //    return View(model);
        //}
        [HttpGet]
        public ActionResult List(string id="0")
        {
            if (User.Identity.IsAuthenticated)
            {
                _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
                model.Projects = _serviceProject.GetUserProjects(_currentUser);
                model.IdCurrentProject = Convert.ToInt32(id);
            }
            else
            {
                var proj = new List<Project>();
                proj.Add(new Project
                {
                    ProjectID = 1,
                    ProjectName = "Example",
                    ProjectDescription = "This project for demonstration service"
                });
                model.IdCurrentProject = proj[0].ProjectID;
                
                model.Projects = proj;                    
                
            }

            return View(model);
        }

        [Authorize]
        public ActionResult CreateProjectPartial()
        {
            return PartialView("CreateProjectPartial");
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateProjectPartial(ProjectViewModel projectModel)
        {
            _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                var newProject = Mapper.Map<ProjectViewModel, Project>(projectModel);
                newProject.ProjectOwner = User.Identity.Name;
                _serviceProject.SaveProject(newProject);
                return PartialView("Success");
            }
            return PartialView("CreateProjectPartial");
        }
        [Authorize]
        public ActionResult UpdateProjectPartial(int id)
        {
            model.IdCurrentProject = Convert.ToInt32(id);
            Project currentProject = _serviceProject.GetProjects().FirstOrDefault(a => a.ProjectID == model.IdCurrentProject);
            var newProject = Mapper.Map<Project, ProjectViewModel>(currentProject);
            if (newProject != null)
            {
                return PartialView("UpdateProjectPartial", newProject);
            }
            return View("List");
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateProjectPartial(ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                var newProject = Mapper.Map<ProjectViewModel, Project>(project);
                newProject.ProjectOwner = User.Identity.Name;
                _serviceProject.SaveProject(newProject);
                return PartialView("Success");
            }
            return PartialView("UpdateProjectPartial", project);
        }

        [Authorize]
        public ActionResult DeleteProjectPartial(int id)
        {
            model.IdCurrentProject = Convert.ToInt32(id);
            Project currentProject = _serviceProject.GetProjects().FirstOrDefault(a => a.ProjectID == model.IdCurrentProject);
            var newProject = Mapper.Map<Project, ProjectViewModel>(currentProject);
            if (newProject != null)
            {
                return PartialView("DeleteProjectPartial", newProject);
            }
            return View("List");
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeleteProjectPartial(ProjectViewModel project)
        {
            var newProject = Mapper.Map<ProjectViewModel, Project>(project);
            newProject.ProjectOwner = User.Identity.Name;
            newProject.Delflag = 1;
            _serviceProject.SaveProject(newProject);
            return PartialView("Success");
        }


    }
}