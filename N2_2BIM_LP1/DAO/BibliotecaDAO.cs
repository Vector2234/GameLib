using N2_2BIM_LP1.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace N2_2BIM_LP1.DAO
{
    public class BibliotecaDAO : PadraoDAO<BibliotecaViewModel>
    {
        protected override SqlParameter[] CriaParametros(BibliotecaViewModel model)
        {
            SqlParameter[] param = new SqlParameter[] 
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("JogoId", model.JogoId),
                new SqlParameter("Status", model.status.ToString()),
                new SqlParameter("PorcentagemConclusao", model.PorcentagemConclusao),
                new SqlParameter("DataInicio", model.DataInicio),
                new SqlParameter("DataFim", model.DataFim)
            };

            return param;
        }

        protected override BibliotecaViewModel MontaModel(DataRow registro)
        {
            BibliotecaViewModel b = new BibliotecaViewModel()
            {
                Id = Convert.ToInt32(registro["Id"]),
                JogoId = Convert.ToInt32(registro["JogoId"]),
                status = (Status)Enum.Parse(typeof(Status), registro["Status"].ToString()),
                PorcentagemConclusao = Convert.ToDecimal(registro["PorcentagemConclusao"]),
                DataInicio = registro["DataInicio"] != DBNull.Value ? Convert.ToDateTime(registro["DataInicio"]) : (DateTime?)null,
                DataFim = registro["DataFim"] != DBNull.Value ? Convert.ToDateTime(registro["DataFim"]) : (DateTime?)null
            };

            return b;
        }

        protected override void SetTabela()
        {
            Tabela = "Bibiloteca";
        }
    }
}
