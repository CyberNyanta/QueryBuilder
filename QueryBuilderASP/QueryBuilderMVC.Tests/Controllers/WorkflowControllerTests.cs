using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QueryBuilder.DAL.Models;
using QueryBuilder.Services.Contracts;
using QueryBuilder.Utils.Encryption;
using QueryBuilderMVC.Controllers;
using QueryBuilderMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QueryBuilderMVC.Mappings;
using AutoMapper;

namespace QueryBuilderMVC.Controllers.Tests
{
    [TestClass()]
    public class WorkflowControllerTests
    {

        #region HelpMethod
        public void SetFakeContext(Controller controller, bool IsAuthenticated)
        {

            var httpContext = MakeFakeContext(IsAuthenticated);
            ControllerContext context =
            new ControllerContext(
            new RequestContext(httpContext,
            new RouteData()), controller);
            controller.ControllerContext = context;
        }


        private HttpContextBase MakeFakeContext(bool IsAuthenticated)
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();

            context.Setup(c => c.Request).Returns(request.Object);
            context.Setup(c => c.Response).Returns(response.Object);
            context.Setup(c => c.Session).Returns(session.Object);
            context.Setup(c => c.Server).Returns(server.Object);
            context.Setup(c => c.User).Returns(user.Object);
            user.Setup(c => c.Identity).Returns(identity.Object);
            identity.Setup(i => i.IsAuthenticated).Returns(IsAuthenticated);
            identity.Setup(i => i.Name).Returns("admin");

            return context.Object;
        }

        //mockProjectService.Setup(m => m.Projects).Returns(new List<Project> {
        //new Project
        //{
        //    ProjectName =  "MyProject",
        //    ProjectID = 1,
        //    ProjectDescription = "MyDescription",
        //    ProjectsShares = new List<ProjectsShare>
        //        {
        //            new ProjectsShare
        //            {
        //                ProjectId = 1,
        //                UserId = "dsfsdf",
        //                UserRole = 1
        //            }

        //        },
        //   ConnectionDBs = new List<ConnectionDB>
        //   {
        //       new ConnectionDB
        //       {
        //           ConnectionID = 1,
        //           ConnectionName = "MyConnection",
        //           DatabaseName = "MyDatabase",
        //           LoginDB = "MyLogin",
        //           PasswordDB = Rijndael.EncryptStringToBytes("MyPassword"),
        //           ConnectionOwner = 1,
        //           ServerName = "MyServer"
        //       }
        //   }
        //}
        //}.AsQueryable());



        #endregion

        //public Mock<IProjectRepositoryForMock> mockProjectService = new Mock<IProjectRepositoryForMock>();



        private ConnectionViewModel _connectionModel = new ConnectionViewModel();
        private QueryViewModel _queryModel = new QueryViewModel();
        private QueryHistoryViewModel _queryHistoryModel = new QueryHistoryViewModel();
        private WorkflowController wc;
        private readonly ProjectsListViewModel _projectListModel = new ProjectsListViewModel();

        [TestInitialize]
        public void Initial()
        {
            var mockIProjectService = new Mock<IProjectService>();
            var mockIProjectsShareService = new Mock<IProjectsShareService>();
            var mockIUserService = new Mock<IUserService>();
            var mockIConnectionDbService = new Mock<IConnectionDbService>();
            var mockIQueryService = new Mock<IQueryService>();
            var mockIQueriesHistoryService = new Mock<IQueriesHistoryService>();
            Mapper.Initialize(m => m.AddProfile<ViewModelToDomainMappingProfile>());
            Mapper.Initialize(m => m.AddProfile<DomainToViewModelMappingProfile>());
            mockIProjectService.Setup(a => a.GetProjects()).Returns(new List<Project>() {new Project() });
            mockIUserService.Setup(a => a.GetUsers()).Returns(new List<ApplicationUser>());
            mockIProjectsShareService.Setup(a => a.GetUserProjects(new ApplicationUser())).Returns(new List<Project>());
            mockIConnectionDbService.Setup(a => a.GetConnectionDBs()).Returns(new List<ConnectionDB>());
            mockIQueryService.Setup(a => a.GetQueries()).Returns(new List<Query>());
            mockIQueriesHistoryService.Setup(a => a.GetQueriesHistory()).Returns(new List<QueryHistory>());

            wc = new WorkflowController(mockIProjectService.Object, mockIUserService.Object, mockIProjectsShareService.Object,
                mockIConnectionDbService.Object, mockIQueryService.Object, mockIQueriesHistoryService.Object);

        }    

        #region List
        [TestMethod()]
        public void ListTestViewResultNotNullNotAutorized()
        {
            SetFakeContext(wc, false);
            ViewResult result = wc.List() as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void ListTestViewResultNotNullNotAutorizedID()
        {
            SetFakeContext(wc, false);
            ViewResult result = wc.List("1") as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void ListTestViewResultNotNullAutorized()
        {
            SetFakeContext(wc, true);
            ViewResult result = wc.List() as ViewResult;
            Assert.IsNotNull(result.Model);
        }
        [TestMethod()]
        public void ListTestViewResultNotNullAutorizedID()
        {
            SetFakeContext(wc, true);
            ViewResult result = wc.List("1") as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void ListViewStringInViewBagAutorized()
        {
            SetFakeContext(wc, true);
            ViewResult result = wc.List() as ViewResult;
            Assert.AreEqual(0, result.ViewBag.CountInvited);
            Assert.AreEqual("choose project", result.ViewBag.name);
            Assert.AreEqual("No description", result.ViewBag.desk);
            Assert.AreEqual("ConnectionName", result.ViewBag.ConnectionName);
            Assert.AreEqual("DatabaseName", result.ViewBag.DatabaseName);
            Assert.AreEqual("ServerName", result.ViewBag.ServerName);
        }

        [TestMethod()]
        public void ListViewStringInViewBagAutorizedID()
        {
            SetFakeContext(wc, true);
            ViewResult result = wc.List("1") as ViewResult;
            Assert.AreEqual(0, result.ViewBag.CountInvited);
            Assert.AreEqual("ConnectionName", result.ViewBag.ConnectionName);
            Assert.AreEqual("connections.DatabaseName", result.ViewBag.DatabaseName);
            Assert.AreEqual("ServerName", result.ViewBag.ServerName);
        }

        #endregion

        #region ListProject
        [TestMethod()]
        public void ListProjectPartialTestNotAutorized()
        {
            SetFakeContext(wc, false);
            PartialViewResult result = wc.ListProjectPartial() as PartialViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void ListProjectViewEqualsListCshtml()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListProjectPartial() as PartialViewResult;
            Assert.AreEqual("ListProjectPartial", result.ViewName);
        }

        [TestMethod()]
        public void ListProjectPartialTestAutorized()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListProjectPartial() as PartialViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void ListProjectPartialStringInViewBagAutorized()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListProjectPartial() as PartialViewResult;
            Assert.AreEqual(0, result.ViewBag.CountInvited);
        }

        #endregion

        [TestMethod()]
        public void GetExampleProjectTest()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        #region ListConnection
        [TestMethod()]
        public void ListConnectionViewEqualsListCshtml()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListConnectionPartial() as PartialViewResult;
            Assert.AreEqual("ListConnectionPartial", result.ViewName);
        }

        [TestMethod()]
        public void ListConnectionTest()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListConnectionPartial() as PartialViewResult;
           
            Assert.IsNotNull(result.Model);
           
        }

        #endregion

        #region ListQuery
        [TestMethod()]
        public void ListQueryViewEqualsListCshtml()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListQueryPartial() as PartialViewResult;
            Assert.AreEqual("ListQueryPartial", result.ViewName);
        }

        [TestMethod()]
        public void ListQueryTest()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListQueryPartial() as PartialViewResult;

            Assert.IsNotNull(result.Model);

        }

        #endregion

        #region ListQuery
        [TestMethod()]
        public void ListHistoryViewEqualsListCshtml()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListHistoryPartial() as PartialViewResult;
            Assert.AreEqual("ListHistoryPartial", result.ViewName);
        }

        [TestMethod()]
        public void ListHistoryTest()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.ListHistoryPartial() as PartialViewResult;
            Assert.IsNotNull(result.Model);
        }

        #endregion

        #region CreateProject
        [TestMethod()]
        public void CreateProjectPartialEquals()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.CreateProjectPartial() as PartialViewResult;
            Assert.AreEqual("CreateProjectPartial", result.ViewName);
        }

        [TestMethod()]
        public void CreateProjectPartialTest()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.CreateProjectPartial() as PartialViewResult;
            Assert.IsNull(result.Model);
        }

        [TestMethod()]
        public void CreateProjectPartialTestPostValid()
        {
            SetFakeContext(wc, true);
            var projectModel = new ProjectViewModel();
            projectModel.Name = "dfdf";

            PartialViewResult result = wc.CreateProjectPartial(projectModel) as PartialViewResult;
            Assert.AreEqual("Success", result.ViewName);
        }

        [TestMethod()]
        public void CreateProjectPartialTestPostNotValid()
        {
            SetFakeContext(wc, true);
            wc.ModelState.AddModelError("name", "error");
            PartialViewResult result = wc.CreateProjectPartial(new ProjectViewModel()) as PartialViewResult;
            Assert.AreEqual("CreateProjectPartial", result.ViewName);
        }

        #endregion

        #region UpdateProject
        [TestMethod()]
        public void UpdateProjectPartialEquals()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.UpdateProjectPartial(1) as PartialViewResult;
            Assert.AreEqual("UpdateProjectPartial", result.ViewName);
        }

        [TestMethod()]
        public void UpdateProjectPartialTest()
        {
            SetFakeContext(wc, true);
            PartialViewResult result = wc.UpdateProjectPartial(1) as PartialViewResult;
            Assert.IsNotNull(result.Model);
        }

        

        #endregion

        [TestMethod()]
        public void DeleteProjectPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteProjectPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateConnectionPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateConnectionPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateConnectionPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateConnectionPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteConnectionPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteConnectionPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateQueryPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateQueryPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateQueryPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateQueryPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteQueryPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteQueryPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InviteUserToProjectPartialTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InviteUserToProjectPartialTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AcceptInviteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteInviteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetGridModelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveGridToPdfTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveGridToExcelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SendQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SendResultQueryTest()
        {
            Assert.Fail();
        }
    }
}