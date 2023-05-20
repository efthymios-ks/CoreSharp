using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSharp.DependencyInjection.ByInstaller.Interfaces;

public interface IServiceCollectionInstaller
{
    void Install(IServiceCollection serviceCollection, IConfiguration configuration);
}
