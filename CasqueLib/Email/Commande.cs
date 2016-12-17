using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack.OrmLite;

namespace CasqueLib.Email
{
  /// <summary>
  /// Classe pour gérer la composition et l'envoi d'un email de commande
  /// </summary>
  public class Commande : EmailComposer
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Commande"/>
    /// </summary>
    public Commande() :
      base(Folder.EFolder.Commande)
    { 
    }

    /// <summary>
    /// La commande
    /// </summary>
    public CommandeView LaCommande { get; private set; }

    /// <summary>
    /// Le contenu du template d'une ligne de commande
    /// </summary>
    public string TemplateCommandeLigne { get; private set; }
    
    /// <summary>
    /// Load l'objet et indique si tout est ok
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <param name="cle">La clé de la commande</param>
    /// <returns>Le message d'erreur ou vide si tout est OK</returns>
    public override string Load(IDbConnection db, int cle)
    {
      // La config
      this.Config = MailConfig.Get(db);
      if (this.Config == null)
      {
        return "Aucune configuration des emails trouvée";
      }

      // La commande
      this.LaCommande = db.Select<CommandeView>(x => x.Cle == cle).FirstOrDefault();
      if (this.LaCommande == null)
      {
        return "Aucune commande trouvée";
      }
      else if (this.LaCommande.EnvoieEmail == null)
      {
        return string.Format("La commande {0} ({1}) n'est pas valide pour l'envoie par email !", this.LaCommande.Numero, this.LaCommande.Cle);
      }

      // sujet
      this.Sujet = this.Config.SubjetFournisseur;
      if (!string.IsNullOrWhiteSpace(this.LaCommande.FournisseurSujetEmail))
      { // Le sujet est surchargé par le sujet contenu dans la fiche du fournisseur
        this.Sujet = this.LaCommande.FournisseurSujetEmail;
      }

      if (string.IsNullOrWhiteSpace(this.Sujet))
      {
        return "Le sujet des emails pour les commandes est vide";
      }

      // le destinataire
      if (!string.IsNullOrWhiteSpace(this.LaCommande.FournisseurEmail))
      { // on laisse passer un destinataire vide : s'il n'y en a pas on n'envéra pas d'email !
        if (!MailConfig.IsValidEmail(this.LaCommande.FournisseurEmail))
        {
          return "L'adresse email du fournisseur n'est pas valide !";
        }
      }

      this.DestinataireEmail = this.LaCommande.FournisseurEmail;

      // Les pièces de la commande
      this.LaCommande.Pieces = db.SqlList<CommandeLigneView>("EXEC dbo.commande_ligne_liste @comdId", new { comdId = cle });
      if (!this.LaCommande.Pieces.Any())
      {
        return string.Format("La commande {0} ({1}) n'a pas de pièces ce n'est pas normal !", this.LaCommande.Numero, this.LaCommande.Cle);
      }

      // remplir les N° d'étiquettes
      foreach (CommandeLigneView lg in this.LaCommande.Pieces.Where(x => x.Quantite > 0))
      {
        lg.Etiquettes = db.SqlList<NomCle>("EXEC dbo.commande_ligne_etiquette @colgId", new { colgId = lg.Cle }).Select(x => x.Nom).ToList();
      }

      // Le template de la commande
      this.Template = this.ExtractTemplate(Folder.FullPath("/PartialsEmail/Commande.html"));
      if (string.IsNullOrWhiteSpace(this.Template))
      {
        return "Le template d'email global est vide. Ce n'est pas normal !";
      }

      // Le template des pièces de la commande
      this.TemplateCommandeLigne = this.ExtractTemplate(Folder.FullPath("/PartialsEmail/CommandeLigne.html"));
      if (string.IsNullOrWhiteSpace(this.TemplateCommandeLigne))
      {
        return "Le template d'email des pièces est vide. Ce n'est pas normal !";
      }

      // Pas de soucis ça cruise !!
      return string.Empty;
    }

    /// <summary>
    /// Load l'objet pour génération de la pièce jointe uniquement
    /// (pas de config email loadée
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <param name="cmd">La commande</param>
    /// <returns>le nombre de pièces</returns>
    public int Load(IDbConnection db, CommandeView cmd)
    {
      this.LaCommande = cmd;

      // a ce stade la commande est incomplète on complète
      this.LaCommande.Pieces = db.SqlList<CommandeLigneView>("EXEC dbo.commande_ligne_liste @comdId", new { comdId = this.LaCommande.Cle });
      if (this.LaCommande.Pieces.Any())
      { // y a des pièce on complète les infos
        foreach (CommandeLigneView lg in this.LaCommande.Pieces.Where(x => x.Quantite > 0))
        { // remplir les N° d'étiquettes
          lg.Etiquettes = db.SqlList<NomCle>("EXEC dbo.commande_ligne_etiquette @colgId", new { colgId = lg.Cle }).Select(x => x.Nom).ToList();
        }
      }

      return this.LaCommande != null && this.LaCommande.Pieces != null ? this.LaCommande.Pieces.Count : 0;
    }

    /// <summary>
    /// Génère le fichier en picèe jointe
    /// A la sortie de cette procédure si un fichier doit être joint la propertie "PieceJointe" contient le fullPath du fichier
    /// Sinon si aucun fichier n'est à joindre la propertie "PieceJointe" doit être vide 
    /// </summary>
    public override void GenerePieceJointe()
    {
      // on n'oublie pas de générer le nom de la pièce jointe
      this.PieceJointeName = string.Format("Commande-{0}-{1}.xlsx", this.LaCommande.Numero, Guid.NewGuid());

      base.GenerePieceJointe();
    }

    /// <summary>
    /// Renvoie les données pour la génération du fichier Excel en Pièce jointe à la commande
    /// </summary>
    /// <returns>les données</returns>
    protected override byte[] GetPieceJointeData()
    {
      byte[] result;
      using (var package = new ExcelPackage())
      {
        using (ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Etiquettes"))
        {
          // première ligne
          worksheet.Cells[1, 1].Value = "Référence Lot";
          worksheet.Cells[1, 2].Value = "Etiquette";
          worksheet.Cells[1, 3].Value = "Pièce";
          worksheet.Cells[1, 4].Value = "Ref. Pièce";
          worksheet.Cells[1, 5].Value = "Couleur";
          worksheet.Cells[1, 6].Value = "Taille";
          using (var range = worksheet.Cells[1, 1, 1, 6])
          {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(119, 119, 119));
            range.Style.Font.Color.SetColor(Color.FromArgb(229, 229, 229));
          }

          // les suivantes
          int row = 2;
          foreach (CommandeLigneView cmdLigne in this.LaCommande.Pieces.Where(x => x.Quantite > 0))
          {
            if (cmdLigne.Etiquettes != null)
            {
              foreach (string etiquette in cmdLigne.Etiquettes)
              {
                worksheet.Cells[row, 1].Value = cmdLigne.Reference;
                worksheet.Cells[row, 2].Value = etiquette;
                worksheet.Cells[row, 3].Value = cmdLigne.TypePieceNom;
                worksheet.Cells[row, 4].Value = cmdLigne.TypePieceCode;
                worksheet.Cells[row, 5].Value = cmdLigne.CouleurNom;
                worksheet.Cells[row, 6].Value = cmdLigne.TailleNom;
                row++;
              }
            }
          }

          // Create an autofilter for the range
          if (row > 1)
          {
            worksheet.Cells["A1:E" + (row - 1).ToString()].AutoFilter = true;
          }

          // Autofit columns for all cells
          worksheet.Cells.AutoFitColumns(0);

          package.Workbook.Properties.Title = string.Format("Détail des étiquettes de la commande {0}", this.LaCommande.Numero);
          package.Workbook.Properties.Author = "Tracking Center";
          package.Workbook.Properties.Company = "Tracking Center";

          result = package.GetAsByteArray();
        }
      }

      return result;
    }

    /// <summary>
    /// Remplace dans Text tous les markers par les infos de la commande
    /// </summary>
    /// <param name="texte">Le texte de départ (contient des markeurs)</param>
    /// <param name="isSujet">Indique si on traite le sujet ou le contenu</param>
    /// <returns>le texte mergé</returns>
    protected override string Replace(string texte, bool isSujet)
    {
      string sujet = texte;
      sujet = sujet.Replace("##FournisseurNom##", this.LaCommande.FournisseurNom);
      sujet = sujet.Replace("##FournisseurAdresseCommande##", this.LaCommande.FournisseurAdresseCommandeHtml);
      sujet = sujet.Replace("##EnvoieEmail##", this.LaCommande.EnvoieEmail.Value.ToString("dd/MM/yyyy hh:mm"));
      sujet = sujet.Replace("##Numero##", this.LaCommande.Numero);
      sujet = sujet.Replace("##Saisie##", this.LaCommande.Saisie.ToString("dd/MM/yyyy hh:mm"));
      sujet = sujet.Replace("##StatutNom##", this.LaCommande.StatutNom);
      sujet = sujet.Replace("##NombreProduit##", this.LaCommande.NombreProduit.ToString());
      sujet = sujet.Replace("##NombreProduitEtiquette##", this.LaCommande.NombreProduitEtiquette.ToString());
      sujet = sujet.Replace("##MontantCommande##", this.LaCommande.MontantCommande.ToString("N2") + " €");
      
      if (!isSujet)
      { // c'est le contenu : un remplacement supplémentaire pour les pièces
        StringBuilder txtPieces = new StringBuilder();
        foreach (CommandeLigneView piece in this.LaCommande.Pieces.Where(x => x.Quantite > 0))
        {
          txtPieces.Append(this.Replace(this.TemplateCommandeLigne, piece));
        }

        sujet = sujet.Replace("##CommandeLignes##", txtPieces.ToString());
      }
      
      return sujet;
    }

    /// <summary>
    /// Remplace dans Text tous les markers par les infos de la pièce concernée
    /// </summary>
    /// <param name="text">Le template</param>
    /// <param name="piece">La pièce</param>
    /// <returns>Le texte remplacé</returns>
    private string Replace(string text, CommandeLigneView piece)
    {
      string leTexte = text;
      leTexte = leTexte.Replace("##Piece.CouleurNom##", piece.CouleurNom);
      leTexte = leTexte.Replace("##Piece.CouleurCode##", piece.CouleurCode);
      if (piece.Frais.HasValue && piece.Frais.Value != 0)
      {
        leTexte = leTexte.Replace("##Piece.Frais##", piece.Frais.Value.ToString("N2") + " €");
      }
      else
      {
        leTexte = leTexte.Replace("##Piece.Frais##", string.Empty);
      }

      if (piece.PrixUnitaire.HasValue && piece.PrixUnitaire.Value != 0)
      {
        leTexte = leTexte.Replace("##Piece.PrixUnitaire##", piece.PrixUnitaire.Value.ToString("N2") + " €");
      }
      else
      {
        leTexte = leTexte.Replace("##Piece.PrixUnitaire##", string.Empty);
      }

      leTexte = leTexte.Replace("##Piece.Quantite##", piece.Quantite.ToString());
      leTexte = leTexte.Replace("##Piece.TailleNom##", piece.TailleNom);
      leTexte = leTexte.Replace("##Piece.TailleCode##", piece.TailleCode);
      leTexte = leTexte.Replace("##Piece.TypePieceAvecTag##", piece.TypePieceAvecTag ? "Oui" : "Non");
      leTexte = leTexte.Replace("##Piece.TypePieceNom##", piece.TypePieceNom);
      leTexte = leTexte.Replace("##Piece.TypePieceCode##", piece.TypePieceCode);
      leTexte = leTexte.Replace("##Piece.Reference##", piece.Reference);
      return leTexte;
    }
  }
}
