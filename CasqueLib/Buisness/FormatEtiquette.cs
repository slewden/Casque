using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe un format d'étiquette
  /// (fichier de positionnement des données pour l'impression)
  /// </summary>
  [Alias("format_etiquette")]
  public class FormatEtiquette
  {
    /// <summary>
    /// Clé du format
    /// </summary>
    [AutoIncrement]
    [Alias("fmte_id")]
    public int Cle { get; set; }
    
    /// <summary>
    /// Nom du fichier de mise en page de l'étiquette
    /// </summary>
    [Alias("fmte_fichier")]
    public string Fichier { get; set; }

    /// <summary>
    /// L'url d'accès au fichier template
    /// </summary>
    [Ignore]
    public string PhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.ModeleEtiquette, this.Fichier);
      }
    }
  }
}
