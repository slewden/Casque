using System.Data;
using System.Linq;
using System.Net;
using CasqueLib.Buisness.View;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Livraison.Detail
{
  /// <summary>
  /// Service pour le détail d'une livraison
  /// </summary>
  public class LivraisonDetailService : FsService
  {
    /// <summary>
    /// Traite un envoie par email des infos de la livraison au client
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <param name="livraisonCle">la cle de la livraison</param>
    /// <param name="emailSuplementaire">Adresse email complémentaire</param>
    public static void EnvoieLivraisonParEmail(IDbConnection db, int livraisonCle, string emailSuplementaire)
    {
      CasqueLib.Email.Livraison mail = new Email.Livraison();
      string msg = mail.Load(db, livraisonCle);
      if (string.IsNullOrWhiteSpace(msg))
      {
        msg = mail.Send(emailSuplementaire);
        if (!string.IsNullOrWhiteSpace(msg))
        {
          throw new HttpError(HttpStatusCode.BadRequest, msg);
        }
      }
      else
      {
        throw new HttpError(HttpStatusCode.BadRequest, msg);
      }
    }

    /// <summary>
    /// Get : Renvoie la livraison demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(LivraisonDetailRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'Clé' Non valide");
      }

      LivraisonDetailResponse rep = new LivraisonDetailResponse();
      rep.Livraison = this.Db.Select<LivraisonView>(x => x.Cle == request.Cle).FirstOrDefault();
      if (rep.Livraison != null)
      {
        rep.FactoriseCartons(this.Db.Select<CartonLivreView>(x => x.LivraisonCle == request.Cle).OrderBy(x => x.CartonIndex));
        
        if (rep.Livraison.ClientCle <= 0)
        { // Livraison incomplète : proposer la liste des clients
          rep.Clients = this.Db.Select<ClientView>().OrderBy(x => x.Nom).ToList();
        }
        
        return rep;
      }
      else
      {
        return new HttpError(HttpStatusCode.BadRequest, "'Clé' inValide");
      }
    }
    
    /// <summary>
    /// Post : envoie un email au client
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(LivraisonDetailRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'Clé' Non valide");
      }

      LivraisonDetailService.EnvoieLivraisonParEmail(this.Db, request.Cle, request.EmailSuplementaire);
      return null;
    }
  }
}
