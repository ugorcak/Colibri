﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colibri.Data;
using Colibri.Models;
using Colibri.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Colibri.Areas.Admin.Controllers
{
    /*
     * Controller to define the Special Tags for the Products
     * 
     * authorize only the SuperAdminEndUser
     */
    [Authorize(Roles = StaticDetails.SuperAdminEndUser)]
    [Area("Admin")]
    public class SpecialTagsController : Controller
    {
        private readonly ColibriDbContext _colibriDbContext;

        public SpecialTagsController(ColibriDbContext colibriDbContext)
        {
            _colibriDbContext = colibriDbContext;
        }

        [Route("Admin/SpecialTags/Index")]
        public IActionResult Index()
        {
            var specialTagsList = _colibriDbContext.SpecialTags.ToList();

            return View(specialTagsList);
        }

        // Get: /<controller>/Create
        [Route("Admin/SpecialTags/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // Post: /<controller>/Create
        // @param SpecialTag
        [Route("Admin/SpecialTags/Create")]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTags specialTags)
        {
            // Check the State Model Binding
            if (ModelState.IsValid)
            {
                _colibriDbContext.Add(specialTags);
                await _colibriDbContext.SaveChangesAsync();

                // avoid Refreshing the POST Operation -> Redirect
                //return View("Details", newCategory);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // one can simply return to the Form View again for Correction
                return View(specialTags);
            }
        }

        [Route("Admin/SpecialTags/Edit/{id}")]
        // Get: /<controller>/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // search for the ID
            var specialTag = await _colibriDbContext.SpecialTags.FindAsync(id);

            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        // Post: /<controller>/Edit
        // @param Category
        [Route("Admin/SpecialTags/Edit/{id}")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecialTags specialTags)
        {
            // the IDs should be equal
            if (id != specialTags.Id)
            {
                return NotFound();
            }

            // Check the State Model Binding
            if (ModelState.IsValid)
            {
                // Update the Changes
                _colibriDbContext.Update(specialTags);
                await _colibriDbContext.SaveChangesAsync();

                // avoid Refreshing the POST Operation -> Redirect
                //return View("Details", newCategory);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // one can simply return to the Form View again for Correction
                return View(specialTags);
            }
        }

        [Route("Admin/SpecialTags/Details/{id}")]
        // Get: /<controller>/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // search for the ID
            var specialTag = await _colibriDbContext.SpecialTags.FindAsync(id);

            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        // Get: /<controller>/Delete
        [Route("Admin/SpecialTags/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // search for the ID
            var specialTag = await _colibriDbContext.SpecialTags.FindAsync(id);

            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        // Post: /<controller>/Delete
        // @param Category
        [Route("Admin/SpecialTags/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var specialTag = await _colibriDbContext.SpecialTags.FindAsync(id);

            _colibriDbContext.SpecialTags.Remove(specialTag);

            // Update the Changes
            await _colibriDbContext.SaveChangesAsync();

            // avoid Refreshing the POST Operation -> Redirect
            //return View("Details", newCategory);
            return RedirectToAction(nameof(Index));
        }
    }
}