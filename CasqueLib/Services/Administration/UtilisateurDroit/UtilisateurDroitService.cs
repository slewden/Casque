using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Joins;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Administration.UtilisateurDroit
{
  /// <summary>
  /// Service pour la gestion des droits d'un utilisateur
  /// </summary>
  public class UtilisateurDroitService : FsService
  {
    /// <summary>
    /// Get : Renvoie les droits de l'utilisateur
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(UtilisateurDroitRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'Clé' Non valide");
      }

      UtilisateurDroitResponse rep = new UtilisateurDroitResponse();
      rep.Utilisateur = this.Db.Select<Utilisateur>(x => x.Cle == request.Cle).FirstOrDefault();
      rep.Droits = this.Db.SqlList<PageDroitView>("[dbo].[page_droit_liste] @utilId", new { utilId = request.Cle });
      return rep;
    }

    /// <summary>
    /// Post : Synchronisation des droits d'un utilisateur
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(UtilisateurDroitRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'clé' non valide");
      }

      string sql = PageDroitView.GetSqlSynchronise(request.Cle, request.Droits);
      if (!string.IsNullOrWhiteSpace(sql))
      {
        this.Db.ExecuteNonQuery(sql);
      }

      return null;
    }
  }
}
