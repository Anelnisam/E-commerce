using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_commercesite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;


namespace E_commercesite.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index(String pr)
        {
            String username = HttpContext.Session.GetString("Username");
            if (!String.IsNullOrEmpty(username))
            {
                ViewData["Username"] = username;
            }
            ViewData["totalPrice"] = pr;

            return View();
        }

    public IActionResult ChargeCard([Bind("number", "expMonth", "expYear", "CCV")] CreditCard creditCard)

        {
            StripeConfiguration.SetApiKey("sk_test_LYk237i5xPRlstCABBIWqDsb00eWmKk85w");

            if (!ModelState.IsValid)
            {
                return View(creditCard);
            }

            var tokenServices = new StripeTokenCreateOptions
            {
                Card = new StripeCreditCardOptions
                {
                    Number = creditCard.number,
                    ExpirationMonth = creditCard.expMonth,
                    ExpirationYear = creditCard.expYear,
                    Cvc = creditCard.CCV

                }
            };
            StripeTokenService service = new StripeTokenService();
            try
            {
                //StripeToken token = service.Create(tokenServices);
                // double totalPrice = double.Parse(ViewData["totalPrice"].ToString());
                // int totalPriceInt = (int)(100 * totalPrice);
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
                        totalPrice += allFood.ElementAt(allFood.Count() - 1).Price;
                    }
                }

                TempData["totalPrice"] = totalPrice;

              
                int totalPriceInt = (int)(100 * totalPrice);


                var chargeOption = new StripeChargeCreateOptions
                {
                    Amount = totalPriceInt,
                    Currency = "usd",
                    Description = "Description",
                    SourceTokenOrExistingSourceId = "tok_amex"
                };

                StripeChargeService chargeService = new StripeChargeService();
                StripeCharge charge = chargeService.Create(chargeOption);

            }catch(StripeException e)
            {
                ViewData["CardError"] = e.Message;
            }

            return View("Index");
        }
    }
}