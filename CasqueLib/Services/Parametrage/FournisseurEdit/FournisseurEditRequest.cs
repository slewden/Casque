using CasqueLib.Buisness.View;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.FournisseurEdit
{
  /// <summary>
  /// Classe requête pour la gestion d'un fournisseur
  /// </summary>
  [Api("Casque")]
  [Route("/fournisseurGet/{ApiKey}/", Verbs = "GET")]
  [Route("/fournisseurEdit/{ApiKey}/", Verbs = "POST")]
  [Route("/fournisseurDelete/{ApiKey}/", Verbs = "DELETE")]
  public class FournisseurEditRequest : RequestBase, IReturn<FournisseurEditResponse>
  {
    /// <summary>
    /// Clé du fournisseur pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Le fournisseur édité
    /// </summary>
    public FournisseurView Fournisseur { get; set; }
  }
}
