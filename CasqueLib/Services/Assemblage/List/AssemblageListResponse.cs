using System.Collections.Generic;
using CasqueLib.Buisness.View;
using CasqueLib.Common;

namespace CasqueLib.Services.Assemblage.List
{
  /// <summary>
  /// Le resultat d'une recherche d'assemblage
  /// </summary>
  public class AssemblageListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des assemblages
    /// </summary>
    public List<AssemblageView> Assemblages { get; set; }

    /// <summary>
    /// La liste des casques pour filtre des assemblages
    /// </summary>
    public List<NomCle> Casques { get; set; }
  }
}
