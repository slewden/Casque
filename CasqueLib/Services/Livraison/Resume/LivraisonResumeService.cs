using System.Linq;
using CasqueLib.Buisness.View;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Livraison.Resume
{
  /// <summary>
  /// Service pour la liste résumée par client des livraisons
  /// </summary>
  public class LivraisonResumeService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des livraisons demandées
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(LivraisonResumeRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      LivraisonResumeResponse rep = new LivraisonResumeResponse();
      if (request.Mode == 1)
      {
        rep.Livraisons = this.Db.Select<ClientLivraisonView>().OrderBy(x => x.DerniereLivraison).Take(10).ToList();
      }
      else
      {
        rep.Livraisons = this.Db.Select<ClientLivraisonView>().OrderByDescending(x => x.Nombre).Take(10).ToList();
      }

      return rep;
    }
  }
}
