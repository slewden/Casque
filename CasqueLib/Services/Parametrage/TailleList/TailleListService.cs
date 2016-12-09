using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Buisness;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.TailleList
{
  /// <summary>
  /// Service pour la liste des tailles
  /// </summary>
  public class TailleListService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des tailles demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(TailleListRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      request.CheckPagination();
      TailleListResponse rep = new TailleListResponse();
      List<Taille> res = new List<Taille>();
      List<BaseListCompteur> nombres = new List<BaseListCompteur>();
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.taille_liste @page, @pageSize, @tri, @search";
        cmd.Parameters.Add(new SqlParameter("page", request.Page));
        cmd.Parameters.Add(new SqlParameter("pageSize", request.PageSize));
        cmd.Parameters.Add(new SqlParameter("tri", request.Tri ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("search", request.SearchText ?? string.Empty));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<Taille>();                // Table 1 : Les tailles
          reader.NextResult();
          nombres = reader.CustomConvertToList<BaseListCompteur>();  // Table 2 : le Total
          reader.NextResult();
        }
      });

      rep.Tailles = res;
      rep.Fill(nombres.FirstOrDefault(), request.PageSize);
      return rep;
    }
  }
}
