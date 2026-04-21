using AutoMapper;
using GoodHamburgerApi.Dtos;
using GoodHamburgerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerApi.Controllers
{
    [Route("api/cardapio")]
    [ApiController]
    public class CardapioController : ControllerBase
    {
        private readonly ICardapioService _cardapioService;

        public CardapioController(ICardapioService cardapioService, IMapper mapper)
        {
            _cardapioService = cardapioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetCardapio()
        {
            var cardapio = await _cardapioService.ObterCardapio().ConfigureAwait(false);

            return Ok(cardapio);
        }
    }
}