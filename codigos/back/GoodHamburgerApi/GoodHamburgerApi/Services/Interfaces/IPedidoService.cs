using GoodHamburgerApi.Dtos;

namespace GoodHamburgerApi.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDto>> ObterListaPedidosAsync();
        Task<PedidoDto?> ObterPedidoPorIdAsync(int id);

        Task<PedidoDto> CriarPedidoAsync(CriarPedidoDto criarPedidoDto);
        Task<PedidoDto?> AtualizarPedidoAsync(int id, CriarPedidoDto criarPedidoDto);
        Task<bool> RemoverPedidoAsync(int id);
    }
}