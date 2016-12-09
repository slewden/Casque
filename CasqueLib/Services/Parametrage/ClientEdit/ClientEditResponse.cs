using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.ClientEdit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'un client
  /// </summary>
  public class ClientEditResponse
  {
    /// <summary>
    /// Le client manipulé
    /// </summary>
    public ClientView Client { get; set; }
  }
}
