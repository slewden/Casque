using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Administration.PosteEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'un carton
  /// </summary>
  public class PosteEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie le poste demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(PosteEditRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      PosteEditResponse rep = new PosteEditResponse();
      if (request.Cle > 0)
      {
        rep.Poste = this.Db.Select<Poste>(x => x.Cle == request.Cle).FirstOrDefault();
      }
      else
      {
        rep.Poste = new Poste();
      }

      rep.Config = new Matos.Lecteur.SimpleReaderParameters(rep.Poste.ConfigurationTxt);
      rep.PosteTypes = Poste.ListPosteTypes();
      rep.Affectations = Poste.ListAffectations();
      return rep;
    }

    /// <summary>
    /// Delete : Supprime le poste demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(PosteEditRequest request)
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
        this.Db.Delete<Poste>(x => x.Cle == request.Cle);
      }
      catch
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
      }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'un poste
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(PosteEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Poste == null || !request.Poste.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'poste' non valide ou incomplet");
      }

      if (request.Config != null && request.Config.IsValid.Valid)
      { // la config manuelle est valide elle remplace la config Texte
        request.Poste.ConfigurationTxt = request.Config.ToString();
      }

      Poste u;
      if (request.Cle <= 0)
      { // insertion
        this.Db.Insert<Poste>(request.Poste);
        request.Poste.Cle = (int)this.Db.GetLastInsertId();
        u = request.Poste;
      }
      else
      {
        u = this.Db.Select<Poste>(x => x.Cle == request.Cle).FirstOrDefault();
        if (u == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        u.Nom = request.Poste.Nom;
        u.PosteType = request.Poste.PosteType;
        u.Description = request.Poste.Description;
        u.PageCode = request.Poste.PageCode;
        u.ConfigurationTxt = request.Poste.ConfigurationTxt;
        this.Db.Update<Poste>(u);
      }

      return new PosteEditResponse() { Poste = u };
    }
  }
}
