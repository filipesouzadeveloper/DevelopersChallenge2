using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Entities;
using System;
using System.Threading.Tasks;

namespace DevChallenge.Import.OFX.Application.Interfaces
{
    public interface IImportarArquivoAppService
    {
        RegistroBanco ExecutarLeituraAsync(string arquivoOFX);
    }
}
