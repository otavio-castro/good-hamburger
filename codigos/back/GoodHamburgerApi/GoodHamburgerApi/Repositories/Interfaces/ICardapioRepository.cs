using System.Collections.Generic;
using GoodHamburgerApi.Context.Models;

namespace GoodHamburgerApi.Repositories.Interfaces
{
    public interface ICardapioRepository
    {
        Task<IEnumerable<Produto>> ObterCardapio(string jsonBytes);
    }
}
