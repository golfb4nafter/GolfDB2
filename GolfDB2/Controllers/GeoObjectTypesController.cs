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
    public class GeoObjectTypesController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: GeoObjectTypes
        public ActionResult Index()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View(db.GeoObjectTypes.ToList());
        }

        // GET: GeoObjectTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoObjectType geoObjectType = db.GeoObjectTypes.Find(id);
            if (geoObjectType == null)
            {
                return HttpNotFound();
            }
            return View(geoObjectType);
        }

        // GET: GeoObjectTypes/Create
        public ActionResult Create()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View();
        }

        // POST: GeoObjectTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,GeoObjectType1,GeoObjectDescription")] GeoObjectType geoObjectType)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.GeoObjectTypes.Add(geoObjectType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(geoObjectType);
        }

        // GET: GeoObjectTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoObjectType geoObjectType = db.GeoObjectTypes.Find(id);
            if (geoObjectType == null)
            {
                return HttpNotFound();
            }
            return View(geoObjectType);
        }

        // POST: GeoObjectTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,GeoObjectType1,GeoObjectDescription")] GeoObjectType geoObjectType)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.Entry(geoObjectType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geoObjectType);
        }

        // GET: GeoObjectTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeoObjectType geoObjectType = db.GeoObjectTypes.Find(id);
            if (geoObjectType == null)
            {
                return HttpNotFound();
            }
            return View(geoObjectType);
        }

        // POST: GeoObjectTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeoObjectType geoObjectType = db.GeoObjectTypes.Find(id);
            db.GeoObjectTypes.Remove(geoObjectType);
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
