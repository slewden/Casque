using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Livraison.List
{
  /// <summary>
  /// Le resultat d'une recherche de livraisons
  /// </summary>
  public class LivraisonListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des livraisons
    /// </summary>
    public List<LivraisonView> Livraisons { get; set; }
  }
}
