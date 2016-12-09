using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CartonList
{
  /// <summary>
  /// Classe requête pour la liste des cartons
  /// </summary>
  [Api("Casque")]
  [Route("/cartonList/{ApiKey}", Verbs = "GET")]
  public class CartonListRequest : RequestPagineBase, IReturn<CartonListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
