using System;
using System.Collections.Generic;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Toutes les infos d'un assemblage
  /// Utilisé lors de la livraison
  /// </summary>
  public class TechniqueAssemblage
  {
    /// <summary>
    /// Clé de l'assemblage 
    /// </summary>
    [Alias("asse_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Clé du casque
    /// </summary>
    [Alias("casq_id")]
    public int CasqueCle { get; set; }

    /// <summary>
    /// Clé de l'utilisateur qui fait l'assemblage
    /// </summary>
    [Alias("util_id")]
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// Date de la création de l'assemblage
    /// </summary>
    [Alias("asse_d_creation")]
    public DateTime Creation { get; set; }

    /// <summary>
    /// Tag de l'assemblage
    /// </summary>
    [Alias("asse_tag")]
    public string Tag { get; set; }

    /// <summary>
    /// Date de la validation de l'assemblage
    /// </summary>
    [Alias("asse_d_validation")]
    public DateTime Validation { get; set; }

    /// <summary>
    /// Clé de la livraison
    /// </summary>
    [Alias("livr_id")]
    public int LivraisonCle { get; set; }

    /// <summary>
    /// Clé du carton de la livraison
    /// </summary>
    [Alias("cart_id_livraison")]
    public int CartonCle { get; set; }

    /// <summary>
    /// Index du carton dans la livraison
    /// </summary>
    [Alias("asse_carton_index")]
    public int CartonIndex { get; set; }

    /// <summary>
    /// Nom du casque
    /// </summary>
    [Alias("casq_nom")]
    public string CasqueNom { get; set; }

    /// <summary>
    /// Code du casque
    /// </summary>
    [Alias("casq_code")]
    public string CasqueCode { get; set; }

    /// <summary>
    /// Photo du casque
    /// </summary>
    [Alias("casq_photo")]
    public string CasquePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo du casque
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
    /// La liste des détails
    /// </summary>
    [Ignore]
    public List<AssemblageLivrableDetail> Pieces { get; set; }
  }
}
