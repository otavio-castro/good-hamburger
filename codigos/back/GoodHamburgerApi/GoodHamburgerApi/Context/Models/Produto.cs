using System.ComponentModel.DataAnnotations.Schema;

namespace GoodHamburgerApi.Context.Models
{
    // Modelo do Produto do Cardápio para representar os itens do JSON.
    [NotMapped]
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        // Categoria pode ser Sanduiche, Acompanhamento, Bebida, etc.
        public string Categoria { get; set; } = string.Empty;
    }
}
