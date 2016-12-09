using System.Collections.Generic;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe réponse pour la recherche des utilisation poste en cours
  /// </summary>
  public class LecteurEnCoursResponse
  {
    /// <summary>
    /// La liste à afficher
    /// </summary>
    public List<UtilisationPosteEnCoursView> LecteursEnCours { get; set; }

    /// <summary>
    /// Aucun lecteur bloque pour la page en cours
    /// </summary>
    public bool NoReaderFound { get; set; }
    
    /// <summary>
    /// La liste des postes possibles
    /// </summary>
    public List<Poste> Postes { get; set; }
  }
}
