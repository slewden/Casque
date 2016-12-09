using System.Collections.Generic;
using CasqueLib.Buisness;

namespace CasqueLib.Services.Administration.UtilisateurList
{
  /// <summary>
  /// Le resultat d'une recherche d'utilisateurs
  /// </summary>
  public class UtilisateurListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des utilisateurs
    /// </summary>
    public List<Utilisateur> Utilisateurs { get; set; }
  }
}
