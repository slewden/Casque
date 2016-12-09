using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Login
{
  /// <summary>
  /// classe requete pour la page Admin login
  /// </summary>
  [Api("Casque")]
  [Route("/login", Verbs = "POST,DELETE")]
  public class LoginRequest : IReturn<LoginResponse>
  {
    /// <summary>
    /// Le Login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Le mot de passe
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// La clé d'authentification (pour le DELETE)
    /// </summary>
    public string ApiKey { get; set; }
  }
}
