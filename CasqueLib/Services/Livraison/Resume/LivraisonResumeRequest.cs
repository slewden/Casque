using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Livraison.Resume
{
  /// <summary>
  /// Classe requête pour la liste résumée par client des livraisons
  /// </summary>
  [Api("Casque")]
  [Route("/livraisonResume/{ApiKey}", Verbs = "GET")]
  public class LivraisonResumeRequest : RequestPagineBase, IReturn<LivraisonResumeResponse>
  {
    /// <summary>
    /// Le mode d'affichage
    /// 0 = Top 10 des client avec le plus de commandes
    /// 1 = Les 10 dernières
    /// </summary>
    public int Mode { get; set; }
  }
}
