using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.TypePieceEdit
{
  /// <summary>
  /// Requete pour l'upload de photos
  /// </summary>
  [Api("NoUtopia")]
  [Route("/typePiecePhoto/{ApiKey}/", Verbs = "POST")]
  public class TypePieceEditPhotoRequest : RequestBase, IReturn<TypePieceEditPhotoResponse>
  {
  }
}
