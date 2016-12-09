using ServiceStack.DataAnnotations;

namespace CasqueLib.Common
{
  /// <summary>
  /// Classe de base pour les compteurs de résultats des listes de recherche d'une table trié et paginée
  /// </summary>
  public class BaseListCompteur
  {
    /// <summary>
    /// Le nombre total de valeurs trouvées
    /// </summary>
    [Alias("nombre")]
    public int Nombre { get; set; }

    /// <summary>
    /// La page retournée
    /// </summary>
    [Alias("page")]
    public int Page { get; set; }

    /// <summary>
    /// La colonne qui sert au tri
    /// </summary>
    [Alias("tri")]
    public string Tri { get; set; }

    /// <summary>
    /// Le sens du tri
    /// </summary>
    [Alias("tri_sens")]
    public string TriSensTxt { get; set; }

    /// <summary>
    /// Indique si le tri est croissant
    /// </summary>
    [Ignore]
    public bool TriAsc
    {
      get
      {
        return this.TriSensTxt.ToUpper() == "ASC";
      }
    }

    /// <summary>
    /// Renvoie le texte complet du tri
    /// </summary>
    [Ignore]
    public string LeTri
    {
      get
      {
        return string.Format("{0}:{1}", this.Tri, this.TriSensTxt);
      }
    }
  }
}
