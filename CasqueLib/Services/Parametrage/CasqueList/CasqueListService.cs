using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.CasqueList
{
  /// <summary>
  /// Service pour la liste des casques
  /// </summary>
  public class CasqueListService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des casques demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CasqueListRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      request.CheckPagination();
      CasqueListResponse rep = new CasqueListResponse();
      List<CasqueView> res = new List<CasqueView>();
      List<BaseListCompteur> nombres = new List<BaseListCompteur>();
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.casque_liste @page, @pageSize, @tri, @search";
        cmd.Parameters.Add(new SqlParameter("page", request.Page));
        cmd.Parameters.Add(new SqlParameter("pageSize", request.PageSize));
        cmd.Parameters.Add(new SqlParameter("tri", request.Tri ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("search", request.SearchText ?? string.Empty));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<CasqueView>();            // Table 1 : Les casques
          reader.NextResult();
          nombres = reader.CustomConvertToList<BaseListCompteur>();  // Table 2 : le Total
          reader.NextResult();
        }
      });

      rep.Casques = res;
      rep.Fill(nombres.FirstOrDefault(), request.PageSize);
      return rep;
    }
  }
}
