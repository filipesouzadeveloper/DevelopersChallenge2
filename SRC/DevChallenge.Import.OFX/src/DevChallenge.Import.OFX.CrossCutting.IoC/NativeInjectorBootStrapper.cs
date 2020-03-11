using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Repositories;
using DevChallenge.Import.OFX.Application.DadosBanco;
using DevChallenge.Import.OFX.Application.Interfaces;
using DevChallenge.Import.OFX.Infra.Data.Contexts;
using DevChallenge.Import.OFX.Infra.Data.DadosBanco.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevChallenge.Import.OFX.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<SFContext>();
            services.AddScoped<IImportarArquivoAppService, ImportarArquivoAppService>();
            services.AddScoped<IImportarArquivoRepository, ImportarArquivoRepository>();

        }
    }
}
