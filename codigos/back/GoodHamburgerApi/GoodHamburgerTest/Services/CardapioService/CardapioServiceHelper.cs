using GoodHamburgerApi.Context.Models;

namespace GoodHamburgerTest.Services.CardapioService
{
    public static class CardapioServiceHelper
    {
        public static Produto[] GerarCardapioMock()
        {
            var random = new Random();

            return
            [
                new Produto { Id = 1, Nome = $"X Burger {random.Next(100, 999)}", Categoria = "Sanduiche", Preco = 5m },
                new Produto { Id = 4, Nome = $"Batata frita {random.Next(100, 999)}", Categoria = "Acompanhamento", Preco = 2m },
                new Produto { Id = 5, Nome = $"Refrigerante {random.Next(100, 999)}", Categoria = "Bebida", Preco = 2.5m }
            ];
        }
    }
}
