using Dapper;
using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Entities;
using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Repositories;
using DevChallenge.Import.OFX.Infra.Data.Contexts;
using System.Threading.Tasks;

namespace DevChallenge.Import.OFX.Infra.Data.DadosBanco.Repositories
{
    public class ImportarArquivoRepository : Repository<RegistroBanco>, IImportarArquivoRepository
    {
        public ImportarArquivoRepository(SFContext context) : base(context)
        {
        }
        public void IncluirAsync(RegistroBanco entity)
        {
            _context.Connection.ExecuteAsync(
                 @"INSERT 
                     INTO IMPORTAR_ARQUIVO_OFX ( CODE
                                               , SEVERITY
                                               , DTSERVER
                                               , LANGUAGE
                                               , TRNUID
                                               , CURDEF
                                               , BANKID
                                               , ACCTID
                                               , ACCTTYPE
                                               , DTSTART
                                               , DTEND
                                               , TRNTYPE
                                               , DTPOSTED
                                               , TRNAMT
                                               , MEMO
                                               )
                                        VALUES ( CODE
                                               , SEVERITY
                                               , DTSERVER
                                               , LANGUAGE
                                               , TRNUID
                                               , CURDEF
                                               , BANKID
                                               , ACCTID
                                               , ACCTTYPE
                                               , DTSTART
                                               , DTEND
                                               , TRNTYPE
                                               , DTPOSTED
                                               , TRNAMT
                                               , MEMO
                                               )",
            new
            {
                CODE = entity.Code,
                SEVERITY = entity.Severity,
                DTSERVER = entity.Dtserver,
                LANGUAGE = entity.Language,
                TRNUID = entity.Trnuid,
                CURDEF = entity.Curdef,
                BANKID = entity.Bankid,
                ACCTID = entity.Acctid,
                ACCTTYPE = entity.Accttype,
                DTSTART = entity.Dtstart,
                DTEND = entity.Dtend,
                TRNTYPE = entity.DetalhesTransacao,
                DTPOSTED = entity.DetalhesTransacao,
                TRNAMT = entity.DetalhesTransacao,
                MEMO = entity.DetalhesTransacao
            });
        }
    }
}