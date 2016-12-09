using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Lectures
{
  /// <summary>
  /// Classe requête pour la finalisation d'une livraison (l'association du client)
  /// </summary>
  [Api("Casque")]
  [Route("/finaliseLivraison/{ApiKey}", Verbs = "POST")]
  public class FinaliseLivraisonRequest : RequestBase, IReturn<FinaliseLivraisonResponse>
  {
    /// <summary>
    /// La clé de la livraison
    /// </summary>
    public int LivraisonCle { get; set; }

    /// <summary>
    /// La clé du client
    /// </summary>
    public int ClientCle { get; set; }

    /// <summary>
    /// Active l'envoie par email du BL ou pas
    /// </summary>
    public bool ProcessEnvoie { get; set; }
  }
}
