using System;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe une étiquette en BDD
  /// </summary>
  [Alias("etiquette")]
  public class Etiquette
  {
    /// <summary>
    /// La clé de l'étiquette
    /// </summary>
    [Alias("etqu_id")]
    public int Cle { get; set; }

    /// <summary>
    /// La ligne de commande qui a créé l'étiquette
    /// </summary>
    [Alias("colg_id")]
    public int CommandeLigneCle { get; set; }

    /// <summary>
    /// Le N° de tag de l'étiquette
    /// </summary>
    [Alias("etqu_numero")]
    public string Numero { get; set; }

    /// <summary>
    /// La date de création de l'étiquette
    /// </summary>
    [Alias("etqu_d_creation")]
    public DateTime Creation { get; set; }

    /// <summary>
    /// la date d'entrée dans le stock
    /// </summary>
    [Alias("etqu_d_entree_stock")]
    public DateTime? EntreeStock { get; set; }

    /// <summary>
    /// L'utilisateur qui mémorise l'entrée en stock
    /// </summary>
    [Alias("util_id_entree_stock")]
    public int EntreeStockUtilisateurCle { get; set; }

    /// <summary>
    /// La date de l'assemblage de l'étiquette
    /// </summary>
    [Alias("etqu_d_assemblage")]
    public DateTime? Assemblage { get; set; }

    /// <summary>
    /// La clé de l'assemblage auquel appartient l'étiquette
    /// </summary>
    [Alias("asse_id")]
    public int AssemblageCle { get; set; }

    /// <summary>
    /// La clé de l'utilisateur qui a enregistré l'entrée dans le stock
    /// </summary>
    [Alias("util_id_entree_stock")]
    public int AssemblageUtilisateurCle { get; set; }

    /// <summary>
    /// La date de livraison de l'étiquette
    /// </summary>
    [Alias("etqu_d_livraison")]
    public DateTime? Livraison { get; set; }

    /// <summary>
    /// La clé de l'utilisateur qui a enregistré la livraison
    /// </summary>
    [Alias("util_id_livraison")]
    public int LivraisonUtilisateurCle { get; set; }
  }
}
