using CasqueLib.Common;

namespace CasqueLib.Services
{
  /// <summary>
  /// Classe de base pour les response paginées
  /// </summary>
  public class ResponseBase : BaseListCompteur
  {
    /// <summary>
    /// La taille des pages
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Le nombre de page
    /// </summary>
    public int NombrePage
    {
      get
      {
        if (this.PageSize == 0)
        {
          this.PageSize = 1;
        }

        return (this.Nombre / this.PageSize) + (this.Nombre % this.PageSize == 0 ? 0 : 1);
      }
    }

    /// <summary>
    /// Remplit toi avec les infos de compteurs renvoié par la PS
    /// </summary>
    /// <param name="cpt">Le compteur</param>
    /// <param name="pageSize">La taille de pagination demandée</param>
    public void Fill(BaseListCompteur cpt, int pageSize)
    {
      if (cpt != null)
      {
        this.Nombre = cpt.Nombre;
        this.Page = cpt.Page;
        this.Tri = cpt.Tri;
        this.TriSensTxt = cpt.TriSensTxt;
      }

      this.PageSize = pageSize;
    }
  }
}
