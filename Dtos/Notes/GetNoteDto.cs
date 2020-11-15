using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesAPI.Dtos.Notes
{
    public class GetNoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string keyword { get; set; }
        public string Discription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UserId { get; set; }
    }
}