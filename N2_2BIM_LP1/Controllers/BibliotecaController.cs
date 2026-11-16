using Microsoft.AspNetCore.Mvc;
using N2_2BIM_LP1.DAO;
using N2_2BIM_LP1.Models;

namespace N2_2BIM_LP1.Controllers
{
    public class BibliotecaController : PadraoController<BibliotecaViewModel>
    {
        public BibliotecaController()
        {
            DAO = new BibliotecaDAO();
            GeraProximoId = false;
        }

        protected override void ValidaDados(BibliotecaViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            //NECESSÁRIO DEFINIR COMO VAI FUNCIONAR ANTES DE FAZER AS VALIDAÇÕES
        }
    }
}
