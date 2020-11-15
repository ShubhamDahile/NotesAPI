using NotesAPI.Dtos.Notes;
using NotesAPI.Models;
using NotesAPI.Services.NoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;


namespace NotesAPI.Controllers
{
    [Authorize]
    public class NotesController : ApiController
    {
        private readonly INoteService _noteService;
        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllNotes()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value);

            ServiceResponse<IEnumerable<GetNoteDto>> response = await _noteService.AllNotesByUserId(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetNote(int id)
        {
            ServiceResponse<GetNoteDto> response = await _noteService.GetNoteById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostNote(NoteDto newNote)
        {
            ServiceResponse<GetNoteDto> response = await _noteService.AddNewNote(newNote);
            if (response.Success)
            {
                return Ok(response);
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutNote(GetNoteDto newNote)
        {
            ServiceResponse<GetNoteDto> response = await _noteService.UpdateNote(newNote);
            if (response.Success)
            {
                return Ok(response);
            }
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteNote(int id)
        {
            ServiceResponse<bool> response = await _noteService.DeleteNoteById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return Ok(response);
        }

    }
}
