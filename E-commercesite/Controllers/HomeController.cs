using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_commercesite.Models;
using Microsoft.AspNetCore.Http;

namespace E_commercesite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            String username = HttpContext.Session.GetString("Username");
            if (!String.IsNullOrEmpty(username))
            {
                ViewData["Username"] = username;
            }

            SQLFunction functions = HttpContext.RequestServices.GetService(typeof(SQLFunction)) as SQLFunction;
            List<Food> allFood = functions.RetriveFood(true);

            return View(allFood);

        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Username", String.Empty);

            return Redirect("Index");
        }

        public  bool UpdateCart(String itemid)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return false;
            }
            SQLFunction functions = HttpContext.RequestServices.GetService(typeof(SQLFunction)) as SQLFunction;
            String username = HttpContext.Session.GetString("Username");
            String listString = functions.GetUserCart(username);
            listString += itemid + ",";
            functions.UpdateCart(username, listString);

            return true;
        }

        public IActionResult ViewCart()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "User");
            }
            return RedirectToAction("Cart", "Foods");
        }


        public IActionResult AllFood()

        {
            String username = HttpContext.Session.GetString("Username");
            if (!String.IsNullOrEmpty(username))
            {
                ViewData["Username"] = username;
            }
            SQLFunction functions = HttpContext.RequestServices.GetService(typeof(SQLFunction)) as SQLFunction;
            List<Food> allFood = functions.RetriveFood(false);
            return View(allFood);
        }

        public IActionResult About()
        {
            String username = HttpContext.Session.GetString("Username");
            if (!String.IsNullOrEmpty(username))
            {
                ViewData["Username"] = username;
            }

            return View();
        }

        public IActionResult Contact()
        {
            String username = HttpContext.Session.GetString("Username");
            if (!String.IsNullOrEmpty(username))
            {
                ViewData["Username"] = username;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SignUp()
        {
            return RedirectToAction("Create", "User");
        }

    }
}
