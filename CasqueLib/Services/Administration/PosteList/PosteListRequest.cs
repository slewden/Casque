using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Administration.PosteList
{
  /// <summary>
  /// Classe requête pour la liste des postes
  /// </summary>
  [Api("Casque")]
  [Route("/posteList/{ApiKey}", Verbs = "GET")]
  public class PosteListRequest : RequestPagineBase, IReturn<PosteListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
