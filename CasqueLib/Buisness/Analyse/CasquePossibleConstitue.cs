using System.Collections.Generic;
using CasqueLib.Common;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// La constitution d'un casque possible
  /// Utilisé lors de l'assemblage
  /// </summary>
  public class CasquePossibleConstitue
  {
    /// <summary>
    /// La clé du casque
    /// </summary>
    public int CasqueCle { get; set; }

    /// <summary>
    /// La clé du type de pièce
    /// </summary>
    public int TypePieceCle { get; set; }

    /// <summary>
    /// Nom du type de pièce
    /// </summary>
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Description du type de pièce
    /// </summary>
    public string TypePieceDescription { get; set; }

    /// <summary>
    /// Code du type de pièce
    /// </summary>
    public string TypePieceCode { get; set; }

    /// <summary>
    /// Photo du type de pièce
    /// </summary>
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    public string TypePiecePhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.TypePiece, this.TypePiecePhoto);
      }
    }

    /// <summary>
    /// La clé de la couleur
    /// </summary>
    public int CouleurCle { get; set; }

    /// <summary>
    /// Nom de la couleur
    /// </summary>
    public string CouleurNom { get; set; }

    /// <summary>
    /// Description de la couleur
    /// </summary>
    public string CouleurDescription { get; set; }

    /// <summary>
    /// Code de la couleur
    /// </summary>
    public string CouleurCode { get; set; }

    /// <summary>
    /// La clé de la taille
    /// </summary>
    public int TailleCle { get; set; }

    /// <summary>
    /// Nom de la taille
    /// </summary>
    public string TailleNom { get; set; }

    /// <summary>
    /// Description de la taille
    /// </summary>
    public string TailleDescription { get; set; }

    /// <summary>
    /// Code de la taille
    /// </summary>
    public string TailleCode { get; set; }

    /// <summary>
    /// La liste des etiquette
    /// </summary>
    public List<CasquePossibleConstitueEtiquette> Tags { get; set; }

    /// <summary>
    /// Le nombre d'étiquette dans le casque
    /// </summary>
    public int NombreEtiquette
    {
      get
      {
        if (this.Tags == null)
        {
          return 0;
        }
        else
        {
          return this.Tags.Count;
        }
      }
    }
  }
}
