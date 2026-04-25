using GoodHamburgerApi.Context.Models;

namespace GoodHamburgerApi.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ObterListaAsync();
        Task<Pedido?> ObterPorIdAsync(int id);
        Task<Pedido> CriarAsync(Pedido pedido);
        Task<Pedido> AtualizarAsync(Pedido pedido);
        Task<bool> RemoverAsync(int id);
    }
}
