using System;
using System.Linq;
using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Commande.Resume
{
  /// <summary>
  /// Service pour l'écran réumé des commandes
  /// </summary>
  public class CommandeResumeService : FsService
  {
    /// <summary>
    /// Get : Renvoie les infos
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CommandeResumeRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      CommandeResumeResponse rep = new CommandeResumeResponse();
      rep.Commandes = this.Db.Select<CommandeResumeData>();
      return rep;
    }

    /// <summary>
    /// Post : Acquitte une commande
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(CommandeResumeRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'Clé' Non valide");
      }

      Buisness.Commande cmd = this.Db.Select<Buisness.Commande>(x => x.Cle == request.Cle).FirstOrDefault();
      if (cmd == null)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'Clé' invalide");
      }

      if (cmd.Acquittee != null)
      {
        return new HttpError(HttpStatusCode.BadRequest, "Commande déjà acquittée");
      }

      // mise à jour de l'acquittement de la commande
      cmd.Acquittee = DateTime.Now;
      this.Db.UpdateOnly(cmd, x => new { x.Acquittee }, u => u.Cle == cmd.Cle);
      
      return null;
    }
  }
}
