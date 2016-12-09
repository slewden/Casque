using System;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// La vue d'un assemblage
  /// </summary>
  [Alias("v_assemblage")]
  public class AssemblageView
  {
    /// <summary>
    /// La clé de l'assemblge
    /// </summary>
    [Alias("asse_id")]
    public int Cle { get; set; }

    /// <summary>
    /// La clé du casque
    /// </summary>
    [Alias("casq_id")]
    public int CasqueCle { get; set; }

    /// <summary>
    /// La clé de l'utilisateur qui fait l'assemblage
    /// </summary>
    [Alias("util_id_assemblage")]
    public int UtilisateurAssembleCle { get; set; }

    /// <summary>
    /// La date de création
    /// </summary>
    [Alias("asse_d_creation")]
    public DateTime Creation { get; set; }

    /// <summary>
    /// Le tag de l'assemblage
    /// </summary>
    [Alias("asse_tag")]
    public string Tag { get; set; }

    /// <summary>
    /// La date de validation
    /// </summary>
    [Alias("asse_d_validation")]
    public DateTime? Validation { get; set; }

    /// <summary>
    /// La clé de la livraison
    /// </summary>
    [Alias("livr_id")]
    public int LivraisonCle { get; set; }

    /// <summary>
    /// La clé du carton de livraison
    /// </summary>
    [Alias("cart_id_livraison")]
    public int CartonCle { get; set; }

    /// <summary>
    /// L'index du carton dans la commande
    /// </summary>
    [Alias("asse_carton_index")]
    public int CartonIndex { get; set; }

    /// <summary>
    /// Le nom du casque
    /// </summary>
    [Alias("casq_nom")]
    public string CasqueNom { get; set; }

    /// <summary>
    /// Le code du casque
    /// </summary>
    [Alias("casq_code")]
    public string CasqueCode { get; set; }

    /// <summary>
    /// La photo du casque
    /// </summary>
    [Alias("casq_photo")]
    public string CasquePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string CasquePhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.Casque, this.CasquePhoto);
      }
    }

    /// <summary>
    /// Le nom du carton
    /// </summary>
    [Alias("cart_nom")]
    public string CartonNom { get; set; }

    /// <summary>
    /// Le code du carton
    /// </summary>
    [Alias("cart_code")]
    public string CartonCode { get; set; }

    /// <summary>
    /// Le nom de l'utilisateur qui a assemblé
    /// </summary>
    [Alias("util_nom_assemble")]
    public string UtilisateurAssembleNom { get; set; }

    /// <summary>
    /// L'email de l'utilisateur qui a assemblé
    /// </summary>
    [Alias("util_email_assemble")]
    public string UtilisateurAssembleEmail { get; set; }

    /// <summary>
    /// La date de validation de la livraison
    /// </summary>
    [Alias("livr_d_validation")]
    public DateTime? Livraison { get; set; }

    /// <summary>
    /// Le nom de l'utilisateur qui a livré
    /// </summary>
    [Alias("util_nom_livre")]
    public string UtilisateurLivreNom { get; set; }

    /// <summary>
    /// L'email de l'utilisateur qui a livré
    /// </summary>
    [Alias("util_email_livre")]
    public string UtilisateurLivreEmail { get; set; }

    /// <summary>
    /// Le nom du client livré
    /// </summary>
    [Alias("clfo_nom")]
    public string ClientNom { get; set; }

    /// <summary>
    /// L'email du client livré
    /// </summary>
    [Alias("clfo_email")]
    public string ClientEmail { get; set; }

    /// <summary>
    /// Le nombre de pièce dans l'assemblage
    /// </summary>
    [Alias("nombre_piece")]
    public int NombrePiece { get; set; }

    /// <summary>
    /// Statut de l'assemblage
    /// 1 = En cours de construction, 2 = En stock, 3 = livré
    /// </summary>
    [Alias("statut")]
    public int StatutInt { get; set; }

    /// <summary>
    /// Nom du statut
    /// </summary>
    [Ignore]
    public string StatutNom
    {
      get
      {
        switch (this.StatutInt)
        {
          case 1:
            return "En cours";
          case 2:
            return "En Stock";
          case 3:
            return "Livré";
        }

        return string.Format("??{0} ??", this.StatutInt);
      }
    }
  }
}
