using GoodHamburgerFront.Models;

namespace GoodHamburgerFront.Models.Components;

public sealed class PedidoFormsSectionViewModel
{
    public List<ProdutoViewModel> Sanduiches { get; set; } = [];
    public ProdutoViewModel? Batata { get; set; }
    public ProdutoViewModel? Refrigerante { get; set; }
}
