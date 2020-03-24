using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirReservation.Models;
using PagedList.Mvc;
using PagedList;

namespace AirReservation.Controllers
{
    [Authorize]
    public class CountriesController : Controller
    {
        private AirReservationDBMvcEntities db = new AirReservationDBMvcEntities();

        // GET: Countries
        [Authorize(Roles ="Admin, Staff")]
        public ActionResult Index(string SortOrder, string currentFilter, string searchString, int?page)
        {
            ViewBag.SortOrder = SortOrder;
            //var country = db.Countries.ToList();
            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var country = from C in db.Countries select C;

            if (!String.IsNullOrEmpty(searchString))
            {
                country = country.Where(s => s.Country1.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (SortOrder)
            {
                case "Asc":
                    {
                        country = country.OrderBy(a => a.Country1);
                        break;
                    }
                case "Desc":
                    {
                        country = country.OrderByDescending(a => a.Country1);
                        break;
                    }
                default:
                    {
                        country = country.OrderBy(a => a.Country1);
                        break;
                    }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(country.ToPagedList(pageNumber, pageSize));

            //return View(country.ToList());
        }

        // GET: Countries/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: Countries/Create
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryID,Country1,travelCost")] Country country)
        {
            if (ModelState.IsValid)
            {
                db.Countries.Add(country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: Countries/Edit/5
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryID,Country1,travelCost")] Country country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Country country = db.Countries.Find(id);
            db.Countries.Remove(country);
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
