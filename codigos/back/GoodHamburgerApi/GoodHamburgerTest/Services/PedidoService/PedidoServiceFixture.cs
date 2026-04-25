using AutoMapper;
using GoodHamburgerApi.Context.Models;
using GoodHamburgerApi.Repositories.Interfaces;
using GoodHamburgerApi.Services.Interfaces;
using Moq;
using GoodHamburgerTest.Shared;
using PedidoServiceClass = GoodHamburgerApi.Services.PedidoService;

namespace GoodHamburgerTest.Services.PedidoService
{
    public class PedidoServiceFixture : FixtureBase<PedidoServiceClass>
    {
        public Mock<IPedidoRepository> PedidoRepositoryMock { get; }
        public Mock<ICardapioService> CardapioServiceMock { get; }
        public IMapper Mapper { get; }

        public PedidoServiceFixture()
        {
            PedidoRepositoryMock = CreateMock<IPedidoRepository>();
            CardapioServiceMock = CreateMock<ICardapioService>();
            Mapper = CreateMapper();

            Sut = new PedidoServiceClass(PedidoRepositoryMock.Object, CardapioServiceMock.Object, Mapper);
        }

        public void GerarCenarioCriacaoPedidoValido()
        {
            ResetMocks();

            CardapioServiceMock
                .Setup(service => service.ObterCardapio())
                .ReturnsAsync(PedidoServiceHelper.CardapioPadrao());

            PedidoRepositoryMock
                .Setup(repo => repo.CriarAsync(It.IsAny<Pedido>()))
                .ReturnsAsync((Pedido pedido) => pedido);
        }

        public void GerarCenarioCriacaoPedidoComCardapioPadrao()
        {
            ResetMocks();

            CardapioServiceMock
                .Setup(service => service.ObterCardapio())
                .ReturnsAsync(PedidoServiceHelper.CardapioPadrao());
        }

        public void GerarCenarioObterPedidoPorIdExistente(Pedido pedido, int id)
        {
            ResetMocks();

            CardapioServiceMock
                .Setup(service => service.ObterCardapio())
                .ReturnsAsync(PedidoServiceHelper.CardapioPadrao());

            PedidoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(id))
                .ReturnsAsync(pedido);
        }

        public void GerarCenarioObterPedidoPorIdInexistente()
        {
            ResetMocks();

            PedidoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Pedido?)null);
        }

        public void GerarCenarioAtualizacaoPedidoExistente(Pedido pedido, int id)
        {
            ResetMocks();

            CardapioServiceMock
                .Setup(service => service.ObterCardapio())
                .ReturnsAsync(PedidoServiceHelper.CardapioPadrao());

            PedidoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(id))
                .ReturnsAsync(pedido);

            PedidoRepositoryMock
                .Setup(repo => repo.AtualizarAsync(It.IsAny<Pedido>()))
                .ReturnsAsync((Pedido pedidoAtualizado) => pedidoAtualizado);
        }

        public void GerarCenarioAtualizacaoPedidoInexistente()
        {
            ResetMocks();

            PedidoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Pedido?)null);
        }

        public void GerarCenarioRemocaoPedido(bool removido, int? id = null)
        {
            ResetMocks();

            if (id.HasValue)
            {
                PedidoRepositoryMock
                    .Setup(repo => repo.RemoverAsync(id.Value))
                    .ReturnsAsync(removido);

                return;
            }

            PedidoRepositoryMock
                .Setup(repo => repo.RemoverAsync(It.IsAny<int>()))
                .ReturnsAsync(removido);
        }
    }
}
