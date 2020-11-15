using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesAPI.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;
    }
}