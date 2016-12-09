using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using CasqueLib.Services.Livraison.Detail;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack.OrmLite;

namespace CasqueLib.Email
{
  /// <summary>
  /// Classe pour gérer la composition et l'envoi d'un email de livraison
  /// </summary>
  public class Livraison : EmailComposer
  {
    /// <summary>
    /// Les infos de la livraison
    /// </summary>
    public LivraisonDetailResponse Reponse { get; private set; }

    /// <summary>
    /// Le template d'iterration pour la première ligne d'un carton
    /// </summary>
    public string TemplateCarton1 { get; private set; }

    /// <summary>
    /// Le template d'iterration pour les lignes suivant la permière d'un carton
    /// </summary>
    public string TemplateCartonN { get; private set; }

    /// <summary>
    /// Le template d'iterration pour les etiquette d'une casque
    /// </summary>
    public string TemplateCasqueEtiquette { get; private set; }

    /// <summary>
    /// Charge l'objet
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <param name="cle">La clé de la livraison</param>
    /// <returns>Le message d'erreur. Vide sinon</returns>
    public override string Load(System.Data.IDbConnection db, int cle)
    {
      // La config
      this.Config = MailConfig.Get(db);
      if (this.Config == null)
      {
        return "Aucune configuration des emails trouvée";
      }

      // La livraison
      this.Reponse = new LivraisonDetailResponse();
      this.Reponse.Livraison = db.Select<LivraisonView>(x => x.Cle == cle).FirstOrDefault();
      if (this.Reponse.Livraison == null)
      {
        return "Aucune livraison trouvée";
      }

      if (this.Reponse.Livraison.ClientCle <= 0)
      { // Livraison incomplète
        return "Livraison incomplète. Précisez le client avant d'envoyer un email";
      }

      // Le sujet
      this.Sujet = this.Config.SubjetClient;
      if (!string.IsNullOrWhiteSpace(this.Reponse.Livraison.ClientSujectEmail))
      { // Le sujet est surchargé par le sujet contenu dans la fiche du client
        this.Sujet = this.Reponse.Livraison.ClientSujectEmail;
      }

      if (string.IsNullOrWhiteSpace(this.Sujet))
      {
        return "Le sujet des emails pour les livraisons est vide";
      }

      // le destinataire
      if (!string.IsNullOrWhiteSpace(this.Reponse.Livraison.ClientEmail))
      { // si pas d'email pas d'envoi, mais c'est pas bloquant
        if (!MailConfig.IsValidEmail(this.Reponse.Livraison.ClientEmail))
        {
          return "L'adresse email du client n'est pas valide !";
        }
      }

      this.DestinataireEmail = this.Reponse.Livraison.ClientEmail;

      // détail des cartons
      this.Reponse.FactoriseCartons(db.Select<CartonLivreView>(x => x.LivraisonCle == cle).OrderBy(x => x.CartonIndex));

      // Le template du bon de livraison
      this.Template = this.ExtractTemplate(Folder.FullPath("/PartialsEmail/Livraison.html"));
      if (string.IsNullOrWhiteSpace(this.Template))
      {
        return "Le template d'email global est vide. Ce n'est pas normal !";
      }

      // Le template d'iterration casque
      this.TemplateCarton1 = this.ExtractTemplate(Folder.FullPath("/PartialsEmail/LivraisonCarton1.html"));
      if (string.IsNullOrWhiteSpace(this.TemplateCarton1))
      {
        return "Le template d'email carton est vide. Ce n'est pas normal !";
      }

      // Le template d'iterration casque
      this.TemplateCartonN = this.ExtractTemplate(Folder.FullPath("/PartialsEmail/LivraisonCartonN.html"));
      if (string.IsNullOrWhiteSpace(this.TemplateCartonN))
      {
        return "Le template d'email carton2 est vide. Ce n'est pas normal !";
      }

      // Le template d'iterration casque
      this.TemplateCasqueEtiquette = this.ExtractTemplate(Folder.FullPath("/PartialsEmail/LivraisonCasqueEtiquette.html"));
      if (string.IsNullOrWhiteSpace(this.TemplateCasqueEtiquette))
      {
        return "Le template d'email casque est vide. Ce n'est pas normal !";
      }

      // Pas de soucis ça cruise !!
      return string.Empty;
    }

    /// <summary>
    /// Génère le fichier en picèe jointe
    /// A la sortie de cette procédure si un fichier doit être joint la propertie "PieceJointe" contient le fullPath du fichier
    /// Sinon si aucun fichier n'est à joindre la propertie "PieceJointe" doit être vide 
    /// </summary>
    protected override void GenerePieceJointe()
    {
      string fileName = Folder.FullPath(Folder.EFolder.Livraison, string.Format("Livraison-{0}-{1}.xlsx", this.Reponse.Livraison.Reference, Guid.NewGuid()));

      byte[] result;
      using (var package = new ExcelPackage())
      {
        using (ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Etiquettes"))
        {
          // première ligne
          worksheet.Cells[1, 1].Value = "Etiquette";
          worksheet.Cells[1, 2].Value = "Index";
          worksheet.Cells[1, 3].Value = "Casque";
          worksheet.Cells[1, 4].Value = "Ref. casque";
          using (var range = worksheet.Cells[1, 1, 1, 4])
          {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(119, 119, 119));
            range.Style.Font.Color.SetColor(Color.FromArgb(229, 229, 229));
          }

          // les suivantes
          int row = 2;
          foreach (CartonLivreInfo carton in this.Reponse.Cartons)
          {
            foreach (CasqueLivreInfo casque in carton.Casques)
            {
              foreach (EtiquetteInfo etiquette in casque.Etiquettes)
              {
                worksheet.Cells[row, 1].Value = etiquette.Numero;
                worksheet.Cells[row, 2].Value = etiquette.IndexCommande;
                worksheet.Cells[row, 3].Value = casque.CasqueNom;
                worksheet.Cells[row, 4].Value = casque.CasqueCode;
                row++;
              }
            }
          }

          // Create an autofilter for the range
          worksheet.Cells["A1:D" + row.ToString()].AutoFilter = true;

          // Autofit columns for all cells
          worksheet.Cells.AutoFitColumns(0);

          package.Workbook.Properties.Title = string.Format("Détail des étiquettes de la livraison {0}", this.Reponse.Livraison.Reference);
          package.Workbook.Properties.Author = "Tracking Center";
          package.Workbook.Properties.Company = "Tracking Center";

          result = package.GetAsByteArray();
        }
      }

      // save on disk
      FileInfo fi = new FileInfo(fileName);
      if (fi.Exists)
      { // on remplace systématiquement le fichier existant
        fi.Delete();
      }

      using (var stream = File.Create(fileName)) 
      {
        stream.Write(result, 0, result.Length);
        ////stream.Close();
      }

      this.PieceJointe = fileName;
    }

    /// <summary>
    ///  Remplace dans Text tous les markers par les infos de la livraison
    /// </summary>
    /// <param name="texte">Le texte de départ (contient des markeurs)</param>
    /// <param name="isSujet">Indique si on traite le sujet ou le contenu</param>
    /// <returns>le texte mergé</returns>
    protected override string Replace(string texte, bool isSujet)
    {
      string sujet = texte;
      sujet = sujet.Replace("##ClientNom##", this.Reponse.Livraison.ClientNom);
      sujet = sujet.Replace("##ClientAdresseLivraison##", this.Reponse.Livraison.ClientAdresseLivraisonHtml);
      sujet = sujet.Replace("##Creation##", this.Reponse.Livraison.Creation.ToString("dd/MM/yyyy hh:mm"));
      sujet = sujet.Replace("##Reference##", this.Reponse.Livraison.Reference);
      sujet = sujet.Replace("##NombreCarton##", this.Reponse.Livraison.NombreCarton.ToString());
      sujet = sujet.Replace("##NombrePiece##", this.Reponse.Livraison.NombrePiece.ToString());
      sujet = sujet.Replace("##UtilisateurNom##", this.Reponse.Livraison.UtilisateurNom);
      if (this.Reponse.Livraison.Validation.HasValue)
      {
        sujet = sujet.Replace("##Validation##", this.Reponse.Livraison.Validation.Value.ToString("dd/MM/yyyy hh:mm"));
      }
      else
      {
        sujet = sujet.Replace("##Validation##", string.Empty);
      }

      if (!isSujet)
      { // c'est le contenu : un remplacement supplémentaire pour les cartons
        StringBuilder txtPieces = new StringBuilder();
        foreach (CartonLivreInfo carton in this.Reponse.Cartons)
        {
          txtPieces.Append(this.Replace(this.TemplateCarton1, carton, carton.Casques[0]));
          for (int i = 1; i < carton.Casques.Count; i++)
          {
            txtPieces.Append(this.Replace(this.TemplateCartonN, carton, carton.Casques[i]));
          }
        }

        sujet = sujet.Replace("##LivraisonCartons##", txtPieces.ToString());
      }

      return sujet;
    }

    /// <summary>
    /// Remplace les markers du template par les infos du carton et du 1er casque
    /// </summary>
    /// <param name="texte">Le template</param>
    /// <param name="carton">Le carton</param>
    /// <param name="casque">Le casque</param>
    /// <returns>Le texte remplacé</returns>
    private string Replace(string texte, CartonLivreInfo carton, CasqueLivreInfo casque)
    {
      string sujet = texte;
      sujet = sujet.Replace("##Carton.CartonIndex##", (carton.CartonIndex + 1).ToString());
      sujet = sujet.Replace("##Carton.Nom##", carton.Nom);
      sujet = sujet.Replace("##Carton.Description##", carton.DescriptionHtml);
      sujet = sujet.Replace("##Carton.NombreCasque##", carton.Casques.Count.ToString());

      sujet = sujet.Replace("##Carton.Casque.Nombre##", casque.Etiquettes.Count.ToString());
      sujet = sujet.Replace("##Carton.Casque.CasqueNom##", casque.CasqueNom);
      sujet = sujet.Replace("##Carton.Casque.CasqueCode##", casque.CasqueCode);
      StringBuilder txt = new StringBuilder();
      string sepi = string.Empty;
      foreach (EtiquetteInfo etiquette in casque.Etiquettes)
      {
        txt.Append(sepi);
        txt.Append(this.Replace(this.TemplateCasqueEtiquette, etiquette));
        sepi = "<br/>";
      }

      sujet = sujet.Replace("##Carton.Casque.Etiquettes##", txt.ToString());

      return sujet;
    }

    /// <summary>
    /// Remplace les markers du template par les infos de l'étiquette
    /// </summary>
    /// <param name="texte">Le template</param>
    /// <param name="etiquette">Les infos de l'étiquette</param>
    /// <returns>Le texte remplacé</returns>
    private string Replace(string texte, EtiquetteInfo etiquette)
    {
      string sujet = texte;
      sujet = sujet.Replace("##Casque.Etiquette.IndexCommande##", etiquette.IndexCommande.ToString());
      sujet = sujet.Replace("##Casque.Etiquette.Index##", etiquette.Index.ToString());
      sujet = sujet.Replace("##Casque.Etiquette.Numero##", etiquette.Numero);
      return sujet;
    }
  }
}