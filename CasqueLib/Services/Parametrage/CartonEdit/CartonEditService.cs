using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.CartonEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'un carton
  /// </summary>
  public class CartonEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie le carton demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CartonEditRequest request)
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

      CartonEditResponse rep = new CartonEditResponse();
      rep.Carton = this.Db.Select<Carton>(x => x.Cle == request.Cle).FirstOrDefault();
      return rep;
    }

    /// <summary>
    /// Delete : Supprime le carton demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(CartonEditRequest request)
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
        this.Db.Delete<Carton>(x => x.Cle == request.Cle);
      }
      catch
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
      }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'un carton
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(CartonEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Carton == null || !request.Carton.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'carton' non valide ou incomplet");
      }

      Carton u;
      if (request.Cle <= 0)
      { // insertion
        this.Db.Insert<Carton>(request.Carton);
        request.Carton.Cle = (int)this.Db.GetLastInsertId();
        u = request.Carton;
      }
      else
      {
        u = this.Db.Select<Carton>(x => x.Cle == request.Cle).FirstOrDefault();
        if (u == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        u.Nom = request.Carton.Nom;
        u.Code = request.Carton.Code;
        u.Description = request.Carton.Description;
        this.Db.Update<Carton>(u);
      }

      return new CartonEditResponse() { Carton = u };
    }
  }
}
