using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.CasqueEdit
{
  /// <summary>
  /// Requete pour l'upload de photos
  /// </summary>
  [Api("NoUtopia")]
  [Route("/casquePhoto/{ApiKey}/", Verbs = "POST")]
  public class CasqueEditPhotoRequest : RequestBase, IReturn<CasqueEditPhotoResponse>
  {
  }
}
