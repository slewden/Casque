using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.ClientEdit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'un client
  /// </summary>
  public class ClientEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie le client demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(ClientEditRequest request)
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

      ClientEditResponse rep = new ClientEditResponse();
      rep.Client = this.Db.Select<ClientView>(x => x.Cle == request.Cle).FirstOrDefault();
      return rep;
    }

    /// <summary>
    /// Delete : Supprime le client demandé
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(ClientEditRequest request)
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
        this.Db.Delete<ClientFournisseur>(x => x.Cle == request.Cle);
      }
      catch
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer");
      }

      return null;
    }

    /// <summary>
    /// Post : Création ou Modification d'un client
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(ClientEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Client == null || !request.Client.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'client' non valide ou incomplet");
      }

      ClientView u;
      if (request.Cle <= 0)
      { // insertion
        this.Db.Insert<ClientFournisseur>(request.Client.ToClientFournisseur());
        request.Client.Cle = (int)this.Db.GetLastInsertId();
        u = request.Client;
      }
      else
      {
        ClientFournisseur c = this.Db.Select<ClientFournisseur>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }

        c.Nom = request.Client.Nom;
        c.AdresseCommande = request.Client.AdresseCommande;
        c.AdresseLivraison = request.Client.AdresseLivraison;
        c.Email = request.Client.Email;
        c.SujetEmail = request.Client.SujetEmail;
        this.Db.Update<ClientFournisseur>(c);

        u = this.Db.Select<ClientView>(x => x.Cle == request.Cle).FirstOrDefault();
        if (c == null)
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }
      }

      return new ClientEditResponse() { Client = u };
    }
  }
}
