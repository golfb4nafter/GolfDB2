using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GolfDB2.Models;

namespace GolfDB2.Controllers
{
    public class GeoSpatialTablesController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: GeoSpatialTables
        public ActionResult Index()
        {
            return View(db.GeoSpatialTables.ToList());
        }

        // GET: GeoSpatialTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoSpatialTable geoSpatialTable = db.GeoSpatialTables.Find(id);
            if (geoSpatialTable == null)
            {
                return HttpNotFound();
            }
            return View(geoSpatialTable);
        }

        // GET: GeoSpatialTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GeoSpatialTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseId,Latitude,Longitude,Altitude,LocationDescription,GoogleMapsViewUrl")] GeoSpatialTable geoSpatialTable)
        {
            if (ModelState.IsValid)
            {
                db.GeoSpatialTables.Add(geoSpatialTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(geoSpatialTable);
        }

        // GET: GeoSpatialTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoSpatialTable geoSpatialTable = db.GeoSpatialTables.Find(id);
            if (geoSpatialTable == null)
            {
                return HttpNotFound();
            }
            return View(geoSpatialTable);
        }

        // POST: GeoSpatialTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId,Latitude,Longitude,Altitude,LocationDescription,GoogleMapsViewUrl")] GeoSpatialTable geoSpatialTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(geoSpatialTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geoSpatialTable);
        }

        // GET: GeoSpatialTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoSpatialTable geoSpatialTable = db.GeoSpatialTables.Find(id);
            if (geoSpatialTable == null)
            {
                return HttpNotFound();
            }
            return View(geoSpatialTable);
        }

        // POST: GeoSpatialTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeoSpatialTable geoSpatialTable = db.GeoSpatialTables.Find(id);
            db.GeoSpatialTables.Remove(geoSpatialTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
