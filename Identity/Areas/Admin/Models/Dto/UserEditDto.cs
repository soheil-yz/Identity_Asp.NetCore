﻿namespace Identity.Areas.Admin.Models.Dto
{
    public class UserEditDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhotoNumber { get; set; }
    }
}