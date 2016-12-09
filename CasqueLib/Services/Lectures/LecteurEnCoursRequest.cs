using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe requete pour lister les lecteurs en cours d'utilisation et supprimer les utilisations poste non désirées
  /// </summary>
  [Api("Casque")]
  [Route("/lecteursEnCours/{ApiKey}", Verbs = "GET")]
  public class LecteurEnCoursRequest : RequestBase, IReturn<LecteurEnCoursResponse>
  {
    /// <summary>
    /// La page qui fait la demande
    /// </summary>
    public string PageCode { get; set; }

    /// <summary>
    /// La clé de l'utilisation poste à deleter
    /// </summary>
    public int UtilisationPosteCle { get; set; }
  }
}
