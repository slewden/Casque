using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Administration.MailConfig
{
  /// <summary>
  /// Classe requête pour la configuration des emails
  /// </summary>
  [Api("Casque")]
  [Route("/mailconfigGet/{ApiKey}/", Verbs = "GET")]
  [Route("/mailconfigGet/{ApiKey}/", Verbs = "POST")]
  public class MailConfigRequest : RequestBase, IReturn<MailConfigResponse>
  {
    /// <summary>
    /// La config email édité
    /// </summary>
    public CasqueLib.Buisness.MailConfig Config { get; set; }
  }
}
