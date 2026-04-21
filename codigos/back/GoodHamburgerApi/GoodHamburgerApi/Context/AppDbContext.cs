using GoodHamburgerApi.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerApi.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItens { get; set; }
}