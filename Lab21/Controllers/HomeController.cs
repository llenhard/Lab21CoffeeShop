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
                newUser.Balance = 100;
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
                    return RedirectToAction("Shop", "Home");
                 }

                return RedirectToAction("Register", "Home", new { error = "Invalid login info!" });
            }
        }

        public ActionResult Welcome()
        {
            using (ShopDB db = new ShopDB())
            {
                if ((Customer)Session["Logged"] == null)
                {//if theyre not logged in send em back
                    return RedirectToAction("Register", "Home");
                }
                Customer LoggedIn = (Customer)Session["Logged"];
                ViewBag.User = LoggedIn;
                ViewBag.Orders = db.Orders.ToList().Where(o => o.UserName == LoggedIn.UserName);
                return View();
            }
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

        public ActionResult Shop(string error = "")
        {
            ViewBag.Error = error;

            using(ShopDB db = new ShopDB())
            {
                ViewBag.Products = db.Items.ToList();
                return View();
            }
        }

        public bool CanPurchase(Item item, int quantity = 1)
        {
            if (Session["Logged"] != null)
            {
                Customer buying = (Customer)Session["Logged"];
                if(buying.Balance > item.Price * quantity)
                {
                    return true;
                }
            }

            return false;
        }

        public ActionResult UpdateInfo(int target, string value)
        {
            using (ShopDB db = new ShopDB())
            {
                Customer toUpdate = (Customer)Session["Logged"];
                db.Customers.Attach(toUpdate);
                switch (target)
                {//i wish i knew a better way to do this but i didnt wanna make a bunch of
                    case 2: toUpdate.Password = value; break;//individual methods either
                    case 3: toUpdate.Email = value; break;
                    case 4: toUpdate.Dob = DateTime.Parse(value); break;
                    case 5: toUpdate.Color = value; break;
                }
                db.SaveChanges();
                return RedirectToAction("Welcome");
            }

        }
        public ActionResult Purchase(string itemname, int quantity = 1)
        {
            using(ShopDB db = new ShopDB())
            {
                Customer buyer = (Customer)Session["Logged"];
                db.Customers.Attach(buyer);
                Item item = db.Items.Single(i => i.Name == itemname);

                if (CanPurchase(item, quantity))
                {
                    Order purchase = new Order();
                    purchase.Item = item;
                    purchase.User = buyer;
                    purchase.UserName = buyer.UserName;
                    purchase.Quantity = quantity;
                    purchase.Name = item.Name;
                    buyer.Balance -= quantity * item.Price;
                    item.Quantity -= quantity;
                    db.Orders.Add(purchase);
                    db.SaveChanges();
                    return RedirectToAction("Shop");
                }
                
                return RedirectToAction("Shop", new { error="Insufficient funds."});
            }

            
        }
    }
}