using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe une couleur possible pour un type de pièce
  /// </summary>
  [Alias("v_couleur_type_piece")]
  public class CouleurTypePiece : Couleur
  {
    /// <summary>
    /// Renvoie un objet vide
    /// </summary>
    [Ignore]
    public static CouleurTypePiece Empty
    {
      get
      {
        CouleurTypePiece res = new CouleurTypePiece();
        res.Nom = string.Empty;
        return res;
      }
    }
    
    /// <summary>
    /// La clé de la pices
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }
  }
}
