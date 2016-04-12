﻿using System;
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
using System.Text;
using System.Web.Configuration;
using QueryBuilder.Constants;
using QueryBuilder.Utils.Mailers;

namespace QueryBuilderMVC.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly IProjectService _serviceProject;
        private readonly IUserService _serviceUser;
        private readonly IConnectionDbService _serviceConnection;
        private readonly IProjectsShareService _serviceProjectsShareService;

        private readonly ProjectViewModel _projectModel = new ProjectViewModel();
        private readonly ConnectionViewModel _connectionModel = new ConnectionViewModel();
        private readonly ProjectsListViewModel _projectListModel = new ProjectsListViewModel();

        private ApplicationUser _currentUser;

        // GET: Product
        public WorkflowController(IProjectService serviceProject, IUserService serviceUser, 
            IProjectsShareService serviceProjectsShare, IConnectionDbService serviceConnection)
        {
            _serviceProject = serviceProject;
            _serviceUser = serviceUser;
            _serviceConnection = serviceConnection;
            _serviceProjectsShareService = serviceProjectsShare;
        }

        [HttpGet]
        public ActionResult List(string id="0")
        {
            if (User.Identity.IsAuthenticated)
            {
                _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
                var projects = _serviceProjectsShareService.GetUserProjects(_currentUser);                
                var projectsViewModel = Mapper.Map<IEnumerable<Project>, IEnumerable<ProjectsListViewModel>>(projects).ToList();
                var countInvited = 0;
                foreach (var project in projectsViewModel)
                {
                    project.UserRole = _serviceProjectsShareService.GetUserRole(_currentUser, project.ProjectID);
                    if (project.UserRole == 0)
                    {
                        countInvited++;
                    }
                }

                ViewBag.CountInvited = countInvited;
                _projectModel.Projects = projectsViewModel;
                _projectModel.IdCurrentProject = Convert.ToInt32(id);
                if (id != "0")
                {
                    var connectionsCurrentProject = _serviceConnection.GetConnectionDBs(_projectModel.IdCurrentProject);
                    _projectModel.ConnectionDbs = Mapper.Map<IEnumerable<ConnectionDB>, IEnumerable<ConnectionsListViewModel>>(connectionsCurrentProject).ToList();
                    
                    var currentProject = _serviceProject.GetProject(_projectModel.IdCurrentProject);
                    if (currentProject != null)
                    {
                        ViewBag.name = currentProject.ProjectName;
                        ViewBag.desk = currentProject.ProjectDescription;
                    }
                    ViewBag.Count = _projectModel.ConnectionDbs.Count();

                    if (ViewBag.Count == 0)
                    {
                        ViewBag.ConnectionName = "ConnectionName";
                        ViewBag.DatabaseName = "connections.DatabaseName";
                        ViewBag.ServerName = "ServerName";
                    }
                   
                }
                else
                {
                    ViewBag.name = "choose project";
                    ViewBag.desk = "No description";
                    ViewBag.ConnectionName = "ConnectionName";
                    ViewBag.DatabaseName = "DatabaseName";
                    ViewBag.ServerName = "ServerName";
                }
              

            }
            else
            {
                var proj = new List<ProjectsListViewModel>
                {
                    new ProjectsListViewModel
                    {
                        ProjectID = 1,
                        ProjectName = "Example",
                        ProjectDescription = "This project for demonstration service"
                    }
                };
                _projectModel.IdCurrentProject = proj[0].ProjectID;
                
                _projectModel.Projects = proj;                    
                
            }

            return View(_projectModel);
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
                _serviceProject.SaveProject(newProject);

                _serviceProjectsShareService.AddUserToProjectsShare(newProject, _currentUser, UserRoleProjectsShareConstants.Owner);

                return PartialView("Success");
            }
            return PartialView("CreateProjectPartial");
        }

        [Authorize]
        public ActionResult UpdateProjectPartial(int id)
        {
            _projectModel.IdCurrentProject = Convert.ToInt32(id);
            var currentProject = _serviceProject.GetProject(_projectModel.IdCurrentProject);
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
                _serviceProject.SaveProject(newProject);
                ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;

                return PartialView("Success");
            }
            return PartialView("UpdateProjectPartial", project);
        }

        [Authorize]
        public ActionResult DeleteProjectPartial(int id)
        {
            _projectModel.IdCurrentProject = Convert.ToInt32(id);
            var currentProject = _serviceProject.GetProject(_projectModel.IdCurrentProject);
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
            _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            var projects = _serviceProjectsShareService.GetUserProjects(_currentUser);
            var projectsViewModel = Mapper.Map<IEnumerable<Project>, IEnumerable<ProjectsListViewModel>>(projects).ToList();
            var deleteProject = projectsViewModel.FirstOrDefault(x => x.ProjectID == project.IdCurrentProject);
            deleteProject.UserRole = _serviceProjectsShareService.GetUserRole(_currentUser, project.IdCurrentProject);
            if (deleteProject.UserRole == UserRoleProjectsShareConstants.Shared)
                {
                    _serviceProjectsShareService.DeleteUserFromProjectsShare(_serviceProject.GetProject(deleteProject.ProjectID), _currentUser);
                }
            if (deleteProject.UserRole == UserRoleProjectsShareConstants.Owner)
            {
                var newproject = Mapper.Map<ProjectViewModel, Project>(project);
                newproject.Delflag = 1;
                _serviceProject.SaveProject(newproject);

            }
                return PartialView("Success");
           
        }

        [Authorize]
        public ActionResult CreateConnectionPartial(int id, int count)
        {
            _connectionModel.ConnectionOwner = id;
            _connectionModel.ConnectionCount = count;
            _connectionModel.ServerName = _serviceConnection.GetConnectionDBs(id).FirstOrDefault().ServerName ;
            return PartialView("CreateConnectionPartial", _connectionModel);

        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateConnectionPartial(ConnectionViewModel connection)
        {
            if (ModelState.IsValid)
            {
                var newConnection = Mapper.Map<ConnectionViewModel, ConnectionDB>(connection);
                _serviceConnection.SaveConnection(newConnection);

                ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;
                return PartialView("Success");

            }
            return PartialView("CreateConnectionPartial",connection);
        }

        [Authorize]
        public ActionResult UpdateConnectionPartial(int id)
        {
            var currentConnection = _serviceConnection.GetConnectionDBs().FirstOrDefault(x => x.ConnectionID == id);
            var newConnection = Mapper.Map<ConnectionDB, ConnectionViewModel>(currentConnection);

            return PartialView("UpdateConnectionPartial", newConnection);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateConnectionPartial(ConnectionViewModel connection)
        {
           
            if (ModelState.IsValid)
            {
				if (connection.IsConnectionValid())
				{
					var newConnection = Mapper.Map<ConnectionViewModel, ConnectionDB>(connection);
					_serviceConnection.SaveConnection(newConnection);

                    ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;
                    return PartialView("Success");
				}

				return PartialView("Failure");
			}

            return PartialView("UpdateConnectionPartial", connection);
        }

        [Authorize]
        public ActionResult DeleteConnectionPartial(int id)
        {
            var currentConnection = _serviceConnection.GetConnectionDBs().FirstOrDefault(x => x.ConnectionID == id);
                var newConnection = Mapper.Map<ConnectionDB, ConnectionViewModel>(currentConnection);
            if (newConnection != null)
            {
                return PartialView("DeleteConnectionPartial", newConnection);
            }
            return View("List");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteConnectionPartial(ConnectionViewModel connection)
        {
            var newConnection = Mapper.Map<ConnectionViewModel, ConnectionDB>(connection);
            newConnection.Delflag = 1;
            _serviceConnection.SaveConnection(newConnection);

            ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;
            return PartialView("Success");
        }

        [Authorize]
        public ActionResult InviteUserToProjectPartial(int id)
        {
            var users = _serviceProjectsShareService.GetUsersForSharedProject(_serviceProject.GetProject(id));

            var usersViewModel = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UsersListViewModel>>(users);

            var model = new UserViewModel
            {
                Users = usersViewModel,
                ProjectId = id
            };

            return PartialView("_InviteUserToProjectPartial", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult InviteUserToProjectPartial(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userForShared = _serviceUser.GetUserByID(user.UserId);

                var projectForShared = _serviceProject.GetProject(user.ProjectId);

                _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
                _serviceProjectsShareService.AddUserToProjectsShare(projectForShared, userForShared, UserRoleProjectsShareConstants.Invited, _currentUser);

                var bodyMail = _currentUser.UserName + " invited you to a project!";
                SmtpMailer.Instance(WebConfigurationManager.OpenWebConfiguration("~/web.config")).SendMail(userForShared.Email, "Invitation to project", bodyMail);

                ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;
                return PartialView("Success");
            }
            var users = _serviceProjectsShareService.GetUsersForSharedProject(_serviceProject.GetProject(user.ProjectId));

            user.Users = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UsersListViewModel>>(users);

            return PartialView("_InviteUserToProjectPartial", user);
        }

        [HttpGet]
        [Authorize]
        public  ActionResult AcceptInvite(int id)
        {           
            _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());
            var projectForShared = _serviceProject.GetProject(id);
            _serviceProjectsShareService.AddUserToProjectsShare(projectForShared, _currentUser, UserRoleProjectsShareConstants.Shared);

            if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());

            return RedirectToAction("List", "Workflow");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteInvite(int id)
        {
            _currentUser = _serviceUser.GetUserByID(User.Identity.GetUserId());

            _serviceProjectsShareService.DeleteUserFromProjectsShare(_serviceProject.GetProject(id), _currentUser);

            if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());

            return RedirectToAction("List", "Workflow");
        }

    }
}