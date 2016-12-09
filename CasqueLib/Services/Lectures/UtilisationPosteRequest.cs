using System.Collections.Generic;
using CasqueLib.Buisness;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe requête pour la gestiondes utilisation de poste
  /// (Démarrage de session de lectures, arrêts et liste des lecteurs possibles)
  /// </summary>
  [Api("Casque")]
  [Route("/lecteursGet/{ApiKey}", Verbs = "GET")]
  [Route("/utilisationPosteStart/{ApiKey}", Verbs = "PUT")]
  ////[Route("/utilisationPosteClose/{ApiKey}", Verbs = "POST")]
  [Route("/utilisationPosteDelete/{ApiKey}", Verbs = "DELETE")]
  public class UtilisationPosteRequest : RequestBase, IReturn<UtilisationPosteResponse>
  {
    /// <summary>
    /// Le code de la page qui fait la demande
    /// (Reception, Assemblage, ... Voir classe CasqueLib.Buisness.Poste.GetNomAffectation)
    /// </summary>
    public string PageCode { get; set; }

    /// <summary>
    /// La clé du poste choisi
    /// </summary>
    public int PosteCle { get; set; }

    /// <summary>
    /// La clé de l'utilisateur
    /// </summary>
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// Clé de l'utilisation poste à supprimer ou valider
    /// </summary>
    public int UtilisationPosteCle { get; set; }

    /// <summary>
    /// Les lectures enregistrées
    /// </summary>
    public List<Lecture> Lectures { get; set; }
  }
}
