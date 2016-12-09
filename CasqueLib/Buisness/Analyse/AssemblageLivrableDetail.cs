using System.Collections.Generic;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Détail des pièces d'un assemblage
  /// Utilisé lors de la livraison : pour détaillé l'assemblage lu
  /// </summary>
  public class AssemblageLivrableDetail
  {
    /// <summary>
    /// Clé de l'assemblage 
    /// </summary>
    [Alias("asse_id")]
    public int AssemblageCle { get; set; }

    /// <summary>
    /// La clé de l'étiquette
    /// </summary>
    [Alias("etqu_id")]
    public int EtiquetteCle { get; set; }

    /// <summary>
    /// La clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// La clé de la couleur
    /// </summary>
    [Alias("coul_id")]
    public int CouleurCle { get; set; }

    /// <summary>
    /// La clé de la taille
    /// </summary>
    [Alias("tail_id")]
    public int TailleCle { get; set; }

    /// <summary>
    /// Le numéro de tag associé
    /// </summary>
    [Alias("numero")]
    public string Numero { get; set; }

    /// <summary>
    /// Le numéro de tag s'il est lu
    /// </summary>
    [Alias("numero_lu")]
    public string NumeroLu { get; set; }
    
    /// <summary>
    /// Le type d'état du tag et l'opération attendue :
    /// 1 : Tag commandé, attente de reception
    /// 2 : reçu, attente d'assemblage
    /// 3 : assemblé, attente de livraison
    /// 4 : livré
    /// </summary>
    [Alias("tag_operation")]
    public int OperationInt { get; set; } 
    
    /// <summary>
    /// Nom du type de pièce
    /// </summary>
    [Alias("typi_nom")]
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Code du type de pièce
    /// </summary>
    [Alias("typi_code")]
    public string TypePieceCode { get; set; }

    /// <summary>
    /// Photo du type de pièce
    /// </summary>
    [Alias("typi_photo")]
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string TypePiecePhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.TypePiece, this.TypePiecePhoto);
      }
    }

    /// <summary>
    /// Le nom de la couleur
    /// </summary>
    [Alias("coul_nom")]
    public string CouleurNom { get; set; }

    /// <summary>
    /// Le code de la couleur
    /// </summary>
    [Alias("coul_code")]
    public string CouleurCode { get; set; }

    /// <summary>
    /// Nom de la taille
    /// </summary>
    [Alias("tail_nom")]
    public string TailleNom { get; set; }

    /// <summary>
    /// Code de la taille
    /// </summary>
    [Alias("tail_code")]
    public string TailleCode { get; set; }

    /// <summary>
    /// Les infos du tag reporté pour normaliser le JS
    /// </summary>
    [Ignore]
    public List<DetailCommandeTagLu> Tags { get; set; }

    /// <summary>
    /// Crée la collection des tag et la remplit avec les infos du tag
    /// </summary>
    public void CreateTags()
    {
      this.Tags = new List<DetailCommandeTagLu>();
      this.Tags.Add(new DetailCommandeTagLu() 
      { 
        Numero = (this.Numero ?? string.Empty).Trim(),
        StatutInt = this.OperationInt,
      });
    }
  }
}
