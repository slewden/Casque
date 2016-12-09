using System.Collections.Generic;
using System.Linq;
using CasqueLib.Buisness;
using CasqueLib.Buisness.Analyse;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe réponse pour le web service de reception de pièces
  /// </summary>
  public class AnalyseResponse
  {
    /// <summary>
    /// La liste des tags non reconnus
    /// (Rempli à chaque fois s'il y a lieu)
    /// </summary>
    public List<string> TagInconnus { get; set; }

    /// <summary>
    /// La clé de l'utilisation poste créé
    /// (Rempli à chaque fois)
    /// </summary>
    public int UtilisationPosteCle { get; set; }

    /// <summary>
    /// Les commandes qui contient le tag
    /// (Remplis dans l'interface réception)
    /// </summary>
    public List<DetailCommande> Commandes { get; set; }
    
    /// <summary>
    /// La liste des casques à constituer possibles
    /// </summary>
    public List<CasquePossible> Casques { get; set; }

    /// <summary>
    /// renvoie le nombre de casque trouvé complets
    /// </summary>
    public int NombreCasqueComplet
    {
      get
      { 
        if (this.Casques != null && this.Casques.Any())
        { // Compte !
          return this.Casques.Count(x => x.Complet);
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// Renvoie le nombre de casques éligibles
    /// </summary>
    public int NombreCasqueEligible
    { 
      get
      {
        if (this.Casques != null && this.Casques.Any())
        { // Compte !
          return this.Casques.Count(x => x.Eligible);
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// La liste des assemblages livrable
    /// </summary>
    public List<TechniqueAssemblage> Assemblages { get; set; }

    /// <summary>
    /// La liste des cartons utilisables pour les livraisons
    /// </summary>
    public List<Carton> Cartons { get; set; }

    /// <summary>
    /// Les historiques des tag lus
    /// </summary>
    public List<ConsultationEtiquette> TagConnus { get; set; }
  }
}
