using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe requête pour la suppression d'un assemblage en cours de constitution
  /// </summary>
  [Api("Casque")]
  [Route("/assemblageDelete/{ApiKey}", Verbs = "DELETE")]
  [Route("/livraisonDelete/{ApiKey}", Verbs = "DELETE")]
  public class AssemblageDeleteRequest : RequestBase, IReturn<AssemblageDeleteResponse>
  {
    /// <summary>
    /// Le code de la page qui fait la demande
    /// (Reception, Assemblage, ... Voir classe CasqueLib.Buisness.Poste.GetNomAffectation)
    /// </summary>
    public string PageCode { get; set; }

    /// <summary>
    /// Clé de l'assemblage à supprimer
    /// </summary>
    public int Cle { get; set; }
  }
}
