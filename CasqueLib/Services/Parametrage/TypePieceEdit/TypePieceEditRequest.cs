using CasqueLib.Buisness.View;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.TypePieceEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'un type de pièce
  /// </summary>
  [Api("Casque")]
  [Route("/typePieceGet/{ApiKey}/", Verbs = "GET")]
  [Route("/typePieceEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/typePieceDelete/{ApiKey}/", Verbs = "DELETE")]
  public class TypePieceEditRequest : RequestBase, IReturn<TypePieceEditResponse>
  {
    /// <summary>
    /// Clé du type de pièce pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Le type de pièce édité
    /// </summary>
    public TypePieceView TypePiece { get; set; }
  }
}
