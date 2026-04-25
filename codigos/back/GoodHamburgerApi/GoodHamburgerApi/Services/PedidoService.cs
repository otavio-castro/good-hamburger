using AutoMapper;
using GoodHamburgerApi.Context.Models;
using GoodHamburgerApi.Dtos;
using GoodHamburgerApi.Repositories.Interfaces;
using GoodHamburgerApi.Services.Interfaces;

namespace GoodHamburgerApi.Services
{
    public class PedidoService : IPedidoService
    {
        private const string CategoriaSanduiche = "Sanduiche";
        private const string CategoriaAcompanhamento = "Acompanhamento";
        private const string CategoriaBebida = "Bebida";

        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICardapioService _cardapioService;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository pedidoRepository, ICardapioService cardapioService, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _cardapioService = cardapioService;
            _mapper = mapper;
        }

        public async Task<PedidoDto> CriarPedidoAsync(CriarPedidoDto criarPedidoDto)
        {
            var cardapio = await _cardapioService.ObterCardapio().ConfigureAwait(false);
            var pedido = CriarPedidoModel(criarPedidoDto, cardapio);

            await _pedidoRepository.CriarAsync(pedido).ConfigureAwait(false);

            return MapearPedidoDto(pedido, cardapio);
        }

        public async Task<PedidoDto?> ObterPedidoPorIdAsync(int id)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id).ConfigureAwait(false);

            if (pedido == null)
            {
                return null;
            }

            var cardapio = await _cardapioService.ObterCardapio().ConfigureAwait(false);

            return MapearPedidoDto(pedido, cardapio);
        }

        public async Task<IEnumerable<PedidoDto>> ObterListaPedidosAsync()
        {
            var pedidos = await _pedidoRepository.ObterListaAsync().ConfigureAwait(false);
            var cardapio = await _cardapioService.ObterCardapio().ConfigureAwait(false);

            return pedidos.Select(pedido => MapearPedidoDto(pedido, cardapio));
        }

        public async Task<PedidoDto?> AtualizarPedidoAsync(int id, CriarPedidoDto criarPedidoDto)
        {
            var pedidoExistente = await _pedidoRepository.ObterPorIdAsync(id).ConfigureAwait(false);

            if (pedidoExistente == null)
            {
                return null;
            }

            var cardapio = await _cardapioService.ObterCardapio().ConfigureAwait(false);
            var pedidoAtualizado = CriarPedidoModel(criarPedidoDto, cardapio);

            pedidoExistente.Subtotal = pedidoAtualizado.Subtotal;
            pedidoExistente.ValorDesconto = pedidoAtualizado.ValorDesconto;
            pedidoExistente.Total = pedidoAtualizado.Total;
            pedidoExistente.Itens = pedidoAtualizado.Itens;

            await _pedidoRepository.AtualizarAsync(pedidoExistente).ConfigureAwait(false);

            return MapearPedidoDto(pedidoExistente, cardapio);
        }

        public async Task<bool> RemoverPedidoAsync(int id)
        {
            return await _pedidoRepository.RemoverAsync(id).ConfigureAwait(false);
        }

        private static List<ProdutoDto> ObterProdutosCardapioDoPedido(CriarPedidoDto criarPedidoDto, IEnumerable<ProdutoDto> cardapio)
        {
            var produtosSelecionados = criarPedidoDto.ProdutoIds
                .Select(id => cardapio.FirstOrDefault(produto => produto.Id == id))
                .ToList();

            var idsNaoEncontrados = criarPedidoDto.ProdutoIds
                .Where(id => produtosSelecionados.All(produto => produto?.Id != id))
                .Distinct()
                .ToList();

            if (idsNaoEncontrados.Count > 0)
            {
                throw new KeyNotFoundException($"Produto(s) não encontrado(s) no cardápio: {string.Join(", ", idsNaoEncontrados)}.");
            }

            var produtos = produtosSelecionados
                .OfType<ProdutoDto>()
                .ToList();
            return produtos;
        }

        private static Pedido CriarPedidoModel(CriarPedidoDto criarPedidoDto, IEnumerable<ProdutoDto> cardapio)
        {
            if (criarPedidoDto.ProdutoIds.Count == 0)
            {
                throw new ArgumentException("Pedido inválido: informe ao menos 1 item.");
            }

            ValidarItensDuplicados(criarPedidoDto.ProdutoIds);

            List<ProdutoDto> produtosDoCardapioExistentesNoPedido = ObterProdutosCardapioDoPedido(criarPedidoDto, cardapio);

            ValidarQuantidadesPorCategoria(produtosDoCardapioExistentesNoPedido);

            var subtotal = produtosDoCardapioExistentesNoPedido.Sum(produto => produto.Preco);
            var valorDesconto = AplicarDesconto(subtotal, produtosDoCardapioExistentesNoPedido);
            var total = subtotal - valorDesconto;

            var pedido = new Pedido
            {
                Subtotal = subtotal,
                ValorDesconto = valorDesconto,
                Total = total,
                Itens = produtosDoCardapioExistentesNoPedido.Select(produto => new PedidoItem
                {
                    ProdutoId = produto.Id,
                    PrecoUnitario = produto.Preco
                }).ToList()
            };

            return pedido;
        }

        private PedidoDto MapearPedidoDto(Pedido pedido, IEnumerable<ProdutoDto> produtosDoCardapio)
        {
            var pedidoDto = _mapper.Map<PedidoDto>(pedido);

            Parallel.ForEach(pedido.Itens, itemPedido =>
            {
                // Busca o produto do cardápio pelo ProdutoId salvo no item do pedido.
                var produto = produtosDoCardapio.FirstOrDefault(produto => produto.Id == itemPedido.ProdutoId);

                // Busca o item mapeado no DTO para preencher campos adicionais.
                var itemDto = pedidoDto.Itens.FirstOrDefault(item => item.Id == itemPedido.Id);

                // Se encontrou ambos, completa o Nome que será exibido no front.
                if (produto != null && itemDto != null)
                    itemDto.Nome = produto.Nome;
            });

            return pedidoDto;
        }

        private static void ValidarItensDuplicados(List<int> produtoIds)
        {
            var idsDuplicados = produtoIds
                .GroupBy(id => id)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            if (idsDuplicados.Count > 0)
            {
                throw new ArgumentException($"Itens duplicados no pedido: {string.Join(", ", idsDuplicados)}.");
            }
        }

        private static void ValidarQuantidadesPorCategoria(List<ProdutoDto> produtos)
        {
            var quantidadeSanduiches = produtos.Count(produto => produto.Categoria.Equals(CategoriaSanduiche, StringComparison.OrdinalIgnoreCase));
            var quantidadeBatatas = produtos.Count(produto => produto.Categoria.Equals(CategoriaAcompanhamento, StringComparison.OrdinalIgnoreCase));
            var quantidadeRefrigerantes = produtos.Count(produto => produto.Categoria.Equals(CategoriaBebida, StringComparison.OrdinalIgnoreCase));

            if (quantidadeSanduiches > 1)
            {
                throw new ArgumentException("Pedido inválido: é permitido apenas 1 sanduíche por categoria.");
            }

            if (quantidadeBatatas > 1)
            {
                throw new ArgumentException("Pedido inválido: é permitido apenas 1 batata frita por categoria.");
            }

            if (quantidadeRefrigerantes > 1)
            {
                throw new ArgumentException("Pedido inválido: é permitido apenas 1 refrigerante por categoria.");
            }
        }

        private static decimal AplicarDesconto(decimal subtotal, List<ProdutoDto> produtos)
        {
            var temSanduiche = produtos.Any(produto => produto.Categoria.Equals(CategoriaSanduiche, StringComparison.OrdinalIgnoreCase));
            var temBatata = produtos.Any(produto => produto.Categoria.Equals(CategoriaAcompanhamento, StringComparison.OrdinalIgnoreCase));
            var temRefrigerante = produtos.Any(produto => produto.Categoria.Equals(CategoriaBebida, StringComparison.OrdinalIgnoreCase));

            var percentualDesconto = 0m;

            if (temSanduiche && temBatata && temRefrigerante)
            {
                percentualDesconto = 0.20m;
            }
            else if (temSanduiche && temRefrigerante)
            {
                percentualDesconto = 0.15m;
            }
            else if (temSanduiche && temBatata)
            {
                percentualDesconto = 0.10m;
            }

            return Math.Round(subtotal * percentualDesconto, 2);
        }
    }
}