using GoodHamburgerApi.Dtos;
using Moq;

namespace GoodHamburgerTest.Services.CardapioService
{
    public class CardapioServiceTeste : IClassFixture<CardapioServiceFixture>
    {
        private readonly CardapioServiceFixture _fixture;

        public CardapioServiceTeste(CardapioServiceFixture fixture)
        {
            _fixture = fixture;
        }

        #region Fluxo de consulta do cardápio

        [Fact]
        public async Task DeveRetornarCardapio_QuandoArquivoExistir()
        {
            #region Arrange

            var produtos = _fixture.GerarCenarioCardapioComItens();

            #endregion

            #region Act

            var resultado = (await _fixture.Sut.ObterCardapio()).ToList();

            #endregion

            #region Assert

            Assert.Equal(3, resultado.Count);
            Assert.Contains(resultado, x => x is ProdutoDto dto && dto.Nome == produtos[0].Nome);
            _fixture.CardapioRepositoryMock.Verify(repo => repo.ObterCardapio(It.IsAny<string>()), Times.Once);
            _fixture.CardapioFileProviderMock.Verify(provider => provider.Exists(), Times.Once);

            #endregion
        }

        [Fact]
        public async Task DeveRetornarVazio_QuandoArquivoNaoExistir()
        {
            #region Arrange

            _fixture.GerarCenarioCardapioVazio();

            #endregion

            #region Act

            var resultado = (await _fixture.Sut.ObterCardapio()).ToList();

            #endregion

            #region Assert

            Assert.Empty(resultado);
            _fixture.CardapioRepositoryMock.Verify(repo => repo.ObterCardapio(It.IsAny<string>()), Times.Never);
            _fixture.CardapioFileProviderMock.Verify(provider => provider.Exists(), Times.Once);

            #endregion
        }

        #endregion
    }
}