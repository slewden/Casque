using System.Collections.Generic;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.CasqueConstitue
{
  /// <summary>
  /// Le resultat de l'édition des pièces qui constituent un casque
  /// </summary>
  public class CasqueConstitueResponse
  {
    /// <summary>
    /// Le casque manipulé
    /// </summary>
    public CasqueView Casque { get; set; }

    /// <summary>
    /// Les pièces qui constituent le casque
    /// </summary>
    public List<CasqueConstitueView> Pieces { get; set; }
    
    /// <summary>
    /// Le nombre d'assemblage fait 
    /// (utilisé pour savoir si on peut ou non modifier l'assemblage)
    /// </summary>
    public int NombreAssemblage { get; set; }
  }
}
