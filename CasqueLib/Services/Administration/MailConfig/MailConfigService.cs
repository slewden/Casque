using System.Net;
using ServiceStack.Common.Web;

namespace CasqueLib.Services.Administration.MailConfig
{
  /// <summary>
  /// Service pour la gestion de la config email
  /// </summary>
  public class MailConfigService : FsService
  {
    /// <summary>
    /// Get : Renvoie la config demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(MailConfigRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      MailConfigResponse rep = new MailConfigResponse();
      rep.Config = CasqueLib.Buisness.MailConfig.Get(this.Db);
      if (rep.Config == null)
      {
        rep.Config = new Buisness.MailConfig();
      }

      return rep;
    }

    /// <summary>
    /// Post : Création ou Modification de la config email
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(MailConfigRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Config == null || !request.Config.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'configuration emails' non valide ou incomplète");
      }

      request.Config.Save(this.Db);
      return null;
    }
  }
}
