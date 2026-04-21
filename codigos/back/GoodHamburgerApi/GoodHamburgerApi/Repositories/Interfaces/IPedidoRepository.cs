using GoodHamburgerApi.Context.Models;

namespace GoodHamburgerApi.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ObterListaAsync();
        Task<Pedido> CriarAsync(Pedido pedido);
    }
}
