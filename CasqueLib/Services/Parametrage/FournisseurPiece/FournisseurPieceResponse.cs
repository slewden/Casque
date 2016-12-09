using System.Collections.Generic;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.FournisseurPiece
{
  /// <summary>
  /// Le resultat de l'édition des pièces fournies par un fournisseur
  /// </summary>
  public class FournisseurPieceResponse
  {
    /// <summary>
    /// Le fournisseur manipulé
    /// </summary>
    public FournisseurView Fournisseur { get; set; }

    /// <summary>
    /// Les pièces fournies
    /// </summary>
    public List<FournisseurPieceView> Pieces { get; set; }
  }
}
