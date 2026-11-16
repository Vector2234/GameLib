using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2_2BIM_LP1.DAO;
using N2_2BIM_LP1.Models;
using System.IO;

namespace N2_2BIM_LP1.Controllers
{
    public class UsuarioController : PadraoController<UsuarioViewModel>
    {
        public UsuarioController() 
        {
            DAO = new UsuarioDAO();
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

        protected override void ValidaDados(UsuarioViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);
            if (string.IsNullOrEmpty(model.NomeUsuario))
                ModelState.AddModelError("NomeUsuario", "Insira um nome de usuário.");

            //NECESSÁRIO ADICIONAR A VERIFICAÇÃO PARA NOMES E EMAILS IGUAIS

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Insira o email.");

            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("Senha", "Insira a senha.");

            if (model.FotoPerfil == null && operacao == "I")
                ModelState.AddModelError("FotoPerfil", "Escolha uma foto de perfil.");

            if (model.FotoPerfil != null && model.FotoPerfil.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("FotoPerfil", "Imagem limitada a 2 mb.");

            if (ModelState.IsValid)
            {
                if (operacao == "A" && model.FotoPerfil == null)
                {
                    UsuarioViewModel usu = DAO.Consulta(model.Id);
                    model.FotoPerfilEmByte = usu.FotoPerfilEmByte;
                }
                else
                {
                    model.FotoPerfilEmByte = ConvertImageToByte(model.FotoPerfil);
                }
            }
        }
    }
}
