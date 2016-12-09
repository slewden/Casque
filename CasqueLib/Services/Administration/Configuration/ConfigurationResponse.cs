using System.Collections.Generic;
using CasqueLib.Buisness;

namespace CasqueLib.Services.Administration.Configuration
{
  /// <summary>
  /// Le resultat des traitements CRUD des configurations
  /// </summary>
  public class ConfigurationResponse
  {
    /// <summary>
    /// Les configurations
    /// </summary>
    public List<CasqueLib.Buisness.Configuration> Configurations { get; set; }
  }
}
