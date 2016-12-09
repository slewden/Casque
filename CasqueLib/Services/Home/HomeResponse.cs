using System.Collections.Generic;
using CasqueLib.Buisness.Report;
using CasqueLib.Common;

namespace CasqueLib.Services.Home
{
  /// <summary>
  /// Réponse pour la home page
  /// </summary>
  public class HomeResponse 
  {
    /// <summary>
    /// Les statistiques de la home
    /// </summary>
    public List<CompteurBase> Statistiques { get; set; }

    /// <summary>
    /// Liste des compteurs 
    /// </summary>
    public List<NomCle> CompteurMenu { get; set; }
  }
}
