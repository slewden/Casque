using System.Linq;
using ServiceStack.Common.Web;

namespace CasqueLib.Services.Administration.Configuration
{
  /// <summary>
  /// Service pour la gestion d'une configuration
  /// </summary>
  public class ConfigurationService : FsService
  {
    /// <summary>
    /// Get : Renvoie les configurations (les manquantes sont crée et stockée en BDD avant le retour de la réponse !)
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(ConfigurationRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      ConfigurationResponse rep = new ConfigurationResponse();
      rep.Configurations = CasqueLib.Buisness.Configuration.GetAll(this.Db);
      return rep;
    }

    /// <summary>
    /// Post : Modification des configs reçues s'il y a lieu
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(ConfigurationRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Configurations != null && request.Configurations.Any())
      {
        foreach (CasqueLib.Buisness.Configuration cfg in request.Configurations)
        {
          cfg.Save(this.Db);
        }
      }

      return null;
    }
  }
}
