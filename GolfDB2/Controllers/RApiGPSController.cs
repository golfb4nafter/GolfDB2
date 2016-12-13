using System.Web.Http;
using System.Web.Http.Description;
using GolfDB2.Models;
using System.Web.Mvc;

namespace GolfDB2.Controllers
{
    public class RApiGPSController : ApiController
    {
        // GET: api/RApiGpsPointList
        [ResponseType(typeof(JsonResult))]
        public IHttpActionResult GetGpsPointList(int id)
        {
            return Json(MiscLists.GetGeoSpatialDataPointsByCourseId(id, null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}