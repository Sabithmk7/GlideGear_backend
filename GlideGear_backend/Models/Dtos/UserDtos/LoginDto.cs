﻿using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models.Dtos.UserDtos
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}