using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.FournisseurEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'un fournisseur
  /// </summary>
  public class FournisseurEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie le fournisseur demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(FournisseurEditRequest request)
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

      FournisseurEditResponse rep = new FournisseurEditResponse();
      rep.Fournisseur = this.Db.Select<FournisseurView>(x => x.Cle == request.Cle).FirstOrDefault();
      if (request.ModeRead)
      {
        rep.Pieces = this.Db.SqlList<FournisseurPieceView>("[dbo].[fournisseur_piece_liste] @clfoId", new { clfoId = request.Cle });
      }

      return rep;
    }

    /// <summary>
    /// Delete : Supprime le fournisseur demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(FournisseurEditRequest request)
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
        this.Db.Delete<Buisness.FournisseurPiece>(x => x.FournisseurCle == request.Cle);
        this.Db.Delete<ClientFournisseur>(x => x.Cle == request.Cle);
      }
      catch
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
      }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'un fournisseur
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(FournisseurEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Fournisseur == null || !request.Fournisseur.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'fournisseur' non valide ou incomplet");
      }

      FournisseurView u;
      if (request.Cle <= 0)
      { // insertion
        this.Db.Insert<ClientFournisseur>(request.Fournisseur.ToClientFournisseur());
        request.Fournisseur.Cle = (int)this.Db.GetLastInsertId();
        u = request.Fournisseur;
      }
      else
      {
        ClientFournisseur c = this.Db.Select<ClientFournisseur>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        c.Nom = request.Fournisseur.Nom;
        c.AdresseCommande = request.Fournisseur.AdresseCommande;
        c.AdresseLivraison = request.Fournisseur.AdresseLivraison;
        c.Email = request.Fournisseur.Email;
        c.SujetEmail = request.Fournisseur.SujetEmail;
        this.Db.Update<ClientFournisseur>(c);

        u = this.Db.Select<FournisseurView>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }
      }

      return new FournisseurEditResponse() { Fournisseur = u };
    }
  }
}
