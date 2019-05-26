using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab21.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        //public ActionResult TryRegister(string name, string password, string email, DateTime dob, string color, string pineapple)
        //{

        //}

        public ActionResult Welcome()
        {
            
            return View();
        }

        public bool CheckConnection()
        {
            ShopDB db = new ShopDB();

            try
            {
                db.Database.Connection.Open();
                db.Database.Connection.Close();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

        public ActionResult CheckDB()
        {
            if (CheckConnection())
            {
                ViewBag.DBStatus = "Connection safu";
                return View();
            }

            ViewBag.DBStatus = "Connection bad";
            return View();
        }
    }
}