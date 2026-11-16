using N2_2BIM_LP1.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace N2_2BIM_LP1.DAO
{
    public class ConquistaDAO : PadraoDAO<ConquistaViewModel>
    {
        protected override SqlParameter[] CriaParametros(ConquistaViewModel model)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("JogoId", model.JogoId),
                new SqlParameter("Nome", model.Nome),
                new SqlParameter("Descricao", model.Descricao)
            };

            return param;
        }

        protected override ConquistaViewModel MontaModel(DataRow registro)
        {
            ConquistaViewModel c = new ConquistaViewModel()
            {
                Id = Convert.ToInt32(registro["Id"]),
                JogoId = Convert.ToInt32(registro["JogoId"]),
                Nome = registro["Nome"].ToString(),
                Descricao = registro["Descricao"].ToString()
            };

            return c;
        }

        protected override void SetTabela()
        {
            Tabela = "Conquista";
        }
    }
}
