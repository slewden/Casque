using System;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe une utilisation d'un poste de lecture / ecriture / impression
  /// </summary>
  [Alias("utilisation_poste")]
  public class UtilisationPoste
  {
    /// <summary>
    /// La clé de l'utilisation
    /// </summary>
    [AutoIncrement]
    [Alias("utpo_id")]
    public int Cle { get; set; }

    /// <summary>
    /// clé de l'utilisateur
    /// </summary>
    [Alias("util_id")]
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// clé du poste
    /// </summary>
    [Alias("post_id")]
    public int PosteCle { get; set; }

    /// <summary>
    /// Date d'ouverture du poste
    /// </summary>
    [Alias("utpo_d_debut")]
    public DateTime Creation { get; set; }

    /// <summary>
    /// Date de fin des lectures
    /// </summary>
    [Alias("utpo_d_fin")]
    public DateTime? Fin { get; set; }
  }
}
