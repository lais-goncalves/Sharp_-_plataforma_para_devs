using Newtonsoft.Json;
using Sharp.Models.Portfolios.Projetos.TiposDeProjetos;

namespace Sharp.Models.Portfolios.Projetos
{
    public class FabricaDeProjetos
    {
        #region Métodos
        private T? converterProjetoPara<T>(dynamic projeto)
        {
            string strProjeto = JsonConvert.SerializeObject(projeto);
            return JsonConvert.DeserializeObject<T>(strProjeto);
        }

        public BaseProjeto? CriarProjeto(dynamic p)
        {
            try
            {
                string? tipoProjeto = p["tipo"];
                if (tipoProjeto == "github")
                {
                    return converterProjetoPara<ProjetoGitHub>(p);
                }
                else
                {
                    return converterProjetoPara<Projeto>(p);
                }
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
        #endregion Métodos
    }
}
