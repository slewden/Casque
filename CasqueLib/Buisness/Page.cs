using System.Collections.Generic;
using System.Data;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe une page
  /// </summary>
  [Alias("page")]
  public class Page
  {
    /// <summary>
    /// Code de la page
    /// </summary>
    [Alias("page_code")]
    public string Cle { get; set; }
    
    /// <summary>
    /// Code de la page parent
    /// </summary>
    [Alias("pere_page_code")]
    public string PereCle { get; set; }

    /// <summary>
    /// Ce droit est à gérer ou pas ?
    /// 0 = c'est pas un droit, 1 = on peut gérer le droit, 2 = visible mais non modifiable
    /// </summary>
    [Alias("page_avec_droit")]
    public int AvecDroitInt { get; set; }
    
    /// <summary>
    /// niveau de la page
    /// </summary>
    [Alias("page_niveau")]
    public int Niveau { get; set; }

    /// <summary>
    /// Nom de la page
    /// </summary>
    [Alias("page_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Url de la page
    /// </summary>
    [Alias("page_url")]
    public string Url { get; set; }

    /// <summary>
    /// Description de la page
    /// </summary>
    [Alias("page_description")]
    public string Description { get; set; }

    /// <summary>
    /// Picto associé à la page
    /// </summary>
    [Alias("page_image")]
    public string Image { get; set; }

    /// <summary>
    /// Ordre dans le niveau du menu
    /// </summary>
    [Alias("page_ordre")]
    public int Ordre { get; set; }
  }
}
