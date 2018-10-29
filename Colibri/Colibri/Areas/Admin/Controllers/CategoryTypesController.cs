﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colibri.Data;
using Colibri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Colibri.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Administrator, PowerUser")]
    [Area("Admin")]
    public class CategoryTypesController : Controller
    {
        private ColibriDbContext _colibriDbContext;

        public CategoryTypesController(ColibriDbContext colibriDbContext)
        {
            _colibriDbContext = colibriDbContext;
        }

        // Get: /<controller>/Create
        [HttpGet("CategoryTypes/Create")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // Post: /<controller>/Create
        // @param Category
        [HttpPost("CategoryTypes/Create")]
        //[Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryTypes categoryTypes)
        {
            // Check the State Model Binding
            if (ModelState.IsValid)
            {
                _colibriDbContext.Add(categoryTypes);
                await _colibriDbContext.SaveChangesAsync();

                // avoid Refreshing the POST Operation -> Redirect
                //return View("Details", newCategory);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // one can simply return to the Form View again for Correction
                return View(categoryTypes);
            }
        }

        // Get: /<controller>/Edit
        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // search for the ID
            var categoryType = await _colibriDbContext.CategoryTypes.FindAsync(id);

            if (categoryType == null)
            {
                return NotFound();
            }
            return View(categoryType);
        }

        // Post: /<controller>/Edit
        // @param Category
        [HttpPost("CategoryTypes/Edit")]
        //[Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryTypes categoryTypes)
        {
            // the IDs should be equal
            if (id != categoryTypes.Id)
            {
                return NotFound();
            }

            // Check the State Model Binding
            if (ModelState.IsValid)
            {
                // Update the Changes
                _colibriDbContext.Update(categoryTypes);
                await _colibriDbContext.SaveChangesAsync();

                // avoid Refreshing the POST Operation -> Redirect
                //return View("Details", newCategory);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // one can simply return to the Form View again for Correction
                return View(categoryTypes);
            }
        }

        // Get: /<controller>/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // search for the ID
            var categoryType = await _colibriDbContext.CategoryTypes.FindAsync(id);

            if (categoryType == null)
            {
                return NotFound();
            }
            return View(categoryType);
        }

        // Get: /<controller>/Delete
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // search for the ID
            var categoryType = await _colibriDbContext.CategoryTypes.FindAsync(id);

            if (categoryType == null)
            {
                return NotFound();
            }
            return View(categoryType);
        }

        // Post: /<controller>/Delete
        // @param Category
        [HttpPost("CategoryTypes/Delete"),ActionName("Delete")]
        //[Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryType = await _colibriDbContext.CategoryTypes.FindAsync(id);

            _colibriDbContext.CategoryTypes.Remove(categoryType);

            // Update the Changes
            await _colibriDbContext.SaveChangesAsync();

            // avoid Refreshing the POST Operation -> Redirect
            //return View("Details", newCategory);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            var categoryTypesList = _colibriDbContext.CategoryTypes.ToList();

            return View(categoryTypesList);
        }
    }
}