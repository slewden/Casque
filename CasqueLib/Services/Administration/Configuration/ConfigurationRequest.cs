using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Administration.Configuration
{
  /// <summary>
  /// Classe requête pour la gestion des configurations
  /// </summary>
  [Api("Casque")]
  [Route("/configurationsGet/{ApiKey}/", Verbs = "GET")]
  [Route("/configurationsGet/{ApiKey}/", Verbs = "POST")]
  public class ConfigurationRequest : RequestBase, IReturn<ConfigurationResponse>
  {
    /// <summary>
    /// Les configurations
    /// </summary>
    public List<CasqueLib.Buisness.Configuration> Configurations { get; set; }
  }
}
