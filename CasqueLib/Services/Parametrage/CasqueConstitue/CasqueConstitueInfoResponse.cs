using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.CasqueConstitue
{
  /// <summary>
  /// Les infos de taill et couleur pour un type de pièce choisi
  /// </summary>
  public class CasqueConstitueInfoResponse
  {
    /// <summary>
    /// Le type de pièce choisi
    /// </summary>
    public int TypePieceCle { get; set; }
    
    /// <summary>
    /// Les tailles possibles pour ce type de pièce
    /// </summary>
    public List<TailleTypePiece> Tailles { get; set; }
    
    /// <summary>
    /// Les couelurs possibles pour ce type de pièce
    /// </summary>
    public List<CouleurTypePiece> Couleurs { get; set; }
  }
}
