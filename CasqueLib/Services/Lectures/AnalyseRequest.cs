using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe requête pour gestion de quoi faire après une lecture de tag
  /// info à l'arrivée d'un tag, enregistrement d'une action (fonction du pageCode)
  /// </summary>
  [Api("Casque")]
  [Route("/analyseTag/{ApiKey}", Verbs = "GET")]
  [Route("/analyseCloture/{ApiKey}", Verbs = "POST")]
  public class AnalyseRequest : RequestBase, IReturn<AnalyseResponse>
  {
    /// <summary>
    /// Le code de la page qui fait la demande
    /// (Reception, Assemblage, ... Voir classe CasqueLib.Buisness.Poste.GetNomAffectation)
    /// </summary>
    public string PageCode { get; set; }
    
    /// <summary>
    /// La clé du poste choisi
    /// </summary>
    public int PosteCle { get; set; }

    /// <summary>
    /// Clé de l'utilisation poste à supprimer ou valider
    /// </summary>
    public int UtilisationPosteCle { get; set; }

    /// <summary>
    /// La clé de l'utilisateur
    /// </summary>
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// Le ou les tags lus
    /// </summary>
    public List<string> Tags { get; set; }

    /// <summary>
    /// La clé de l'objet choisi (fonction de PageCode)
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Les tags lus
    /// </summary>
    public List<string> Lectures { get; set; }

    /// <summary>
    /// Uniquement pour les livraisons : fourni la clé de la livraison en cours
    /// </summary>
    public int LivraisonCle { get; set; }

    /// <summary>
    /// Uniquement pour les livraisons : fourni l'indexu du carton scanné
    /// </summary>
    public int CartonIndex { get; set; }
  }
}
