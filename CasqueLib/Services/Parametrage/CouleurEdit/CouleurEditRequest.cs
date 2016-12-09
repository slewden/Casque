using CasqueLib.Buisness;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CouleurEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'une couleur
  /// </summary>
  [Api("Casque")]
  [Route("/couleurGet/{ApiKey}/", Verbs = "GET")]
  [Route("/couleurEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/couleurDelete/{ApiKey}/", Verbs = "DELETE")]
  public class CouleurEditRequest : RequestBase, IReturn<CouleurEditResponse>
  {
    /// <summary>
    /// Clé de la couleur pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// La couleur édité
    /// </summary>
    public Couleur Couleur { get; set; }
  }
}
