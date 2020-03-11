using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DevChallenge.Import.OFX.Infra.Data.Contexts
{
    public class SFContext : IDisposable
    {
        private readonly IConfiguration _configuration;
        public IDbConnection Connection { get; private set; }

        public SFContext(IConfiguration config)
        {
            _configuration = config;
            Connection = CreateConnection();
        }

        private IDbConnection CreateConnection()
        {
            return new OracleConnection(_configuration.GetSection("Oracle:ConnectionString").Value);
        }

        public void Dispose()
        {
            if (Connection == null) return;

            Connection.Dispose();
            Connection = null;
        }
    }
}
