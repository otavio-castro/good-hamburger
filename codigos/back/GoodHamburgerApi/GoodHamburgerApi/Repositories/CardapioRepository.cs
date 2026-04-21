using GoodHamburgerApi.Context.Models;
using GoodHamburgerApi.Repositories.Interfaces;
using System.Text.Json;

namespace GoodHamburgerApi.Repositories
{
    public class CardapioRepository : ICardapioRepository
    {
        public async Task<IEnumerable<Produto>> ObterCardapio(string jsonBytes)
        {
            var produtos = JsonSerializer.Deserialize<List<Produto>>(jsonBytes);

            return produtos ?? [];
        }
    }
}