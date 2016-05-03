﻿using System;
using System.Web.Mvc;
using QueryBuilder.DAL.Models;
using QueryBuilderMVC.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using AutoMapper;
using QueryBuilder.Services.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using QueryBuilder.Constants;
using QueryBuilder.Utils.Mailers;
using QueryBuilder.Utils.Encryption;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using ActiveDatabaseSoftware.ActiveQueryBuilder.Web.Control;

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

        public WorkflowController(IProjectService serviceProject, IUserService serviceUser,
            IProjectsShareService serviceProjectsShare, IConnectionDbService serviceConnection)
        {
            _serviceProject = serviceProject;
            _serviceUser = serviceUser;
            _serviceConnection = serviceConnection;
            _serviceProjectsShareService = serviceProjectsShare;
        }

        [HttpGet]
        public ActionResult List(string id = "0")
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
                    project.CountUsersForShared = _serviceProjectsShareService.GetUsersForSharedProject(_serviceProject.GetProject(project.ProjectID)).Count();
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
                    //
                    //Create treeView from database in first connection;
                    //
                    //
                    if (_projectModel.ConnectionDbs.Any())
                    {
                        var connect = _projectModel.ConnectionDbs.First();
                        var sqlConnection = String.Format("Data source= {0}; Initial catalog= {1}; UID= {2}; Password= {3};",
                                           connect.ServerName, connect.DatabaseName, connect.LoginDB, Rijndael.DecryptStringFromBytes(connect.PasswordDB));
                        ViewBag.ConnectionString = sqlConnection;


                    }

                    ////
                    ///
                    ///
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
                        ProjectDescription = "This project for demonstration service",
                        UserRole = UserRoleProjectsShareConstants.Owner,
                     }

                };
                var connect = new List<ConnectionsListViewModel>
                {
                    new ConnectionsListViewModel
                    {
                        ConnectionID = -1,
                        ConnectionName = "Example",
                        ConnectionOwner = 1,
                        DatabaseName = "defaultsamples",
                        LoginDB = "scrumtracker01@e7g8mfm8ri",
                        ServerName = "tcp:e7g8mfm8ri.database.windows.net,1433",
                        PasswordDB = Rijndael.EncryptStringToBytes("Instance@1")
                    }
                };
                _projectModel.ConnectionDbs = connect;
                _projectModel.IdCurrentProject = proj[0].ProjectID;
                _projectModel.Name = proj[0].ProjectName;
                _projectModel.Description = proj[0].ProjectDescription;
                _projectModel.Projects = proj;

            }

            return View(_projectModel);
        }
        [HttpPost]
        public ActionResult ListProjectPartial()
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

            return PartialView("ListProjectPartial", _projectModel);
        }


        #region Project
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

                ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;
                return PartialView("Success");
            }
            return PartialView("CreateProjectPartial");
        }


        [Authorize]
        public ActionResult UpdateProjectPartial(int id)
        {
            _projectModel.IdCurrentProject = id;
            var currentProject = _serviceProject.GetProject(_projectModel.IdCurrentProject);
            //var newProject = Mapper.Map<Project, ProjectViewModel>(currentProject);
            ProjectViewModel newProject = new ProjectViewModel();
            newProject.Name = currentProject.ProjectName;
            newProject.Description = currentProject.ProjectDescription;
            newProject.IdCurrentProject = currentProject.ProjectID;
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
            _projectModel.IdCurrentProject = id;
            var currentProject = _serviceProject.GetProject(_projectModel.IdCurrentProject);
            //var newProject = Mapper.Map<Project, ProjectViewModel>(currentProject);
            var newProject = new ProjectViewModel
            {
                Name = currentProject.ProjectName,
                Description = currentProject.ProjectDescription,
                IdCurrentProject = currentProject.ProjectID
            };
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
            if (deleteProject != null)
            {
                deleteProject.UserRole = _serviceProjectsShareService.GetUserRole(_currentUser, project.IdCurrentProject);
                if (deleteProject.UserRole == UserRoleProjectsShareConstants.Shared)
                {
                    _serviceProjectsShareService.DeleteUserFromProjectsShare(_serviceProject.GetProject(deleteProject.ProjectID), _currentUser);
                }

                if (deleteProject.UserRole == UserRoleProjectsShareConstants.Owner)
                {
                    var newproject = Mapper.Map<ProjectViewModel, Project>(project);
                    newproject.Delflag = DelflagConstants.UnactiveSet;
                    _serviceProject.SaveProject(newproject);

                    _serviceConnection.DeleteProjectConnections(deleteProject.ProjectID);
                }
            }

            ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;
            return PartialView("Success");
        }
        #endregion

        #region Connection
        [Authorize]
        public ActionResult CreateConnectionPartial(int id, int count = 0)
        {
            _connectionModel.ConnectionOwner = id;
            _connectionModel.ConnectionCount = count;
            if (count != 0)
            {
                var connection = _serviceConnection.GetConnectionDBs(id).FirstOrDefault();
                if (connection != null)
                {
                    _connectionModel.ServerName = connection.ServerName;
                    _connectionModel.LoginDB = connection.LoginDB;
                    _connectionModel.PasswordDB = Rijndael.DecryptStringFromBytes(connection.PasswordDB);

                }
            }
            return PartialView("CreateConnectionPartial", _connectionModel);

        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateConnectionPartial(ConnectionViewModel connection)
        {
            if (ModelState.IsValid)
            {
                ViewBag.IdCurrentProject = connection.ConnectionOwner;
                if (connection.IsConnectionValid())
                {
                    var newConnection = Mapper.Map<ConnectionViewModel, ConnectionDB>(connection);
                    _serviceConnection.SaveConnection(newConnection);

                    ViewBag.Title = "Success";
                    ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;
                    return PartialView("Result");
                }
                ModelState.AddModelError("", "The connection failed. Check entered data");
            }
            return PartialView("CreateConnectionPartial", connection);
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
                ViewBag.IdCurrentProject = connection.ConnectionOwner;
                if (connection.IsConnectionValid())
                {
                    var newConnection = Mapper.Map<ConnectionViewModel, ConnectionDB>(connection);
                    _serviceConnection.SaveConnection(newConnection);

                    ViewBag.Title = "Success";
                    ViewBag.PreviousPage = System.Web.HttpContext.Current.Request.UrlReferrer;

                    return PartialView("Result");



                }
                ModelState.AddModelError("", "The connection failed. Check entered data");
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
        #endregion

        #region Invite
        [Authorize]
        public ActionResult InviteUserToProjectPartial(int id)
        {
            var users = _serviceProjectsShareService.GetUsersForSharedProject(_serviceProject.GetProject(id)).Take(10);

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
        public ActionResult AcceptInvite(int id)
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



        [HttpPost]
        [Authorize]
        public JsonResult SearchUser(string prefix, int projectId)
        {
            var allUsers = _serviceProjectsShareService.GetUsersForSharedProject(_serviceProject.GetProject(projectId));

            var userName = from user in allUsers
                           where user.UserName.Contains(prefix)
                           select new { user.UserName, user.Id };

            return Json(userName, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Grid
        public string GetData()
        {
            var dataTableForGrid = GetDataTableForGrid();

            return JsonConvert.SerializeObject(dataTableForGrid);
        }

        public string GetGridModel()
        {
            var dataTableForGrid = GetDataTableForGrid();

            var header = (from DataColumn column in dataTableForGrid.Columns
                          select new DataGridModel
                          {
                              Name = column.ColumnName,
                              Index = column.ColumnName,
                              Sortable = true,
                              Align = "center"
                          }).ToList();

            return JsonConvert.SerializeObject(header);
        }

        private DataTable GetDataTableForGrid()
        {
            var table = new DataTable();


            //using (var conn = new SqlConnection("Data Source=(local);Initial Catalog=QueryBuilder;Integrated Security=true; App=EntityFramework"))
            //{
            //    string query = "Select QueryBuilder.dbo.Project.ProjectName As uB1,  QueryBuilder.dbo.Project.ProjectID As uB2 From QueryBuilder.dbo.Project";

            //    using (var cmd = new SqlCommand(query, conn))
            //    {
            //        SqlDataAdapter adapt = new SqlDataAdapter(cmd);
            //        conn.Open();
            //        adapt.Fill(table);
            //        conn.Close();
            //    }
            //}



            // FOR TEST
            //table.Columns.Add("Dosage", typeof(int));
            //table.Columns.Add("Drug", typeof(string));
            //table.Columns.Add("Patient", typeof(string));
            //table.Columns.Add("Date", typeof(DateTime));

            //table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            //table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            //table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            //table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            //table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            return table;
        }
        #endregion

        public FileStreamResult SaveQuery(string query)
        {
            var byteArray = Encoding.ASCII.GetBytes(query);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "Query.txt");
        }
        public ActionResult SaveQuery2(string query)
        {
            var byteArray = Encoding.ASCII.GetBytes(query);
            var stream = new MemoryStream(byteArray);

            return RedirectToAction("SaveQuery", new {query = "jjk" });
        }
    }
}