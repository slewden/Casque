using System.Collections.Generic;
using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.TypePieceEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'un Type de piece
  /// </summary>
  public class TypePieceEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie le type de piece demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(TypePieceEditRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      TypePieceEditResponse rep = new TypePieceEditResponse();
      if (request.Cle <= 0)
      {
        rep.TypePiece = new TypePieceView();
        rep.TypePiece.CleTailles = new List<int>();
        rep.TypePiece.CleCouleurs = new List<int>();
      }
      else
      {
        rep.TypePiece = this.Db.Select<TypePieceView>(x => x.Cle == request.Cle).FirstOrDefault();
        rep.TypePiece.CleTailles = this.Db.Select<TypePieceTaille>(x => x.TypePieceCle == request.Cle).Select(x => x.TailleCle).ToList();
        rep.TypePiece.CleCouleurs = this.Db.Select<TypePieceCouleur>(x => x.TypePieceCle == request.Cle).Select(x => x.CouleurCle).ToList();
      }

      rep.Couleurs = this.Db.Select<Couleur>().OrderBy(x => x.Code).ToList();
      rep.Tailles = this.Db.Select<Taille>().OrderBy(x => x.Ordre).ToList();
      return rep;
    }

    /// <summary>
    /// Delete : Supprime le type de pièce demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(TypePieceEditRequest request)
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
      TypePiece c = this.Db.Select<TypePiece>(x => x.Cle == request.Cle).FirstOrDefault();
      if (c != null)
      {
        string photo = string.Empty;
        if (!string.IsNullOrWhiteSpace(c.Photo))
        {
          photo = c.Photo;
        }

        try
        {
          this.Db.ExecuteNonQuery("dbo.type_piece_delete @typiId", new { typiId = request.Cle });
          if (!string.IsNullOrWhiteSpace(photo))
          { // on ne supprime la photo que si tout est ok
            TypePieceEditPhotoService.DeletePhoto(photo);
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
    /// Post : Création ou Modification d'un type de pièce
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(TypePieceEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.TypePiece == null || !request.TypePiece.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'fournisseur' non valide ou incomplet");
      }

      TypePieceView u;
      if (request.Cle <= 0)
      { // insertion (la photo a été uploadée avant !)
        this.Db.Insert<TypePiece>(request.TypePiece.ToTypePiece());
        request.TypePiece.Cle = (int)this.Db.GetLastInsertId();
        u = request.TypePiece;
      }
      else
      {
        TypePiece c = this.Db.Select<TypePiece>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        string photo = string.Empty;
        if (!string.IsNullOrWhiteSpace(c.Photo) && !string.IsNullOrWhiteSpace(request.TypePiece.Photo) && c.Photo != request.TypePiece.Photo)
        { // y avait une photo, y a de nouveau une photo, c'est pas les mêmes noms ==> effacer l'ancienne photo (la nouvelle photo est déjà uploadée !)
          photo = c.Photo;
        }

        c.Nom = request.TypePiece.Nom;
        c.Code = request.TypePiece.Code;
        c.AvecTag = request.TypePiece.AvecTag;
        c.Description = request.TypePiece.Description;
        c.Photo = request.TypePiece.Photo;
        try
        {
          this.Db.Update<TypePiece>(c);
          if (!string.IsNullOrWhiteSpace(photo))
          { // on fait la photo après au cas ou 
            TypePieceEditPhotoService.DeletePhoto(photo);
          }
        }
        catch
        {
          return new HttpError(HttpStatusCode.BadRequest,  "Mise à jour Impossible");
        }

        u = this.Db.Select<TypePieceView>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }
      }

      // Ci dessous : prendre u.Cle et pas request.Cle : Sinon ca marche pas en insert !
      // Synchronisation des couleurs 
      string sql = TypePieceCouleur.GetSqlSynchronise(u.Cle, request.TypePiece.CleCouleurs);
      if (!string.IsNullOrWhiteSpace(sql))
      {
        this.Db.ExecuteNonQuery(sql);
      }

      // Synchronisation des tailles
      sql = TypePieceTaille.GetSqlSynchronise(u.Cle, request.TypePiece.CleTailles);
      if (!string.IsNullOrWhiteSpace(sql))
      {
        this.Db.ExecuteNonQuery(sql);
      }

      return new TypePieceEditResponse() { TypePiece = u };
    }
  }
}
