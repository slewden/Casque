using ServiceStack.DataAnnotations;

namespace CasqueLib.Common
{
  /// <summary>
  /// pour remonter uniquement des Id
  /// </summary>
  public class CleOnly
  {
    /// <summary>
    /// La clé
    /// </summary>
    [Alias("id")]
    public int Cle { get; set; }
  }
}
