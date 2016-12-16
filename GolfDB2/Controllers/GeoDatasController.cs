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
    public class GeoDatasController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: GeoDatas
        public ActionResult Index()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View(db.GeoDatas.ToList());
        }

        // GET: GeoDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoData geoData = db.GeoDatas.Find(id);
            if (geoData == null)
            {
                return HttpNotFound();
            }
            return View(geoData);
        }

        // GET: GeoDatas/Create
        public ActionResult Create()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View();
        }

        // POST: GeoDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GeoSpatialDataId,GeoObjectDescription,GeoObjectType,HoleId,OrderNumber,CourseId,YardsToFront,YardsToMiddle,YardsToBack")] GeoData geoData)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.GeoDatas.Add(geoData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(geoData);
        }

        // GET: GeoDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoData geoData = db.GeoDatas.Find(id);
            if (geoData == null)
            {
                return HttpNotFound();
            }
            return View(geoData);
        }

        // POST: GeoDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GeoSpatialDataId,GeoObjectDescription,GeoObjectType,HoleId,OrderNumber,CourseId,YardsToFront,YardsToMiddle,YardsToBack")] GeoData geoData)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.Entry(geoData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geoData);
        }

        // GET: GeoDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoData geoData = db.GeoDatas.Find(id);
            if (geoData == null)
            {
                return HttpNotFound();
            }
            return View(geoData);
        }

        // POST: GeoDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeoData geoData = db.GeoDatas.Find(id);
            db.GeoDatas.Remove(geoData);
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
