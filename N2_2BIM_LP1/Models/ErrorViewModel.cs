using System;

namespace N2_2BIM_LP1.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModel(string erro)
        {
            this.Erro = erro;
        }

        public ErrorViewModel()
        {
        }

        public string Erro { get; set; }
    }
}
