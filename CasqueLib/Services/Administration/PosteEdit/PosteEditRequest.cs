using CasqueLib.Buisness;
using CasqueLib.Matos.Lecteur;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Administration.PosteEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'un Poste
  /// </summary>
  [Api("Casque")]
  [Route("/posteGet/{ApiKey}/", Verbs = "GET")]
  [Route("/posteEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/posteDelete/{ApiKey}/", Verbs = "DELETE")]
  public class PosteEditRequest : RequestBase, IReturn<PosteEditResponse>
  {
    /// <summary>
    /// Clé du poste pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Le poste édité
    /// </summary>
    public Poste Poste { get; set; }

    /// <summary>
    /// Config du lecteur
    /// </summary>
    public SimpleReaderParameters Config { get; set; }
  }
}
