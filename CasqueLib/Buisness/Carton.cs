using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe un carton
  /// </summary>
  [Alias("carton")]
  public class Carton
  {
    #region Properties
    /// <summary>
    /// La clé du carton
    /// </summary>
    [AutoIncrement]
    [Alias("cart_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Le nom du carton
    /// </summary>
    [Alias("cart_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Le code du carton
    /// </summary>
    [Alias("cart_code")]
    public string Code { get; set; }

    /// <summary>
    /// La description du carton
    /// </summary>
    [Alias("cart_description")]
    public string Description { get; set; }

    /// <summary>
    /// La description en HTML
    /// </summary>
    [Ignore]
    public string DescriptionHtml
    {
      get
      {
        if (!string.IsNullOrWhiteSpace(this.Description))
        {
          return this.Description.Replace("\n", "<br />\n");
        }
        else
        {
          return string.Empty;
        }
      }
    }
    #endregion

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
