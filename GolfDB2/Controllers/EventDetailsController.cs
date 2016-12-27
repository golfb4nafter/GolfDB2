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
    public class EventDetailsController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: EventDetails
        public ActionResult Index()
        {
            return View(db.EventDetails.ToList());
        }

        // GET: EventDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(id);
            if (eventDetail == null)
            {
                return HttpNotFound();
            }
            return View(eventDetail);
        }

        // GET: EventDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EventId,CourseId,PlayFormat,NumberOfHoles,IsShotgunStart,Sponsor,PlayListId")] GolfDB2.Models.EventDetail eventDetail)
        {
            if (ModelState.IsValid)
            {
                db.EventDetails.Add(eventDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventDetail);
        }

        // GET: EventDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            int eventDetailId = 0;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //// using EventId lookup EventDetails Id
            //string query = string.Format("SELECT Id from EventDetail WHERE EventId={0}", id);

            //List<SqlListParam> parms = new List<SqlListParam>();
            //parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            //parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            //string resp = SqlLists.SqlQuery(query, parms, null);

            //if (string.IsNullOrEmpty(resp))
            //{

            //}
            //else
            //{
            //    KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            //    eventDetailId = int.Parse(kvp.Value);
            //}

            //// If no detail record exists create an empty entry




            GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(id);

            if (eventDetail == null)
            {
                return HttpNotFound();
            }
            return View(eventDetail);
        }

        // POST: EventDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EventId,CourseId,PlayFormat,NumberOfHoles,IsShotgunStart,Sponsor,PlayListId")] GolfDB2.Models.EventDetail eventDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventDetail);
        }

        // GET: EventDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(id);
            if (eventDetail == null)
            {
                return HttpNotFound();
            }
            return View(eventDetail);
        }

        // POST: EventDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GolfDB2.Models.EventDetail eventDetail = db.EventDetails.Find(id);
            db.EventDetails.Remove(eventDetail);
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
