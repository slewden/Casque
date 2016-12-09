using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.TypePieceList
{
  /// <summary>
  /// Le resultat d'une recherche de types de pièces
  /// </summary>
  public class TypePieceListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des types de pièces
    /// </summary>
    public List<TypePieceView> TypePieces { get; set; }
  }
}
