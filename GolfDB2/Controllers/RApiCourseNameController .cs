using System.Web.Http;
using System.Web.Http.Description;
using GolfDB2.Models;
using System.Web.Mvc;

namespace GolfDB2.Controllers
{
    public class RApiCourseNameController : ApiController
    {
        // GET: api/RAPICourseName/1
        [ResponseType(typeof(SelectListItem))]
        public IHttpActionResult GetCourseName(int id)
        {
            return Json(MiscLists.GetCourseNameById(id, null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}