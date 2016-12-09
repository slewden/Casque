using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe la configuration d'expédition des emails
  /// </summary>
  [Alias("mail_config")]
  public class MailConfig
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="MailConfig"/>
    /// </summary>
    public MailConfig()
    {
      this.Port = 25; // port par défaut pour les serveurs SMTP
    }

    #region Properties
    /// <summary>
    /// Adresse du serveur SMTP
    /// </summary>
    [Alias("mail_host")]
    public string Host { get; set; }

    /// <summary>
    /// port du serveur SMTP (25 par défaut)
    /// </summary>
    [Alias("mail_port")]
    public int Port { get; set; }

    /// <summary>
    /// Login utilisé pour s'hautentifier sur le serveur Smtp
    /// </summary>
    [Alias("mail_user")]
    public string User { get; set; }

    /// <summary>
    /// Mot de passe associé au login
    /// </summary>
    [Alias("mail_password")]
    public string Password { get; set; }

    /// <summary>
    /// Adresse email de l'expéditeur
    /// </summary>
    [Alias("mail_from")]
    public string FromEmail { get; set; }

    /// <summary>
    /// Liste des adresse email à mettre ne BCC des emails envoyés
    /// </summary>
    [Alias("mail_bcc")]
    public string BCCEmails { get; set; }

    /// <summary>
    /// Sujet à utiliser pour l'envoie des emails aux fournisseurs
    /// </summary>
    [Alias("mail_subject_fournisseur")]
    public string SubjetFournisseur { get; set; }

    /// <summary>
    /// Sujet à utiliser pour l'envoie des emails aux clients
    /// </summary>
    [Alias("mail_subject_client")]
    public string SubjetClient { get; set; }
    #endregion
    
    /// <summary>
    /// Renvoie l'objet mail config chargé
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <returns>l'objet mail config chargé</returns>
    public static MailConfig Get(IDbConnection db)
    {
      MailConfig cfg = db.Select<MailConfig>().FirstOrDefault();
      if (cfg == null)
      {
        cfg = new MailConfig();
      }

      return cfg;
    }

    /// <summary>
    /// Indique si l'email saisi est valide
    /// </summary>
    /// <param name="email">L'adresse à valider</param>
    /// <returns>true si ok</returns>
    public static bool IsValidEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        return false;
      }

      ////return Regex.IsMatch(this.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
      // idem celui HTML5
      return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)+(\.[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)?)\Z", RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Mémorise les données
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    public void Save(IDbConnection db)
    {
      MailConfig cfg = db.Select<MailConfig>().FirstOrDefault();
      if (cfg == null)
      {
        db.Insert(this);
      }
      else
      {
        db.Update(this);
      }
    }

    /// <summary>
    /// indique si la config minimale est présente
    /// </summary>
    /// <returns>True si complet</returns>
    public bool IsComplet()
    {
      if (string.IsNullOrWhiteSpace(this.Host))
      {
        return false;
      }

      if (this.Port < 0)
      {
        return false;
      }

      if (string.IsNullOrWhiteSpace(this.FromEmail))
      {
        return false;
      }
      else if (!MailConfig.IsValidEmail(this.FromEmail))
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Envoie l'email 
    /// </summary>
    /// <param name="destinataire">Le destinataire</param>
    /// <param name="destinataire2">Un second destinataire (optionnel)</param>
    /// <param name="sujet">Le sujet</param>
    /// <param name="contenu">Le contenu</param>
    /// <param name="pieceJointe">Nom du fichier piece jointe</param>
    /// <returns>Un message si erreur. Vide sinon</returns>
    public string SendEmail(string destinataire, string destinataire2, string sujet, string contenu, string pieceJointe)
    {
      MailMessage msg = new MailMessage();

      if (!string.IsNullOrWhiteSpace(destinataire))
      {
        msg.To.Add(new MailAddress(destinataire));
      }

      if (!string.IsNullOrWhiteSpace(destinataire2))
      {
        msg.To.Add(new MailAddress(destinataire2));
      }

      if (msg.To.Any())
      { // on a un destinataire (au moins un) pas de destinataire pas de message !
        msg.From = new MailAddress(this.FromEmail);
        msg.Subject = sujet;
        msg.IsBodyHtml = true;
        msg.Body = contenu;
        if (!string.IsNullOrWhiteSpace(this.BCCEmails))
        {
          string[] ems = this.BCCEmails.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
          foreach (string em in ems)
          {
            msg.Bcc.Add(new MailAddress(em));
          }
        }

        if (!string.IsNullOrWhiteSpace(pieceJointe) && System.IO.File.Exists(pieceJointe))
        {
          msg.Attachments.Add(new Attachment(pieceJointe));
        }

        SmtpClient client = new SmtpClient();
        client.Port = this.Port;
        client.Host = this.Host;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        if (string.IsNullOrWhiteSpace(this.User))
        {
          client.UseDefaultCredentials = true;
        }
        else
        {
          client.UseDefaultCredentials = false;
          client.Credentials = new System.Net.NetworkCredential(this.User, this.Password);
        }

        try
        {
          client.Send(msg);
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
      }

      return string.Empty;
    }
  }
}
