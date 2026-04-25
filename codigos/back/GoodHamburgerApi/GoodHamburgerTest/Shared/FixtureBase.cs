using AutoMapper;
using GoodHamburgerApi.Profiles;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace GoodHamburgerTest.Shared
{
    public abstract class FixtureBase<TSut> : IDisposable where TSut : class
    {
        private readonly List<Mock> _mocks = [];

        public TSut Sut { get; protected set; } = null!;

        protected Mock<TDependency> CreateMock<TDependency>() where TDependency : class
        {
            var mock = new Mock<TDependency>();
            _mocks.Add(mock);
            return mock;
        }

        protected static IMapper CreateMapper()
        {
            var mapperConfigExpression = new MapperConfigurationExpression();
            mapperConfigExpression.AddProfile<AutoMapperProfile>();

            var mapperConfig = new MapperConfiguration(mapperConfigExpression, NullLoggerFactory.Instance);
            return mapperConfig.CreateMapper();
        }

        public virtual void ResetMocks()
        {
            foreach (var mock in _mocks)
            {
                mock.Reset();
            }
        }

        public virtual void Dispose()
        {
            ResetMocks();
            GC.SuppressFinalize(this);
        }
    }
}
