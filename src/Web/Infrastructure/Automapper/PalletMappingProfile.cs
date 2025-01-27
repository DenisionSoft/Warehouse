using AutoMapper;
using Warehouse.Business.Pallets;
using Warehouse.Domain.Entities;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.Web.Infrastructure.Automapper;

internal sealed class PalletMappingProfile : Profile
{
    public PalletMappingProfile()
    {
        CreateMap<Pallet, PalletResponse>();
        CreateMap<CreatePalletRequest, CreatePalletCommand>().BeforeMap(MapperPropertyValidator.CheckForDisallowedNulls);
        CreateMap<UpdatePalletRequest, UpdatePalletCommand>().BeforeMap(MapperPropertyValidator.CheckForDisallowedNulls);
    }
}
