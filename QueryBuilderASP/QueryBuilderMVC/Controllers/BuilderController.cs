using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueryBuilderMVC.Models;
using System.Data.SqlClient;

namespace QueryBuilderMVC.Controllers
{
    public class BuilderController : Controller
    {
        // GET: Builder
        public ActionResult ERModel(string connectionString="Data Source=(local);Initial Catalog=QueryBuilder;Integrated Security=True;Application Name=EntityFramework")
		{
			var sqlConnection = new SqlConnection(connectionString);
			var viewmodel = new ERModelViewModel(sqlConnection);
            return View(viewmodel);
        }

		public JsonResult GetDBModel(string connectionString)
		{
			var movies = new List<object>();
			
			

			return Json(movies, JsonRequestBehavior.AllowGet);

		}
    }
}