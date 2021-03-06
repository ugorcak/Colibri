﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colibri.Models
{
    // Identity User
    public class ApplicationUser : IdentityUser
    {
        // Accessors
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int UserId { get; set; }

        public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string CareOf { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }

        // Admin Area
        public bool IsSuperAdmin { get; set; }

        // Rating the User
        public int NumberOfApplicationUserRates { get; set; }
        public double UserRating { get; set; }

        // Logging
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
