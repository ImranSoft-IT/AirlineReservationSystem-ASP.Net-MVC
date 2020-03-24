using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirReservation.Models;
using PagedList;

namespace AirReservation.Controllers
{
    [Authorize]
    public class BookingFlightsController : Controller
    {
        private AirReservationDBMvcEntities db = new AirReservationDBMvcEntities();

        // GET: BookingFlights
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var bookingFlights = db.BookingFlights.Include(b => b.Country).Include(b => b.FlightInfo).Include(b => b.PassengerInfo);
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else { searchString = currentFilter; }
            ViewBag.CurrentFilter = searchString;
            var bookingF = db.BookingFlights.Include(b => b.Country).Include(b => b.FlightInfo).Include(b => b.PassengerInfo);
            //from P in db.BookingFlights select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                bookingF = bookingF.Where(s => s.PassengerInfo.FirstName.ToUpper().Contains(searchString.ToUpper()) ||
                s.FromPlace.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    bookingF = bookingF.OrderByDescending(d => d.Destination);
                    break;
                case "Date":
                    bookingF = bookingF.OrderBy(s => s.FlightDate);
                    break;
                case "date_desc":
                    bookingF = bookingF.OrderByDescending(s => s.FlightDate);
                    break;
                default:
                    bookingF = bookingF.OrderBy(s => s.Destination);
                    break;
            }
            //return View(passengers.ToList());
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(bookingF.ToPagedList(pageNumber, pageSize));

            //return View(bookingFlights.ToList());
        }

        // GET: BookingFlights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingFlight bookingFlight = db.BookingFlights.Find(id);
            if (bookingFlight == null)
            {
                return HttpNotFound();
            }
            return View(bookingFlight);
        }

        // GET: BookingFlights/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1");
            ViewBag.FlightID = new SelectList(db.FlightInfoes, "FlightID", "FlightNunber");
            ViewBag.PassengerID = new SelectList(db.PassengerInfoes, "PassengerID", "FirstName");
            return View();
        }

        // POST: BookingFlights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,FromPlace,Destination,FlightDate,PassengerID,CountryID,FlightID")] BookingFlight bookingFlight)
        {
            if (ModelState.IsValid)
            {
                db.BookingFlights.Add(bookingFlight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", bookingFlight.CountryID);
            ViewBag.FlightID = new SelectList(db.FlightInfoes, "FlightID", "FlightNunber", bookingFlight.FlightID);
            ViewBag.PassengerID = new SelectList(db.PassengerInfoes, "PassengerID", "FirstName", bookingFlight.PassengerID);
            return View(bookingFlight);
        }
        [Authorize(Roles ="Admin, Staff")]
        // GET: BookingFlights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingFlight bookingFlight = db.BookingFlights.Find(id);
            if (bookingFlight == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", bookingFlight.CountryID);
            ViewBag.FlightID = new SelectList(db.FlightInfoes, "FlightID", "FlightNunber", bookingFlight.FlightID);
            ViewBag.PassengerID = new SelectList(db.PassengerInfoes, "PassengerID", "FirstName", bookingFlight.PassengerID);
            return View(bookingFlight);
        }

        // POST: BookingFlights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,FromPlace,Destination,FlightDate,PassengerID,CountryID,FlightID")] BookingFlight bookingFlight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookingFlight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", bookingFlight.CountryID);
            ViewBag.FlightID = new SelectList(db.FlightInfoes, "FlightID", "FlightNunber", bookingFlight.FlightID);
            ViewBag.PassengerID = new SelectList(db.PassengerInfoes, "PassengerID", "FirstName", bookingFlight.PassengerID);
            return View(bookingFlight);
        }
        [Authorize(Roles ="Admin")]
        // GET: BookingFlights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingFlight bookingFlight = db.BookingFlights.Find(id);
            if (bookingFlight == null)
            {
                return HttpNotFound();
            }
            return View(bookingFlight);
        }

        // POST: BookingFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingFlight bookingFlight = db.BookingFlights.Find(id);
            db.BookingFlights.Remove(bookingFlight);
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
