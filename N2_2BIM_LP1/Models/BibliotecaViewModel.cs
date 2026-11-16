using System;

namespace N2_2BIM_LP1.Models
{
    public class BibliotecaViewModel : PadraoViewModel
    {
        public int UsuarioId { get; set; }

        public int JogoId { get; set; }

        public Status status { get; set; }

        public decimal PorcentagemConclusao { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
