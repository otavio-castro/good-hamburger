using GoodHamburgerApi.Context;
using GoodHamburgerApi.Context.Models;
using GoodHamburgerApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerApi.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;

        public PedidoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Pedido>> ObterListaAsync()
        {
            return await _appDbContext.Pedidos
                .Include(pedido => pedido.Itens)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Pedido> CriarAsync(Pedido pedido)
        {
            await _appDbContext.Pedidos.AddAsync(pedido).ConfigureAwait(false);
            await _appDbContext.SaveChangesAsync().ConfigureAwait(false);
            return pedido;
        }
    }
}