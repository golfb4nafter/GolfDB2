using System.Web.Http;
using System.Web.Http.Description;
using GolfDB2.Models;
using System.Web.Mvc;

namespace GolfDB2.Controllers
{
    public class RApiGPSDescriptionController : ApiController
    {
        // GET: api/RApiGpsDescription/1
        [ResponseType(typeof(JsonResult))]
        public IHttpActionResult GetGpsDescription(int id)
        {
            return Json(MiscLists.GetGeoSpatialDataPointDescriptionById(id, null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}