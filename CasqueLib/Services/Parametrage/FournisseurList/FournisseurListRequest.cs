using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.FournisseurList
{
  /// <summary>
  /// Classe requête pour la liste des fournisseurs
  /// </summary>
  [Api("Casque")]
  [Route("/fournisseurList/{ApiKey}", Verbs = "GET")]
  public class FournisseurListRequest : RequestPagineBase, IReturn<FournisseurListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
