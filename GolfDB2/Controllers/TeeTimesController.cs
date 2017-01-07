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
    public class TeeTimesController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: TeeTimes
        public ActionResult Index()
        {
            return View(db.TeeTimes.ToList());
        }

        // GET: TeeTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.TeeTime teeTime = db.TeeTimes.Find(id);
            if (teeTime == null)
            {
                return HttpNotFound();
            }
            return View(teeTime);
        }

        // GET: TeeTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeeTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TeeTimeOffset,Tee_Time,CourseId,EventId,ReservedByName,TelephoneNumber,HoleId,NumberOfPlayers,PlayerNames")] GolfDB2.Models.TeeTime teeTime)
        {
            if (ModelState.IsValid)
            {
                db.TeeTimes.Add(teeTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teeTime);
        }

        // GET: TeeTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.TeeTime teeTime = db.TeeTimes.Find(id);
            if (teeTime == null)
            {
                return HttpNotFound();
            }
            return View(teeTime);
        }

        // POST: TeeTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TeeTimeOffset,Tee_Time,CourseId,EventId,ReservedByName,TelephoneNumber,HoleId,NumberOfPlayers,PlayerNames")] GolfDB2.Models.TeeTime teeTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teeTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teeTime);
        }

        // GET: TeeTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.TeeTime teeTime = db.TeeTimes.Find(id);
            if (teeTime == null)
            {
                return HttpNotFound();
            }
            return View(teeTime);
        }

        // POST: TeeTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GolfDB2.Models.TeeTime teeTime = db.TeeTimes.Find(id);
            db.TeeTimes.Remove(teeTime);
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
