using CardapioServiceClass = GoodHamburgerApi.Services.CardapioService;

namespace GoodHamburgerTest.Services.CardapioService
{
    public class CardapioServiceTeste
    {
        public CardapioServiceClass CardapioService { get; private set; }

        public CardapioServiceTeste()
        {
            // O código pode apresentar erro de build aqui devido à nova injeção de dependência na controller.
            // Conforme pedido, o arquivo de testes não sofrerá mais alterações por enquanto.
            //CardapioService = new CardapioServiceClass();
        }

        [Fact]
        public void Test1()
        {
        }
    }
}