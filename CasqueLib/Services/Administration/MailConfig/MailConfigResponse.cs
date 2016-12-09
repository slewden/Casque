using CasqueLib.Buisness;

namespace CasqueLib.Services.Administration.MailConfig
{
  /// <summary>
  /// Le resultat des traitements CRUD de la config email
  /// </summary>
  public class MailConfigResponse
  {
    /// <summary>
    /// La config email édité
    /// </summary>
    public CasqueLib.Buisness.MailConfig Config { get; set; }
  }
}
