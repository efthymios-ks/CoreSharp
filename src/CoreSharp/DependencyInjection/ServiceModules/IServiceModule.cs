using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSharp.DependencyInjection.ServiceModules;

public interface IServiceModule
{
    void Install(IServiceCollection serviceCollection, IConfiguration configuration);
}
