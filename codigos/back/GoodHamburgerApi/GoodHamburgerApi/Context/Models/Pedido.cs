using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodHamburgerApi.Context.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public List<PedidoItem> Itens { get; set; } = [];

        public decimal Subtotal { get; set; }

        public decimal ValorDesconto { get; set; }
        public decimal Total { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }

    public class PedidoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Pedido))]
        public int PedidoId { get; set; }

        [Required]
        public Pedido Pedido { get; set; } = null!;

        // Referência ao ID do Produto que virá do arquivo JSON do cardápio.
        // Como o cardápio não será editável no banco, ProdutoId apenas liga com o JSON.
        public int ProdutoId { get; set; }

        public decimal PrecoUnitario { get; set; }

        // Propriedade não salva no banco de dados, serve apenas para carregar os detalhes
        // do cardápio em memória e ajudar na exibição do item do pedido e cálculos.
        [NotMapped]
        public Produto? ProdutoDetalhes { get; set; }
    }
}