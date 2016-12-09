using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Classe qui mappe le type de pièce fournies par un fournisseur
  /// </summary>
  [Alias("fournisseur_piece")]
  public class FournisseurPiece
  {
    /// <summary>
    /// La clé du fournisseur
    /// </summary>
    [Alias("clfo_id")]
    public int FournisseurCle { get; set; }
    
    /// <summary>
    /// La clé du type de piece
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// Prix unitaire de la pièce chez le fournisseur
    /// </summary>
    [Alias("fopi_prix_unitaire")]
    public decimal PrixUnitaire { get; set; }

    /// <summary>
    /// Montant des frais à chaque commande
    /// </summary>
    [Alias("fopi_frais_commande")]
    public int Frais { get; set; }
  }
}
