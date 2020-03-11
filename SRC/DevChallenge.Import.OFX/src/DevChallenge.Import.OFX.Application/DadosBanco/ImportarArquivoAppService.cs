using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Entities;
using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Repositories;
using DevChallenge.Import.OFX.Application.Interfaces;
using DevChallenge.Import.OFX.Application.ViewModel;
using DevChallenge.Import.OFX.CrossCutting.Common.Helpers;

namespace DevChallenge.Import.OFX.Application.DadosBanco
{
    public class ImportarArquivoAppService : IImportarArquivoAppService
    {
        private readonly IImportarArquivoRepository _importarArquivoRepository;
        public ImportarArquivoAppService(IImportarArquivoRepository importarArquivoRepository)
        {
            _importarArquivoRepository = importarArquivoRepository;
        }
        public RegistroBanco ExecutarLeituraAsync(string arquivoOFX)
        {
            var executarLeitura = LeituraArquivoOFX.ExecutarLeitura(arquivoOFX);
            _importarArquivoRepository.IncluirAsync(executarLeitura);
            return executarLeitura;
        }
    }
}
