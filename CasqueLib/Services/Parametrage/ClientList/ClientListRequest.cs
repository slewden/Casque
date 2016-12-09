using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.ClientList
{
  /// <summary>
  /// Classe requête pour la liste des cartons
  /// </summary>
  [Api("Casque")]
  [Route("/clientList/{ApiKey}", Verbs = "GET")]
  public class ClientListRequest : RequestPagineBase, IReturn<ClientListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
