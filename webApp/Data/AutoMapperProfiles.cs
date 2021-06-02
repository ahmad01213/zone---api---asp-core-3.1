using System;
using AutoMapper;
using webApp.Dtos;
using webApp.Models;

namespace webApp.Data
{
    public class AutoMapperProfiles
   : Profile
    {
       
            public AutoMapperProfiles()
        {

            CreateMap<CityToAddDto, City>();
            CreateMap<CartAddRequest, Cart>();

        }
    }
}
