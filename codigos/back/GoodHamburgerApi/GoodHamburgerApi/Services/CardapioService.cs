using AutoMapper;
using GoodHamburgerApi.Dtos;
using GoodHamburgerApi.Repositories.Interfaces;
using GoodHamburgerApi.Services.Interfaces;

namespace GoodHamburgerApi.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly ICardapioRepository _cardapioRepository;
        private readonly IMapper _mapper;
        private readonly ICardapioFileProvider _cardapioFileProvider;

        public CardapioService(ICardapioRepository cardapioRepository, IMapper mapper, ICardapioFileProvider cardapioFileProvider)
        {
            _cardapioRepository = cardapioRepository;
            _mapper = mapper;
            _cardapioFileProvider = cardapioFileProvider;
        }

        public async Task<IEnumerable<ProdutoDto>> ObterCardapio()
        {
            if (!_cardapioFileProvider.Exists())
                return [];

            var jsonBytes = _cardapioFileProvider.ReadAllText();

            var cardapio = await _cardapioRepository.ObterCardapio(jsonBytes).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ProdutoDto>>(cardapio);
        }
    }
}