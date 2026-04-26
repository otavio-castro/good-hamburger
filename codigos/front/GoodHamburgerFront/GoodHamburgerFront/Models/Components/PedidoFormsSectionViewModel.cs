using GoodHamburgerFront.Models;

namespace GoodHamburgerFront.Models.Components;

public sealed class PedidoFormsSectionViewModel
{
    public List<ProdutoViewModel> Sanduiches { get; set; } = [];
    public ProdutoViewModel? Batata { get; set; }
    public ProdutoViewModel? Refrigerante { get; set; }
    public int? BuscarPedidoId { get; set; }
    public int? EditarPedidoId { get; set; }
    public int? SanduicheId { get; set; }
    public bool IncluirBatata { get; set; }
    public bool IncluirRefrigerante { get; set; }
}
