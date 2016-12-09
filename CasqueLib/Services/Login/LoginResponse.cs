using System.Collections.Generic;

namespace CasqueLib.Services.Login
{
  /// <summary>
  /// Classe de réponse pour les service login
  /// </summary>
  public class LoginResponse
  {
    /// <summary>
    /// La clé d'authentification
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Le nom du user
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// La clé du user
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Les menus autorisés de l'utilisateur
    /// </summary>
    public List<PageForUser> Menus { get; set; }
  }
}
