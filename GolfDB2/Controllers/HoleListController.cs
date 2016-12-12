using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GolfDB2.Models;
using System.Web.Mvc;


namespace GolfDB2.Controllers
{
    public class HoleListController : ApiController
    {
        // GET: api/HoleList/1
        [ResponseType(typeof(SelectListItem))]
        public IHttpActionResult GetHole(int id)
        {
            return Json(MiscLists.GetHoleListByCourseId(id, null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}