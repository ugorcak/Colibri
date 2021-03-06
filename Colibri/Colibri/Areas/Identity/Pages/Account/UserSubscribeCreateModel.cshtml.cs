using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Colibri.Data;
using Colibri.Models;
using Colibri.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Colibri.Areas.Identity.Pages.Account
{
    public class UserSubscribeCreateModel : PageModel
    {
        private readonly ColibriDbContext _colibriDbContext;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public UserSubscribeCreateModel(
            ColibriDbContext colibriDbContext,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _colibriDbContext = colibriDbContext;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public UserSubscribeNotificationsViewModel UserSubscribeNotificationsViewModel { get; set; }

        //[BindProperty]
        //public CategoryTypes CategoryTypes { get; set; }

        //public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGet()
        {
            UserSubscribeNotificationsViewModel = new UserSubscribeNotificationsViewModel
            {
                ApplicationUserCategoryTypesSubscriber = new ApplicationUserCategoryTypesSubscriber(),
                UserSubscriber = await _colibriDbContext.ApplicationUserCategoryTypesSubscribers
                                            .Include(m => m.CategoryTypes)
                                            .ToListAsync(),
                CategoryTypes = await _colibriDbContext.CategoryTypes
                                            .OrderBy(d => d.Name)
                                            .ToListAsync()
            };

            return Page();
        }

        // POST
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Security Claims
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;

                // Claims Identity
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                UserSubscribeNotificationsViewModel.ApplicationUserCategoryTypesSubscriber.ApplicationUserId =
                    claim.Value;

                // create a DB Entry
                var result = await _colibriDbContext.ApplicationUserCategoryTypesSubscribers
                                                        .AddAsync(UserSubscribeNotificationsViewModel
                                                            .ApplicationUserCategoryTypesSubscriber);

                if (result != null)
                {
                    await _colibriDbContext.SaveChangesAsync();
                    _logger.LogInformation("User chose the Notification Category Type.");
                }
                return RedirectToAction("Index", "", new { area = "" });
            }
            return Page();
        }
    }
}