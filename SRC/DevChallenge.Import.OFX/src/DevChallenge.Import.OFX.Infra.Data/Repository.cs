using Dapper;
using Dapper.FluentMap;
using DevChallenge.Import.OFX.Api.Domain.Contracts;
using DevChallenge.Import.OFX.Infra.Data.Contexts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevChallenge.Import.OFX.Infra.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected string Campos;
        protected string Tabela;
        protected string Parametros;
        protected virtual string CamposSemAlias =>
           string.Join(',', Campos.Split(',').ToList().Select(s => s.Split("as")[0]));

        protected SFContext _context;

        public Repository(SFContext context)
        {
            if (FluentMapper.EntityMaps.IsEmpty)
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
            }

            _context = context;
        }
        public virtual async Task InserirAsync(TEntity entity)
        {
            var sbSql = new StringBuilder("Insert into ");
            sbSql.Append(Tabela);
            sbSql.Append(" ( ");
            sbSql.Append(CamposSemAlias);
            sbSql.Append(" ) values ( ");
            sbSql.Append(Parametros);
            sbSql.Append(" ) ");

            await _context.Connection.ExecuteAsync(sbSql.ToString(), entity);
        }
    }
}
