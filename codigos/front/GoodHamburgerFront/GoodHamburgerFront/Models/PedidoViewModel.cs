namespace GoodHamburgerFront.Models;

public sealed class PedidoViewModel
{
    public int Id { get; set; }
    public List<PedidoItemViewModel> Itens { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal Total { get; set; }
}
