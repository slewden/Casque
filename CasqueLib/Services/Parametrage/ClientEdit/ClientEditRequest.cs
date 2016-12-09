using CasqueLib.Buisness.View;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.ClientEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'un client
  /// </summary>
  [Api("Casque")]
  [Route("/clientGet/{ApiKey}/", Verbs = "GET")]
  [Route("/clientEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/clientDelete/{ApiKey}/", Verbs = "DELETE")]
  public class ClientEditRequest : RequestBase, IReturn<ClientEditResponse>
  {
    /// <summary>
    /// Clé du client pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Le client édité
    /// </summary>
    public ClientView Client { get; set; }
  }
}
