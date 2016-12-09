using System.Collections.Generic;
using CasqueLib.Buisness;

namespace CasqueLib.Services.Parametrage.CouleurList
{
  /// <summary>
  /// Le resultat d'une recherche de couleurs
  /// </summary>
  public class CouleurListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des couleurs
    /// </summary>
    public List<Couleur> Couleurs { get; set; }
  }
}
