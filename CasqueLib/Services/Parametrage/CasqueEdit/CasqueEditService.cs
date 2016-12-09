using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.CasqueEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'un casque
  /// </summary>
  public class CasqueEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie le casque demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CasqueEditRequest request)
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

      CasqueEditResponse rep = new CasqueEditResponse();
      rep.Casque = this.Db.Select<CasqueView>(x => x.Cle == request.Cle).FirstOrDefault();
      if (request.ModeRead)
      {
        rep.Pieces = this.Db.SqlList<CasqueConstitueView>("[dbo].[casque_piece_liste] @casqId", new { casqId = request.Cle });
        foreach (var p in rep.Pieces)
        {
          if (p.TailleCle > 0)
          {
            p.Taille = this.Db.Select<TailleTypePiece>(x => x.Cle == p.TailleCle).FirstOrDefault();
          }

          if (p.CouleurCle > 0)
          {
            p.Couleur = this.Db.Select<CouleurTypePiece>(x => x.Cle == p.CouleurCle).FirstOrDefault(); 
          }
        }
      }

      return rep;
    }

    /// <summary>
    /// Delete : Supprime le casque demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(CasqueEditRequest request)
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

        // on load le casque pour supprimer le fichier s'il y a lieu
        Casque c = this.Db.Select<Casque>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c != null)
        {
          string photo = string.Empty;
          if (!string.IsNullOrWhiteSpace(c.Photo))
          {
            photo = c.Photo; 
          }

          try
          {
            this.Db.ExecuteNonQuery("dbo.casque_delete @casqId", new { casqId = request.Cle });
            if (!string.IsNullOrWhiteSpace(photo))
            { // on efface la photo que ssi le delete a fonctionné !
              CasqueEditPhotoService.DeletePhoto(photo);
            }
          }
          catch
          {
            return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
          }
        }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'un casque
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(CasqueEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Casque == null || !request.Casque.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'fournisseur' non valide ou incomplet");
      }

      CasqueView u;
      if (request.Cle <= 0)
      { // insertion (la photo a été uploadée avant !)
        this.Db.Insert<Casque>(request.Casque.ToCasque());
        request.Casque.Cle = (int)this.Db.GetLastInsertId();
        u = request.Casque;
      }
      else
      {
        Casque c = this.Db.Select<Casque>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        string deletePhoto = string.Empty;
        if (!string.IsNullOrWhiteSpace(c.Photo) && !string.IsNullOrWhiteSpace(request.Casque.Photo) && c.Photo != request.Casque.Photo)
        { // y avait une photo, y a de nouveau une photo, c'est pas les mêmes noms ==> effacer l'ancienne photo (la nouvelle photo est déjà uploadée !)
          deletePhoto = c.Photo;
        }

        c.Nom = request.Casque.Nom;
        c.Code = request.Casque.Code;
        c.Description = request.Casque.Description;
        c.Photo = request.Casque.Photo;
        try
        {
          this.Db.Update<Casque>(c);
          if (!string.IsNullOrWhiteSpace(deletePhoto))
          {
            CasqueEditPhotoService.DeletePhoto(deletePhoto);
          }
        }
        catch
        {
          return new HttpError(HttpStatusCode.BadRequest, "Impossible de mettre à jour le casque");
        }

        u = this.Db.Select<CasqueView>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }
      }

      return new CasqueEditResponse() { Casque = u };
    }
  }
}
