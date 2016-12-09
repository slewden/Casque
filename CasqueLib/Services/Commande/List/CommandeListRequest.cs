using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Commande.List
{
  /// <summary>
  /// Classe requête pour la liste des commandes
  /// </summary>
  [Api("Casque")]
  [Route("/comamndeList/{ApiKey}", Verbs = "GET")]
  public class CommandeListRequest : RequestPagineBase, IReturn<CommandeListResponse>
  {
    /// <summary>
    /// Type de statut de commande voir CommandeResumeData.EStatutCommande pour les valeurs
    /// </summary>
    public int Statut { get; set; }
  }
}
