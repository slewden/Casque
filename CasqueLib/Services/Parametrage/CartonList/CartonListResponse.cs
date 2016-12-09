using System.Collections.Generic;
using CasqueLib.Buisness;

namespace CasqueLib.Services.Parametrage.CartonList
{
  /// <summary>
  /// Le resultat d'une recherche de cartons
  /// </summary>
  public class CartonListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des cartons
    /// </summary>
    public List<Carton> Cartons { get; set; }
  }
}
