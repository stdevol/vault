using System;
using System.Threading;
using AutoMapper;
using JetBrains.Annotations;
using Vault.Core.Organizations;

namespace Vault.Core
{
    [UsedImplicitly]
    public class MapperConfigurator
    {
        private static readonly Lazy<MapperConfiguration> MapperConfiguration
            = new Lazy<MapperConfiguration>(() => new MapperConfiguration(Configure), LazyThreadSafetyMode.ExecutionAndPublication);

        public static IMapper CreateMapper()
            => MapperConfiguration.Value.CreateMapper();

        private static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<OrganizationProfile>();
        }
    }
}
