﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commercesite.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace E_commercesite.Controllers
{
    public class FoodsController : Controller
    {
        private readonly EcommerceContext _context;

        public FoodsController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Foods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Food.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .FirstOrDefaultAsync(m => m.ID == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Price,Image")] Food food)
        {
            if (ModelState.IsValid)
            {
                byte[] fileBytes;
                SQLFunction functions = HttpContext.RequestServices.GetService(typeof(SQLFunction)) as SQLFunction;
                using (MemoryStream ms = new MemoryStream())
                {
  
                    food.Image.CopyTo(ms);
                    fileBytes = ms.ToArray();

                }
                functions.InsertFood(food.Name, food.Price, fileBytes);
              _context.Add(food);
              await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(food);
        }

        public IActionResult PurchaseOrder()
        {
            String username = HttpContext.Session.GetString("Username");
            if (!String.IsNullOrEmpty(username))
            {
                ViewData["Username"] = username;
            }
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "User");
            }
            double totalPrice = Double.Parse(TempData["totalPrice"].ToString());


            return RedirectToAction("Index", "Payment", totalPrice);
        }

        public IActionResult Cart()
        {
            String username = HttpContext.Session.GetString("Username");
            if (!String.IsNullOrEmpty(username))
            {
                ViewData["Username"] = username;
            }
            SQLFunction functions = HttpContext.RequestServices.GetService(typeof(SQLFunction)) as SQLFunction;
            String listString = functions.GetUserCart(username);

            String[] listArray = listString.Split(",");

            double totalPrice = 0.0;
            List<Food> allFood = new List<Food>();
            foreach (String s in listArray)
            {
               if (!String.IsNullOrEmpty(s))
               {

                    allFood.Add(functions.RetriveCart(s));
                   // totalPrice += allFood.ElementAt(allFood.Count() - 1).Price;
               }   
  
            }
            
            TempData["totalPrice"] = totalPrice;

            return View(allFood);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price")] Food food)
        {
            if (id != food.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .FirstOrDefaultAsync(m => m.ID == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Food.FindAsync(id);
            _context.Food.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.ID == id);
        }
    }
}
