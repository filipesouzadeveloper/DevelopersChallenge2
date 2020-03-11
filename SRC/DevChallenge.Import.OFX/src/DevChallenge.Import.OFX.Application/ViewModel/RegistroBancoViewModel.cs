using System;
using System.Collections.Generic;

namespace DevChallenge.Import.OFX.Application.ViewModel
{
    public class RegistroBancoViewModel
    {
        public String Code { get; set; }
        public String Severity { get; set; }
        public DateTime Dtserver { get; set; }
        public String Language { get; set; }
        public String Trnuid { get; set; }
        public String Curdef  { get; set; }
        public int Bankid { get; set; }
        public String Acctid { get; set; }
        public String Accttype { get; set; }
        public DateTime Dtstart { get; set; }
        public DateTime Dtend { get; set; }
        public IList<DetalhesTransacaoViewModel> DetalhesTransacao { get; set; }

        public void AdicionarTransacao(DetalhesTransacaoViewModel transacoes)
        {
            if (this.DetalhesTransacao == null)
            {
                this.DetalhesTransacao = new List<DetalhesTransacaoViewModel>();
            }
            this.DetalhesTransacao.Add(transacoes);

        }
    }
}
