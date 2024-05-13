using API.Models;
using API.Requests;
using API.Responses;
using AutoMapper;

namespace API.Mappings;

public class StaffProfile : Profile
{
    public StaffProfile()
    {
        CreateMap<AddStaffRequest, Staff>().ReverseMap();
        CreateMap<EditStaffRequest, Staff>().ReverseMap();
        CreateMap<StaffResponse, Staff>().ReverseMap();
        CreateMap<StaffsResponse, Staff>().ReverseMap();
    }
}