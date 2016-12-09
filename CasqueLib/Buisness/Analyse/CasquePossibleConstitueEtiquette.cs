namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Etiquette lue lors de l'assemblage d'un casque
  /// Utilisé lors de l'assemblage
  /// </summary>
  public class CasquePossibleConstitueEtiquette
  {
    /// <summary>
    /// Clé de l'étiquette
    /// </summary>
    public int EtiquetteCle { get; set; }

    /// <summary>
    /// Numero du tag attendu
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
