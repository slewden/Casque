using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Commande.Resume
{
  /// <summary>
  /// Classe requête pour l'écran résumé des commandes
  /// </summary>
  [Api("Casque")]
  [Route("/commandeResume/{ApiKey}/", Verbs = "GET")]
  [Route("/commandeAcquitte/{ApiKey}/", Verbs = "POST")]
  public class CommandeResumeRequest : RequestBase, IReturn<CommandeResumeResponse>
  {
    /// <summary>
    /// La clé de la commande à acquitter
    /// </summary>
    public int Cle { get; set; }
  }
}
