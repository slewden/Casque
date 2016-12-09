using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Détail d'une étiquette
  /// </summary>
  [Alias("v_etiquette")]
  public class EtiquetteView : Etiquette
  {
    /// <summary>
    /// Clé de la commande
    /// </summary>
    [Alias("comd_id")]
    public int CommandeCle { get; set; }

    /// <summary>
    /// La référence de la ligne de commande
    /// </summary>
    [Alias("colg_reference")]
    public string CommandeLigneReference { get; set; }

    /// <summary>
    /// Clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// Nom du type de pièces
    /// </summary>
    [Alias("typi_nom")]
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Code du type de pièces
    /// </summary>
    [Alias("typi_code")]
    public string TypePieceCode { get; set; }

    /// <summary>
    /// Photo du type de pièces
    /// </summary>
    [Alias("typi_photo")]
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string PhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.TypePiece, this.TypePiecePhoto);
      }
    }

    /// <summary>
    /// Clé de la couleur
    /// </summary>
    [Alias("coul_id")]
    public int CouleurCle { get; set; }

    /// <summary>
    /// Nom de la couleur
    /// </summary>
    [Alias("coul_nom")]
    public string CouleurNom { get; set; }

    /// <summary>
    /// Code de la couleur
    /// </summary>
    [Alias("coul_code")]
    public string CouleurCode { get; set; }

    /// <summary>
    /// Clé de la taille
    /// </summary>
    [Alias("tail_id")]
    public int TailleCle { get; set; }

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
  }
}
