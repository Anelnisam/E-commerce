using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_commercesite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commercesite.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View("Login");
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CreateUser([Bind("username", "password", "confirm")]User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            System.Diagnostics.Debug.WriteLine(user.username);
            SQLFunction functions = HttpContext.RequestServices.GetService(typeof(SQLFunction)) as SQLFunction;

            functions.InsertUser(user.username, user.password);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginUser ([Bind("username", "password")]User user)
        {
            if (ModelState["username"].Errors.Any() || ModelState["password"].Errors.Any())
            {
                return View("Login");
            }

            SQLFunction functions = HttpContext.RequestServices.GetService(typeof(SQLFunction)) as SQLFunction;
            if (functions.CheckUser(user.username, user.password))
            {
                HttpContext.Session.SetString("Username", user.username);
                return RedirectToAction("Index", "Home");
            }

            ViewData["Invalid"] = "Invalid Username/Password";
            return View("Login");
        }
    }
}