using System;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Information sur un client qui se déconnecte du hub
  /// </summary>
  public class HubConnectorEventDeconnexionClient : EventArgs
  {
    /// <summary>
    /// La clé du client
    /// </summary>
    public string ClientId { get; set; }
  }
}
