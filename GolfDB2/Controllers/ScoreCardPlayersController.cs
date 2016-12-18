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
    public class ScoreCardPlayersController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: ScoreCardPlayers
        public ActionResult Index()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View(db.ScoreCardPlayers.ToList());
        }

        // GET: ScoreCardPlayers/Details/5
        public ActionResult Details(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreCardPlayer scoreCardPlayer = db.ScoreCardPlayers.Find(id);
            if (scoreCardPlayer == null)
            {
                return HttpNotFound();
            }
            return View(scoreCardPlayer);
        }

        // GET: ScoreCardPlayers/Create
        public ActionResult Create()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View();
        }

        // POST: ScoreCardPlayers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ScoreCardId,GolferName,GolferId")] ScoreCardPlayer scoreCardPlayer)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.ScoreCardPlayers.Add(scoreCardPlayer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoreCardPlayer);
        }

        // GET: ScoreCardPlayers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreCardPlayer scoreCardPlayer = db.ScoreCardPlayers.Find(id);
            if (scoreCardPlayer == null)
            {
                return HttpNotFound();
            }
            return View(scoreCardPlayer);
        }

        // POST: ScoreCardPlayers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ScoreCardId,GolferName,GolferId")] ScoreCardPlayer scoreCardPlayer)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.Entry(scoreCardPlayer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoreCardPlayer);
        }

        // GET: ScoreCardPlayers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreCardPlayer scoreCardPlayer = db.ScoreCardPlayers.Find(id);
            if (scoreCardPlayer == null)
            {
                return HttpNotFound();
            }
            return View(scoreCardPlayer);
        }

        // POST: ScoreCardPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoreCardPlayer scoreCardPlayer = db.ScoreCardPlayers.Find(id);
            db.ScoreCardPlayers.Remove(scoreCardPlayer);
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
