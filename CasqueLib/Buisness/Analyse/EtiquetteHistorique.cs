using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Classe pour mémorise les infos d'historique d'un tag
  /// </summary>
  public class EtiquetteHistorique
  {
    /// <summary>
    /// Le Numero d'étiquette
    /// </summary>
    [Alias("numero")]
    public string Numero { get; set; }

    /// <summary>
    /// La clé de l'étiquette
    /// </summary>
    [Alias("etqu_id")]
    public int EtiquetteCle { get; set; }

    /// <summary>
    /// La clé de la commande
    /// </summary>
    [Alias("comd_id")]
    public int CommandeCle { get; set; }

    /// <summary>
    /// La référence de la ligne de comande
    /// </summary>
    [Alias("colg_reference")]
    public string Reference { get; set; }

    /// <summary>
    /// La clé de l'assemblage
    /// </summary>
    [Alias("asse_id")]
    public int AssemblageCle { get; set; }

    /// <summary>
    /// La clé de la livraison
    /// </summary>
    [Alias("livr_id")]
    public int LivraisonCle { get; set; }
  }
}
