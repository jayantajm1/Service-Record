

using AutoMapper;
using Service_Record.DAL.Entities;
using Service_Record.Models.DTOs;

namespace Service_Record.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserRegistrationDTO>().ReverseMap();



        }

    }
}