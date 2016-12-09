using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe une taille possible pour un type de pièce
  /// </summary>
  [Alias("v_taille_type_piece")]
  public class TailleTypePiece : Taille
  {
    /// <summary>
    /// Renvoie un objet vide
    /// </summary>
    [Ignore]
    public static TailleTypePiece Empty
    {
      get
      {
        TailleTypePiece res = new TailleTypePiece();
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
