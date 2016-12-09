using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Assemblage.Resume
{
  /// <summary>
  /// Classe requête pour la liste résumé par casque des assemblages
  /// </summary>
  [Api("Casque")]
  [Route("/assemblageResume/{ApiKey}", Verbs = "GET")]
  public class AssemblageResumeRequest : RequestPagineBase, IReturn<AssemblageResumeResponse>
  {
  }
}
