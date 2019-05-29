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

        public ActionResult Register(string error)
        {
            ViewBag.Error = error;//todo: make this message more clear
            return View();
        }

        public ActionResult TryRegister(string name, string password, string email, DateTime? dob, string color, string pineapple)
        {
            using (ShopDB db = new ShopDB())
            {

                if(db.Customers.Any(c => c.UserName == name))
                {
                    return RedirectToAction("Register", "Home", new { error= "That name is already in use." });
                }

                Customer newUser = new Customer();
                newUser.UserName = name;
                newUser.Password = password;
                newUser.Email = email;
                newUser.Dob = dob;
                newUser.Color = color;
                newUser.Pineapple = pineapple == "Yes" ? true : false;
                db.Customers.Add(newUser);
                db.SaveChanges();
                return RedirectToAction("TryLogin", "Home", new { name, password });
                
            }
        }

        public ActionResult TryLogin(string name, string password)
        {
            using(ShopDB db = new ShopDB())
            {
                 if(db.Customers.Any(u => u.UserName == name && u.Password == password))
                 {
                    Customer loggingIn = db.Customers.Find(name);
                    Session["Logged"] = loggingIn;
                    return RedirectToAction("Welcome", "Home");
                 }

                return RedirectToAction("Register", "Home", new { error = "Invalid login info!" });
            }
        }

        public ActionResult Welcome()
        {
            if ((Customer)Session["Logged"] == null)
            {
                return RedirectToAction("Register", "Home");
            }
            Customer LoggedIn = (Customer)Session["Logged"];
            ViewBag.Name = LoggedIn.UserName;
            return View();
        }

        public bool CheckConnection()
        {//i used this to debug early and i'd probably get rid of it if it mattered
            using (ShopDB db = new ShopDB())
            {
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
        }
        
        public ActionResult CheckDB()
        {//i used this to debug early and i'd probably get rid of it if it mattered
            if (CheckConnection())
            {
                ViewBag.DBStatus = "Connection safu";
                return View();
            }

            ViewBag.DBStatus = "Connection bad";
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}