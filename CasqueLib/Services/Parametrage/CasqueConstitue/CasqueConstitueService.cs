using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Joins;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Parametrage.CasqueConstitue
{
  /// <summary>
  /// Service pour la gestion des types de pièces qui constituent un casque
  /// </summary>
  public class CasqueConstitueService : FsService
  {
    /// <summary>
    /// Get : Renvoie les types de pièces qui constituent un casque
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CasqueConstitueRequest request)
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

      CasqueConstitueResponse rep = new CasqueConstitueResponse();
      rep.Casque = this.Db.Select<CasqueView>(x => x.Cle == request.Cle).FirstOrDefault();
      List<CleOnly> cles = null;
      this.Db.Exec(cmd => 
                      { 
                        cmd.CommandText = "[dbo].[casque_piece_liste] @casqId";
                        cmd.Parameters.Add(new SqlParameter("casqId", request.Cle));
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                          rep.Pieces = reader.CustomConvertToList<CasqueConstitueView>();
                          reader.NextResult();
                          cles = reader.CustomConvertToList<CleOnly>();
                        }
                      });

      if (cles != null && cles.Any())
      {
        rep.NombreAssemblage = cles.FirstOrDefault().Cle;
      }
      else
      {
        rep.NombreAssemblage = 0;
      }

      foreach (var p in rep.Pieces)
      { // pour les pièces faisant partie du casque on pré remplit
        if (p.CasqueCle == request.Cle)
        {
          CasqueConstitueInfoRequest q = new CasqueConstitueInfoRequest() { Cle = p.Cle, ApiKey = request.ApiKey, ModeRead = request.ModeRead };
          CasqueConstitueInfoResponse r = (CasqueConstitueInfoResponse)this.Get(q);
          if (r != null)
          {
            p.Tailles = r.Tailles;
            p.Couleurs = r.Couleurs;
            p.Taille = p.Tailles.Where(x => x.Cle == p.TailleCle).FirstOrDefault();
            p.Couleur = p.Couleurs.Where(x => x.Cle == p.CouleurCle).FirstOrDefault();

            p.Tailles.Insert(0, TailleTypePiece.Empty);
            p.Couleurs.Insert(0, CouleurTypePiece.Empty);
          }
        }
      }

      return rep;
    }

    /// <summary>
    /// Get : Renvoie les infos de couleurs et taille pour une pièce choisie
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CasqueConstitueInfoRequest request)
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

      CasqueConstitueInfoResponse rep = new CasqueConstitueInfoResponse();
      rep.Tailles = this.Db.Select<TailleTypePiece>(x => x.TypePieceCle == request.Cle);
      rep.Couleurs = this.Db.Select<CouleurTypePiece>(x => x.TypePieceCle == request.Cle);
      return rep;
    }

    /// <summary>
    /// Post : Synchronisation des pièces qui constituent le casque
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(CasqueConstitueRequest request)
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

      if (!string.IsNullOrWhiteSpace(request.Nom))
      { // mise à jour du nom
        Casque c = this.Db.Select<Casque>(x => x.Cle == request.Cle).FirstOrDefault();
        c.Nom = request.Nom;
        this.Db.UpdateOnly(c, x => new { x.Nom }, u => u.Cle == c.Cle);
      }

      string sql = CasqueConstitueView.GetSqlSynchronise(request.Cle, request.Pieces);
      if (!string.IsNullOrWhiteSpace(sql))
      {
        this.Db.ExecuteNonQuery(sql);
      }

      return null;
    }
  }
}
