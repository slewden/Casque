using System.Collections.Generic;
using CasqueLib.Buisness;

namespace CasqueLib.Services.Parametrage.TailleList
{
  /// <summary>
  /// Le resultat d'une recherche de tailles
  /// </summary>
  public class TailleListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des tailles
    /// </summary>
    public List<Taille> Tailles { get; set; }
  }
}
