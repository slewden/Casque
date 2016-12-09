using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Assemblage.Detail
{
  /// <summary>
  /// Classe requête pour le détail d'un assemblage
  /// </summary>
  [Api("Casque")]
  [Route("/assemblageDetail/{ApiKey}", Verbs = "GET")]
  public class AssemblageDetailRequest : RequestBase, IReturn<AssemblageDetailResponse>
  {
    /// <summary>
    /// Clé de l'assemblage à détailler
    /// </summary>
    public int Cle { get; set; }
  }
}
