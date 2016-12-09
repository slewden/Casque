using CasqueLib.Buisness;

namespace CasqueLib.Services.Administration.UtilisateurEdit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'un utilistateur
  /// </summary>
  public class UtilisateurEditResponse
  {
    /// <summary>
    /// L'utilisateur manipulé
    /// </summary>
    public Utilisateur Utilisateur { get; set; }
  }
}
