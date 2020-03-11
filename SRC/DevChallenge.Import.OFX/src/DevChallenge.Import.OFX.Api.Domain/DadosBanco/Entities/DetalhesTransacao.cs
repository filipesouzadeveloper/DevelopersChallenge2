using System;

namespace DevChallenge.Import.OFX.Api.Domain.DadosBanco.Entities
{
    public class DetalhesTransacao
    {
        public String Trntype { get; set; }
        public DateTime Dtposted { get; set; }
        public double Trnamt { get; set; }
        public String Memo { get; set; }
    }
}
