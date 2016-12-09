using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.TailleEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'une taille
  /// </summary>
  public class TailleEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie la taille demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(TailleEditRequest request)
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

      TailleEditResponse rep = new TailleEditResponse();
      rep.Taille = this.Db.Select<Taille>(x => x.Cle == request.Cle).FirstOrDefault();
      return rep;
    }

    /// <summary>
    /// Delete : Supprime la taille demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(TailleEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
      }

      try
      {
        this.Db.Delete<Taille>(x => x.Cle == request.Cle);
        this.Db.ExecuteNonQuery("EXEC dbo.taille_range_par_ordre");
      }
      catch
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
      }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'une taille
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(TailleEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Taille == null || !request.Taille.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'taille' non valide ou incomplète");
      }

      Taille u;
      if (request.Cle <= 0)
      { // insertion
        this.Db.Insert<Taille>(request.Taille);
        request.Taille.Cle = (int)this.Db.GetLastInsertId();
        u = request.Taille;
      }
      else
      {
        u = this.Db.Select<Taille>(x => x.Cle == request.Cle).FirstOrDefault();
        if (u == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        u.Nom = request.Taille.Nom;
        u.Code = request.Taille.Code;
        u.Ordre = request.Taille.Ordre;
        u.Description = request.Taille.Description;
        this.Db.Update<Taille>(u);
      }

      this.Db.ExecuteNonQuery("EXEC dbo.taille_range_par_ordre");
      return new TailleEditResponse() { Taille = u };
    }
  }
}
