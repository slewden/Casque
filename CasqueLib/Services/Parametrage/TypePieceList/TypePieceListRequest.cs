using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.TypePieceList
{
  /// <summary>
  /// Classe requête pour la liste des types de pièces
  /// </summary>
  [Api("Casque")]
  [Route("/typePieceList/{ApiKey}", Verbs = "GET")]
  public class TypePieceListRequest : RequestPagineBase, IReturn<TypePieceListResponse>
  {
    // Rien de spécifique pour l'instant
  }
}
