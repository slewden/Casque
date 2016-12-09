using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Administration.UtilisateurEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'un utilisateur
  /// </summary>
  public class UtilisateurEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie l'utilisateur demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(UtilisateurEditRequest request)
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

      UtilisateurEditResponse rep = new UtilisateurEditResponse();
      rep.Utilisateur = this.Db.Select<Utilisateur>(x => x.Cle == request.Cle).FirstOrDefault();
      return rep;
    }

    /// <summary>
    /// Delete : Supprime l'utilisateur demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(UtilisateurEditRequest request)
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
        this.Db.Delete<Utilisateur>(x => x.Cle == request.Cle);
      }
      catch
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
      }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'un utilisateur
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(UtilisateurEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Utilisateur == null || !request.Utilisateur.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'utilisateur' non valide ou incomplet");
      }

      Utilisateur u;
      if (request.Cle <= 0)
      { // insertion
        this.Db.Insert<Utilisateur>(request.Utilisateur);
        request.Utilisateur.Cle = (int)this.Db.GetLastInsertId();
        u = request.Utilisateur;
      }
      else
      {
        u = this.Db.Select<Utilisateur>(x => x.Cle == request.Cle).FirstOrDefault();
        if (u == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        u.Nom = request.Utilisateur.Nom;
        u.Login = request.Utilisateur.Login;
        u.Password = request.Utilisateur.Password;
        u.Actif = request.Utilisateur.Actif;
        u.Email = request.Utilisateur.Email;
        this.Db.Update<Utilisateur>(u);
      }

      return new UtilisateurEditResponse() { Utilisateur = u };
    }
  }
}
