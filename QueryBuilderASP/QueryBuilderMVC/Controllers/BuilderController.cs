using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueryBuilderMVC.Models;
using System.Data.SqlClient;
using QueryBuilder.Services.Contracts;

namespace QueryBuilderMVC.Controllers
{
    public class BuilderController : Controller
    {
        private readonly IConnectionDbService _serviceConnection;

        // GET: Builder
        [HttpGet]
        public ActionResult ERModel(int id)
		{
            //connectionString = "Data Source=(local);Initial Catalog=QueryBuilder;Integrated Security=True;Application Name=EntityFramework"

            var Connection = _serviceConnection.GetConnectionDb(id);
            string sqlConnection = "Data source=" + Connection.ServerName + ";Initial catalog=" + Connection.DatabaseName + "Integrated Security=True;";
            var sql = new SqlConnection(sqlConnection);
            
            var viewmodel = new ERModelViewModel(sql);
            return View(viewmodel);
        }

		public JsonResult GetDBModel(string connectionString)
		{
			var movies = new List<object>();
			
			

			return Json(movies, JsonRequestBehavior.AllowGet);

		}
    }
}