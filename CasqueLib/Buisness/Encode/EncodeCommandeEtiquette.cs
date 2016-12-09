using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Encode
{
  /// <summary>
  /// Infos utiles pour encoder une étiquette d'un pièce simple
  /// </summary>
  public class EncodeCommandeEtiquette
  {
    /// <summary>
    /// Le numéro de position dans la commande
    /// </summary>
    [Alias("numero")]
    public int Numero { get; set; }

    /// <summary>
    /// Le numéro d'étiquette
    /// </summary>
    [Alias("etiquette")]
    public string Etiquette { get; set; }

    /// <summary>
    /// La date d'encodage
    /// </summary>
    [Alias("date")]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// La clé de la ligne de commande concernée
    /// </summary>
    [Alias("colg_id")]
    public int CommandeLigneCle { get; set; }

    /// <summary>
    /// La référence de la ligne commandée
    /// </summary>
    [Alias("colg_reference")]
    public string CommandeLigneReference { get; set; }

    /// <summary>
    /// La clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// Le nom du type de pièce
    /// </summary>
    [Alias("typi_nom")]
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Le code du type de pièce
    /// </summary>
    [Alias("typi_code")]
    public string TypePieceCode { get; set; }

    /// <summary>
    /// La clé de la taille
    /// </summary>
    [Alias("tail_id")]
    public int TailleCle { get; set; }

    /// <summary>
    /// Le nom de la taille
    /// </summary>
    [Alias("tail_nom")]
    public string TailleNom { get; set; }

    /// <summary>
    /// Le code de la taille
    /// </summary>
    [Alias("tail_code")]
    public string TailleCode { get; set; }

    /// <summary>
    /// La clé de la couleur
    /// </summary>
    [Alias("coul_id")]
    public int CouleurCle { get; set; }

    /// <summary>
    /// Le nom de la couleur
    /// </summary>
    [Alias("coul_nom")]
    public string CouleurNom { get; set; }

    /// <summary>
    /// Le code de la couleur
    /// </summary>
    [Alias("coul_code")]
    public string CouleurCode { get; set; }
  }
}
