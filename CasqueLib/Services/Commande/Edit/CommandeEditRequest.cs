using CasqueLib.Buisness.View;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Commande.Edit
{
  /// <summary>
  /// Classe requête pour la gestion d'une commande
  /// </summary>
  [Api("Casque")]
  [Route("/commandeGet/{ApiKey}/", Verbs = "GET")]
  [Route("/commandeInsert/{ApiKey}/", Verbs = "PUT")]
  [Route("/commandeEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/commandeDelete/{ApiKey}/", Verbs = "DELETE")]
  public class CommandeEditRequest : RequestBase, IReturn<CommandeEditResponse>
  {
    /// <summary>
    /// Clé de la commande pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// La commande est validée
    /// </summary>
    public bool Validation { get; set; }

    /// <summary>
    /// L'utilisateur valide la demande d'envoie d'email
    /// </summary>
    public bool EnvoieEmail { get; set; }
    
    /// <summary>
    /// Valide le fait que l'email doit partir ou pas
    /// </summary>
    public bool ProcessEnvoie { get; set; }

    /// <summary>
    /// L'utilisateur aquitte la fin de reception de la commande
    /// </summary>
    public bool Acquittee { get; set; }

    /// <summary>
    /// La commande éditée
    /// </summary>
    public CommandeView Commande { get; set; }

    /// <summary>
    /// Adresse email supplémentaire lors de l'envoie de la commande par email
    /// </summary>
    public string EmailSuplementaire { get; set; }

    /// <summary>
    /// Génère le fichier Excle des pièces ou pas
    /// </summary>
    public bool Excel { get; set; }
  }
}
