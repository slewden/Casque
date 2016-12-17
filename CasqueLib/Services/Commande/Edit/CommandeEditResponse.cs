using System.Collections.Generic;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;
using CasqueLib.Common;

namespace CasqueLib.Services.Commande.Edit
{
  /// <summary>
  /// Le resultat des traitements CRUD d'une commande
  /// </summary>
  public class CommandeEditResponse
  {
    /// <summary>
    /// La commande manipulée
    /// </summary>
    public CommandeView Commande { get; set; }

    /// <summary>
    /// La liste des fournisseurs
    /// </summary>
    public List<NomCle> Fournisseurs { get; set; }

    /// <summary>
    /// La liste des postes possibles
    /// </summary>
    public List<Poste> Postes { get; set; }

    /// <summary>
    /// Indique que la configuration pour envoie de l'email permet de le faire
    /// </summary>
    public bool ConfigEmailOk { get; set; }

    /// <summary>
    /// renvoie la configuration qui donne la quantité maxi par ligne
    /// </summary>
    public int CommandeLigneQuantiteMax { get; set; }

    /// <summary>
    /// Le nom du fichier Excel associé à la commande (remplit ssi la demande contient Excel=1)
    /// </summary>
    public string ExcelFileUrl { get; set; }
  }
}
