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

        public async Task<Pedido?> ObterPorIdAsync(int id)
        {
            return await _appDbContext.Pedidos
                .Include(pedido => pedido.Itens)
                .FirstOrDefaultAsync(pedido => pedido.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<Pedido> CriarAsync(Pedido pedido)
        {
            await _appDbContext.Pedidos.AddAsync(pedido).ConfigureAwait(false);
            await _appDbContext.SaveChangesAsync().ConfigureAwait(false);
            return pedido;
        }

        public async Task<Pedido> AtualizarAsync(Pedido pedido)
        {
            _appDbContext.Pedidos.Update(pedido);
            await _appDbContext.SaveChangesAsync().ConfigureAwait(false);
            return pedido;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var pedido = await _appDbContext.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);

            if (pedido == null)
            {
                return false;
            }

            _appDbContext.Pedidos.Remove(pedido);
            await _appDbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}