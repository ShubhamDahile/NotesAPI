using AutoMapper;
using NotesAPI.Dtos.Notes;
using NotesAPI.Models;
using NotesAPI.Models.Account;
using NotesAPI.Models.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Security.Principal;
using System.Security.Claims;

namespace NotesAPI.Services.NoteService
{
    public class NoteService : INoteService
    {        
        private readonly DataContext _context;
        public NoteService()
        {
            _context = new DataContext();
        }

        private int GetUserId()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            User userInDb = _context.Users.SingleOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            return userInDb.Id;            
        }

        public async Task<ServiceResponse<GetNoteDto>> AddNewNote(NoteDto newNote)
        {
            ServiceResponse <GetNoteDto> serviceResponse = new ServiceResponse<GetNoteDto>();
            try
            {
                newNote.CreatedDate = DateTime.Now;
                newNote.UpdatedDate = newNote.CreatedDate;
                newNote.UserId = GetUserId();
                _context.Notes.Add(Mapper.Map<NoteDto, Note>(newNote));
                await _context.SaveChangesAsync();

                Note noteInDb = _context.Notes.Where(n => n.UserId == newNote.UserId && n.Title.ToLower() == newNote.Title.ToLower())
                                    .OrderByDescending(n => n.Id)
                                    .FirstOrDefault();
                serviceResponse.Data = Mapper.Map<Note, GetNoteDto>(noteInDb);
                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

                return serviceResponse;
            }                        
        }

        public async Task<ServiceResponse<IEnumerable<GetNoteDto>>> AllNotesByUserId(int userId)
        {
            ServiceResponse<IEnumerable<GetNoteDto>> serviceResponse = new ServiceResponse<IEnumerable<GetNoteDto>>();
            serviceResponse.Data = _context.Notes.Where(n=>n.UserId==userId).ToList().Select(Mapper.Map<Note, GetNoteDto>);

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteNoteById(int id)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            int userId = GetUserId();
            var noteInDb = _context.Notes.SingleOrDefault(n => n.Id == id && n.UserId == userId);
            if (noteInDb == null)
            {
                serviceResponse.Data = false;
                serviceResponse.Success = false;
                serviceResponse.Message = "Note Not Found";

                return serviceResponse;
            }

            _context.Notes.Remove(noteInDb);
            await _context.SaveChangesAsync();

            serviceResponse.Data = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetNoteDto>> GetNoteById(int id)
        {
            ServiceResponse<GetNoteDto> serviceResponse = new ServiceResponse<GetNoteDto>();
            int userId = GetUserId();
            var noteInDb = _context.Notes.SingleOrDefault(n => n.Id == id && n.UserId == userId);
            if (noteInDb == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Note Not Found";

                return serviceResponse;
            }

            serviceResponse.Data = Mapper.Map<Note, GetNoteDto>(noteInDb);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetNoteDto>> UpdateNote(GetNoteDto note)
        {
            ServiceResponse<GetNoteDto> serviceResponse = new ServiceResponse<GetNoteDto>();
            try
            {
                Note noteInDb = _context.Notes.SingleOrDefault(n => n.Id == note.Id && n.UserId == note.UserId);
                if (noteInDb == null)
                {
                    serviceResponse.Data = note;
                    serviceResponse.Success = false;
                    serviceResponse.Message = " Note Not Found";

                    return serviceResponse;
                }

                
                noteInDb.UpdatedDate = DateTime.Now;
                Mapper.Map<GetNoteDto, Note>(note, noteInDb);
                await _context.SaveChangesAsync();

                serviceResponse.Data = Mapper.Map<GetNoteDto>(_context.Notes.SingleOrDefault(n => n.Id == note.Id && n.UserId == note.UserId));
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

                return serviceResponse;
            }
        }
    }
}