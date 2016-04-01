using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueryBuilderMVC.Controllers
{
    public class BuilderController : Controller
    {
        // GET: Builder
        public ActionResult ERModel()
        {
            return View();
        }

		public JsonResult GetDBModel(string connectionString)
		{
			var movies = new List<object>();

			

			return Json(movies, JsonRequestBehavior.AllowGet);

		}
    }
}