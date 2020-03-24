using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AirReservation.Models;
using PagedList;

namespace AirReservation.Controllers
{
    [Authorize]
    public class FlightInfoesController : Controller
    {
        private AirReservationDBMvcEntities db = new AirReservationDBMvcEntities();

        // GET: FlightInfoes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else { searchString = currentFilter; }
            ViewBag.CurrentFilter = searchString;
            var flights = from P in db.FlightInfoes select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                flights = flights.Where(s => s.FlightNunber.ToUpper().Contains(searchString.ToUpper()) ||
                s.Landing.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    flights = flights.OrderByDescending(d => d.FlightNunber);
                    break;
                default:
                    flights = flights.OrderBy(s => s.FlightNunber);
                    break;
            }
            //return View(passengers.ToList());
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return View(flights.ToPagedList(pageNumber, pageSize));
        }

        // GET: FlightInfoes/Details/5

        public ActionResult Sample()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Staff")]
        public JsonResult UpdateFlightsInfo(string flightJson)
        {
            var js = new JavaScriptSerializer();
            FlightInfo[] flights = js.Deserialize<FlightInfo[]>(flightJson);

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.FlightInfoes.AddRange(flights);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json("Flight Info are updated");

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return Json("Problem");
                }

            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightInfo flightInfo = db.FlightInfoes.Find(id);
            if (flightInfo == null)
            {
                return HttpNotFound();
            }
            return View(flightInfo);
        }

        // GET: FlightInfoes/Create
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FlightInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FlightID,FlightNunber,TakeOff,Landing")] FlightInfo flightInfo)
        {
            if (ModelState.IsValid)
            {
                db.FlightInfoes.Add(flightInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flightInfo);
        }

        // GET: FlightInfoes/Edit/5
        [Authorize(Roles ="Admin, Staff")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightInfo flightInfo = db.FlightInfoes.Find(id);
            if (flightInfo == null)
            {
                return HttpNotFound();
            }
            return View(flightInfo);
        }

        // POST: FlightInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlightID,FlightNunber,TakeOff,Landing")] FlightInfo flightInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flightInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flightInfo);
        }

        // GET: FlightInfoes/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightInfo flightInfo = db.FlightInfoes.Find(id);
            if (flightInfo == null)
            {
                return HttpNotFound();
            }
            return View(flightInfo);
        }

        // POST: FlightInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlightInfo flightInfo = db.FlightInfoes.Find(id);
            db.FlightInfoes.Remove(flightInfo);
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
