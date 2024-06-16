using AutoMapper;
using FlightManagementSystem.Entities;
using FlightManagementSystem.Models;

namespace FlightManagementSystem
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, UserDto>();
        }
    }
}
