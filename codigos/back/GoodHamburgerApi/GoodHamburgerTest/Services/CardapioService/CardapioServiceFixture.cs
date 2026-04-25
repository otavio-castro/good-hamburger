using AutoMapper;
using GoodHamburgerApi.Context.Models;
using GoodHamburgerApi.Repositories.Interfaces;
using GoodHamburgerApi.Services.Interfaces;
using Moq;
using GoodHamburgerTest.Shared;
using CardapioServiceClass = GoodHamburgerApi.Services.CardapioService;

namespace GoodHamburgerTest.Services.CardapioService
{
    public class CardapioServiceFixture : FixtureBase<CardapioServiceClass>
    {
        public Mock<ICardapioRepository> CardapioRepositoryMock { get; }
        public Mock<ICardapioFileProvider> CardapioFileProviderMock { get; }
        public IMapper Mapper { get; }

        public CardapioServiceFixture()
        {
            CardapioRepositoryMock = CreateMock<ICardapioRepository>();
            CardapioFileProviderMock = CreateMock<ICardapioFileProvider>();
            Mapper = CreateMapper();

            Sut = new CardapioServiceClass(CardapioRepositoryMock.Object, Mapper, CardapioFileProviderMock.Object);
        }

        public Produto[] GerarCenarioCardapioComItens()
        {
            ResetMocks();

            var produtos = CardapioServiceHelper.GerarCardapioMock();

            CardapioFileProviderMock
                .Setup(provider => provider.Exists())
                .Returns(true);

            CardapioFileProviderMock
                .Setup(provider => provider.ReadAllText())
                .Returns("[{\"Id\":1}]");

            CardapioRepositoryMock
                .Setup(repo => repo.ObterCardapio(It.IsAny<string>()))
                .ReturnsAsync(produtos);

            return produtos;
        }

        public void GerarCenarioCardapioVazio()
        {
            ResetMocks();

            CardapioFileProviderMock
                .Setup(provider => provider.Exists())
                .Returns(false);
        }
    }
}
