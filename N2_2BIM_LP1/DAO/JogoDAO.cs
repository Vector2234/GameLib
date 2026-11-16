using N2_2BIM_LP1.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace N2_2BIM_LP1.DAO
{
    public class JogoDAO : PadraoDAO<JogoViewModel>
    {
        protected override SqlParameter[] CriaParametros(JogoViewModel model)
        {
            object imgByte = model.CapaEmByte;
            if (imgByte == null)
                imgByte = DBNull.Value;

            SqlParameter[] param = new SqlParameter[] 
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("Nome", model.Nome),
                new SqlParameter("Descricao", model.Descricao),
                new SqlParameter("DataLancamento", model.DataLancamento),
                new SqlParameter("Capa", imgByte)
            };

            return param;
        }

        protected override JogoViewModel MontaModel(DataRow registro)
        {
            JogoViewModel j = new JogoViewModel() 
            {
                Id = Convert.ToInt32(registro["Id"]),
                Nome = registro["Nome"].ToString(),
                Descricao = registro["Descricao"].ToString(),
                DataLancamento = Convert.ToDateTime(registro["DataLancamento"])
            };

            if (registro["Capa"] != DBNull.Value)
                j.CapaEmByte = registro["Capa"] as byte[];

            return j;
        }

        protected override void SetTabela()
        {
            Tabela = "Jogo";
        }
    }
}
