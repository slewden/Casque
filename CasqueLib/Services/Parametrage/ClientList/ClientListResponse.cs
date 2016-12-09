using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.ClientList
{
  /// <summary>
  /// Le resultat d'une recherche de clients
  /// </summary>
  public class ClientListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des cartons
    /// </summary>
    public List<ClientView> Clients { get; set; }
  }
}
