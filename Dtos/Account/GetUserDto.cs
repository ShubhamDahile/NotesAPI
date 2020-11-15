using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesAPI.Dtos.Account
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}