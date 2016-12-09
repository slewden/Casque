using System.Collections.Generic;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Commande.List
{
  /// <summary>
  /// Le resultat d'une recherche de commande
  /// </summary>
  public class CommandeListResponse : ResponseBase
  {
    /// <summary>
    /// La liste paginée des commandes
    /// </summary>
    public List<CommandeView> Commandes { get; set; }
  }
}
