using System.Collections.Generic;
using System.Linq;

namespace CasqueLib.Services.Commande.Resume
{
  /// <summary>
  /// Le resultat des infos résumé des commandes
  /// </summary>
  public class CommandeResumeResponse
  {
    /// <summary>
    /// La listes des commandes par statut
    /// </summary>
    public List<CommandeResumeData> Commandes { get; set; }

    /// <summary>
    /// Nombre total de commandes
    /// </summary>
    public int Total
    {
      get
      {
        if (this.Commandes != null)
        {
          return this.Commandes.Sum(x => x.Nombre);
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// Nombre total de pièce dans les commandes
    /// </summary>
    public int TotalPiece
    {
      get
      {
        if (this.Commandes != null)
        {
          return this.Commandes.Sum(x => x.TotalPiece);
        }
        else
        {
          return 0;
        }
      }
    }
  }
}
