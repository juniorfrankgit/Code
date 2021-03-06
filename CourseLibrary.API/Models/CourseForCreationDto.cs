﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    public class CourseForCreationDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
