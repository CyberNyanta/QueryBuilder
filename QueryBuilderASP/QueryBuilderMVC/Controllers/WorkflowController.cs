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

        private readonly ProjectViewModel projectModel = new ProjectViewModel();
        private readonly ConnectionViewModel connectionModel = new ConnectionViewModel();

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
                projectModel.Projects = _serviceProject.GetUserProjects(_currentUser);
                projectModel.IdCurrentProject = Convert.ToInt32(id);
                if (id != "0")
                {
                    Project currentProject = _serviceProject.GetProjects().FirstOrDefault(a => a.ProjectID == projectModel.IdCurrentProject);
                    ViewBag.name = currentProject.ProjectName;
                    ViewBag.desk = currentProject.ProjectDescription;
                }
                else
                {
                    ViewBag.name = "choose project";
                    ViewBag.desk = "No description";
                }
              

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
                projectModel.IdCurrentProject = proj[0].ProjectID;
                
                projectModel.Projects = proj;                    
                
            }

            return View(projectModel);
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
            projectModel.IdCurrentProject = Convert.ToInt32(id);
            Project currentProject = _serviceProject.GetProjects().FirstOrDefault(a => a.ProjectID == projectModel.IdCurrentProject);
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
            projectModel.IdCurrentProject = Convert.ToInt32(id);
            Project currentProject = _serviceProject.GetProjects().FirstOrDefault(a => a.ProjectID == projectModel.IdCurrentProject);
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

        [Authorize]
        public ActionResult CreateConnectionPartial(int id)
        {
            connectionModel.ConnectionOwner = id;

            return PartialView("CreateConnectionPartial", connectionModel);

        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateConnectionPartial(ConnectionViewModel connection)
        {
            if (ModelState.IsValid)
            {
                var newConnection = Mapper.Map<ConnectionViewModel, ConnectionDB>(connection);
                _serviceConnection.SaveConnection(newConnection);
                return PartialView("Success");

            }
            return PartialView("CreateConnectionPartial",connection);
        }

    }
}