﻿namespace GlideGear_backend.Models.Dtos.UserDtos
{
    public class UserViewDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public bool isBlocked { get; set; }
    }
}
