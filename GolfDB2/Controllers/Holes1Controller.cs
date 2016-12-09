using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using GolfDB2.Models;

namespace GolfDB2.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using GolfDB2.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Hole>("Holes1");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class Holes1Controller : ODataController
    {
        private GolfDB db = new GolfDB();

        // GET: odata/Holes1
        [EnableQuery]
        public IQueryable<Hole> GetHoles1()
        {
            return db.Holes;
        }

        // GET: odata/Holes1(5)
        [EnableQuery]
        public SingleResult<Hole> GetHole([FromODataUri] int key)
        {
            return SingleResult.Create(db.Holes.Where(hole => hole.Id == key));
        }

        // PUT: odata/Holes1(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Hole> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Hole hole = db.Holes.Find(key);
            if (hole == null)
            {
                return NotFound();
            }

            patch.Put(hole);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(hole);
        }

        // POST: odata/Holes1
        public IHttpActionResult Post(Hole hole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Holes.Add(hole);
            db.SaveChanges();

            return Created(hole);
        }

        // PATCH: odata/Holes1(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Hole> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Hole hole = db.Holes.Find(key);
            if (hole == null)
            {
                return NotFound();
            }

            patch.Patch(hole);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(hole);
        }

        // DELETE: odata/Holes1(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Hole hole = db.Holes.Find(key);
            if (hole == null)
            {
                return NotFound();
            }

            db.Holes.Remove(hole);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HoleExists(int key)
        {
            return db.Holes.Count(e => e.Id == key) > 0;
        }
    }
}
