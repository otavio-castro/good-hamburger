using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using GoodHamburgerFront.Models;

namespace GoodHamburgerFront.Services;

public sealed class GoodHamburgerApiService : IGoodHamburgerApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GoodHamburgerApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<List<ProdutoViewModel>> ObterCardapioAsync()
    {
        var client = CriarClient();
        var cardapio = await client.GetFromJsonAsync<List<ProdutoViewModel>>("/api/cardapio").ConfigureAwait(false);
        return cardapio ?? [];
    }

    public async Task<List<PedidoViewModel>> ObterPedidosAsync()
    {
        var client = CriarClient();
        var pedidos = await client.GetFromJsonAsync<List<PedidoViewModel>>("/api/pedidos").ConfigureAwait(false);
        return pedidos ?? [];
    }

    public Task<ApiResult> CriarPedidoAsync(List<int> produtoIds)
    {
        var body = new CriarPedidoRequest { ProdutoIds = produtoIds };
        return EnviarAsync(HttpMethod.Post, "/api/pedidos", body);
    }

    public Task<ApiResult> AtualizarPedidoAsync(int pedidoId, List<int> produtoIds)
    {
        var body = new CriarPedidoRequest { ProdutoIds = produtoIds };
        return EnviarAsync(HttpMethod.Put, $"/api/pedidos/{pedidoId}", body);
    }

    public Task<ApiResult> ExcluirPedidoAsync(int pedidoId)
    {
        return EnviarAsync(HttpMethod.Delete, $"/api/pedidos/{pedidoId}", null);
    }

    private HttpClient CriarClient()
    {
        var client = _httpClientFactory.CreateClient();
        var baseUrl = _configuration["Api:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("Configuração Api:BaseUrl não encontrada.");
        }

        client.BaseAddress = new Uri(baseUrl);
        return client;
    }

    private async Task<ApiResult> EnviarAsync(HttpMethod method, string rota, CriarPedidoRequest? body)
    {
        var client = CriarClient();
        using var request = new HttpRequestMessage(method, rota);

        if (body != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return new ApiResult(true, null);
        }

        var mensagem = await TentarExtrairMensagemErroAsync(response).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new ApiResult(false, mensagem ?? "Recurso não encontrado.");
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return new ApiResult(false, mensagem ?? "Dados inválidos para a operação.");
        }

        return new ApiResult(false, mensagem ?? "Erro ao processar requisição na API.");
    }

    private static async Task<string?> TentarExtrairMensagemErroAsync(HttpResponseMessage response)
    {
        try
        {
            var json = await response.Content.ReadFromJsonAsync<ErroApiResponse>().ConfigureAwait(false);
            return json?.Mensagem;
        }
        catch
        {
            return null;
        }
    }
}
