using System;

namespace DevChallenge.Import.OFX.Application.ViewModel
{
    public class DetalhesTransacaoViewModel
    {
        public String Trntype { get; set; }
        public DateTime Dtposted { get; set; }
        public double Trnamt { get; set; }
        public String Memo { get; set; }
    }
}
