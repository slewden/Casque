namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Le détail d'une tag lu dans une commande
  /// Utilisé lors de la reception des pièces
  /// </summary>
  public class DetailCommandeTagLu
  {
    /// <summary>
    /// Le Numéro de tag lu
    /// </summary>
    public string Numero { get; set; }

    /// <summary>
    /// Le type d'état du tag et l'opération attendue :
    /// 1 : Tag commandé, attente de reception
    /// 2 : reçu, attente d'assemblage
    /// 3 : assemblé, attente de livraison
    /// 4 : livré
    /// </summary>
    public int StatutInt { get; set; }
  }
}
