using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.CouleurEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'une couleur
  /// </summary>
  public class CouleurEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie la couleur demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CouleurEditRequest request)
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

      CouleurEditResponse rep = new CouleurEditResponse();
      rep.Couleur = this.Db.Select<Couleur>(x => x.Cle == request.Cle).FirstOrDefault();
      return rep;
    }

    /// <summary>
    /// Delete : Supprime la couleur demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(CouleurEditRequest request)
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
        this.Db.Delete<Couleur>(x => x.Cle == request.Cle);
      }
      catch
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
      }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'une couleur
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(CouleurEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Couleur == null || !request.Couleur.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'couleur' non valide ou incomplète");
      }

      Couleur u;
      if (request.Cle <= 0)
      { // insertion
        this.Db.Insert<Couleur>(request.Couleur);
        request.Couleur.Cle = (int)this.Db.GetLastInsertId();
        u = request.Couleur;
      }
      else
      {
        u = this.Db.Select<Couleur>(x => x.Cle == request.Cle).FirstOrDefault();
        if (u == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        u.Nom = request.Couleur.Nom;
        u.Code = request.Couleur.Code;
        u.Description = request.Couleur.Description;
        this.Db.Update<Couleur>(u);
      }

      return new CouleurEditResponse() { Couleur = u };
    }
  }
}
