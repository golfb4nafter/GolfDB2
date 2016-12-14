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
    public class RApiNineLabelsListController : ApiController
    {
        // GET: api/RAPINineLabelsList/1
        [ResponseType(typeof(SelectListItem))]
        public IHttpActionResult GetHoleList(int id)
        {
            return Json(MiscLists.GetNineLabelsByCourseIdAndType(id, "Nine", null));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}