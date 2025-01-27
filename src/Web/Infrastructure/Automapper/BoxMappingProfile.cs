using AutoMapper;
using Warehouse.Business.Boxes;
using Warehouse.Domain.Entities;
using Warehouse.Web.Contracts.Models.Box;

namespace Warehouse.Web.Infrastructure.Automapper;

internal sealed class BoxMappingProfile : Profile
{
    public BoxMappingProfile()
    {
        CreateMap<Box, BoxResponse>();
        CreateMap<CreateBoxRequest, CreateBoxCommand>().BeforeMap(MapperPropertyValidator.CheckForDisallowedNulls);
        CreateMap<UpdateBoxRequest, UpdateBoxCommand>().BeforeMap(MapperPropertyValidator.CheckForDisallowedNulls);
    }
}
