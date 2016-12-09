using CasqueLib.Buisness.View;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CasqueEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'un casque
  /// </summary>
  [Api("Casque")]
  [Route("/casqueGet/{ApiKey}/", Verbs = "GET")]
  [Route("/casqueEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/casqueDelete/{ApiKey}/", Verbs = "DELETE")]
  public class CasqueEditRequest : RequestBase, IReturn<CasqueEditResponse>
  {
    /// <summary>
    /// Clé du casque pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Le casque édité
    /// </summary>
    public CasqueView Casque { get; set; }
  }
}
