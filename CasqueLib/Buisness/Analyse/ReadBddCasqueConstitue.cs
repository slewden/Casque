using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Classe de lecture en base pour la constitution d'un casque
  /// </summary>
  public class ReadBddCasqueConstitue
  {
    /// <summary>
    /// La clé du casque
    /// </summary>
    [Alias("casq_id")]
    public int CasqueCle { get; set; }

    /// <summary>
    /// La clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// Nom du type de pièce
    /// </summary>
    [Alias("typi_nom")]
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Description du type de pièce
    /// </summary>
    [Alias("typi_description")]
    public string TypePieceDescription { get; set; }

    /// <summary>
    /// Code du type de pièce
    /// </summary>
    [Alias("typi_code")]
    public string TypePieceCode { get; set; }

    /// <summary>
    /// Photo du type de pièce
    /// </summary>
    [Alias("typi_photo")]
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// La clé de la couleur
    /// </summary>
    [Alias("coul_id")]
    public int CouleurCle { get; set; }

    /// <summary>
    /// Nom de la couleur
    /// </summary>
    [Alias("coul_nom")]
    public string CouleurNom { get; set; }

    /// <summary>
    /// Description de la couleur
    /// </summary>
    [Alias("coul_description")]
    public string CouleurDescription { get; set; }

    /// <summary>
    /// Code de la couleur
    /// </summary>
    [Alias("coul_code")]
    public string CouleurCode { get; set; }

    /// <summary>
    /// La clé de la taille
    /// </summary>
    [Alias("tail_id")]
    public int TailleCle { get; set; }

    /// <summary>
    /// Nom de la taille
    /// </summary>
    [Alias("tail_nom")]
    public string TailleNom { get; set; }

    /// <summary>
    /// Description de la taille
    /// </summary>
    [Alias("tail_description")]
    public string TailleDescription { get; set; }

    /// <summary>
    /// Code de la taille
    /// </summary>
    [Alias("tail_code")]
    public string TailleCode { get; set; }

    /// <summary>
    /// La clé de l'étiquette
    /// </summary>
    [Alias("etqu_id")]
    public int EtiquetteCle { get; set; }

    /// <summary>
    /// Le numero de tag lu
    /// </summary>
    [Alias("etqu_numero")]
    public string Numero { get; set; }
    
    /// <summary>
    /// Le type d'état du tag et l'opération attendue :
    /// 1 : Tag commandé, attente de reception
    /// 2 : reçu, attente d'assemblage
    /// 3 : assemblé, attente de livraison
    /// 4 : livré
    /// </summary>
    [Alias("tag_operation")]
    public int OperationInt { get; set; }
  }
}
