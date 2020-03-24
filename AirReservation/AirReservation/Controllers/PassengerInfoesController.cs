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
    public class PassengerInfoesController : Controller
    {
        private AirReservationDBMvcEntities db = new AirReservationDBMvcEntities();

        // GET: PassengerInfoes
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
            var passengers = from P in db.PassengerInfoes select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                passengers = passengers.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper()) || 
                s.LastName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    passengers = passengers.OrderByDescending(s => s.FirstName);
                    break;
                case "Date":
                    passengers = passengers.OrderBy(s => s.DateOfBirth);
                    break;
                case "date_desc":
                    passengers = passengers.OrderByDescending(s => s.DateOfBirth);
                    break;
                default:
                    passengers = passengers.OrderBy(s => s.FirstName);
                    break;
            }
            //return View(passengers.ToList());
            int pageSize = 5; 
            int pageNumber = (page ?? 1); 
            return View(passengers.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult IndexSingle(int? PassengerId, int? CountryId)
        {
            var singleSelectList = new SelectedInfoModel();
            singleSelectList.passengers = db.PassengerInfoes.ToList();
            if (PassengerId == null)
            {
                if (Session["PassengerId"] != null)
                {
                    PassengerId = Convert.ToInt32(Session["PassengerId"].ToString());
                }
            }

            if (PassengerId != null)
            {
                Session["PassengerId"] = PassengerId;
                singleSelectList.bookingFlights = db.BookingFlights.Where(w => w.PassengerID == PassengerId.Value).ToList();

            }

            if (CountryId != null)
            {
                singleSelectList.countries = db.Countries.Include(i => i.BookingFlights).Where(w => w.CountryID == CountryId.Value).ToList();
            }

            return View(singleSelectList);
        }

        // GET: PassengerInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassengerInfo passengerInfo = db.PassengerInfoes.Find(id);
            if (passengerInfo == null)
            {
                return HttpNotFound();
            }
            return View(passengerInfo);
        }

        // GET: PassengerInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PassengerInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PassengerID,FirstName,LastName,DateOfBirth,Age,PhoneNumber,Email,passportNo,ImageUrl")] PassengerInfo passengerInfo, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                ImageFileCreate.SaveAs(Server.MapPath("~/Images/Passenger") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Images/Passenger/" + ImageFileCreate.FileName;
                passengerInfo.ImageUrl = filePath;
                db.PassengerInfoes.Add(passengerInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(passengerInfo);
        }

        [Authorize(Roles = "Admin, Staff")]
        // GET: PassengerInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassengerInfo passengerInfo = db.PassengerInfoes.Find(id);
            if (passengerInfo == null)
            {
                return HttpNotFound();
            }
            return View(passengerInfo);
        }

        // POST: PassengerInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PassengerID,FirstName,LastName,DateOfBirth,Age,PhoneNumber,Email,passportNo,ImageUrl")] PassengerInfo passengerInfo, HttpPostedFileBase ImageFileCreate)
        {
            if (ImageFileCreate.ContentLength > 0 && ModelState.IsValid)
            {
                System.IO.File.Delete(Server.MapPath(passengerInfo.ImageUrl));

                ImageFileCreate.SaveAs(Server.MapPath("~/Images/Passenger") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Images/Passenger/" + ImageFileCreate.FileName;
                passengerInfo.ImageUrl = filePath;
                db.Entry(passengerInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(passengerInfo);
        }

        [Authorize(Roles ="Admin")]
        // GET: PassengerInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassengerInfo passengerInfo = db.PassengerInfoes.Find(id);
            if (passengerInfo == null)
            {
                return HttpNotFound();
            }
            return View(passengerInfo);
        }

        // POST: PassengerInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PassengerInfo passengerInfo = db.PassengerInfoes.Find(id);
            System.IO.File.Delete(Server.MapPath(passengerInfo.ImageUrl));

            db.PassengerInfoes.Remove(passengerInfo);
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
