using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Info sur un tag lu : viens de étiquette ou d'un assemblage (ou aucun des 2 si inconnu)
  /// </summary>
  public class ReadBddTagInfo
  {
    /// <summary>
    /// Clé de la commande
    /// </summary>
    [Alias("comd_id")]
    public int CommandeCle { get; set; }

    /// <summary>
    /// Numéro de la commande
    /// </summary>
    [Alias("comd_numero")]
    public string CommandeNumero { get; set; }

    /// <summary>
    /// client de la commande
    /// </summary>
    [Alias("clfo_nom")]
    public string CommandeClientNom { get; set; }

    /// <summary>
    /// La clé de l'étiquette du tag 
    /// si 0 : voir assemblage pour savoir d'ou vient le tag
    /// </summary>
    [Alias("etqu_id")]
    public int EtiquetteCle { get; set; }

    /// <summary>
    /// La clé de l'assemblage auquel appartient le tag
    /// Si = 0  et si EtiquetteCle = 0 alors c'est un tag inconnu
    /// Si = 0  et si EtiquetteCle différent de 0 alors c'est un tag non assemblé
    /// Si différent de 0 si EtiquetteCle = 0 alors c'est un tag de casque (le principal de l'assemblage)
    /// Si différent de 0 si EtiquetteCle différent de 0 alors c'est un tag d'une pièce d'un assemblage
    /// </summary>
    [Alias("asse_id")]
    public int AssemblageCle { get; set; }

    /// <summary>
    /// Le numéro du tag
    /// </summary>
    [Alias("tag_numero")]
    public string Numero { get; set; }

    /// <summary>
    /// Clé du casque quand on lit une étiquette casque
    /// </summary>
    [Alias("casq_id")]
    public int CasqueCle { get; set; }
  }
}
