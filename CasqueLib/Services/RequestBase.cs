namespace CasqueLib.Services
{
  /// <summary>
  /// Classe de base pour les requetes authentifées
  /// </summary>
  public abstract class RequestBase
  {
    /// <summary>
    /// Clé d'autentification
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Indique que la demande est issu d'un mode readonly
    /// </summary>
    public bool ModeRead { get; set; }
  }
}
