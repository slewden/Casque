using System.Linq;
using CasqueLib.Buisness.View;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Assemblage.Detail
{
  /// <summary>
  /// Service pour le détail d'un assemblage
  /// </summary>
  public class AssemblageDetailService : FsService
  {
    /// <summary>
    /// Get : Renvoie les infos de l'assemblage demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(AssemblageDetailRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Clé' Non valide");
      }

      AssemblageDetailResponse rep = new AssemblageDetailResponse();
      rep.Assemblage = this.Db.Select<AssemblageView>(x => x.Cle == request.Cle).FirstOrDefault();
      rep.Etiquettes = this.Db.Select<EtiquetteView>(x => x.AssemblageCle == request.Cle);
      return rep;
    }
  }
}
