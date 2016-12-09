using System.Collections.Generic;
using CasqueLib.Buisness;
using CasqueLib.Buisness.View;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe réponse pour le web service de reception de pièces
  /// </summary>
  public class UtilisationPosteResponse
  {
    /// <summary>
    /// Message s'il est remplit doit : 
    /// - Etre affiché 
    /// - Empécher le démarrage de l'opération
    /// </summary>
    public string MessageBloquant { get; set; }
    
    /// <summary>
    /// La liste des postes possibles
    /// </summary>
    public List<Poste> Postes { get; set; }

    /// <summary>
    /// La clé de l'utilisation poste créé
    /// </summary>
    public int UtilisationPosteCle { get; set; }

    /// <summary>
    /// Renvoie la clé de l'assemblage créé
    /// </summary>
    public int AssemblageCle { get; set; }

    /// <summary>
    /// Renvoie les infos de la livraison créé ou complétée
    /// </summary>
    public LivraisonView Livraison { get; set; }

    /// <summary>
    /// La liste des cartons utilisables pour les livraisons
    /// </summary>
    public List<Carton> Cartons { get; set; }

    /// <summary>
    /// La liste des clients
    /// </summary>
    public List<ClientView> Clients { get; set; }

    /// <summary>
    /// Indique de la configuration d'envoie des email est ok
    /// </summary>
    public bool ConfigEmailOk { get; set; }
  }
}
