using ServiceStack.DataAnnotations;

namespace CasqueLib.Common
{
  /// <summary>
  /// Pour stoquer un nom et un id
  /// </summary>
  public class NomCle
  {
    /// <summary>
    /// Le nom
    /// </summary>
    [Alias("nom")]
    public string Nom { get; set; }

    /// <summary>
    /// La clé
    /// </summary>
    [Alias("id")]
    public int Cle { get; set; }
  }
}
