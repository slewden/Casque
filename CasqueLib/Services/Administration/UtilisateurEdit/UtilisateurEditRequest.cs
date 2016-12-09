using CasqueLib.Buisness;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Administration.UtilisateurEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'un utilisateur
  /// </summary>
  [Api("Casque")]
  [Route("/utilisateurGet/{ApiKey}/", Verbs = "GET")]
  [Route("/utilisateurEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/utilisateurDelete/{ApiKey}/", Verbs = "DELETE")]
  public class UtilisateurEditRequest : RequestBase, IReturn<UtilisateurEditResponse>
  {
    /// <summary>
    /// Clé de l'utilisateur pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// L'utilisateur édité
    /// </summary>
    public Utilisateur Utilisateur { get; set; }
  }
}
