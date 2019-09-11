using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeatherApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Message = "Admin Page";
            IEnumerable<User> users;
            using (var dbContext = new WeatherEntities())
            {
                users = (
                    from u in dbContext.Users
                    select u
                    ).ToList();
            }
            ViewBag.Users = users;
                return View();
        }

        public ActionResult AddUser(User userToAdd)
        {
            if (userToAdd == null)
            {
                throw new ArgumentNullException(nameof(userToAdd), "No user was provided to add to the database.");
            }

            using (var dbContext = new WeatherEntities())
            {
                dbContext.Users.Add(userToAdd);
                dbContext.SaveChanges();
            }
            
            return RedirectToRoute("Default", new { controller = "Admin", action = "Index" });
        }
    }
}