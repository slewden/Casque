using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Matos.ClientOwin;

namespace CasqueServeur.Lecteur
{
  /// <summary>
  /// Pilote les lecteur et encodeurs
  /// </summary>
  public class ServiceClient : GenericClient<SimpleReader, SimpleWriter>
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="ServiceClient"/>
    /// </summary>
    /// <param name="logger">L'event log pour logger</param>
    public ServiceClient(EventLog logger)
    {
      this.Logger = logger;
    }

    /// <summary>
    /// le moyen de loguer
    /// </summary>
    public EventLog Logger { get; private set; }

    /// <summary>
    /// S'assure que la configuration est correcte
    /// </summary>
    /// <returns>true si correcte</returns>
    protected override bool CheckConfig()
    {
      string url = System.Configuration.ConfigurationManager.AppSettings["ServerUrl"];
      if (string.IsNullOrWhiteSpace(url))
      {
        this.Logger.WriteEntry("Configuration 'ServerUrl' absente doit contenir l'url du serveur web qui contient le hub. Connexion au Hub annulée. Service inopérant.", EventLogEntryType.Error);
        return false;
      }
      else
      {
        this.Url = url;
      }

      string readers = System.Configuration.ConfigurationManager.AppSettings["Readers"];
      int n = 0;
      if (string.IsNullOrWhiteSpace(readers))
      {
        this.Logger.WriteEntry("Configuration 'Readers' absente doit contenir 1 pour writer, 2 pour reader ou 3 pour les deux. Connexion au Hub annulée. Service inopérant.", EventLogEntryType.Error);
        return false;
      }

      if (!int.TryParse(readers, out n))
      {
        this.Logger.WriteEntry(string.Format("Configuration 'Readers' incorrecte. '{0}' n'est pas un entier valide. 'Readers' doit contenir 1 pour writer, 2 pour reader ou 3 pour les deux. Connexion au Hub annulée. Service inopérant.", readers), EventLogEntryType.Error);
        return false;
      }

      if (n <= 0 || n > 3)
      {
        this.Logger.WriteEntry("Configuration 'Readers' non valide doit contenir 1 pour writer, 2 pour reader ou 3 pour les deux. Connexion au Hub annulée. Service inopérant.", EventLogEntryType.Error);
        return false;
      }

      this.DriversFor = n;

      return true;
    }

    /// <summary>
    /// Logue les infos (a l'écran si possible) dans le journal d'event sinon
    /// </summary>
    /// <param name="template">le template a loguer</param>
    /// <param name="args">Les paramètres</param>
    protected override void LogInfo(string template, params object[] args)
    {
      string msg = string.Format(template, args);
      if (Environment.UserInteractive)
      {
        ConsoleColor clr = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(msg);
        Console.ForegroundColor = clr;
      }
      else
      {
        this.Logger.WriteEntry(msg, EventLogEntryType.Information);
      }
    }

    /// <summary>
    /// Logue une erreur (a l'écran si possible) dans le journal d'event sinon
    /// </summary>
    /// <param name="template">le template a loguer</param>
    /// <param name="args">Les paramètres</param>
    protected override void LogErreur(string template, params object[] args)
    {
      string msg = string.Format(template, args);
      if (Environment.UserInteractive)
      {
        ConsoleColor clr = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ForegroundColor = clr;
      }
      else
      {
        this.Logger.WriteEntry(msg, EventLogEntryType.Error);
      }
    }
  }
}
