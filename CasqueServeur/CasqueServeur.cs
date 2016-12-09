using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using CasqueLib.Matos.Lecteur;
using CasqueLib.Matos.ServerOwin;
using CasqueServeur.Lecteur;

namespace CasqueServeur
{
  /// <summary>
  /// Classe de base pour le lancement de l'application
  /// </summary>
  public partial class CasqueServeur : ServiceBase
  {
    /// <summary>
    /// Le timer
    /// </summary>
    private Timer timer1;

    /// <summary>
    /// L'objet de backup
    /// </summary>
    private BackupInfo backup;

    /// <summary>
    /// Le client du hub qui pilote les lecteurs et encodeurs
    /// </summary>
    private ServiceClient client;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="CasqueServeur"/>
    /// </summary>
    public CasqueServeur()
    {
      this.InitializeComponent();

      if (!System.Diagnostics.EventLog.SourceExists("Casque"))
      {
        System.Diagnostics.EventLog.CreateEventSource("Casque", "CasqueLog");
      }
      
      this.eventLog1.Source = "Casque";
      this.eventLog1.Log = "CasqueLog";
    }

    /// <summary>
    /// Méthode pour débugger l'application en mode console
    /// </summary>
    /// <param name="args">Arguments de la ligne de commande</param>
    internal void TestStartupAndStop(string[] args)
    {
      this.OnStart(args);
      Console.WriteLine("-----------------------------------------");
      Console.WriteLine("--- Press enter to quit -----------------");
      Console.WriteLine("-----------------------------------------");
      Console.ReadLine();
      this.OnStop();
    }

    #region Services Events
    /// <summary>
    /// A faire au démarrage du service
    /// </summary>
    /// <param name="args">les arguments de la ligne de commande</param>
    protected override void OnStart(string[] args)
    {
      this.eventLog1.WriteEntry("Démarrage du service...", EventLogEntryType.SuccessAudit);

      this.backup = new BackupInfo();
      this.timer1 = new Timer();
      this.timer1.Elapsed += this.Timer1Elapsed;
      this.timer1.Interval = 1000; // 1 seconde
      this.timer1.Start();

      this.client = new ServiceClient(this.eventLog1);
      this.client.Connecte();
    }

    /// <summary>
    /// A faire quand le service s'arrête
    /// </summary>
    protected override void OnStop()
    {
      this.eventLog1.WriteEntry("Arrêt du service...", EventLogEntryType.Information);
      try
      {
        this.client.Deconnecte();
        this.client.Dispose();
      }
      finally
      {
        this.eventLog1.WriteEntry("Service arrété.", EventLogEntryType.SuccessAudit);
      }
    }
    #endregion

    /// <summary>
    /// Intervale du timer
    /// </summary>
    /// <param name="sender">qui appelle</param>
    /// <param name="e">argument souvent inutile</param>
    private void Timer1Elapsed(object sender, ElapsedEventArgs e)
    {
      if (this.backup.IsTime)
      {
        this.backup.Working = true;
        try
        {
          this.eventLog1.WriteEntry("Démarrage du Backup...", EventLogEntryType.SuccessAudit);
          string msg = this.backup.Process();
          if (string.IsNullOrWhiteSpace(msg))
          {
            this.eventLog1.WriteEntry("Fin du Backup.", EventLogEntryType.SuccessAudit);
          }
          else
          {
            this.eventLog1.WriteEntry("Erreur lors du Backup : " + msg, EventLogEntryType.Error);
          }
        }
        finally
        {
          this.backup.Working = false;
        }
      }
    }
  }
}
