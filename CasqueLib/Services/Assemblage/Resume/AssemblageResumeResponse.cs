using System.Collections.Generic;
using System.Linq;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Assemblage.Resume
{
  /// <summary>
  /// Le resultat d'une recherche dela liste résumé par casque d'assemblage
  /// </summary>
  public class AssemblageResumeResponse 
  {
    /// <summary>
    /// La liste paginée des assemblages
    /// </summary>
    public List<CasqueAssembleView> Assemblages { get; set; }

    /// <summary>
    /// Le total
    /// </summary>
    public int Total
    {
      get
      {
        if (this.Assemblages != null && this.Assemblages.Any())
        {
          return this.Assemblages.Sum(x => x.Nombre);
        }
        else
        {
          return 0;
        }
      }
    }
  }
}
