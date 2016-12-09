using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using CasqueLib.Common;

namespace CasqueServeur
{
  /// <summary>
  /// Fait un backup de la base quand il faut 
  /// (fait une fois par jour !!)
  /// </summary>
  public class BackupInfo
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="BackupInfo"/>
    /// </summary>
    public BackupInfo()
    {
      this.NewBackupTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 0, 0);
      while (DateTime.Now > this.NewBackupTime)
      {
        this.NewBackupTime = this.NewBackupTime.AddDays(1);
      }

      this.FileTemplate = ConfigurationManager.AppSettings["backupPath"];

      this.ConnexionString = System.Configuration.ConfigurationManager.ConnectionStrings["casque"].ToString();
      this.BddName = string.Empty;
      if (!string.IsNullOrWhiteSpace(this.ConnexionString))
      {
        string[] nfos = this.ConnexionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        var nfo = nfos.Where(x => x.StartsWith("Initial Catalog=")).Select(x => x.Substring(16)).FirstOrDefault();
        if (nfo != null && !string.IsNullOrWhiteSpace(nfo))
        { // Trouvé
          this.BddName = nfo;
        }
      }
    }

    /// <summary>
    /// Est-ce l'heure de faire le backup
    /// </summary>
    public bool IsTime
    {
      get
      {
        if (!this.Working && DateTime.Now > this.NewBackupTime)
        {
          return this.AllOkForBackup();
        }
        else
        {
          return false;
        }
      }
    }

    /// <summary>
    /// Vérouille l'objet le temps du débug
    /// </summary>
    public bool Working { get; set; }

    /// <summary>
    /// Le template du dossier et nom de fichier de sauvegarde 
    /// (on y ajoute la date par string.format)
    /// </summary>
    protected string FileTemplate { get; private set; }

    /// <summary>
    /// Date après laquelle on fait le backup
    /// </summary>
    protected DateTime NewBackupTime { get; private set; }

    /// <summary>
    /// Le nom de la base
    /// </summary>
    protected string BddName { get; private set; }

    /// <summary>
    /// La chaine de connexion à la base de données
    /// </summary>
    protected string ConnexionString { get; private set; }

    /// <summary>
    /// Fait le boulot
    /// </summary>
    /// <returns>une chaine vide si ok ou le message d'erreur à logue si erreur d'exécution</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Vérifier si les requêtes SQL présentent des failles de sécurité", Justification = "Verifié !")]
    public string Process()
    {
      string result = string.Empty;
      if (this.AllOkForBackup())
      {
        try
        {
          string sql = string.Format(
    @"BACKUP DATABASE {0}
TO DISK = {1}
   WITH INIT, FORMAT,  
      MEDIANAME = 'Casque App',  
      NAME = 'Sauvegarde complête base Casque';  
",
            this.BddName,
            SqlFormat.String(string.Format(this.FileTemplate, this.NewBackupTime)));
          SqlConnection cnn = null;
          SqlCommand cmd = null;
          try
          {
            cnn = new SqlConnection(this.ConnexionString);
            cmd = new SqlCommand(sql, cnn);
            cnn.Open();
            cmd.ExecuteNonQuery();
          }
          finally
          {
            if (cmd != null)
            {
              cmd.Dispose();
            }

            if (cnn != null)
            {
              cnn.Dispose();
            }
          }

          // c'est ok 
          this.NewBackupTime = this.NewBackupTime.AddDays(1);
          return result;
        }
        catch (Exception ex)
        {
          this.NewBackupTime = this.NewBackupTime.AddHours(1);
          return ex.Message;
        }
      }
      else
      {
        return result;
      }
    }

    /// <summary>
    /// Indique si la configuration des correcte
    /// </summary>
    /// <returns>true si ok</returns>
    protected bool AllOkForBackup()
    {
      bool ok = false;
      if (!string.IsNullOrWhiteSpace(this.FileTemplate))
      {
        string fn = string.Format(this.FileTemplate, DateTime.Now);
        FileInfo fi = new FileInfo(fn);
        ok = fi.Directory.Exists;
      }

      return ok && !string.IsNullOrWhiteSpace(this.BddName);
    }
  }
}
