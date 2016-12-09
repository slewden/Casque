using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Classe qui regrouppe toutes les infos d'une utilisation poste
  /// </summary>
  public class UtilisationPosteView : UtilisationPoste
  {
    /// <summary>
    /// Les infos détailles sur la config du poste utilisé
    /// </summary>
    [Ignore]
    public Poste PosteDetail { get; set; }

    /// <summary>
    /// Les lectures faites durant la session
    /// </summary>
    [Ignore]
    public List<Lecture> Lectures { get; set; }
  }
}
