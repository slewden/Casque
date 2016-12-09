namespace CasqueLib.Common
{
  /// <summary>
  /// Etant la classe nomCle en ajoutant le champ imprimante
  /// </summary>
  public class NomCleImprimante : NomCle
  {
    /// <summary>
    /// Indique si c'est pour une impression (true) ou lecture (false)
    /// </summary>
    public bool Imprimante { get; set; }
  }
}
