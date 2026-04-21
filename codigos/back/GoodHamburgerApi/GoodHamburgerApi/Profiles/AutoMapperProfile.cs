using AutoMapper;
using GoodHamburgerApi.Context.Models;
using GoodHamburgerApi.Dtos;

namespace GoodHamburgerApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Model p/Dto
            CreateMap<Produto, ProdutoDto>();
            CreateMap<PedidoItem, PedidoItemDto>();
            CreateMap<Pedido, PedidoDto>();
        }
    }
}
