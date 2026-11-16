using Microsoft.AspNetCore.Mvc;
using N2_2BIM_LP1.DAO;
using N2_2BIM_LP1.Models;

namespace N2_2BIM_LP1.Controllers
{
    public class GeneroController : PadraoController<GeneroViewModel>
    {
        public GeneroController() 
        {
            DAO = new GeneroDAO();
            GeraProximoId = false;
        }

        protected override void ValidaDados(GeneroViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Insira o nome.");
        }
    }
}
