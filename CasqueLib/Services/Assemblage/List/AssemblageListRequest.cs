using CasqueLib.Common;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Assemblage.List
{
  /// <summary>
  /// Classe requête pour la liste des assemblages
  /// </summary>
  [Api("Casque")]
  [Route("/assemblageList/{ApiKey}", Verbs = "GET")]
  public class AssemblageListRequest : RequestPagineBase, IReturn<AssemblageListResponse>
  {
    /// <summary>
    /// Type de statut de l'assemblage voir CasqueLib.Buisness.View.AssemblageView.Statut pour les valeurs
    /// </summary>
    public int Statut { get; set; }

    /// <summary>
    /// Clé du casque à filtrer
    /// </summary>
    public int CasqueCle { get; set; }
  }
}
