using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Administration.UtilisateurList
{
  /// <summary>
  /// Classe requête pour la liste des utilisateurs
  /// </summary>
  [Api("Casque")]
  [Route("/utilisateurList/{ApiKey}", Verbs = "GET")]
  public class UtilisateurListRequest : RequestPagineBase, IReturn<UtilisateurListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
