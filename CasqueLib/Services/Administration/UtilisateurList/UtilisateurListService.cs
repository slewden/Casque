using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Buisness;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Administration.UtilisateurList
{
  /// <summary>
  /// Service pour la liste des utilisateurs
  /// </summary>
  public class UtilisateurListService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des utilisateurs demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(UtilisateurListRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      request.CheckPagination();
      UtilisateurListResponse rep = new UtilisateurListResponse();
      List<Utilisateur> res = new List<Utilisateur>();
      List<BaseListCompteur> nombres = new List<BaseListCompteur>();
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.utilisateur_liste @page, @pageSize, @tri, @search";
        cmd.Parameters.Add(new SqlParameter("page", request.Page));
        cmd.Parameters.Add(new SqlParameter("pageSize", request.PageSize));
        cmd.Parameters.Add(new SqlParameter("tri", request.Tri ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("search", request.SearchText ?? string.Empty));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<Utilisateur>();           // Table 1 : Les utilisateurs
          reader.NextResult();
          nombres = reader.CustomConvertToList<BaseListCompteur>();  // Table 2 : le Total
          reader.NextResult();
        }
      });

      rep.Utilisateurs = res;
      rep.Fill(nombres.FirstOrDefault(), request.PageSize);
      return rep;
    }
  }
}
