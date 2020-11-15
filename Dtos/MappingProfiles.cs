using AutoMapper;
using NotesAPI.Dtos.Account;
using NotesAPI.Dtos.Notes;
using NotesAPI.Models.Account;
using NotesAPI.Models.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesAPI.Dtos
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
        // Model to Domain
            Mapper.CreateMap<User, GetUserDto>();
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<Note, GetNoteDto>();

        // Domain to Model
            Mapper.CreateMap<GetUserDto, User>();
            Mapper.CreateMap<UserDto, User>();
            Mapper.CreateMap<GetNoteDto, Note>();
            Mapper.CreateMap<NoteDto, Note>();

        // Domain to Domain
            Mapper.CreateMap<LoginUserDto, GetUserDto>();
            Mapper.CreateMap<GetUserDto, LoginUserDto>();

        }
    }
}