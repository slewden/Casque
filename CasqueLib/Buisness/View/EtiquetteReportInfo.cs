using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Classe qui mappe les informations de détail d'un assemblage
  /// (pour affichage uniquement lors des consultations de pièces)
  /// </summary>
  [Alias("v_etiquette_report_info")]
  public class EtiquetteReportInfo
  {
    /// <summary>
    /// Référence de la ligne de commande
    /// </summary>
    [Alias("colg_reference")]
    public string Reference { get; set; }
    
    /// <summary>
    /// Numéro de l'étiquette
    /// </summary>
    [Alias("etqu_numero")]
    public string EtiquetteNumero { get; set; }

    /// <summary>
    /// Clé de l'assemblage concerné
    /// </summary>
    [Alias("asse_id")]
    public int AssemblageCle { get; set; }
    
    /// <summary>
    /// Nom du type de pièces
    /// </summary>
    [Alias("typi_nom")]
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Code du type de pièces
    /// </summary>
    [Alias("typi_code")]
    public string TypePieceCode { get; set; }

    /// <summary>
    /// Photo du type de pièces
    /// </summary>
    [Alias("typi_photo")]
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string PhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.TypePiece, this.TypePiecePhoto);
      }
    }

    /// <summary>
    /// Nom de la couleur
    /// </summary>
    [Alias("coul_nom")]
    public string CouleurNom { get; set; }

    /// <summary>
    /// Code de la couleur
    /// </summary>
    [Alias("coul_code")]
    public string CouleurCode { get; set; }

    /// <summary>
    /// Nom de la taille
    /// </summary>
    [Alias("tail_nom")]
    public string TailleNom { get; set; }

    /// <summary>
    /// Code de la taille
    /// </summary>
    [Alias("tail_code")]
    public string TailleCode { get; set; }

    /// <summary>
    /// Nom du fournisseur
    /// </summary>
    [Alias("clfo_nom")]
    public string FournisseurNom { get; set; }

    /// <summary>
    /// Email du fournisseur de la commande
    /// </summary>
    [Alias("clfo_email")]
    public string FournisseurEmail { get; set; }
    
    /// <summary>
    /// Numéro de la commande
    /// </summary>
    [Alias("comd_numero")]
    public string CommandeNumero { get; set; }
  }
}
