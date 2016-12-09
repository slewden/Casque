using CasqueLib.Matos.ClientOwin;

namespace CasqueReaderSimulator.Lecteur
{
  /// <summary>
  /// Pilote des lecteurs et encodeurs de démo
  /// </summary>
  public class DemoClient : GenericClient<DemoReader, DemoWriter>
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="DemoClient"/>
    /// </summary>
    /// <param name="logger">La liste des log</param>
    public DemoClient(FSimulateur logger)
    {
      this.Logger = logger;
    }

    /// <summary>
    /// le moyen de loguer
    /// </summary>
    public FSimulateur Logger { get; private set; }

    /// <summary>
    /// S'assure que la configuration est correcte
    /// </summary>
    /// <returns>true si correcte</returns>
    protected override bool CheckConfig()
    {
      if (string.IsNullOrWhiteSpace(this.Url))
      {
        this.LogErreur("Adresse du hub non valide");
        return false;
      }

      if (this.DriversFor <= 0 || this.DriversFor > 3)
      {
        this.LogErreur("Gestion des lecteurs invalides");
        return false;
      }

      return true;
    }

    /// <summary>
    /// Log des infos Ok
    /// </summary>
    /// <param name="template">Le texte formattable</param>
    /// <param name="args">les éventuels paramètres</param>
    protected override void LogInfo(string template, params object[] args)
    {
      this.Logger.Log(string.Format(template, args));
    }

    /// <summary>
    /// Log une erreur
    /// </summary>
    /// <param name="template">Le texte formattable</param>
    /// <param name="args">les éventuels paramètres</param>
    protected override void LogErreur(string template, params object[] args)
    {
      this.Logger.Log("Erreur " + string.Format(template, args));
    }
  }
}
