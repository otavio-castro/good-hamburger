using GoodHamburgerApi.Context.Models;
using GoodHamburgerApi.Dtos;

namespace GoodHamburgerTest.Services.PedidoService
{
    public static class PedidoServiceHelper
    {
        public static List<ProdutoDto> CardapioPadrao() =>
        [
            new ProdutoDto { Id = 1, Nome = "X Burger", Categoria = "Sanduiche", Preco = 5m },
            new ProdutoDto { Id = 2, Nome = "X Egg", Categoria = "Sanduiche", Preco = 4.5m },
            new ProdutoDto { Id = 3, Nome = "X Bacon", Categoria = "Sanduiche", Preco = 7m },
            new ProdutoDto { Id = 4, Nome = "Batata frita", Categoria = "Acompanhamento", Preco = 2m },
            new ProdutoDto { Id = 5, Nome = "Refrigerante", Categoria = "Bebida", Preco = 2.5m }
        ];

        public static CriarPedidoDto CriarPedidoComItens(params int[] ids) =>
            new()
            {
                ProdutoIds = ids.ToList()
            };

        public static Pedido PedidoPersistidoComItens(params (int itemId, int produtoId, decimal precoUnitario)[] itens)
        {
            return new Pedido
            {
                Id = 99,
                Subtotal = itens.Sum(x => x.precoUnitario),
                ValorDesconto = 0,
                Total = itens.Sum(x => x.precoUnitario),
                Itens = itens.Select(x => new PedidoItem
                {
                    Id = x.itemId,
                    ProdutoId = x.produtoId,
                    PrecoUnitario = x.precoUnitario
                }).ToList()
            };
        }
    }
}
