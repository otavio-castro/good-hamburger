namespace GoodHamburgerApi.Dtos
{
    public class PedidoItemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
    }
}
