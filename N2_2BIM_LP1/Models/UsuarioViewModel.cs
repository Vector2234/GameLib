using Microsoft.AspNetCore.Http;
using System;

namespace N2_2BIM_LP1.Models
{
    public class UsuarioViewModel : PadraoViewModel
    {
        public string NomeUsuario { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public IFormFile FotoPerfil { get; set; }

        public byte[] FotoPerfilEmByte { get; set; }

        public string FotoPerfilEmBase64
        {
            get
            {
                if (FotoPerfilEmByte != null)
                    return Convert.ToBase64String(FotoPerfilEmByte);
                else
                    return string.Empty;
            }
        }

    }
}
