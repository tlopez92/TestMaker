﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TestMakerFreeWeb.Data
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApplicationUser
    {
        #region Constructor
        public ApplicationUser()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Notes { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Flags { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Lazy-Load Properties
        public virtual List<Quiz> Quizzes { get; set; }
        #endregion
    }
}
