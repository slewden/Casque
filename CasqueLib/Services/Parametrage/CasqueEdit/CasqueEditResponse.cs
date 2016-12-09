using System.Collections.Generic;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Parametrage.CasqueEdit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'un casque
  /// </summary>
  public class CasqueEditResponse
  {
    /// <summary>
    /// Le casque manipulé
    /// </summary>
    public CasqueView Casque { get; set; }

    /// <summary>
    /// Les pièces qui constituent le casque
    /// </summary>
    public List<CasqueConstitueView> Pieces { get; set; }
  }
}
