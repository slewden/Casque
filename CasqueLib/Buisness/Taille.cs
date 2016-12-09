using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe une page
  /// </summary>
  [Alias("taille")]
  public class Taille
  {
    /// <summary>
    /// Clé de la taille
    /// </summary>
    [AutoIncrement]
    [Alias("tail_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Nom de la taille
    /// </summary>
    [Alias("tail_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Code de la taille
    /// </summary>
    [Alias("tail_code")]
    public string Code { get; set; }

    /// <summary>
    /// ordre d'apparition de la taille
    /// </summary>
    [Alias("tail_ordre")]
    public int Ordre { get; set; }

    /// <summary>
    /// Description de la taille
    /// </summary>
    [Alias("tail_description")]
    public string Description { get; set; }

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
