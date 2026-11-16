using N2_2BIM_LP1.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace N2_2BIM_LP1.DAO
{
    public class GeneroDAO : PadraoDAO<GeneroViewModel>
    {
        protected override SqlParameter[] CriaParametros(GeneroViewModel model)
        {
            SqlParameter[] param = new SqlParameter[] 
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("Nome", model.Nome)
            };

            return param;
        }

        protected override GeneroViewModel MontaModel(DataRow registro)
        {
            GeneroViewModel g = new GeneroViewModel() 
            {
                Id = Convert.ToInt32(registro["Id"]),
                Nome = registro["Nome"].ToString()
            };

            return g;
        }

        protected override void SetTabela()
        {
            Tabela = "Genero";
        }
    }
}
