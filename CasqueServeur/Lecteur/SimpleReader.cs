using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Matos.ClientOwin;
using CasqueLib.Matos.Lecteur;
using CasqueLib.Matos.ServerOwin;
using bll = Impinj.OctaneSdk;

namespace CasqueServeur.Lecteur
{
  /// <summary>
  /// Classe qui encapsule et met en oeuvre un lecteur IMPINJ
  /// A utiliser pour de la lecture seule
  /// </summary>
  public class SimpleReader : IReader
  {
    #region Membres
    /// <summary>
    /// Le lecteur
    /// </summary>
    private bll.ImpinjReader reader;

    /// <summary>
    /// Les infos de lecture captées durant la session
    /// </summary>
    private Dictionary<string, Compteur> lectures;
    #endregion

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="SimpleReader"/>
    /// </summary>
    public SimpleReader()
    {
      this.ClientId = string.Empty;
      this.Parameters = null;
      this.lectures = new Dictionary<string, Compteur>();
      this.Running = false;
      this.reader = null;
    }

    #region Events & Properties
    /// <summary>
    /// Evenement pour notifier que quelque chose est arrivé sur le lecteur
    /// La propriété : EEtatLecteur donne l'action qui vient d'être faite
    /// La propriété : ELogType donne sont déroulement : Ok ou erreur...
    /// </summary>
    public event EventHandler<SimpleReaderEventArgs> OnNotifie;
    
    /// <summary>
    /// Les paramètre du lecteur
    /// </summary>
    public SimpleReaderParameters Parameters { get; set; }

    /// <summary>
    /// L'adresse IP du lecteur
    /// </summary>
    public string AdresseIp
    {
      get
      {
        if (this.Parameters == null)
        {
          return string.Empty;
        }
        else
        {
          return this.Parameters.AdresseIP;
        }
      }
    }

    /// <summary>
    /// Le lecteur est démarré et est en cours de lecture
    /// </summary>
    public bool Running { get; private set; }

    /// <summary>
    /// Mémorise le client a qui on doit répondre
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// On peut démarrer le lecteur 
    /// ssi il ne l'est pas déjà et que la config est ok
    /// </summary>
    public bool CanStart
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.ClientId) && this.reader == null && !this.Running;
      }
    }

    /// <summary>
    /// On peut stoppper le lecteur
    /// ssi il est démarré
    /// </summary>
    public bool CanStop
    {
      get
      {
        return this.reader != null && this.Running;
      }
    }
    #endregion

    #region Méthodes publiques
    /// <summary>
    /// Démarre le lecteur
    /// </summary>
    /// <param name="clientId">L'identifiant du client qui demande le start</param>
    public void Start(string clientId)
    {
      // le lecteur existe c'est qu'il est démarré : on stope donc avant
      this.Stop();

      this.ClientId = clientId;

      // init des lectures
      this.ResetLectures();

      // Create une instance du lecteur
      this.reader = new bll.ImpinjReader();

      // Mise en place des délégués pour les events
      this.reader.TagsReported += this.OnReportEvent;
      this.reader.ConnectionLost += this.OnConnectionLost;
      this.reader.KeepaliveReceived += this.OnKeepaliveReceived;

      try
      {
        this.reader.Connect(this.AdresseIp);
      }
      catch (Exception ex)
      {
        string message = ex.ToString();
        if (message.IndexOf("Unable to connect to specified reader in specified time period.") != -1)
        { // pas très propre mais bon 99% des erreurs sont celle la ! ==> on met un message maitrisé
          message = "Impossible de trouver le lecteur. Vérifiez s'il est allumé et connecté au réseau";
        }
        else if (message.IndexOf(string.Format("Error connecting to the reader ({0}) : Timeout", this.AdresseIp)) != -1)
        {
          message = "Impossible de trouver le lecteur. Vérifiez s'il est allumé et connecté au réseau";
        }

        this.LecteurCloseAndDestroy();
        this.FireMessage("Connexion au lecteur impossible : " + message, ELogType.Erreur, EEtatLecteur.Start);
        return;
      }

      if (!this.reader.IsConnected)
      { // erreur à l'ouverture du lecteur
        this.LecteurCloseAndDestroy();
        this.FireMessage("Connexion au lecteur impossible (sans erreur constatée)", ELogType.Erreur, EEtatLecteur.Start);
        return;
      }

      // Mise en place de la configuration du lecteur et démarrage
      if (this.LecteurConfigure())
      { // Config Ok : demarre les lectures
        try
        {
          this.reader.Start();
          this.Running = true;
          this.FireMessage(string.Format("Lecteur à l'adresse {0} démarré.", this.Parameters.AdresseIP), ELogType.RapportOk, EEtatLecteur.Start);

          // Demande au lecteur la liste des tag qu'il a éventuellement stocké en mémoire.
          this.reader.ResumeEventsAndReports();
        }
        catch (Exception ex)
        {
          this.FireMessage("Configuration du lecteur impossible : " + ex.Message, ELogType.Erreur, EEtatLecteur.Start);
        }
      }
    }

    /// <summary>
    /// Arrête le lecteur
    /// </summary>
    public void Stop()
    {
      // fermeture du lecteur
      this.Running = false;
      this.LecteurCloseAndDestroy();
      this.FireMessage("Lecteur stoppé", ELogType.RapportOk, EEtatLecteur.Stop);
    }

    /// <summary>
    /// Initialise la liste des lectures
    /// </summary>
    public void ResetLectures()
    {
      if (this.lectures != null)
      {
        this.lectures.Clear();
      }

      this.FireMessage("Lectures Initialisées", ELogType.RapportOk, EEtatLecteur.Reset);
    }

    /// <summary>
    /// Nettoye proprement le lecteur
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion

    /// <summary>
    /// Dispose qui va bien
    /// </summary>
    /// <param name="p">argument magique</param>
    protected virtual void Dispose(bool p)
    {
      this.Stop();
    }

    #region La communication avec le lecteur
    /// <summary>
    /// Event reçu lors d'une perte de connexion avec le lecteur
    /// </summary>
    /// <param name="reader">Le lecteur</param>
    private void OnConnectionLost(bll.ImpinjReader reader)
    {
      this.Running = false;
      this.LecteurCloseAndDestroy();
      this.FireMessage("Connexion perdue, lecteur stoppé", ELogType.Erreur, EEtatLecteur.Stop);
    }

    /// <summary>
    /// Event reçu par le keepAlive = le maintien de la connection http par le lecteur
    /// </summary>
    /// <param name="reader">Le lecteur</param>
    private void OnKeepaliveReceived(bll.ImpinjReader reader)
    {
      this.FireMessage("Keep Alive reçu", ELogType.RapportOk, EEtatLecteur.KeepAlive);
    }

    /// <summary>
    /// Event appellé par l'interface à l'arrivée d'un message du lecteur
    /// </summary>
    /// <param name="reader">Le lecteur qui déclenche l'action</param>
    /// <param name="report">les infos de lecture</param>
    private void OnReportEvent(bll.ImpinjReader reader, bll.TagReport report)
    {
      if (report == null || report.Tags == null)
      { // pas de données pas de traitement !
        return;
      }

      foreach (bll.Tag t in report.Tags)
      { // Loop through all the tags in the report
        if (t.Epc.CountBytes > 0)
        { // Two possible types of EPC: 96-bit and 128-bit
          this.ProcessTagLu(t.AntennaPortNumber - 1, t.Epc.ToHexString());
        }
      }
    }

    /// <summary>
    /// Désactive le lecteur
    /// </summary>
    private void LecteurCloseAndDestroy()
    {
      if (this.reader != null)
      {
        try
        {
          if (this.reader.IsConnected)
          {
            bool ok = false;
            try
            {
              this.reader.ConnectTimeout = 50; // on force en dur un timeout très faible pour gagner du temps quand le lecteur est Off
              ok = this.reader.QueryStatus().IsSingulating;
            }
            catch
            {
              ok = false;
            }

            if (ok)
            {
              this.reader.Stop();
            }

            this.reader.Disconnect();
          }
        }
        finally
        {
          this.reader = null;
        }
      }
    }

    /// <summary>
    /// Configure le lecteur
    /// </summary>
    /// <returns>True si la config est Ok</returns>
    private bool LecteurConfigure()
    {
      if (this.reader == null)
      { // pas de lecteur pas de config
        return false;
      }

      bll.Settings setting = this.reader.QueryDefaultSettings();
      if (this.Parameters.KeepAliveIntervalSeconds > 0)
      { // Keep Alive activé
        setting.Keepalives = new bll.KeepaliveConfig();
        setting.Keepalives.Enabled = true;
        setting.Keepalives.EnableLinkMonitorMode = true;
        setting.Keepalives.LinkDownThreshold = 1;
        setting.Keepalives.PeriodInMs = 1000 * this.Parameters.KeepAliveIntervalSeconds;
      }

      // Demande au lecteur de mémoriser les lectures de Tag quand la connexion est coupée (Cela n'a pas l'air de marcher !!)
      setting.HoldReportsOnDisconnect = true;

      // Demande au lecteur d'inclure dans les reports le numéro d'antenne et la première date de lecture de chaque tag
      setting.Report.IncludeAntennaPortNumber = true;
      setting.Report.IncludeFirstSeenTime = true;

      // Ajuste les infos des antennes
      for (ushort ant = 1; ant <= SimpleReaderAntenneInfo.LASTPOSITION; ant++)
      {
        try
        {
          bll.AntennaConfig cfg = setting.Antennas.GetAntenna(ant);
          if (this.Parameters.Antennes[ant - 1] != null && this.Parameters.Antennes[ant - 1].Active)
          {
            cfg.PortNumber = ant;
            cfg.MaxTransmitPower = false;
            cfg.TxPowerInDbm = this.Parameters.Antennes[ant - 1].GainDB;
            cfg.IsEnabled = true;
            cfg.PortName = "P" + this.Parameters.Antennes[ant - 1].Position;
          }
          else
          {
            cfg.IsEnabled = false;
          }
        }
        catch (Exception ex)
        {
          this.FireMessage("Erreur lors de la définition d'une configuration : " + ex.ToString(), ELogType.Erreur, EEtatLecteur.Start);
        }
      }

      try
      {
        this.reader.ApplySettings(setting);
        this.reader.SaveSettings();
        this.FireMessage("Lecteur reconfiguré.", ELogType.Normal, EEtatLecteur.Start);
        return true;
      }
      catch (Exception ex)
      {
        this.FireMessage("Erreur lors de l'application de le configuration : " + ex.ToString(), ELogType.Erreur, EEtatLecteur.Start);
        return false;
      }
    }
    #endregion

    #region Traitement des tag et propagation des infos
    /// <summary>
    /// Propage l'évement OnTagLu au parent
    /// </summary>
    /// <param name="anteneIndex">L'index de l'antenne de 0 à 3</param>
    /// <param name="numeroTag">Le numéro de tag lu</param>
    private void ProcessTagLu(int anteneIndex, string numeroTag)
    {
      Compteur cpt;
      if (this.lectures.ContainsKey(numeroTag))
      { // lecture connu : incrementation du compteur
        cpt = this.lectures[numeroTag];
        cpt++;
      }
      else
      { // nouvelle lecture
        this.lectures.Add(numeroTag, new Compteur(numeroTag));
        cpt = this.lectures[numeroTag];
      }

      if (cpt.Nombre >= this.Parameters.Seuil && !cpt.Notifie)
      { // faut envoyer l'info
        this.FireMessage(numeroTag, ELogType.RapportOk, EEtatLecteur.Tag, anteneIndex);
        cpt.Notifie = true;
      }
    }

    /// <summary>
    /// Propage un message au parent
    /// </summary>
    /// <param name="message">Le message à communiquer</param>
    /// <param name="log">le type de log</param>
    /// <param name="action">L'action a loguer</param>
    /// <param name="position">La position du lecteur</param>
    private void FireMessage(string message, ELogType log, EEtatLecteur action, int position = SimpleReaderAntenneInfo.ALLPOSITION)
    {
      if (this.OnNotifie != null)
      {
        SimpleReaderEventArgs args = new SimpleReaderEventArgs(message, log, action, position);
        args.AdresseIP = this.Parameters.AdresseIP;
        this.OnNotifie(this, args);
      }
    }
    #endregion
  }
}
