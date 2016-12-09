using System.Collections.Generic;
using CasqueLib.Buisness.Joins;
using ServiceStack.ServiceHost;

namespace CasqueLib.Services.Parametrage.FournisseurPiece
{
  /// <summary>
  /// Classe requête pour la gestion des type de pièce fournies par un fournisseur
  /// </summary>
  [Api("Casque")]
  [Route("/fournisseurPieceGet/{ApiKey}/", Verbs = "GET")]
  [Route("/fournisseurPieceEdit/{ApiKey}/", Verbs = "POST")]
  public class FournisseurPieceRequest : RequestBase, IReturn<FournisseurPieceResponse>
  {
    /// <summary>
    /// Clé du fournisseur pour les opérations CRUD
    /// </summary>
    public int Cle { get; set; }

    /// <summary>
    /// Les pièces fournies
    /// </summary>
    public List<FournisseurPieceView> Pieces { get; set; }
  }
}
