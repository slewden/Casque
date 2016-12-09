using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Classe qui mappe un casque (type de casque)
  /// </summary>
  [Alias("casque")]
  public class Casque
  {
    /// <summary>
    /// Clé du casque
    /// </summary>
    [AutoIncrement]
    [Alias("casq_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Nom du casque
    /// </summary>
    [Alias("casq_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Code du casque
    /// </summary>
    [Alias("casq_code")]
    public string Code { get; set; }

    /// <summary>
    /// Description du casque
    /// </summary>
    [Alias("casq_description")]
    public string Description { get; set; }

    /// <summary>
    /// Photo du casque
    /// </summary>
    [Alias("casq_photo")]
    public string Photo { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string PhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.Casque, this.Photo);
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
