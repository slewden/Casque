using System.Data;
using System.IO;
using System.Linq;
using CasqueLib.Buisness;
using CasqueLib.Common;

namespace CasqueLib.Email
{
  /// <summary>
  /// Classe de base pour la composition et l'envoie d'email
  /// Cette classe génère aussi la pièce jointe à l'email
  /// </summary>
  public abstract class EmailComposer
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="EmailComposer"/>
    /// </summary>
    /// <param name="fld">le dossier</param>
    public EmailComposer(Folder.EFolder fld)
    {
      this.PieceJointeFolder = fld;
    }

    /// <summary>
    /// La configuration d'expédition
    /// </summary>
    public MailConfig Config { get; protected set; }

    /// <summary>
    /// L'email du destinataire
    /// </summary>
    public string DestinataireEmail { get; protected set; }

    /// <summary>
    /// Le sujet du email
    /// </summary>
    public string Sujet { get; protected set; }

    /// <summary>
    /// Le fichier joint
    /// </summary>
    public string PieceJointe { get; protected set; }

    /// <summary>
    /// L'URL de la pièce jointe
    /// </summary>
    public string PieceJointeUrl { get; protected set; }

    /// <summary>
    /// Le contenu du template principal
    /// </summary>
    public string Template { get; protected set; }
    
    /// <summary>
    /// Le nom de la pièce jointe
    /// </summary>
    protected string PieceJointeName { get; set; }

    /// <summary>
    /// Le dossier d'enregistrement de la pièce jointe
    /// </summary>
    protected Folder.EFolder PieceJointeFolder { get; set; }

    /// <summary>
    /// Load l'objet et indique si tout est ok
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <param name="cle">La clé de la commande</param>
    /// <returns>Le message d'erreur ou vide si tout est OK</returns>
    public abstract string Load(IDbConnection db, int cle);

    /// <summary>
    /// Envoie l'email
    /// </summary>
    /// <param name="emailSuplementaire">L'adresse suplémentaire pour le destinataire du email</param>
    /// <returns>Le messsage d'erreur. Vide si Ok</returns>
    public virtual string Send(string emailSuplementaire)
    {
      // remplacement des paramètres pour le sujet
      string leSujet = this.Replace(this.Sujet, true);
      string contenu = this.Replace(this.Template, false);

      // Génére un fichier XL qui contient les N° de tags à mettre en pièce jointe
      this.GenerePieceJointe();

      // Envoie du message
      return this.Config.SendEmail(this.DestinataireEmail, emailSuplementaire, leSujet, contenu, this.PieceJointe);
    }

    /// <summary>
    /// Génère le fichier en picèe jointe
    /// A la sortie de cette procédure si un fichier doit être joint la propertie "PieceJointe" contient le fullPath du fichier
    /// Sinon si aucun fichier n'est à joindre la propertie "PieceJointe" doit être vide 
    /// </summary>
    public virtual void GenerePieceJointe()
    {
      // génère les données
      byte[] result = this.GetPieceJointeData();

      if (result != null && result.Any())
      {
        this.PieceJointeUrl = Folder.RelativeUrl(this.PieceJointeFolder, this.PieceJointeName);
        string fullFileName = Folder.FullPath(this.PieceJointeFolder, this.PieceJointeName);

        // save on disk
        FileInfo fi = new FileInfo(fullFileName);
        if (fi.Exists)
        { // on remplace systématiquement le fichier existant
          fi.Delete();
        }

        using (var stream = File.Create(fullFileName))
        {
          stream.Write(result, 0, result.Length);
          ////stream.Close();
        }

        this.PieceJointe = fullFileName;
      }
      else
      {
        this.PieceJointeUrl = null;
        this.PieceJointe = null;
      }
    }

    /// <summary>
    /// Génère les données du fichier en pièce jointe
    /// </summary>
    /// <returns>les données</returns>
    protected abstract byte[] GetPieceJointeData();
    
    /// <summary>
    /// Charge un template et renvoie le contenu
    /// </summary>
    /// <param name="fileName">Nom complet du fichier sur disque</param>
    /// <returns>le contenu du template. vide en cas d'erreur</returns>
    protected string ExtractTemplate(string fileName)
    {
      string template = string.Empty;
      if (File.Exists(fileName))
      {
        try
        {
          template = File.ReadAllText(fileName);
        }
        catch
        {
        }
      }

      return template;
    }

    /// <summary>
    /// Remplace dans Text tous les markers par les infos de l'objet métier
    /// </summary>
    /// <param name="texte">le texte de template à formatter</param>
    /// <param name="isSujet">est pour le sujet du email</param>
    /// <returns>le texte formatté</returns>
    protected abstract string Replace(string texte, bool isSujet);
  }
}
