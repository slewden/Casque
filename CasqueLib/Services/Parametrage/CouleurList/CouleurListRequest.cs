using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CouleurList
{
  /// <summary>
  /// Classe requête pour la liste des couleurs
  /// </summary>
  [Api("Casque")]
  [Route("/couleurList/{ApiKey}", Verbs = "GET")]
  public class CouleurListRequest : RequestPagineBase, IReturn<CouleurListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
