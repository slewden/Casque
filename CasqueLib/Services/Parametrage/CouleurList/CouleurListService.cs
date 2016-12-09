using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Buisness;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.CouleurList
{
  /// <summary>
  /// Service pour la liste des couleurs
  /// </summary>
  public class CouleurListService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des tailles demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CouleurListRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      request.CheckPagination();
      CouleurListResponse rep = new CouleurListResponse();
      List<Couleur> res = new List<Couleur>();
      List<BaseListCompteur> nombres = new List<BaseListCompteur>();
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.couleur_liste @page, @pageSize, @tri, @search";
        cmd.Parameters.Add(new SqlParameter("page", request.Page));
        cmd.Parameters.Add(new SqlParameter("pageSize", request.PageSize));
        cmd.Parameters.Add(new SqlParameter("tri", request.Tri ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("search", request.SearchText ?? string.Empty));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<Couleur>();               // Table 1 : Les couleurs
          reader.NextResult();
          nombres = reader.CustomConvertToList<BaseListCompteur>();  // Table 2 : le Total
          reader.NextResult();
        }
      });

      rep.Couleurs = res;
      rep.Fill(nombres.FirstOrDefault(), request.PageSize);
      return rep;
    }
  }
}
