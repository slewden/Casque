using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Livraison.Detail
{
  /// <summary>
  /// Classe requête pour le détail d'une livraison
  /// </summary>
  [Api("Casque")]
  [Route("/livraisonDetail/{ApiKey}", Verbs = "GET")]
  [Route("/livraisonEmail/{ApiKey}", Verbs = "POST")]
  public class LivraisonDetailRequest : RequestBase, IReturn<LivraisonDetailResponse>
  {
    /// <summary>
    /// La clé de la livraison
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Une adresse email supplémentaire pour l'envoie du BL
    /// </summary>
    public string EmailSuplementaire { get; set; }
  }
}
