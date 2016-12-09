using System.Net;
using CasqueLib.Buisness;
using ServiceStack.Common.Web;

namespace CasqueLib.Services.Login
{
  /// <summary>
  /// Classe pour la gestion des login / logOut
  /// </summary>
  public class LoginService : FsService
  {
    /// <summary>
    /// Méthode POST : Demande de login
    /// </summary>
    /// <param name="request">La demande</param>
    /// <returns>La réponse</returns>
    public object Post(LoginRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
      {
        return new HttpError(HttpStatusCode.BadRequest, "Identifiants invalides");
      }

      Utilisateur util = Utilisateur.Get(this.Db, request.Login, request.Password);
      if (util == null)
      {
        return new HttpError(HttpStatusCode.BadRequest, "Identifiants invalides");
      }

      Utilisateur u = Utilisateur.Add(util);
      return new LoginResponse()
      {
        ApiKey = u.ApiKey.ToString(),
        Nom = util.Nom,
        Cle = util.Cle,
        Menus = PageForUser.Load(this.Db, util.Cle)
      };
    }

    /// <summary>
    /// Méthode DELETE : Log off
    /// </summary>
    /// <param name="request">La demande</param>
    /// <returns>La réponse</returns>
    public object Delete(LoginRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.ApiKey))
      {
        return new HttpError(HttpStatusCode.BadRequest, "ApiKey invalide");
      }

      var u = Utilisateur.IsAuthentified(request.ApiKey);
      if (u != null)
      {
        Utilisateur.Remove(u);
      }

      return null;
    }
  }
}
