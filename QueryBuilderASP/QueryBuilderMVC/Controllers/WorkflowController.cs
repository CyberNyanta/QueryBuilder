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

        public ActionResult List()
        {
            _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            model.Projects = _serviceProject.GetUserProjects(_currentUser);
            return View(model);
        }

        public ActionResult CreateProjectPartial()
        {
            return PartialView("CreateProjectPartial");
        }

        [HttpPost]
        public ActionResult CreateProjectPartial(ProjectViewModel projectModel)
        {
            _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            var conn = new ConnectionViewModel();
            if (ModelState.IsValid)
            {
                var newProject = Mapper.Map<ProjectViewModel, Project>(projectModel);
                newProject.ProjectOwner = User.Identity.Name;
                _serviceProject.SaveProject(newProject);
            }
            model.ConnectionDb = conn;
            model.Projects = _serviceProject.GetUserProjects(_currentUser);
            return RedirectToAction("List");
        }

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
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateProjectPartial(ProjectViewModel project)
        {
            var newProject = Mapper.Map<ProjectViewModel, Project>(project);
            newProject.ProjectOwner = User.Identity.Name;
            _serviceProject.SaveProject(newProject);
            return RedirectToAction("List");
        }


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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteProjectPartial(ProjectViewModel project)
        {
            var newProject = Mapper.Map<ProjectViewModel, Project>(project);
            newProject.ProjectOwner = User.Identity.Name;
            newProject.Delflag = 1;
            _serviceProject.SaveProject(newProject);
            return RedirectToAction("List");
        }

        
    }
}