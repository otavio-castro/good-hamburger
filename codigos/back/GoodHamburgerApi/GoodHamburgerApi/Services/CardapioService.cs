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
        private readonly string _cardapioFilePath = Path.Combine("Data", "cardapio.json");

        public CardapioService(ICardapioRepository cardapioRepository, IMapper mapper)
        {
            _cardapioRepository = cardapioRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProdutoDto>> ObterCardapio()
        {
            if (!File.Exists(_cardapioFilePath))
                return [];

            var jsonBytes = File.ReadAllText(_cardapioFilePath);

            var cardapio = await _cardapioRepository.ObterCardapio(jsonBytes).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ProdutoDto>>(cardapio);
        }
    }
}