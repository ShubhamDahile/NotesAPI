using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesAPI.Dtos.Account
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}