using System;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Les infos pour un client du nombre de livraisons
  /// </summary>
  [Alias("v_client_livraison")]
  public class ClientLivraisonView : ClientView
  {
    /// <summary>
    /// Nombre de livraison pour ce client
    /// </summary>
    [Alias("nombre")]
    public int Nombre { get; set; }

    /// <summary>
    /// Nombre de pièces livées pour ce client
    /// </summary>
    [Alias("nombre_piece")]
    public int NombrePiece { get; set; }

    /// <summary>
    /// Dernière date de livraison
    /// </summary>
    [Alias("dernier_livraison")]
    public DateTime DerniereLivraison { get; set; }
  }
}
