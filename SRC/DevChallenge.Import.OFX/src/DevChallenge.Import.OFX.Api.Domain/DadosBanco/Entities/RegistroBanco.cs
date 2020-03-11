using System;
using System.Collections.Generic;

namespace DevChallenge.Import.OFX.Api.Domain.DadosBanco.Entities
{
    public class RegistroBanco
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
        public List<DetalhesTransacao> DetalhesTransacao { get; set; }

        public void AdicionarTransacao(DetalhesTransacao transacoes)
        {
            if (this.DetalhesTransacao == null)
            {
                this.DetalhesTransacao = new List<DetalhesTransacao>();
            }
            this.DetalhesTransacao.Add(transacoes);

        }
    }
}
