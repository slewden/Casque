using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Livraison.List
{
  /// <summary>
  /// Classe requête pour la liste des livraisons
  /// </summary>
  [Api("Casque")]
  [Route("/livraisonList/{ApiKey}", Verbs = "GET")]
  public class LivraisonListRequest : RequestPagineBase, IReturn<LivraisonListResponse>
  {
    /// <summary>
    /// Statut de la livraison 
    /// 0 All, 1 Sans client, 2 avec client
    /// </summary>
    public int Statut { get; set; }
  }
}
