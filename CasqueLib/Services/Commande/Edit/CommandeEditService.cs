using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace CasqueLib.Services.Commande.Edit
{
  /// <summary>
  /// Service pour la gestion du CRUD d'une commande
  /// </summary>
  public class CommandeEditService : FsService
  {
    /// <summary>
    /// Get : Renvoie la commande demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Get(CommandeEditRequest request)
    {
      HttpError err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      CommandeEditResponse rep = new CommandeEditResponse();
      if (request.Cle <= 0)
      {
        rep.Commande = new CommandeView();
      }
      else
      {
        rep.Commande = this.Db.Select<CommandeView>(x => x.Cle == request.Cle).FirstOrDefault();
        if (rep.Commande != null)
        {
          rep.Commande.Pieces = this.Db.SqlList<CommandeLigneView>("EXEC dbo.commande_ligne_liste @comdId, @read", new { comdId = request.Cle, read = request.ModeRead ? 1 : 0 });
          foreach (var cl in rep.Commande.Pieces)
          {
            if (cl.CouleurNombre > 0)
            {
              cl.Couleurs = this.Db.SqlList<NomCle>("EXEC dbo.type_piece_couleur_possible @typiId", new { typiId = cl.TypePieceCle });
            }

            if (cl.TailleNombre > 0)
            {
              cl.Tailles = this.Db.SqlList<NomCle>("EXEC dbo.type_piece_taille_possible @typiId", new { typiId = cl.TypePieceCle });
            }
          }
        }
        else
        {
          return new HttpError(HttpStatusCode.BadRequest, "'Clé' non valide");
        }
      }

      if (request.Excel)
      { // renvoie le flux excel
        Email.Commande cmd = new Email.Commande();
        if (cmd.Load(this.Db, rep.Commande) > 0)
        { // la commande à des pièces ==> on peut générer le fichier joint
          cmd.GenerePieceJointe();
          rep.ExcelFileUrl = cmd.PieceJointeUrl;
        }
      }
      else if (!request.ModeRead)
      { // on est pas en lecture ==> chargement des infos d'édition
        if (rep.Commande != null && rep.Commande.Validation != null && rep.Commande.ImpressionFinie == null)
        { // dans ce cas faut fournir la liste des postes possibles
          rep.Postes = this.Db.SqlList<Poste>("EXEC dbo.poste_disponible_liste @page_code", new { PageCode = "commande" });
        }

        if (rep.Commande != null && rep.Commande.Validation != null && rep.Commande.ImpressionFinie != null)
        { // on en est à un stade ou l'on va envoyer un email ssi la config est ok
          MailConfig config = MailConfig.Get(this.Db);
          rep.ConfigEmailOk = config.IsComplet();
        }

        rep.Fournisseurs = this.Db.SqlList<NomCle>("EXEC dbo.fournisseur_liste_select");

        Configuration cfg = Configuration.Get(this.Db, EConfiguration.NombreMaxiQuantite);
        int nb = 5000000;
        if (!int.TryParse(cfg.Valeur, out nb))
        {
          nb = 500000;
        }

        rep.CommandeLigneQuantiteMax = nb;
      }

      return rep;
    }

    /// <summary>
    /// Delete : Supprime la commande demandée
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Delete(CommandeEditRequest request)
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
        this.Db.ExecuteNonQuery("EXEC dbo.commande_delete @comdId", new { comdId = request.Cle });
      }
      catch (Exception ex)
      {
        return new HttpError(HttpStatusCode.BadRequest, "Impossible de supprimer la commande : " + ex.Message);
      }

      return null;
    }

    /// <summary>
    /// Crée une nouvelle commande
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Put(CommandeEditRequest request)
    {
      var err = this.Verification(request);
      if (err != null)
      {
        return err;
      }

      if (request.Commande == null)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'commande' non valide ou incomplète");
      }

      if (request.Commande.FournisseurCle <= 0 || string.IsNullOrWhiteSpace(request.Commande.Numero) || request.Commande.UtilisateurCle <= 0)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'commande' non valide ou incomplète");
      }

      // crée la commande
      CasqueLib.Buisness.Commande cmd = new Buisness.Commande();
      cmd.Numero = request.Commande.Numero;
      cmd.FournisseurCle = request.Commande.FournisseurCle;
      cmd.UtilisateurCle = request.Commande.UtilisateurCle;
      cmd.Saisie = request.Commande.Saisie;

      // met le tout en bdd
      try
      {
        this.Db.Insert<CasqueLib.Buisness.Commande>(cmd);
        request.Cle = (int)this.Db.GetLastInsertId();
      }
      catch (Exception ex)
      {
        return new HttpError(HttpStatusCode.InternalServerError, "Erreur lors de l'insertion de la commande : " + ex.Message);
      }

      // renvoie la commande insérée
      return this.Get(request);
    }

    /// <summary>
    /// Post : Création ou Modification d'une commande
    /// </summary>
    /// <param name="request">la demande</param>
    /// <returns>La reponse</returns>
    public object Post(CommandeEditRequest request)
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

      if (request.Commande == null || !request.Commande.IsComplet())
      {
        return new HttpError(HttpStatusCode.BadRequest, "'commande' non valide ou incomplète");
      }

      Buisness.Commande cmd = this.Db.Select<Buisness.Commande>(x => x.Cle == request.Cle).FirstOrDefault();
      if (cmd == null)
      {
        return new HttpError(HttpStatusCode.BadRequest, "'clé' non valide");
      }

      if (cmd.Validation == null)
      { // on peut encore faire bouger des infos de la commande
        DateTime? dt = request.Validation ? DateTime.Now : (DateTime?)null;
        if (cmd.Numero != request.Commande.Numero || cmd.Validation != dt || cmd.DelaiSemaine != request.Commande.DelaiSemaine)
        { // y a besoin de mettre à jour la commande
          cmd.Numero = request.Commande.Numero;
          cmd.Validation = dt;
          cmd.DelaiSemaine = request.Commande.DelaiSemaine;
          this.Db.UpdateOnly(cmd, x => new { x.Numero, x.Validation, x.DelaiSemaine }, u => u.Cle == cmd.Cle);
        }

        // on peut aussi faire bouger les lignes
        int index = 1;
        foreach (var lg in request.Commande.Pieces)
        {
          if (lg.Cle > 0 && lg.Quantite == 0)
          { // Suppressionn de la ligne
            this.Db.Delete<Buisness.CommandeLigne>(x => x.Cle == lg.Cle);
          }
          else if (lg.Quantite > 0)
          {
            if (lg.Cle <= 0)
            { // insertion d'une nouvelle ligne
              this.InsertCommandeLigne(cmd, lg, index);
            }
            else
            { // vérifier s'il faut mettre à jour
              Buisness.CommandeLigne ligne = this.Db.Select<Buisness.CommandeLigne>(x => x.Cle == lg.Cle).FirstOrDefault();
              if (ligne == null)
              { // problème ? ==> On inserre
                this.InsertCommandeLigne(cmd, lg, index);
              }
              else if (ligne.TailleCle != lg.TailleCle || ligne.CouleurCle != lg.CouleurCle || ligne.Quantite != lg.Quantite || ligne.PrixUnitaire != lg.PrixUnitaire || ligne.Frais != lg.Frais)
              { // y a eu un changement : on update
                ligne.TailleCle = lg.TailleCle;
                ligne.CouleurCle = lg.CouleurCle;
                ligne.Quantite = lg.Quantite;
                ligne.PrixUnitaire = lg.PrixUnitaire;
                ligne.Frais = lg.Frais;
                this.Db.UpdateOnly(ligne, x => new { x.TailleCle, x.CouleurCle, x.Quantite, x.PrixUnitaire, x.Frais }, u => u.Cle == ligne.Cle);
              }
            }
          }

          index++;
        }
      }
      else if (cmd.ImpressionFinie != null && cmd.EnvoieEmail == null)
      { // l'impression est fini et la commande n'a pas été envoyée au fournisseur
        if (request.EnvoieEmail)
        {
          cmd.EnvoieEmail = DateTime.Now;
          this.Db.UpdateOnly(cmd, x => new { x.EnvoieEmail }, u => u.Cle == cmd.Cle);

          if (request.ProcessEnvoie)
          { // Envoyer par email la commande demandé
            this.EnvoieCommandeParEmail(cmd, request.EmailSuplementaire);
          }
        }
      }
      else if (cmd.EnvoieEmail != null)
      { // la commande est déjà partie au fournisseur
        if (request.EnvoieEmail && request.ProcessEnvoie)
        { // demande de re-envoie du email de la commande
          this.EnvoieCommandeParEmail(cmd, request.EmailSuplementaire);
        }

        if ((cmd.Acquittee == null && request.Acquittee) || (cmd.Acquittee != null && !request.Acquittee))
        { // La commande est envoyée au fournisseur ==> demande d'aquittement change
          cmd.Acquittee = request.Acquittee ? DateTime.Now : (DateTime?)null;
          this.Db.UpdateOnly(cmd, x => new { x.Acquittee }, u => u.Cle == cmd.Cle);
        }
      }

      return this.Get(request);
    }

    /// <summary>
    /// Traite un envoie par email des infos de la commande au fournisseur
    /// </summary>
    /// <param name="cmd">la commande</param>
    /// <param name="emailSuplementaire">Adresse email complémentaire</param>
    private void EnvoieCommandeParEmail(Buisness.Commande cmd, string emailSuplementaire)
    {
      CasqueLib.Email.Commande mail = new Email.Commande();
      string msg = mail.Load(this.Db, cmd.Cle);
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
    /// Insère une ligne de commande en BDD
    /// </summary>
    /// <param name="commande">la commande</param>
    /// <param name="lg">les infos sur la ligne</param>
    /// <param name="index">la position de la ligne dans la commande</param>
    private void InsertCommandeLigne(Buisness.Commande commande, CommandeLigneView lg, int index)
    {
      Buisness.CommandeLigne ligne = new Buisness.CommandeLigne();
      ligne.CommandeCle = commande.Cle;
      ligne.CouleurCle = lg.CouleurCle <= 0 ? (int?)null : lg.CouleurCle;
      ligne.Frais = lg.Frais;
      ligne.PrixUnitaire = lg.PrixUnitaire;
      ligne.Quantite = lg.Quantite;
      ligne.TailleCle = lg.TailleCle <= 0 ? (int?)null : lg.TailleCle;
      ligne.TypePieceCle = lg.TypePieceCle;

      ligne.Reference = string.Format("{0:yyyy-MM}/{1:000000}/{2:000}", commande.Saisie, commande.Cle, index);

      this.Db.Insert<Buisness.CommandeLigne>(ligne);
    }
  }
}
