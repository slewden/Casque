using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Administration.PosteList
{
  /// <summary>
  /// Le resultat d'une recherche de postes
  /// </summary>
  public class PosteListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des postes
    /// </summary>
    public List<PosteCompteur> Postes { get; set; }
  }
}
