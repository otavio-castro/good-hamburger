namespace GoodHamburgerApi.Dtos
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public List<PedidoItemDto> Itens { get; set; } = [];
        public decimal Subtotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal Total { get; set; }
    }
}
