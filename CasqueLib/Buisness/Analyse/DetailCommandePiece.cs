using System.Collections.Generic;
using System.Linq;
using CasqueLib.Common;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Pièces contenues dans le détail d'une commande
  /// Utilisé à la reception de pièce pour détail d'une commande en cours de réception
  /// </summary>
  public class DetailCommandePiece
  {
    /// <summary>
    /// La référence de la lignes de commande (N° de batch)
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// La clé du type de pièce
    /// </summary>
    public int TypePieceCle { get; set; }

    /// <summary>
    /// Le nom du type de pièce
    /// </summary>
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Le code du type de pièce
    /// </summary>
    public string TypePieceCode { get; set; }

    /// <summary>
    /// La photo du type de pièce
    /// </summary>
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// L'url de la photo de la pièce
    /// </summary>
    public string TypePiecePhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.TypePiece, this.TypePiecePhoto);
      }
    }

    /// <summary>
    /// La clé de la taille
    /// </summary>
    public int TailleCle { get; set; }

    /// <summary>
    /// Le nom de la taille
    /// </summary>
    public string TailleNom { get; set; }

    /// <summary>
    /// Le code de la taille
    /// </summary>
    public string TailleCode { get; set; }

    /// <summary>
    /// Le clé de la couleur
    /// </summary>
    public int CouleurCle { get; set; }

    /// <summary>
    /// Le nom de la couleur
    /// </summary>
    public string CouleurNom { get; set; }

    /// <summary>
    /// Le code de la couleur
    /// </summary>
    public string CouleurCode { get; set; }

    /// <summary>
    /// La liste des tag associés à ce type de pièce dans la commande
    /// </summary>
    public List<DetailCommandeTagLu> Tags { get; set; }

    /// <summary>
    /// Le nombre total de tag de ce type
    /// </summary>
    public int TotalTag
    {
      get
      {
        if (this.Tags == null || !this.Tags.Any())
        {
          return 0;
        }
        else
        {
          return this.Tags.Count;
        }
      }
    }

    /// <summary>
    /// Le nombre de tag attendus
    /// </summary>
    public int TotalAttendus
    {
      get
      {
        if (this.Tags == null || !this.Tags.Any())
        {
          return 0;
        }
        else
        {
          return this.Tags.Count(x => x.StatutInt == 1); // 1 == attente reception
        }
      }
    }
  }
}