﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colibri.Data;
using Colibri.Extensions;
using Colibri.Models;
using Colibri.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Colibri.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ColibriDbContext _colibriDbContext;

        // bind the ShoppingCartViewModel
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public ShoppingCartController(ColibriDbContext colibriDbContext)
        {
            _colibriDbContext = colibriDbContext;

            // initialize the ShoppingCartViewModel
            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                Products = new List<Models.Products>()
            };
        }

        // Get Index ShoppingCart
        // retrieve all the Products from the Session
        public async Task<IActionResult> Index()
        {
            // check first, if anything exists in the Session
            // Session Name : "ssSessionOrderExists"
            List<int> lstSessionOrderExists = HttpContext.Session.Get<List<int>>("ssSessionOrderExists");

            if (lstSessionOrderExists.Count > 0)
            {
                foreach (int cartItem in lstSessionOrderExists)
                {
                    // get the Products from the DB
                    // use the eager Method
                    Products products = _colibriDbContext.Products
                        .Include(p => p.CategoryTypes)
                        .Include(p => p.SpecialTags)
                        .Where(p => p.Id == cartItem)
                        .FirstOrDefault();

                    // add the Products to the Shopping Cart
                    ShoppingCartViewModel.Products.Add(products);
                }
            }
            
            // pass the ShoppingCartViewModel to the View
            return View(ShoppingCartViewModel);
        }

        // POST: Index
        // create an Appointment
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult IndexPost()
        {
            // check first, if anything exists in the Session
            // Session Name : "ssSessionOrderExists"
            List<int> lstSessionOrderExists = HttpContext.Session.Get<List<int>>("ssSessionOrderExists");

            // merge (add) the Appointment Date and Time to the Appointment Date itself
            ShoppingCartViewModel.Appointments.AppointmentDate = ShoppingCartViewModel.Appointments.AppointmentDate
                                                                    .AddHours(ShoppingCartViewModel.Appointments.AppointmentTime.Hour)
                                                                    .AddMinutes (ShoppingCartViewModel.Appointments.AppointmentTime.Minute);

            // create an Object for the Appointments
            Appointments appointments = ShoppingCartViewModel.Appointments;

            // add the Appointments to the DB
            _colibriDbContext.Appointments.Add(appointments);
            _colibriDbContext.SaveChanges();

            // by saving one gets the Appointment Id that has been just created
            int appointmentId = appointments.Id;

            // this created Id can be used to insert Records inside the selected Products
            foreach (int productId in lstSessionOrderExists)
            {
                // everytime a new Object will be created
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = productId
                };

                // add to the DB
                _colibriDbContext.ProductsSelectedForAppointment.Add(productsSelectedForAppointment);
            }
            // save the Changes all together after the Iteration
            _colibriDbContext.SaveChanges();

            // After adding the Items to the DB, empty the Cart (by creating a new Session)
            lstSessionOrderExists = new List<int>();
            HttpContext.Session.Set("ssSessionOrderExists", lstSessionOrderExists);

            // redirect to Action
            return RedirectToAction(nameof(Index));
        }
    }
}