using System.Web.Http;
using System.Web.Http.Description;
using GolfDB2.Models;
using System.Web.Mvc;

namespace GolfDB2.Controllers
{
    public class RApiCourseController : ApiController
    {
        // GET: api/RAPICourse/1
        [ResponseType(typeof(SelectListItem))]
        public IHttpActionResult GetCourseNamesList(int id)
        {
            return Json(MiscLists.GetCourseNamesList(null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}