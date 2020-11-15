using NotesAPI.Dtos.Notes;
using NotesAPI.Models;
using NotesAPI.Models.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotesAPI.Services.NoteService
{
    public interface INoteService
    {
        Task<ServiceResponse<IEnumerable<GetNoteDto>>> AllNotesByUserId(int userId);
        Task<ServiceResponse<GetNoteDto>> GetNoteById(int id);
        Task<ServiceResponse<GetNoteDto>> AddNewNote(NoteDto newNote);
        Task<ServiceResponse<GetNoteDto>> UpdateNote(GetNoteDto note);
        Task<ServiceResponse<bool>> DeleteNoteById(int id);
    }
}