using System.Web.Http;
using System.Web.Http.Description;
using GolfDB2.Models;
using System.Web.Mvc;

namespace GolfDB2.Controllers
{
    public class RApiObjectTypeController : ApiController
    {
        // GET: api/RApiObjectType/1
        [ResponseType(typeof(JsonResult))]
        public IHttpActionResult GetObjectType(int id)
        {
            return Json(MiscLists.GetObjectTypeById(id, null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}