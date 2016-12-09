using CasqueLib.Buisness;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.TailleEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'une taille
  /// </summary>
  [Api("Casque")]
  [Route("/tailleGet/{ApiKey}/", Verbs = "GET")]
  [Route("/tailleEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/tailleDelete/{ApiKey}/", Verbs = "DELETE")]
  public class TailleEditRequest : RequestBase, IReturn<TailleEditResponse>
  {
    /// <summary>
    /// Clé de la taille pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// La taille édité
    /// </summary>
    public Taille Taille { get; set; }
  }
}
