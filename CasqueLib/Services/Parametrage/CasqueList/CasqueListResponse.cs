using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.CasqueList
{
  /// <summary>
  /// Le resultat d'une recherche de casques
  /// </summary>
  public class CasqueListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des casques
    /// </summary>
    public List<CasqueView> Casques { get; set; }
  }
}
