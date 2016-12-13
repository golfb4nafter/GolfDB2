using System.Web.Http;
using System.Web.Http.Description;
using GolfDB2.Models;
using System.Web.Mvc;

namespace GolfDB2.Controllers
{
    public class RApiObjectListController : ApiController
    {
        // GET: api/RApiObjectList
        [ResponseType(typeof(JsonResult))]
        public IHttpActionResult GetObjectTypeList()
        {
            return Json(MiscLists.GetObjectTypeList(null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}