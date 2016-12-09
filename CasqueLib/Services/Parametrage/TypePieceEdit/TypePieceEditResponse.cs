using System.Collections.Generic;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.TypePieceEdit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'un type de pièce
  /// </summary>
  public class TypePieceEditResponse
  {
    /// <summary>
    /// Le type de pièce manipulé
    /// </summary>
    public TypePieceView TypePiece { get; set; }
    
    /// <summary>
    /// La liste des tailles possibles
    /// </summary>
    public List<Taille> Tailles { get; set; }

    /// <summary>
    /// La liste des Couleurs possibles
    /// </summary>
    public List<Couleur> Couleurs { get; set; }
  }
}
