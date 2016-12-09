using System.Net;
using CasqueLib.Buisness;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace CasqueLib.Services
{
  /// <summary>
  /// Surcharge service de ServiceStack
  /// </summary>
  public class FsService : Service
  {
    /// <summary>
    /// Utilisateur appellant le service
    /// </summary>
    public Utilisateur Util { get; set; }

    /// <summary>
    /// Vérifie que l'apiKey existe bien et est valide
    /// </summary>
    /// <param name="request">Les infos du request</param>
    /// <returns>HTTPError si pas bon, null sinon</returns>
    public HttpError Verification(RequestBase request)
    {
      return FsService.DoVerification(request.ApiKey);
    }

    /// <summary>
    /// Vérifie que l'apiKey existe bien et est valide
    /// </summary>
    /// <param name="apiKey">Clé api de l'utilsiateur</param>
    /// <returns>HTTPError si pas bon, null sinon</returns>
    protected static HttpError DoVerification(string apiKey)
    {
      if (string.IsNullOrWhiteSpace(apiKey))
      {
        return new HttpError(HttpStatusCode.Forbidden, "Pas d'identifiant");
      }
#if DEBUG
      return null;
#else
      Utilisateur u = Utilisateur.IsAuthentified(apiKey);
      if (u != null)
      {
        return null;
      }
      else
      {
        return new HttpError(HttpStatusCode.Forbidden, "identifiant non valide");
      }
#endif
    }
  }
}
