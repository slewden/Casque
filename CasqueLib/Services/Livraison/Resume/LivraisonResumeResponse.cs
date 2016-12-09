using System.Collections.Generic;
using System.Linq;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Livraison.Resume
{
  /// <summary>
  /// Le resultat d'une recherche de la liste résumée par client des livraisons
  /// </summary>
  public class LivraisonResumeResponse 
  {
    /// <summary>
    /// La liste paginée des livraisons
    /// </summary>
    public List<ClientLivraisonView> Livraisons { get; set; }

    /// <summary>
    /// Le total
    /// </summary>
    public int Total
    {
      get
      {
        if (this.Livraisons != null && this.Livraisons.Any())
        {
          return this.Livraisons.Sum(x => x.Nombre);
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// Le total de pièces livrées pour ce client
    /// </summary>
    public int TotalPiece
    {
      get
      {
        if (this.Livraisons != null && this.Livraisons.Any())
        {
          return this.Livraisons.Sum(x => x.NombrePiece);
        }
        else
        {
          return 0;
        }
      }
    }
  }
}
