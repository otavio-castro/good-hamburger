namespace GoodHamburgerFront.Models;

public sealed class PedidoItemViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
}
