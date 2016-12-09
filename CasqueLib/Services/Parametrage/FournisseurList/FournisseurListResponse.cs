using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.FournisseurList
{
  /// <summary>
  /// Le resultat d'une recherche de fournisseurs
  /// </summary>
  public class FournisseurListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des fournisseurs
    /// </summary>
    public List<FournisseurView> Fournisseurs { get; set; }
  }
}
