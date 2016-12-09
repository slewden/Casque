using System.Linq;
using System.Net;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.FournisseurPiece
{
  /// <summary>
  /// Service pour la gestion des types de pièces fournies par un fournisseur
  /// </summary>
  public class FournisseurPieceService : FsService
  {
    /// <summary>
    /// Get : Renvoie les types de pièces fournies par un fournisseur
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(FournisseurPieceRequest request)
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

      FournisseurPieceResponse rep = new FournisseurPieceResponse();
      rep.Fournisseur = this.Db.Select<FournisseurView>(x => x.Cle == request.Cle).FirstOrDefault();
      rep.Pieces = this.Db.SqlList<FournisseurPieceView>("[dbo].[fournisseur_piece_liste] @clfoId", new { clfoId = request.Cle });
      return rep;
    }

    /// <summary>
    /// Post : Synchronisation des pièces fournies par un fournisseur
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(FournisseurPieceRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Cle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'clé' non valide");
      }

      string sql = FournisseurPieceView.GetSqlSynchronise(request.Cle, request.Pieces);
      if (!string.IsNullOrWhiteSpace(sql))
      {
        this.Db.ExecuteNonQuery(sql);
      }

      return null;
    }
  }
}
