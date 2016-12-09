using System.Collections.Generic;
using CasqueLib.Buisness.Joins;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CasqueConstitue
{
  /// <summary>
  /// Classe requête pour la gestion des constitutions de casque
  /// </summary>
  [Api("Casque")]
  [Route("/casqueConstitueGet/{ApiKey}/", Verbs = "GET")]
  [Route("/casqueConstitueEdit/{ApiKey}/", Verbs = "POST")]
  public class CasqueConstitueRequest : RequestBase, IReturn<CasqueConstitueResponse>
  {
    /// <summary>
    /// Clé du casque pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// En modif ajuste le nom de la pièce
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Les pièces qui constituent le casque
    /// </summary>
    public List<CasqueConstitueView> Pieces { get; set; }
  }
}
