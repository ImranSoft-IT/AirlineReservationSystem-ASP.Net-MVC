using AirReservation.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirReservation.Controllers
{
    [Authorize]
    public class Admin_RollController : Controller
    {
        // GET: Admin_Roll
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            List<Admin_RollCreation> rollList = new List<Admin_RollCreation>();
            if (HttpContext.User.IsInRole("Admin"))
            {
                foreach (var v in roleManager.Roles.ToList())
                {
                    rollList.Add(new Admin_RollCreation()
                    {
                        RoleName = v.Name,
                        Id = 0

                    });
                }


            }
            else
            {

                foreach (var v in roleManager.Roles.Where(w => w.Name != "Admin").ToList())
                {
                    rollList.Add(new Admin_RollCreation()
                    {
                        RoleName = v.Name,
                        Id = 0

                    });
                }



            }
            return View(rollList);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult RollCreate()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]

        public ActionResult RollCreate(Admin_RollCreation admin_RollCreation)
        {

            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));


            IdentityRole idenName = new IdentityRole();
            idenName.Name = admin_RollCreation.RoleName;

            IdentityResult r = roleManager.Create(idenName);
            if (r.Succeeded)
            {

                return RedirectToAction("Index");
            }

            return View();

        }
    }
}