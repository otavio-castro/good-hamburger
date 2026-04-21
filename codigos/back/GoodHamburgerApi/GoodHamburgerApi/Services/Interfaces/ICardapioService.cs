using GoodHamburgerApi.Dtos;

namespace GoodHamburgerApi.Services.Interfaces
{
    public interface ICardapioService
    {
        Task<IEnumerable<ProdutoDto>> ObterCardapio();
    }
}