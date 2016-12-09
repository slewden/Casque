using System.Collections.Generic;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Joins;

namespace CasqueLib.Services.Administration.UtilisateurDroit
{
  /// <summary>
  /// Le resultat de l'édition des droits d'un utilistateur
  /// </summary>
  public class UtilisateurDroitResponse
  {
    /// <summary>
    /// L'utilisateur manipulé
    /// </summary>
    public Utilisateur Utilisateur { get; set; }

    /// <summary>
    /// Les droits édités
    /// </summary>
    public List<PageDroitView> Droits { get; set; }
  }
}
