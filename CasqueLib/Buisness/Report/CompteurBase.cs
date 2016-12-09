using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Report
{
  /// <summary>
  /// Classe de base pour les compteurs
  /// </summary>
  public class CompteurBase
  {
    /// <summary>
    /// Nombre d'occurence
    /// </summary>
    [Alias("nb")]
    public int Nombre { get; set; }

    /// <summary>
    /// Type de compteur
    /// 20 user inactif, 21 user Actif, 30 imprimante de tag, 31 lecteur de tag, 32 imprimante en route, 33 lecteurs en route
    /// </summary>
    [Alias("compteur")]
    public int CompteurInt { get; set; }

    /// <summary>
    /// Nom du compteur
    /// </summary>
    [Alias("compteur_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Picto du compteur
    /// </summary>
    [Alias("compteur_picto")]
    public string Picto { get; set; }

    /// <summary>
    /// La couleur de fond à utiliser
    /// </summary>
    [Alias("compteur_couleur")]
    public string Couleur { get; set; }
  }
}
