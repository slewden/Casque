using CasqueLib.Buisness.View;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Commande.Edit
{
  /// <summary>
  /// Classe requête pour la gestion d'une commande
  /// </summary>
  [Api("Casque")]
  [Route("/commandeCorrige/{ApiKey}/", Verbs = "GET")]
  public class CommandeCorrigeRequest : RequestBase, IReturn<CommandeEditResponse>
  {
    /// <summary>
    /// Clé de la commande pour les opérations de correction
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// id de l'action de correction
    /// </summary>
    public int Action { get; set; }
  }
}
