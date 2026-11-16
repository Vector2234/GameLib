using Microsoft.AspNetCore.Mvc;
using N2_2BIM_LP1.DAO;
using N2_2BIM_LP1.Models;

namespace N2_2BIM_LP1.Controllers
{
    public class ConquistaController : PadraoController<ConquistaViewModel>
    {
        public ConquistaController()
        {
            DAO = new ConquistaDAO();
            GeraProximoId = false;
        }

        protected override void ValidaDados(ConquistaViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Insira o título do jogo.");

            if (string.IsNullOrEmpty(model.Descricao))
                ModelState.AddModelError("Nome", "Insira a descrição.");
        }
    }
}
