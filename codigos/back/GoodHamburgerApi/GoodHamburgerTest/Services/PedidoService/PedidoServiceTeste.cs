using GoodHamburgerApi.Context.Models;
using Moq;

namespace GoodHamburgerTest.Services.PedidoService
{
    public class PedidoServiceTeste : IClassFixture<PedidoServiceFixture>
    {
        private readonly PedidoServiceFixture _fixture;

        public PedidoServiceTeste(PedidoServiceFixture fixture)
        {
            _fixture = fixture;
        }

        #region Fluxo de criação

        [Fact]
        public async Task CriarPedido_ComComboCompleto_AplicaDesconto20()
        {
            #region Arrange

            _fixture.GerarCenarioCardapioPadrao();
            _fixture.GerarCenarioCriacaoPedidoRetornandoMesmoObjeto();

            var dto = PedidoServiceHelper.CriarPedidoComItens(1, 4, 5);

            #endregion

            #region Act

            var resultado = await _fixture.Sut.CriarPedidoAsync(dto);

            #endregion

            #region Assert

            Assert.Equal(9.5m, resultado.Subtotal);
            Assert.Equal(1.9m, resultado.ValorDesconto);
            Assert.Equal(7.6m, resultado.Total);
            Assert.Equal(3, resultado.Itens.Count);
            _fixture.PedidoRepositoryMock.Verify(repo => repo.CriarAsync(It.IsAny<Pedido>()), Times.Once);

            #endregion
        }

        [Fact]
        public async Task CriarPedido_ComItemDuplicado_DeveFalhar()
        {
            #region Arrange

            _fixture.GerarCenarioCardapioPadrao();
            var dto = PedidoServiceHelper.CriarPedidoComItens(1, 1);

            #endregion

            #region Act

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _fixture.Sut.CriarPedidoAsync(dto));

            #endregion

            #region Assert

            Assert.Contains("duplicados", ex.Message, StringComparison.OrdinalIgnoreCase);
            _fixture.PedidoRepositoryMock.Verify(repo => repo.CriarAsync(It.IsAny<Pedido>()), Times.Never);

            #endregion
        }

        #endregion

        #region Fluxo de consulta

        [Fact]
        public async Task ObterPedidoPorId_QuandoExiste_RetornaPedido()
        {
            #region Arrange

            _fixture.GerarCenarioCardapioPadrao();
            var pedido = PedidoServiceHelper.PedidoPersistidoComItens((10, 1, 5m), (11, 4, 2m));
            _fixture.GerarCenarioPedidoPorIdExistente(pedido, 99);

            #endregion

            #region Act

            var resultado = await _fixture.Sut.ObterPedidoPorIdAsync(99);

            #endregion

            #region Assert

            Assert.NotNull(resultado);
            Assert.Equal(99, resultado!.Id);
            Assert.Equal(2, resultado.Itens.Count);

            #endregion
        }

        [Fact]
        public async Task ObterPedidoPorId_QuandoNaoExiste_RetornaNulo()
        {
            #region Arrange

            _fixture.GerarCenarioPedidoPorIdInexistente();

            #endregion

            #region Act

            var resultado = await _fixture.Sut.ObterPedidoPorIdAsync(12345);

            #endregion

            #region Assert

            Assert.Null(resultado);

            #endregion
        }

        #endregion

        #region Fluxo de atualização

        [Fact]
        public async Task AtualizarPedido_QuandoExiste_RecalculaValores()
        {
            #region Arrange

            _fixture.GerarCenarioCardapioPadrao();
            _fixture.GerarCenarioAtualizacaoPedidoRetornandoMesmoObjeto();

            var pedidoExistente = PedidoServiceHelper.PedidoPersistidoComItens((21, 1, 5m));
            _fixture.GerarCenarioPedidoPorIdExistente(pedidoExistente, 99);

            var dto = PedidoServiceHelper.CriarPedidoComItens(2, 5);

            #endregion

            #region Act

            var resultado = await _fixture.Sut.AtualizarPedidoAsync(99, dto);

            #endregion

            #region Assert

            Assert.NotNull(resultado);
            Assert.Equal(7m, resultado!.Subtotal);
            Assert.Equal(1.05m, resultado.ValorDesconto);
            Assert.Equal(5.95m, resultado.Total);
            Assert.Equal(2, resultado.Itens.Count);
            _fixture.PedidoRepositoryMock.Verify(repo => repo.AtualizarAsync(It.IsAny<Pedido>()), Times.Once);

            #endregion
        }

        [Fact]
        public async Task AtualizarPedido_QuandoNaoExiste_RetornaNulo()
        {
            #region Arrange

            _fixture.GerarCenarioPedidoPorIdInexistente();
            var dto = PedidoServiceHelper.CriarPedidoComItens(1, 5);

            #endregion

            #region Act

            var resultado = await _fixture.Sut.AtualizarPedidoAsync(12345, dto);

            #endregion

            #region Assert

            Assert.Null(resultado);
            _fixture.PedidoRepositoryMock.Verify(repo => repo.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);

            #endregion
        }

        #endregion

        #region Fluxo de remoção

        [Fact]
        public async Task RemoverPedido_QuandoExiste_RetornaTrue()
        {
            #region Arrange

            _fixture.GerarCenarioRemocaoPedido(true, 99);

            #endregion

            #region Act

            var resultado = await _fixture.Sut.RemoverPedidoAsync(99);

            #endregion

            #region Assert

            Assert.True(resultado);

            #endregion
        }

        [Fact]
        public async Task RemoverPedido_QuandoNaoExiste_RetornaFalse()
        {
            #region Arrange

            _fixture.GerarCenarioRemocaoPedido(false);

            #endregion

            #region Act

            var resultado = await _fixture.Sut.RemoverPedidoAsync(12345);

            #endregion

            #region Assert

            Assert.False(resultado);

            #endregion
        }

        #endregion
    }
}
