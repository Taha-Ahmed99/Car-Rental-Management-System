using Car_Rental_Web.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Car_Rental_Web.Controllers
{
    [Authorize]
    public class returnCarController : Controller
    {
        CarRentalContexts db = new CarRentalContexts();

        // GET: returnCar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Getid(string carno)
        {
            var carn = (from s in db.Rentals
                        where s.carid == carno
                        select new
                        {
                            StartDate = s.sdate,
                            EndDate = s.edate,
                            Custid = s.custid,
                            carno = s.carid,
                            Fee = s.fee,
                            ElapsedDay = SqlFunctions.DateDiff("day", s.edate, DateTime.Now)
                        }).ToArray();
            return Json(carn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult Save(returnCar ret)
        {
            if (ModelState.IsValid)
            {
                db.returnCars.Add(ret);
                var car = db.carregs.SingleOrDefault(e => e.carNo == ret.carno.ToString());
                if (car == null)
                    return HttpNotFound("CarNo is not valid");

                car.Available = "Yes";
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ret);

        }
            
    }
}