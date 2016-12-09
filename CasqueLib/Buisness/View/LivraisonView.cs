using System;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe la vue livraison : ATTENTION ici pas de classe de base car pas nécessaire
  /// </summary>
  [Alias("v_livraison")]
  public class LivraisonView
  {
    /// <summary>
    /// Clé de la livraison
    /// </summary>
    [Alias("livr_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Date de création de la livraison
    /// </summary>
    [Alias("livr_d_livraison")]
    public DateTime Creation { get; set; }

    /// <summary>
    /// Clé de l'utilisateur qui a créé la livraison
    /// </summary>
    [Alias("util_id")]
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// Clé du client de la commande (nullable)
    /// </summary>
    [Alias("clfo_id")]
    public int ClientCle { get; set; }

    /// <summary>
    /// Date de validation de la livraison
    /// </summary>
    [Alias("livr_d_validation")]
    public DateTime? Validation { get; set; }

    /// <summary>
    /// Prénom et nom de l'utilisateur
    /// </summary>
    [Alias("util_prenom_nom")]
    public string UtilisateurNom { get; set; }

    /// <summary>
    /// email de l'utilisateur
    /// </summary>
    [Alias("util_email")]
    public string UtilisateurEmail { get; set; }

    /// <summary>
    /// Nom du client (nullable)
    /// </summary>
    [Alias("clfo_nom")]
    public string ClientNom { get; set; }

    /// <summary>
    /// email du client (nullable)
    /// </summary>
    [Alias("clfo_email")]
    public string ClientEmail { get; set; }

    /// <summary>
    /// Sujet lors de l'envoie d'email à ce client
    /// </summary>
    [Alias("clfo_sujet_email")]
    public string ClientSujectEmail { get; set; }

    /// <summary>
    /// adresse de livraison du client
    /// </summary>
    [Alias("clfo_adresse_livraison")]
    public string ClientAdresseLivraison { get; set; }

    /// <summary>
    /// L'adresse de livraison en HTML
    /// </summary>
    [Ignore]
    public string ClientAdresseLivraisonHtml
    {
      get
      {
        if (!string.IsNullOrWhiteSpace(this.ClientAdresseLivraison))
        {
          return this.ClientAdresseLivraison.Replace("\n", "<br />\n");
        }
        else
        {
          return string.Empty;
        }
      }
    }

    /// <summary>
    /// Nombre de cartons dans la commande
    /// </summary>
    [Alias("nombre_carton")]
    public int NombreCarton { get; set; }

    /// <summary>
    /// Nombre de casque dans la commande
    /// </summary>
    [Alias("nombre_piece")]
    public int NombrePiece { get; set; }

    /// <summary>
    /// Référence de la livraison (numéro du Bon de livraison)
    /// </summary>
    [Ignore]
    public string Reference
    {
      get
      {
        return string.Format("BL-{0}", this.Cle);
      }
    }
  }
}
