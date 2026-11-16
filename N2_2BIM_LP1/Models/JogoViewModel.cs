using Microsoft.AspNetCore.Http;
using System;

namespace N2_2BIM_LP1.Models
{
    public class JogoViewModel : PadraoViewModel
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime DataLancamento { get; set; }

        public IFormFile Capa { get; set; }

        public byte[] CapaEmByte { get; set; }

        public string CapaEmBase64
        {
            get
            {
                if (CapaEmByte != null)
                    return Convert.ToBase64String(CapaEmByte);
                else
                    return string.Empty;
            }
        }
    }
}
