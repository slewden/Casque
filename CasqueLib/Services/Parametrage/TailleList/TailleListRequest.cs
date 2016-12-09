using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.TailleList
{
  /// <summary>
  /// Classe requête pour la liste des tailles
  /// </summary>
  [Api("Casque")]
  [Route("/tailleList/{ApiKey}", Verbs = "GET")]
  public class TailleListRequest : RequestPagineBase, IReturn<TailleListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
