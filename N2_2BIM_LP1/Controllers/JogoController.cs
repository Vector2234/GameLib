using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2_2BIM_LP1.DAO;
using N2_2BIM_LP1.Models;
using System;
using System.IO;

namespace N2_2BIM_LP1.Controllers
{
    public class JogoController : PadraoController<JogoViewModel>
    {
        public JogoController() 
        {
            DAO = new JogoDAO();
            GeraProximoId = false;
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        protected override void ValidaDados(JogoViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Insira o título do jogo.");

            if (string.IsNullOrEmpty(model.Descricao))
                ModelState.AddModelError("Nome", "Insira a descrição.");

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Insira o título do jogo.");

            if (model.DataLancamento > DateTime.Now)
                ModelState.AddModelError("DataLancamento", "Data inválida.");

            if (model.Capa == null && operacao == "I")
                ModelState.AddModelError("Capa", "Escolha uma imagem para a capa.");
            if (model.Capa != null && model.Capa.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Capa", "Imagem limitada a 2 mb.");
            if (ModelState.IsValid)
            {
                if (operacao == "A" && model.Capa == null)
                {
                    JogoViewModel jog = DAO.Consulta(model.Id);
                    model.CapaEmByte = jog.CapaEmByte;
                }
                else
                {
                    model.CapaEmByte = ConvertImageToByte(model.Capa);
                }
            }

        }
    }
}
