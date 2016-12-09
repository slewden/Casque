using CasqueLib.Buisness;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CartonEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'un carton
  /// </summary>
  [Api("Casque")]
  [Route("/cartonGet/{ApiKey}/", Verbs = "GET")]
  [Route("/cartonEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/cartonDelete/{ApiKey}/", Verbs = "DELETE")]
  public class CartonEditRequest : RequestBase, IReturn<CartonEditResponse>
  {
    /// <summary>
    /// Clé du carton pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Le carton édité
    /// </summary>
    public Carton Carton { get; set; }
  }
}
