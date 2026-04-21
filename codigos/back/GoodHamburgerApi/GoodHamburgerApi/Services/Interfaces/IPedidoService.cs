using GoodHamburgerApi.Dtos;

namespace GoodHamburgerApi.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDto>> ObterListaPedidosAsync();

        Task<PedidoDto> CriarPedidoAsync(CriarPedidoDto criarPedidoDto);
    }
}