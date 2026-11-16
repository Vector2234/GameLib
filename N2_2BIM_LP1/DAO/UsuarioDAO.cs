using N2_2BIM_LP1.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace N2_2BIM_LP1.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override SqlParameter[] CriaParametros(UsuarioViewModel model)
        {
            object imgByte = model.FotoPerfilEmByte;
            if (imgByte == null)
                imgByte = DBNull.Value;

            SqlParameter[] param = new SqlParameter[] 
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("NomeUsuario", model.NomeUsuario),
                new SqlParameter("Email", model.Email),
                new SqlParameter("Senha", model.Senha),
                new SqlParameter("FotoPerfil", imgByte)
            };

            return param;
        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
        {
            UsuarioViewModel u = new UsuarioViewModel() 
            {
                Id = Convert.ToInt32(registro["Id"]),
                NomeUsuario = registro["NomeUsuario"].ToString(),
                Email = registro["Email"].ToString(),
                Senha = registro["Senha"].ToString(),
            };

            if (registro["FotoPerfil"] != DBNull.Value)
                u.FotoPerfilEmByte = registro["FotoPerfil"] as byte[];

            return u;
        }

        protected override void SetTabela()
        {
            Tabela = "Usuario";
        }
    }
}
