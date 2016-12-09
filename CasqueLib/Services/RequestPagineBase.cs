namespace CasqueLib.Services
{
  /// <summary>
  /// Classe de base pour les requetes authentifées et paginées
  /// </summary>
  public abstract class RequestPagineBase : RequestBase
  {
    /// <summary>
    /// Texte cherché
    /// </summary>
    public string SearchText { get; set; }

    /// <summary>
    /// La page à afficher
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// La taille des pages
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// sépcification du tri
    /// </summary>
    public string Tri { get; set; }

    /// <summary>
    /// s'assure que laes paramètres de pagination on les bonnes valeurs
    /// </summary>
    public void CheckPagination()
    {
      if (this.Page < 0)
      {
        this.Page = 0;
      }

      if (this.PageSize <= 0)
      {
        this.PageSize = 10;
      }
    }
  }
}
