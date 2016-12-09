using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CasqueConstitue
{
  /// <summary>
  /// Classe requête pour la gestion des constitution de casque
  /// </summary>
  [Api("Casque")]
  [Route("/casqueConstitueInfoGet/{ApiKey}/", Verbs = "GET")]
  public class CasqueConstitueInfoRequest : RequestBase, IReturn<CasqueConstitueInfoResponse>
  {
    /// <summary>
    /// Le type de pièce choisi
    /// </summary>
    public int Cle { get; set; }
  }
}
