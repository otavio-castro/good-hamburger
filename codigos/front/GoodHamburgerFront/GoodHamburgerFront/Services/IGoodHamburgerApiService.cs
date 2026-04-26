using GoodHamburgerFront.Models;

namespace GoodHamburgerFront.Services;

public interface IGoodHamburgerApiService
{
    Task<List<ProdutoViewModel>> ObterCardapioAsync();
    Task<List<PedidoViewModel>> ObterPedidosAsync();
    Task<PedidoViewModel?> ObterPedidoPorIdAsync(int pedidoId);
    Task<ApiResult> CriarPedidoAsync(List<int> produtoIds);
    Task<ApiResult> AtualizarPedidoAsync(int pedidoId, List<int> produtoIds);
    Task<ApiResult> ExcluirPedidoAsync(int pedidoId);
}
