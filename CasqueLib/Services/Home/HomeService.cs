using System.Linq;
using CasqueLib.Buisness.Report;
using CasqueLib.Common;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Home
{
  /// <summary>
  /// Service pour la liste des utilisateurs
  /// </summary>
  public class HomeService : FsService
  {
    /// <summary>
    /// Get : Renvoie les données de la page d'accueil
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(HomeRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      HomeResponse response = new HomeResponse();
      switch (request.PageCode.ToLower())
      {
        case "administration":
          response.CompteurMenu = this.Db.Select<CompteurAdministration>().Select(x => (NomCle)x).ToList();
          response.Statistiques = null;
          break;
        case "configuration":
          response.CompteurMenu = this.Db.Select<CompteurConfiguration>().Select(x => (NomCle)x).ToList();
          response.Statistiques = null;
          break;
        default:
          response.CompteurMenu = null;
          response.Statistiques = this.Db.Select<CompteurHome>().Select(x => (CompteurBase)x).ToList();
          break;
      }

      return response;
    }
  }
}
