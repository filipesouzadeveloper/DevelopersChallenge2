using DevChallenge.Import.OFX.Api.Domain.Contracts;
using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevChallenge.Import.OFX.Api.Domain.DadosBanco.Repositories
{
    public interface IImportarArquivoRepository : IRepository<RegistroBanco>
    {
        void IncluirAsync(RegistroBanco entity);
    }
}
