using System.Collections.Generic;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.FournisseurEdit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'un fournisseur
  /// </summary>
  public class FournisseurEditResponse
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
