using Car_Rental_Web.Models;
using Car_Rental_Web.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Car_Rental_Web.Controllers
{
    [Authorize]
    public class RentController : Controller
    {
        CarRentalContexts db = new CarRentalContexts();
        // GET: Rent
        public ActionResult Index()
        {
            var res = (from r in db.Rentals
                       join c in db.carregs on r.carid equals c.carNo
                       select new RentalViewModel
                       {
                           id = r.id,
                           carid = r.carid,
                           custid = r.custid,
                           fee=r.fee,
                           sdate = r.sdate,
                           edate = r.edate,
                           Available = c.Available
                       }).ToList();
            return View(res);
        }

        public ActionResult GetCar()
        {
            var car = db.carregs.ToList();
            return Json(car, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult Getid (int id)
        {
            var customer = (from s in db.customers where s.CustId == id select s.CustName).ToList();
            return Json(customer, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public ActionResult Getval(string carno)
        {
            var caravail = (from s in db.carregs where s.carNo == carno select s.Available).FirstOrDefault();
            return Json(caravail, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Save(Rental rent)
        {
            if (ModelState.IsValid)
            {
                db.Rentals.Add(rent);
                var car = db.carregs.SingleOrDefault(e => e.carNo == rent.carid.ToString());
                if (car== null)
                    return HttpNotFound("CarNo is not valid");

                car.Available = "No";
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
               return RedirectToAction("Index");
            }

            return View(rent);
        }
    }
}