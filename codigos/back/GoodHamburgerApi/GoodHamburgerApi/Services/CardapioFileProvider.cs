using GoodHamburgerApi.Services.Interfaces;

namespace GoodHamburgerApi.Services
{
    public class CardapioFileProvider : ICardapioFileProvider
    {
        private readonly string _cardapioFilePath = Path.Combine("Data", "cardapio.json");

        public bool Exists() => File.Exists(_cardapioFilePath);

        public string ReadAllText() => File.ReadAllText(_cardapioFilePath);
    }
}
