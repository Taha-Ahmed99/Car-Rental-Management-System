using Car_Rental_Web.Models;
using Car_Rental_Web.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Car_Rental_Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AcUser user)
        {
            using (var context = new AdminContext())
            {
                bool isValid = context.Logins.Any(x => x.UserName == user.UserId && x.Password == user.Password);

                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(user.UserId, false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid User Name or Password");
                
            }

            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Login user)
        {
            using (var context = new AdminContext())
            {
                context.Logins.Add(user);
                context.SaveChanges();
            }
                return View();
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Account");
        }
    }
}