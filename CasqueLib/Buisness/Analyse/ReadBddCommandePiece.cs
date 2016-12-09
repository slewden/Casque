using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Classe qui mappe les infos d'une pièce commandée
  /// </summary>
  public class ReadBddCommandePiece
  {
    /// <summary>
    /// Clé de la commande
    /// </summary>
    [Alias("comd_id")]
    public int CommandeCle { get; set; }

    /// <summary>
    /// Clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// La référence du groupe de pièce commandé
    /// </summary>
    [Alias("colg_reference")]
    public string Reference { get; set; }

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
    /// La photo du type de pièce
    /// </summary>
    [Alias("typi_photo")]
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// L'url de la photo de la pièce
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
    /// Clé de la Taille du type de pièce
    /// </summary>
    [Alias("tail_id")]
    public int TailleCle { get; set; }

    /// <summary>
    /// Nom de la taille du type de pièce
    /// </summary>
    [Alias("tail_nom")]
    public string TailleNom { get; set; }

    /// <summary>
    /// Code de la taille du type de pièce
    /// </summary>
    [Alias("tail_code")]
    public string TailleCode { get; set; }

    /// <summary>
    /// Clé de la couleur du type de pièce
    /// </summary>
    [Alias("coul_id")]
    public int CouleurCle { get; set; }

    /// <summary>
    /// Nom de la couleur du type de pièce
    /// </summary>
    [Alias("coul_nom")]
    public string CouleurNom { get; set; }

    /// <summary>
    /// Code de la couleur du type de pièce
    /// </summary>
    [Alias("coul_code")]
    public string CouleurCode { get; set; }

    /// <summary>
    /// Le numéro du tag
    /// </summary>
    [Alias("etqu_numero")]
    public string Numero { get; set; }

    /// <summary>
    /// Le type d'état du tag et l'opération attendue :
    /// 1 : Tag commandé, attente de reception
    /// 2 : reçu, attente d'assemblage
    /// 3 : assemblé, attente de livraison
    /// 4 : livré
    /// </summary>
    [Alias("tag_operation")]
    public int OperationInt { get; set; } 
  }
}
