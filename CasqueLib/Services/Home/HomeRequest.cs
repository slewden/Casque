using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Home
{
  /// <summary>
  /// Classe requête pour la liste des utilisateurs
  /// </summary>
  [Api("Casque")]
  [Route("/homeInfos/{ApiKey}", Verbs = "GET")]
  public class HomeRequest : RequestBase, IReturn<HomeResponse>
  {
    /// <summary>
    /// Le code de la page demandée
    /// </summary>
    public string PageCode { get; set; }
  }
}
