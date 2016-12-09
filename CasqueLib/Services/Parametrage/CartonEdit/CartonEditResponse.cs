using CasqueLib.Buisness;

namespace CasqueLib.Services.Parametrage.CartonEdit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'un carton
  /// </summary>
  public class CartonEditResponse
  {
    /// <summary>
    /// Le carton manipulé
    /// </summary>
    public Carton Carton { get; set; }
  }
}
