using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe la table commande ligne
  /// </summary>
  [Alias("commande_ligne")]
  public class CommandeLigne
  {
    /// <summary>
    /// Clé de la ligne de commande
    /// </summary>
    [AutoIncrement]
    [Alias("colg_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Clé de la commande associée à la ligne
    /// </summary>
    [Alias("comd_id")]
    public int CommandeCle { get; set; }

    /// <summary>
    /// Clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// La référence de la ligne commandée
    /// </summary>
    [Alias("colg_reference")]
    public string Reference { get; set; }

    /// <summary>
    /// Clé de la taille (optionnelle)
    /// </summary>
    [Alias("tail_id")]
    public int? TailleCle { get; set; }

    /// <summary>
    /// Clé de la couleur (optionnelle)
    /// </summary>
    [Alias("coul_id")]
    public int? CouleurCle { get; set; }

    /// <summary>
    /// Nombre de pièces commandées
    /// </summary>
    [Alias("colg_quantite")]
    public int Quantite { get; set; }

    /// <summary>
    /// Le prix unitaire de la ligne
    /// </summary>
    [Alias("colg_prix_unitaire")]
    public decimal? PrixUnitaire { get; set; }

    /// <summary>
    /// Les frais de la ligne
    /// </summary>
    [Alias("colg_frais_commande")]
    public decimal? Frais { get; set; }

    /// <summary>
    /// Indique si l'objet est bien remplit pour être envoyé en BDD
    /// (on ne vérifie pas CommandeClé l'application le gère
    /// </summary>
    /// <returns>True si remplit ok</returns>
    public bool IsComplet()
    {
      if (this.TypePieceCle <= 0)
      { // faut un type de pièce
        return false;
      }

      return true;
    }
  }
}
