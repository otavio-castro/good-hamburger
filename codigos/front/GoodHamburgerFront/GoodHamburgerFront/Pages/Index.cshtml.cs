using GoodHamburgerFront.Models;
using GoodHamburgerFront.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoodHamburgerFront.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGoodHamburgerApiService _apiService;

        public IndexModel(IGoodHamburgerApiService apiService)
        {
            _apiService = apiService;
        }

        public List<ProdutoViewModel> Cardapio { get; private set; } = [];
        public List<PedidoViewModel> Pedidos { get; private set; } = [];

        [BindProperty]
        public int? SanduicheId { get; set; }

        [BindProperty]
        public bool IncluirBatata { get; set; }

        [BindProperty]
        public bool IncluirRefrigerante { get; set; }

        [BindProperty]
        public int? EditarPedidoId { get; set; }

        [TempData]
        public string? Sucesso { get; set; }

        [TempData]
        public string? Erro { get; set; }

        public async Task OnGetAsync()
        {
            await CarregarDadosAsync().ConfigureAwait(false);
        }

        public async Task<IActionResult> OnPostCriarAsync()
        {
            await CarregarCardapioAsync().ConfigureAwait(false);
            var produtoIds = MontarProdutoIds();

            if (produtoIds.Count == 0)
            {
                Erro = "Selecione pelo menos um item para criar o pedido.";
                return RedirectToPage();
            }

            var resultado = await _apiService.CriarPedidoAsync(produtoIds).ConfigureAwait(false);

            if (resultado.Ok)
            {
                Sucesso = "Pedido criado com sucesso.";
            }
            else
            {
                Erro = resultado.MensagemErro ?? "Não foi possível criar o pedido.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAtualizarAsync()
        {
            if (!EditarPedidoId.HasValue || EditarPedidoId.Value <= 0)
            {
                Erro = "Pedido inválido para atualização.";
                return RedirectToPage();
            }

            await CarregarCardapioAsync().ConfigureAwait(false);
            var produtoIds = MontarProdutoIds();
            if (produtoIds.Count == 0)
            {
                Erro = "Selecione pelo menos um item para atualizar o pedido.";
                return RedirectToPage();
            }

            var resultado = await _apiService.AtualizarPedidoAsync(EditarPedidoId.Value, produtoIds).ConfigureAwait(false);

            if (resultado.Ok)
            {
                Sucesso = "Pedido atualizado com sucesso.";
            }
            else
            {
                Erro = resultado.MensagemErro ?? "Não foi possível atualizar o pedido.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostExcluirAsync(int id)
        {
            if (id <= 0)
            {
                Erro = "Pedido inválido para exclusão.";
                return RedirectToPage();
            }

            var resultado = await _apiService.ExcluirPedidoAsync(id).ConfigureAwait(false);

            if (resultado.Ok)
            {
                Sucesso = "Pedido removido com sucesso.";
            }
            else
            {
                Erro = resultado.MensagemErro ?? "Não foi possível remover o pedido.";
            }

            return RedirectToPage();
        }

        private async Task CarregarDadosAsync()
        {
            try
            {
                Cardapio = await _apiService.ObterCardapioAsync().ConfigureAwait(false);
                Pedidos = await _apiService.ObterPedidosAsync().ConfigureAwait(false);
            }
            catch
            {
                Erro ??= "Não foi possível carregar dados da API. Verifique se o backend está rodando.";
                Cardapio = [];
                Pedidos = [];
            }
        }

        private async Task CarregarCardapioAsync()
        {
            try
            {
                Cardapio = await _apiService.ObterCardapioAsync().ConfigureAwait(false);
            }
            catch
            {
                Cardapio = [];
            }
        }

        private List<int> MontarProdutoIds()
        {
            var produtoIds = new List<int>();

            if (SanduicheId.HasValue)
            {
                produtoIds.Add(SanduicheId.Value);
            }

            var batata = Cardapio.FirstOrDefault(x => x.Categoria.Equals("Acompanhamento", StringComparison.OrdinalIgnoreCase));
            var refrigerante = Cardapio.FirstOrDefault(x => x.Categoria.Equals("Bebida", StringComparison.OrdinalIgnoreCase));

            if (IncluirBatata && batata != null)
            {
                produtoIds.Add(batata.Id);
            }

            if (IncluirRefrigerante && refrigerante != null)
            {
                produtoIds.Add(refrigerante.Id);
            }

            return produtoIds;
        }

    }
}
