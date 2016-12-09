using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Assemblage.List
{
  /// <summary>
  /// Service pour la liste des assemblages
  /// </summary>
  public class AssemblageListService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des assemblages demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(AssemblageListRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      request.CheckPagination();
      AssemblageListResponse rep = new AssemblageListResponse();
      List<BaseListCompteur> nombres = new List<BaseListCompteur>();
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.assemblage_liste @page, @pageSize, @tri, @search, @statut, @casqId";
        cmd.Parameters.Add(new SqlParameter("page", request.Page));
        cmd.Parameters.Add(new SqlParameter("pageSize", request.PageSize));
        cmd.Parameters.Add(new SqlParameter("tri", request.Tri ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("search", request.SearchText ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("statut", request.Statut));
        cmd.Parameters.Add(new SqlParameter("casqId", request.CasqueCle));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          rep.Assemblages = reader.CustomConvertToList<AssemblageView>();  // Table 1 : Les assemblages
          reader.NextResult();
          nombres = reader.CustomConvertToList<BaseListCompteur>();        // Table 2 : le Total
          reader.NextResult();
          rep.Casques = reader.CustomConvertToList<NomCle>();              // Table 3 : Les casque possible pour filtrage
        }
      });

      rep.Fill(nombres.FirstOrDefault(), request.PageSize);
      return rep;
    }
  }
}
