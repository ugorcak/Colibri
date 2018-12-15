﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colibri.Areas.Admin.Controllers;
using Colibri.Data;
using Colibri.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Colibri.Areas.Customer.Controllers
{
    /*
     * Controller for the Products View
     */
    [Area("Customer")]
    public class ProductsHomeController : Controller
    {
        //private readonly IColibriRepository _repository;
        private readonly ColibriDbContext _colibriDbContext;
        private readonly IStringLocalizer<ProductsHomeController> _localizer;

        //public ProductsHomeController(IColibriRepository repository)
        public ProductsHomeController(ColibriDbContext colibriDbContext, IStringLocalizer<ProductsHomeController> localizer)
        {
            //_repository = repository;
            _colibriDbContext = colibriDbContext;
            _localizer = localizer;
        }

        // Entry (Index) View
        [Route("Customer/ProductsHome/Index")]
        public async Task<IActionResult> Index()
        {
            //var productList = _repository.GetAllProductsAsync();
            var productList = await _colibriDbContext.Products
                    .Include(p => p.CategoryGroups)
                    .Include(p => p.CategoryTypes)
                    //.Include(p => p.SpecialTags)
                    .ToListAsync();

            // i18n
            ViewData["ProductName"] = _localizer["ProductNameText"];
            ViewData["Price"] = _localizer["PriceText"];
            ViewData["ViewDetails"] = _localizer["ViewDetailsText"];

            return View(productList);
        }

        // Details View GET
        [Route("Customer/ProductsHome/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            // get the individual Product
            var product = await _colibriDbContext.Products
                    .Include(p => p.CategoryGroups)
                    .Include(p => p.CategoryTypes)
                    //.Include(p => p.SpecialTags)
                    .Where(p => p.Id == id)
                    .FirstOrDefaultAsync();

            // count the Number of Clicks on the Product
            product.NumberOfClicks += 1;

            // save the Changes in DB
            await _colibriDbContext.SaveChangesAsync();

            // i18n
            ViewData["ProductDetails"] = _localizer["ProductDetailsText"];
            ViewData["Name"] = _localizer["NameText"];
            ViewData["Price"] = _localizer["PriceText"];
            ViewData["CategoryGroup"] = _localizer["CategoryGroupText"];
            ViewData["CategoryType"] = _localizer["CategoryTypeText"];
            ViewData["SpecialTag"] = _localizer["SpecialTagText"];
            ViewData["RemoveFromBag"] = _localizer["RemoveFromBagText"];
            ViewData["Order"] = _localizer["OrderText"];
            ViewData["Description"] = _localizer["DescriptionText"];
            ViewData["BackToList"] = _localizer["BackToListText"];

            return View(product);
        }

        // Details POST
        [Route("Customer/ProductsHome/Details/{id}")]
        [HttpPost,ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            // check first, if anything exists in the Session
            // Session Name : "ssShoppingCart"
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            // check if null -> create new
            if (lstCartItems == null)
            {
                lstCartItems = new List<int>();
            }

            // add the retrieved Item (id)
            lstCartItems.Add(id);
            // set the Session:
            // Session Name, Value
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            // redirect to Action
            return RedirectToAction("Index", "ProductsHome", new { area = "Customer" });
        }

        // Remove (from Bag)
        [Route("Customer/ProductsHome/Remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            {
                if (lstCartItems.Contains(id))
                {
                    // remove the Item (id)
                    lstCartItems.Remove(id);
                }
            }
            // set the Session: Name, Value
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            // redirect to Action
            return RedirectToAction(nameof(Index));
        }

    }
}