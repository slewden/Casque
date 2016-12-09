using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Assemblage.Detail
{
  /// <summary>
  /// Le resultat du détail d'un assemblage
  /// </summary>
  public class AssemblageDetailResponse 
  {
    /// <summary>
    /// L'assemblage en lui même
    /// </summary>
    public AssemblageView Assemblage { get; set; }
    
    /// <summary>
    /// Les étiquettes associées à l'assemblage
    /// </summary>
    public List<EtiquetteView> Etiquettes { get; set; }
  }
}
