using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Commande.List
{
  /// <summary>
  /// Service pour la liste des commandes
  /// </summary>
  public class CommandeListService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des commandes demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CommandeListRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      request.CheckPagination();
      CommandeListResponse rep = new CommandeListResponse();
      List<CommandeView> res = new List<CommandeView>();
      List<BaseListCompteur> nombres = new List<BaseListCompteur>();
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.commande_liste @page, @pageSize, @tri, @search, @statut";
        cmd.Parameters.Add(new SqlParameter("page", request.Page));
        cmd.Parameters.Add(new SqlParameter("pageSize", request.PageSize));
        cmd.Parameters.Add(new SqlParameter("tri", request.Tri ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("search", request.SearchText ?? string.Empty));
        cmd.Parameters.Add(new SqlParameter("statut", request.Statut));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<CommandeView>();  // Table 1 : Les commandes
          reader.NextResult();
          nombres = reader.CustomConvertToList<BaseListCompteur>();  // Table 2 : le Total
          reader.NextResult();
        }
      });

      rep.Commandes = res;
      rep.Fill(nombres.FirstOrDefault(), request.PageSize);
      return rep;
    }
  }
}
