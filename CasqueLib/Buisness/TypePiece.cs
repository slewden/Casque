using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe la table type de pièce
  /// </summary>
  [Alias("type_piece")]
  public class TypePiece
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="TypePiece"/>
    /// </summary>
    public TypePiece()
    {
      this.AvecTag = true;
    }

    /// <summary>
    /// La clé du type de pièce
    /// </summary>
    [AutoIncrement]
    [Alias("typi_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Nom du type de pièce
    /// </summary>
    [Alias("typi_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Avec une taille ou pas
    /// </summary>
    [Alias("typi_code")]
    public string Code { get; set; }

    /// <summary>
    /// Avec une taille ou pas
    /// </summary>
    [Alias("typi_avec_tag")]
    public bool AvecTag { get; set; }

    /// <summary>
    /// Description du type de pièce
    /// </summary>
    [Alias("typi_description")]
    public string Description { get; set; }

    /// <summary>
    /// Photo du type de pièce
    /// </summary>
    [Alias("typi_photo")]
    public string Photo { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string PhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.TypePiece, this.Photo);
      }
    }

    /// <summary>
    /// Indique si l'objet est correctement remplit pour se sauvegarder en bdd
    /// </summary>
    /// <returns>True si complet</returns>
    public bool IsComplet()
    {
      return !string.IsNullOrWhiteSpace(this.Nom) && !string.IsNullOrWhiteSpace(this.Code);
    }
  }
}
