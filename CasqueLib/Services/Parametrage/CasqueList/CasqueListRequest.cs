using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CasqueList
{
  /// <summary>
  /// Classe requête pour la liste des casques
  /// </summary>
  [Api("Casque")]
  [Route("/casqueList/{ApiKey}", Verbs = "GET")]
  public class CasqueListRequest : RequestPagineBase, IReturn<CasqueListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
