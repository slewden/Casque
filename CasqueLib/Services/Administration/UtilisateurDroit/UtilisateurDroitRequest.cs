using System.Collections.Generic;
using CasqueLib.Buisness.Joins;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Administration.UtilisateurDroit
{
  /// <summary>
  /// Classe requête pour la gestion des droits d'un utilisateur
  /// </summary>
  [Api("Casque")]
  [Route("/utilisateurDroitGet/{ApiKey}/", Verbs = "GET")]
  [Route("/utilisateurDroitEdit/{ApiKey}/", Verbs = "POST")]
  public class UtilisateurDroitRequest : RequestBase, IReturn<UtilisateurDroitResponse>
  {
    /// <summary>
    /// Clé de l'utilisateur pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Les droits édités
    /// </summary>
    public List<PageDroitView> Droits { get; set; }
  }
}
