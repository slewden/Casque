using CasqueLib.Buisness.View;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Assemblage.Resume
{
  /// <summary>
  /// Service pour la liste résumé par casque des assemblages
  /// </summary>
  public class AssemblageResumeService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des assemblages demandés
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(AssemblageResumeRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      AssemblageResumeResponse rep = new AssemblageResumeResponse();
      rep.Assemblages = this.Db.Select<CasqueAssembleView>();
      return rep;
    }
  }
}
