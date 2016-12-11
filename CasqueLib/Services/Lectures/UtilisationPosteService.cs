using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Analyse;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using CasqueLib.Services.Livraison.Detail;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Service pour la gestion des phases de lectures de tags
  /// Démarrage de session de lectures, arrêts, liste des lecteurs possibles, info à l'arrivée d'un tag, enregistrement d'une action (fonction du pageCode)
  /// </summary>
  public class UtilisationPosteService : FsService
  {
    /// <summary>
    /// Get : Renvoie la liste des postes utilisable à l'instant T pour la page code demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(UtilisationPosteRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (string.IsNullOrWhiteSpace(request.PageCode))
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'page Code' Non valide");
      }

      UtilisationPosteResponse rep = new UtilisationPosteResponse();
      List<NomCle> nb = this.Db.SqlList<NomCle>("EXEC dbo.action_autorise @pageCode", new { PageCode = request.PageCode });
      if (nb != null && nb.Select(x => x.Cle).Min() == 0)
      {
        switch (request.PageCode)
        {
          case "reception":
          case "assemblage":
          case "livraison":
          case "consultation":
            rep.MessageBloquant = nb.Where(x => x.Cle == 0).Select(x => x.Nom).Aggregate((x, y) => x + ".<br/> " + y);
            break;
        }
      }
      else
      {
        rep.MessageBloquant = string.Empty;
      }

      rep.Postes = this.Db.SqlList<Poste>(
        "EXEC dbo.poste_disponible_liste @pageCode, @utilisationPosteCle",
        new { pageCode = request.PageCode, utilisationPosteCle = request.UtilisationPosteCle });

      if (request.PageCode == "livraison")
      {
        rep.Cartons = this.Db.Select<Carton>();
      }

      return rep;
    }

    /// <summary>
    /// Get : Renvoie la "commande associée" à un tag fourni
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(AnalyseRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Tags == null || !request.Tags.Any())
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Tags' Non valide");
      }

      var valid = request.Tags.Where(x => !string.IsNullOrWhiteSpace(x));
      if (valid == null || !valid.Any())
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Tags' Non valide");
      }

      if (string.IsNullOrWhiteSpace(request.PageCode))
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'PageCode' Non valide");
      }

      // Fonction du pageCode on renvoie les infos
      switch (request.PageCode)
      {
        case "reception":
          return this.ProcessGetReception(request);
        case "assemblage":
          return this.ProcessGetAssemblage(request);
        case "livraison":
          return this.ProcessGetLivraison(request);
        case "consultation":
          return this.ProcessGetConsultation(request);
      }

      // Si on est ici c'est qu'il y a un mauvais pageCode
      return new HttpError(System.Net.HttpStatusCode.BadRequest, "'PageCode' Non valide");
    }

    /// <summary>
    /// Renvoie la liste des utilisation poste en cours
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(LecteurEnCoursRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (string.IsNullOrWhiteSpace(request.PageCode))
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'PageCode' Non valide");
      }

      switch (request.PageCode.ToLower())
      {
        case "reception":
        case "assemblage":  //--- ici on a Assemblage et AssemblagePrint !
        case "livraison":
        case "consultation":
          break;
        default:
          return new HttpError(System.Net.HttpStatusCode.BadRequest, "'PageCode' Non valide");
      }

      LecteurEnCoursResponse rep = new LecteurEnCoursResponse();
      rep.LecteursEnCours = this.Db.Select<UtilisationPosteEnCoursView>();
 
      // Fonction du pageCode on renvoie les infos
      switch (request.PageCode.ToLower())
      {
        case "reception":
          rep.NoReaderFound = !rep.LecteursEnCours.Where(x => x.PageCode == "Reception").Any();
          break;
        case "assemblage":  //--- ici on a Assemblage et AssemblagePrint !
          rep.NoReaderFound = !rep.LecteursEnCours.Where(x => x.PageCode.StartsWith("Assemblage")).Any();
          break;
        case "livraison":
          rep.NoReaderFound = !rep.LecteursEnCours.Where(x => x.PageCode == "Livraison").Any();
          break;
        case "consultation":
          rep.NoReaderFound = !rep.LecteursEnCours.Where(x => x.PageCode == "Consultation").Any();
          break;
      }

      rep.Postes = this.Db.SqlList<Poste>("EXEC dbo.poste_disponible_liste @page_code", new { PageCode = request.PageCode });
      return rep;
    }

    /// <summary>
    /// Put : Crée l'utilisation de poste et renvoie les infos
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Put(UtilisationPosteRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      ////if (!Enum.IsDefined(typeof(Poste.EPosteType), request.TypePosteInt))
      ////{
      ////  return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Type de poste' Non valide");
      ////}

      if (request.PosteCle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Poste' Non valide");
      }

      if (request.UtilisateurCle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Utilisateur' Non valide");
      }

      int cle = this.CreateUtilisationPoste(request.UtilisateurCle, request.PosteCle);

      UtilisationPosteResponse rep = new UtilisationPosteResponse();
      ////rep.Postes = this.Db.SqlList<Poste>("EXEC dbo.poste_disponible_liste @pageCode", new { PageCode = request.TypePosteInt });
      rep.UtilisationPosteCle = cle;

      return rep;
    }

    /// <summary>
    /// Post : Complète la l'opération en cours sur l'utilisation poste avec la liste des tags reçus
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(AnalyseRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.UtilisateurCle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Clé user' Non valide");
      }

      if (request.UtilisationPosteCle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Clé utilisation poste' Non valide");
      }

      if (request.Cle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Clé d''objet' Non valide");
      }

      if (request.Lectures == null || !request.Lectures.Any())
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'lectures' Non valide");
      }

      if (string.IsNullOrWhiteSpace(request.PageCode))
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'PageCode' Non valide");
      }

      // fait le boulot :
      if (!this.CloseUtilisationPoste(request.UtilisationPosteCle, request.UtilisateurCle, request.Lectures.Select(x => new Lecture(x))))
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Clé utilisation poste' incorrecte");
      }

      // renvoie la nouvelle utilisation poste clé
      UtilisationPosteResponse rep = new UtilisationPosteResponse();
      switch (request.PageCode.ToLower())
      {
        case "reception":
          this.SaveEntreeStock(request.UtilisateurCle, request.Lectures);
          break;
        case "assemblage":
          rep.AssemblageCle = this.SaveAssemblage(request.UtilisateurCle, request.Cle, request.Lectures);
          rep.Postes = this.Db.SqlList<Poste>("EXEC dbo.poste_disponible_liste @page_code", new { PageCode = "AssemblagePrint" });
          break;
        case "livraison":
          int laCle = this.SaveLivraison(request.UtilisateurCle, request.LivraisonCle, request.Cle, request.CartonIndex, request.Lectures);
          rep.Livraison = this.Db.Select<LivraisonView>(x => x.Cle == laCle).FirstOrDefault();
          rep.Clients = this.Db.Select<ClientView>().OrderBy(x => x.Nom).ToList();

          MailConfig cfg = MailConfig.Get(this.Db);
          rep.ConfigEmailOk = cfg.IsComplet();
          break;
        case "consultation":
          //// Rien à faire ici
          break;
        default:
          return new HttpError(System.Net.HttpStatusCode.BadRequest, "'PageCode' Non valide");
      }

      rep.UtilisationPosteCle = this.CreateUtilisationPoste(request.UtilisateurCle, request.PosteCle);
      return rep;
    }

    /// <summary>
    /// Post : Termine la livraison
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(FinaliseLivraisonRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.LivraisonCle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Clé de la livraison' Non valide");
      }

      if (request.ClientCle <= 0)
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Clé du client' Non valide");
      }

      // fait le taf
      try
      {
        this.Db.ExecuteNonQuery("EXEC dbo.livraison_termine @livrId, @clfoId", new { livrId = request.LivraisonCle, clfoId = request.ClientCle });

        if (request.ProcessEnvoie)
        { // Envoyer l'email : idem LivraisonDetailService.Post(LivraisonDetailRequest)
          LivraisonDetailService.EnvoieLivraisonParEmail(this.Db, request.LivraisonCle, string.Empty);
        }
      }
      catch (Exception ex)
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de terminer la livraison : " + ex.Message);
      }

      // renvoie la nouvelle utilisation poste clé
      FinaliseLivraisonResponse rep = new FinaliseLivraisonResponse();
      return rep;
    }

    /// <summary>
    /// Delete : Supprimer l'utilisation de poste 
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(UtilisationPosteRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (string.IsNullOrWhiteSpace(request.PageCode))
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Page code' Non valide");
      }

      if (request.UtilisationPosteCle > 0)
      {
        // suppression de l'utilisation Poste
        try
        {
          this.Db.Delete<Lecture>(l => l.UtilisationPosteCle == request.UtilisationPosteCle);
          this.Db.Delete<UtilisationPoste>(u => u.Cle == request.UtilisationPosteCle);
        }
        catch (Exception ex)
        {
          return new HttpError(System.Net.HttpStatusCode.InternalServerError, ex.Message);
        }
      }

      // renvoie la liste des postes possibles
      UtilisationPosteResponse rep = new UtilisationPosteResponse();
      rep.Postes = this.Db.SqlList<Poste>("EXEC dbo.poste_disponible_liste @page_code", new { PageCode = request.PageCode });
      return rep;
    }

    /// <summary>
    /// Delete : Supprimer l'assemblage en cours de constitution
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(AssemblageDeleteRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (string.IsNullOrWhiteSpace(request.PageCode))
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Page code' Vide Non valide");
      }

      if (request.Cle > 0)
      {
        switch (request.PageCode)
        {
          case "assemblage": // suppression de l'assemblage en cours
            return this.AssemblageDelete(request);
          case "livraison":
            return this.LivraisonDelete(request);
        }

        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'Page code' Non valide");
      }
      else
      {
        return new HttpError(System.Net.HttpStatusCode.BadRequest, "'AssemblageCle' vide : Non valide");
      }
    }

    #region Méthodes utiles
    /// <summary>
    /// Calcule la liste des tag inconnus
    /// </summary>
    /// <param name="request">Le tag lu à analyser</param>
    /// <param name="res">Les tags trouvés</param>
    /// <returns>La liste différence des 2</returns>
    private static List<string> ComputeTagInconnus(AnalyseRequest request, IEnumerable<string> res)
    {
      if (res != null && res.Any())
      {
        if (request.Tags.Count > res.Count())
        { // Il y a des tags envoyés non reconnus
          return request.Tags.Except(res.Select(x => x.Trim())).ToList();
        }
        else
        { // y en a pas
          return new List<string>();
        }
      }

      // il le sont tous
      return request.Tags;
    }

    /// <summary>
    /// Renvoie le SQL d'appel à la procédure
    /// </summary>
    /// <param name="procedureName">Nom de la procédure</param>
    /// <param name="request">les infos de paramètre</param>
    /// <returns>Le sql</returns>
    private static string GetAppelProcedure(string procedureName, AnalyseRequest request)
    {
      var valid = request.Tags.Where(x => !string.IsNullOrWhiteSpace(x));

      // construction de l'appel à la procédure get_tags_infos_XXX
      StringBuilder insert = new StringBuilder();
      insert.AppendLine("DECLARE @tbl AS [dbo].[ListTag];");
      insert.AppendLine(" INSERT INTO @tbl (numero) VALUES ");
      insert.Append(valid.Select(x => string.Format(" ({0})", SqlFormat.String(x.Trim()))).Aggregate((x, y) => x + ", " + y));
      insert.AppendLine(";");
      insert.AppendFormat(" EXEC {0} @tbl;", procedureName);
      return insert.ToString();
    }

    /// <summary>
    /// Termine une utilisation poste
    /// </summary>
    /// <param name="utilisationPosteCle">La clé de l'utilisation poste</param>
    /// <param name="utilisateurCle">La clé de l'utilisateur</param>
    /// <param name="lectures">la liste des lectures</param>
    /// <returns>true si ok false si problème</returns>
    private bool CloseUtilisationPoste(int utilisationPosteCle, int utilisateurCle, IEnumerable<Lecture> lectures)
    {
      // Load de l'utilisation
      UtilisationPoste ut = this.Db.Select<UtilisationPoste>(x => x.Cle == utilisationPosteCle).FirstOrDefault();
      if (ut == null)
      {
        return false;
      }

      ut.UtilisateurCle = utilisateurCle;
      ut.Fin = DateTime.Now;
      this.Db.UpdateOnly(ut, x => new { x.Fin }, u => u.Cle == utilisationPosteCle);
      if (lectures != null && lectures.Any())
      {
        foreach (Lecture l in lectures)
        {
          l.UtilisationPosteCle = ut.Cle;
          l.UtilisateurCle = ut.UtilisateurCle;
          this.Db.Insert<Lecture>(l);
        }
      }

      return true;
    }

    /// <summary>
    /// Met à jour les entrées de stock pour les étiquettes concernées
    /// </summary>
    /// <param name="utilisateurCle">L'utilisateur qui fait</param>
    /// <param name="lectures">Les lectures</param>
    private void SaveEntreeStock(int utilisateurCle, List<string> lectures)
    {
      DateTime dt = DateTime.Now;
      int cle;
      foreach (string numero in lectures)
      {
        Etiquette etq = this.Db.Select<Etiquette>(x => x.Numero == numero).FirstOrDefault();
        cle = etq.Cle;
        etq.EntreeStock = dt;
        etq.EntreeStockUtilisateurCle = utilisateurCle;
        this.Db.UpdateOnly(etq, x => new { x.EntreeStock, x.EntreeStockUtilisateurCle }, u => u.Cle == cle);
      }
    }

    /// <summary>
    /// Crée l'assemblage demandé
    /// </summary>
    /// <param name="utilisateurCle">La clé de l'utilisateur qui fait l'action</param>
    /// <param name="casqueCle">La clé du casque choisi</param>
    /// <param name="lectures">La liste des N° De tags</param>
    /// <returns>La clé de l'assemblage créé</returns>
    private int SaveAssemblage(int utilisateurCle, int casqueCle, List<string> lectures)
    {
      var valid = lectures.Where(x => !string.IsNullOrWhiteSpace(x));

      if (valid != null && valid.Any())
      {
        // construction de l'appel à la procédure assemblage_cree
        StringBuilder insert = new StringBuilder();
        insert.AppendFormat(" DECLARE @utilId INT = {0}; ", SqlFormat.ForeignKey(utilisateurCle));
        insert.AppendFormat(" DECLARE @casqId INT = {0}; ", SqlFormat.ForeignKey(casqueCle));
        insert.AppendLine(" DECLARE @tbl AS [dbo].[ListTag];");
        insert.AppendLine(" INSERT INTO @tbl (numero) VALUES ");
        insert.Append(lectures.Select(x => string.Format(" ({0})", SqlFormat.String(x))).Aggregate((x, y) => x + ", " + y));
        insert.AppendLine(";");
        insert.AppendFormat(" EXEC dbo.assemblage_cree @utilId, @casqId, @tbl;");

        CleOnly assembalge = this.Db.SqlList<CleOnly>(insert.ToString()).FirstOrDefault();
        if (assembalge != null)
        {
          return assembalge.Cle;
        }
      }

      // Problème
      return 0;
    }

    /// <summary>
    /// Sauvegarde les pièces de la livraison dans le carton index
    /// </summary>
    /// <param name="utilisateurCle">La clé de l'utilisateur qui fait l'action</param>
    /// <param name="livrId">La clé de la livraison en cours de traitement</param>
    /// <param name="cartonId">La clé du carton</param>
    /// <param name="cartonIndex">L'index du carton dans la commande en cours de traitement</param>
    /// <param name="lectures">Les lectures</param>
    /// <returns>La clé de la livraison</returns>
    private int SaveLivraison(int utilisateurCle, int livrId, int cartonId, int cartonIndex, List<string> lectures)
    {
      if (livrId <= 0)
      { // Creation de la livraison
        // construction de l'appel à la procédure get_tags_infos_XXX
        StringBuilder insert = new StringBuilder();
        insert.AppendFormat(" DECLARE @utilId INT = {0}; ", SqlFormat.ForeignKey(utilisateurCle));
        insert.AppendFormat(" DECLARE @cartId INT = {0}; ", SqlFormat.ForeignKey(cartonId));
        insert.AppendFormat(" DECLARE @cartonIndex SMALLINT = {0}; ", cartonIndex);
        insert.AppendLine(" DECLARE @tbl AS [dbo].[ListIntTable];");
        insert.AppendLine(" INSERT INTO @tbl (id) VALUES ");
        insert.Append(lectures.Select(x => string.Format(" ({0})", SqlFormat.ForeignKey(x.Trim()))).Aggregate((x, y) => x + ", " + y));
        insert.AppendLine(";");
        insert.AppendLine(" EXEC [dbo].[livraison_cree] @utilId, @cartId, @cartonIndex, @tbl;");
        CleOnly livraison = this.Db.SqlList<CleOnly>(insert.ToString()).FirstOrDefault();
        if (livraison != null)
        { // pas de problème
          return livraison.Cle;
        }
      }
      else
      { // ajout d'un carton à la livraison
        StringBuilder insert = new StringBuilder();
        insert.AppendFormat(" DECLARE @utilId INT = {0}; ", SqlFormat.ForeignKey(utilisateurCle));
        insert.AppendFormat(" DECLARE @livrId INT = {0}; ", SqlFormat.ForeignKey(livrId));
        insert.AppendFormat(" DECLARE @cartId INT = {0}; ", SqlFormat.ForeignKey(cartonId));
        insert.AppendFormat(" DECLARE @cartonIndex SMALLINT = {0}; ", cartonIndex);
        insert.AppendLine(" DECLARE @tbl AS [dbo].[ListIntTable];");
        insert.AppendLine(" INSERT INTO @tbl (id) VALUES ");
        insert.Append(lectures.Select(x => string.Format(" ({0})", SqlFormat.ForeignKey(x.Trim()))).Aggregate((x, y) => x + ", " + y));
        insert.AppendLine(";");
        insert.AppendLine(" EXEC [dbo].[livraison_complete] @utilId, @livrId, @cartId, @cartonIndex, @tbl;");
        this.Db.ExecuteNonQuery(insert.ToString());
      }

      return livrId;
    }

    /// <summary>
    /// Création de l'utilisation
    /// </summary>
    /// <param name="utilisateurCle">La clé de l'utilisateur</param>
    /// <param name="posteCle">La cle du poste</param>
    /// <returns>La clé de l'utilisation poste crée</returns>
    private int CreateUtilisationPoste(int utilisateurCle, int posteCle)
    {
      UtilisationPoste ut = new UtilisationPoste()
      {
        Creation = DateTime.Now,
        UtilisateurCle = utilisateurCle,
        PosteCle = posteCle,
      };
      this.Db.Insert<UtilisationPoste>(ut);
      ut.Cle = (int)this.Db.GetLastInsertId();
      return ut.Cle;
    }
    #endregion

    #region Get Info en fonction du poste de lecture
    /// <summary>
    /// Renvoie les infos sur les tags lus dans le contexte d'une consultation
    /// c'est à dire qu'on renvoie l'historique de chaque tag lu
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La réponse</returns>
    private object ProcessGetConsultation(AnalyseRequest request)
    {
      string sql = UtilisationPosteService.GetAppelProcedure("[dbo].[get_tags_infos_consultation]", request);
      List<EtiquetteHistorique> nfos = this.Db.SqlList<EtiquetteHistorique>(sql);
      AnalyseResponse rep = new AnalyseResponse();
      rep.UtilisationPosteCle = request.UtilisationPosteCle;
      rep.TagInconnus = UtilisationPosteService.ComputeTagInconnus(request, nfos.Select(x => x.Numero));

      rep.TagConnus = nfos.Select(x => new ConsultationEtiquette()
            {
              Numero = x.Numero.Trim(),
              Etiquette = x.EtiquetteCle > 0 ? this.Db.Select<EtiquetteView>(y => y.Cle == x.EtiquetteCle).FirstOrDefault() : null,
              Commande = x.CommandeCle > 0 ? this.Db.Select<CommandeView>(y => y.Cle == x.CommandeCle).FirstOrDefault() : null,
              Reference = x.Reference,
              ////TypePiece = x.TypePieceCle > 0 ? this.Db.Select<TypePieceView>(y => y.Cle == x.TypePieceCle).FirstOrDefault() : null,
              ////Casque = x.CasqueCle > 0 ? this.Db.Select<CasqueView>(y => y.Cle == x.CasqueCle).FirstOrDefault() : null,
              Assemblage = x.AssemblageCle > 0 ? this.Db.Select<AssemblageView>(y => y.Cle == x.AssemblageCle).FirstOrDefault() : null,
              Livraison = x.LivraisonCle > 0 ? this.Db.Select<LivraisonView>(y => y.Cle == x.LivraisonCle).FirstOrDefault() : null,
              ////Client = x.ClientCle > 0 ? this.Db.Select<ClientView>(y => y.Cle == x.ClientCle).FirstOrDefault() : null,
            }).ToList();
      foreach (var t in rep.TagConnus)
      {
        t.Compute();
      }

      return rep;
    }
    
    /// <summary>
    /// Renvoie les infos sur les tags lus dans le contexte d'une réception de produits
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>la réponse</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Vérifier si les requêtes SQL présentent des failles de sécurité", Justification = "Anti-injection Vérifié")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "pas trouvé")]
    private object ProcessGetReception(AnalyseRequest request)
    {
      string sql = UtilisationPosteService.GetAppelProcedure("[dbo].[get_tags_infos_reception]", request);
      List<ReadBddTagInfo> res = null;
      List<ReadBddCommandePiece> lst = null;
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = sql;
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<ReadBddTagInfo>();                       // Table 1 : infos sur les tags
          reader.NextResult();
          lst = reader.CustomConvertToList<ReadBddCommandePiece>();                 // Table 2 : les pièces de la commande (s'il y a lieu)
          reader.NextResult();
        }
      });

      AnalyseResponse rep = new AnalyseResponse();
      rep.UtilisationPosteCle = request.UtilisationPosteCle;
      rep.TagInconnus = UtilisationPosteService.ComputeTagInconnus(request, res.Select(x => x.Numero));

      if (res != null && res.Any() && lst != null && lst.Any())
      { // y a des tags connus, et des infos de commande
        rep.Commandes = new List<DetailCommande>();
        var cmds = res.Where(x => x.CommandeCle > 0).Select(x => new { CommandeCle = x.CommandeCle, CommandeNumero = x.CommandeNumero, CommandeClientNom = x.CommandeClientNom });

        DetailCommande cmd;
        foreach (var t in cmds.Distinct())
        {
          cmd = new DetailCommande(t.CommandeCle, t.CommandeNumero, t.CommandeClientNom);
          cmd.Pieces = (from x in lst
                        where x.CommandeCle == t.CommandeCle
                        group x by new
                        {
                          Reference = x.Reference,
                          TypePieceCle = x.TypePieceCle,
                          TypePieceNom = x.TypePieceNom,
                          TypePieceCode = x.TypePieceCode,
                          TypePiecePhoto = x.TypePiecePhoto,
                          TailleCle = x.TailleCle,
                          TailleNom = x.TailleNom,
                          TailleCode = x.TailleCode,
                          CouleurCle = x.CouleurCle,
                          CouleurNom = x.CouleurNom,
                          CouleurCode = x.CouleurCode
                        }
                          into grp
                          select new DetailCommandePiece()
                          {
                            Reference = grp.Key.Reference,
                            TypePieceCle = grp.Key.TypePieceCle,
                            TypePieceNom = grp.Key.TypePieceNom,
                            TypePieceCode = grp.Key.TypePieceCode,
                            TypePiecePhoto = grp.Key.TypePiecePhoto,
                            TailleCle = grp.Key.TailleCle,
                            TailleNom = grp.Key.TailleNom,
                            TailleCode = grp.Key.TailleCode,
                            CouleurCle = grp.Key.CouleurCle,
                            CouleurNom = grp.Key.CouleurNom,
                            CouleurCode = grp.Key.CouleurCode,
                            Tags = grp.Select(x => new DetailCommandeTagLu() { Numero = x.Numero.Trim(), StatutInt = x.OperationInt }).ToList()
                          }).ToList();
          rep.Commandes.Add(cmd);
        }
      }

      return rep;
    }

    /// <summary>
    /// Renvoie les infos sur les tag lus dans le contexte d'un assemblage de pièces
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>la réponse</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Vérifier si les requêtes SQL présentent des failles de sécurité", Justification = "Anti-injection Vérifié")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "pas trouvé")]
    private object ProcessGetAssemblage(AnalyseRequest request)
    {
      string sql = GetAppelProcedure("[dbo].[get_tags_infos_assemblage]", request);
      List<ReadBddTagInfo> res = null;
      List<Casque> casques = null;
      List<ReadBddCasqueConstitue> pieces = null;
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = sql;
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<ReadBddTagInfo>();                       // Table 1 : infos sur les tags
          reader.NextResult();
          casques = reader.CustomConvertToList<Casque>();                           // Table 2 : les casques possibles trouvés
          reader.NextResult();
          pieces = reader.CustomConvertToList<ReadBddCasqueConstitue>();            // Table 3 : les pièces du casque avec leur complétude
          reader.NextResult();
        }
      });

      AnalyseResponse rep = new AnalyseResponse();
      rep.UtilisationPosteCle = request.UtilisationPosteCle;
      rep.TagInconnus = UtilisationPosteService.ComputeTagInconnus(request, res.Select(x => x.Numero));
      int nombreTagOkFournis = request.Tags.Count - rep.TagInconnus.Count;
      rep.Casques = casques.Select(x => new CasquePossible(nombreTagOkFournis)
      {
        Cle = x.Cle,
        Nom = x.Nom,
        Code = x.Code,
        Description = x.Description,
        Photo = x.Photo,
        Pieces = (from p in pieces
                  where p.CasqueCle == x.Cle
                  group p by new
                  {
                    CasqueCle = p.CasqueCle,
                    TypePieceCle = p.TypePieceCle,
                    TypePieceNom = p.TypePieceNom,
                    TypePieceDescription = p.TypePieceDescription,
                    TypePieceCode = p.TypePieceCode,
                    TypePiecePhoto = p.TypePiecePhoto,
                    CouleurCle = p.CouleurCle,
                    CouleurNom = p.CouleurNom,
                    CouleurDescription = p.CouleurDescription,
                    CouleurCode = p.CouleurCode,
                    TailleCle = p.TailleCle,
                    TailleNom = p.TailleNom,
                    TailleDescription = p.TailleDescription,
                    TailleCode = p.TailleCode,
                  }
                    into grp
                    select new CasquePossibleConstitue()
                    {
                      CasqueCle = grp.Key.CasqueCle,
                      TypePieceCle = grp.Key.TypePieceCle,
                      TypePieceNom = grp.Key.TypePieceNom,
                      TypePieceDescription = grp.Key.TypePieceDescription,
                      TypePieceCode = grp.Key.TypePieceCode,
                      TypePiecePhoto = grp.Key.TypePiecePhoto,
                      CouleurCle = grp.Key.CouleurCle,
                      CouleurNom = grp.Key.CouleurNom,
                      CouleurDescription = grp.Key.CouleurDescription,
                      CouleurCode = grp.Key.CouleurCode,
                      TailleCle = grp.Key.TailleCle,
                      TailleNom = grp.Key.TailleNom,
                      TailleDescription = grp.Key.TailleDescription,
                      TailleCode = grp.Key.TailleCode,
                      Tags = pieces.Where(pp => pp.CasqueCle == grp.Key.CasqueCle
                                             && pp.TypePieceCle == grp.Key.TypePieceCle
                                             && pp.CouleurCle == grp.Key.CouleurCle
                                             && pp.TailleCle == grp.Key.TailleCle
                                             && pp.EtiquetteCle > 0)
                                   .Select(zz => new CasquePossibleConstitueEtiquette()
                                   {
                                     EtiquetteCle = zz.EtiquetteCle,
                                     Numero = (zz.Numero ?? string.Empty).Trim(),
                                     StatutInt = zz.OperationInt,
                                   }).ToList()
                    }).ToList()
      }).ToList();
      return rep;
    }

    /// <summary>
    /// Renvoie les infos sur les tag lus dans le contexte de livraison
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>la réponse</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Vérifier si les requêtes SQL présentent des failles de sécurité", Justification = "Anti-injection Vérifié")]
    private object ProcessGetLivraison(AnalyseRequest request)
    {
      string sql = UtilisationPosteService.GetAppelProcedure("[dbo].[get_tags_infos_livraison]", request);
      List<ReadBddTagInfo> res = null;
      List<TechniqueAssemblage> assemblages = null;
      List<AssemblageLivrableDetail> details = null;
      this.Db.Exec(cmd =>
      {
        cmd.CommandText = sql;
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<ReadBddTagInfo>();                       // Table 1 : infos sur les tags
          reader.NextResult();
          assemblages = reader.CustomConvertToList<TechniqueAssemblage>();          // Table 2 : les assemblages trouvés
          reader.NextResult();
          details = reader.CustomConvertToList<AssemblageLivrableDetail>();         // Table 3 : les détails des assemblages trouvés
          reader.NextResult();
        }
      });
      AnalyseResponse rep = new AnalyseResponse();
      rep.UtilisationPosteCle = request.UtilisationPosteCle;
      rep.TagInconnus = UtilisationPosteService.ComputeTagInconnus(request, res.Select(x => x.Numero));
      rep.Cartons = this.Db.Select<Carton>();
      rep.Assemblages = assemblages
                         .Select(a =>
                         {
                           a.Pieces = details.Where(x => x.AssemblageCle == a.Cle)
                             .Select(x =>
                           {
                             x.CreateTags();
                             return x;
                           }).ToList();
                           return a;
                         }).ToList();

      return rep;
    }
    #endregion

    #region Deletes
    /// <summary>
    /// Supprime l'assemblage en cours
    /// </summary>
    /// <param name="request">La demande</param>
    /// <returns>La réponses</returns>
    private object AssemblageDelete(AssemblageDeleteRequest request)
    {
      try
      {
        CleOnly cle = this.Db.SqlList<CleOnly>("EXEC dbo.assemblage_non_valide_delete @asseId", new { asseId = request.Cle }).FirstOrDefault();
        if (cle != null && cle.Cle > 0)
        { // c'est Ok 
          // renvoie la liste des postes possibles
          AssemblageDeleteResponse rep = new AssemblageDeleteResponse();
          return rep;
        }
        else
        {
          return new HttpError(System.Net.HttpStatusCode.BadRequest, "'AssemblageCle' Non valide");
        }
      }
      catch (Exception ex)
      {
        return new HttpError(System.Net.HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    /// <summary>
    /// Supprime l'assemblage en cours
    /// </summary>
    /// <param name="request">La demande</param>
    /// <returns>La réponses</returns>
    private object LivraisonDelete(AssemblageDeleteRequest request)
    {
      try
      {
        this.Db.ExecuteNonQuery("EXEC dbo.livraison_non_valide_delete @livrId", new { livrId = request.Cle });
        AssemblageDeleteResponse rep = new AssemblageDeleteResponse();
        return rep;
      }
      catch (Exception ex)
      {
        return new HttpError(System.Net.HttpStatusCode.InternalServerError, ex.Message);
      }
    }
    #endregion
  }
}
