using AutoMapper;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StateDb, State>();
            CreateMap<State, StateDb>();

            CreateMap<ZipCodeDb, ZipCode>();
            CreateMap<ZipCode, ZipCodeDb>();
        }
    }
}
