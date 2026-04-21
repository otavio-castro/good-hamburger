using GoodHamburgerApi.Dtos;
using GoodHamburgerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerApi.Controllers
{
    [Route("api/pedidos")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidos()
        {
            var pedidos = await _pedidoService.ObterListaPedidosAsync().ConfigureAwait(false);

            return Ok(pedidos);
        }


        [HttpPost]
        public async Task<ActionResult<PedidoDto>> PostPedido([FromBody] CriarPedidoDto criarPedidoDto)
        {
            try
            {
                var pedido = await _pedidoService.CriarPedidoAsync(criarPedidoDto).ConfigureAwait(false);
                return Created($"/api/pedidos/{pedido.Id}", pedido);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
