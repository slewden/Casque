using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.FournisseurList
{
  /// <summary>
  /// Service pour la liste des fournisseurs
  /// </summary>
  public class FournisseurListService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des fournisseurs demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(FournisseurListRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      request.CheckPagination();
      FournisseurListResponse rep = new FournisseurListResponse();
      List<FournisseurView> res = new List<FournisseurView>();
      List<BaseListCompteur> nombres = new List<BaseListCompteur>();
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.fournisseur_liste @page, @pageSize, @tri, @search";
        cmd.Parameters.Add(new SqlParameter("page", request.Page));
        cmd.Parameters.Add(new SqlParameter("pageSize", request.PageSize));
        cmd.Parameters.Add(new SqlParameter("tri", request.Tri ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("search", request.SearchText ?? string.Empty));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<FournisseurView>();       // Table 1 : Les fournisseurs
          reader.NextResult();
          nombres = reader.CustomConvertToList<BaseListCompteur>();  // Table 2 : le Total
          reader.NextResult();
        }
      });

      rep.Fournisseurs = res;
      rep.Fill(nombres.FirstOrDefault(), request.PageSize);
      return rep;
    }
  }
}
