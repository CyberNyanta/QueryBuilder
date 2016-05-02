using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QueryBuilder.University;

namespace QueryBuilderMVC.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var context = new UniversityContext())
            //{
            //    var courses = context.Courses.ToList();
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}