using System.Collections.Generic;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe les infos d'une ligne de commande
  /// </summary>
  [Alias("v_commande_ligne")]
  public class CommandeLigneView : CommandeLigne
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="CommandeLigneView"/>
    /// </summary>
    public CommandeLigneView()
    {
      this.Guid = System.Guid.NewGuid().ToString().Replace("-", "_");
    }

    /// <summary>
    /// Identifiant unique de la ligne
    /// </summary>
    [Ignore]
    public string Guid { get; private set; }

    /// <summary>
    /// Nom du type de pièce
    /// </summary>
    [Alias("typi_nom")]
    public string TypePieceNom { get; set; }
    
    /// <summary>
    /// Code du type de pièce
    /// </summary>
    [Alias("typi_code")]
    public string TypePieceCode { get; set; }

    /// <summary>
    /// Avec une taille ou pas
    /// </summary>
    [Alias("typi_avec_tag")]
    public bool TypePieceAvecTag { get; set; }

    /// <summary>
    /// Description du type de pièce
    /// </summary>
    [Alias("typi_description")]
    public string TypePieceDescription { get; set; }

    /// <summary>
    /// Photo du type de pièce
    /// </summary>
    [Alias("typi_photo")]
    public string TypePiecePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string TypePiecePhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.TypePiece, this.TypePiecePhoto);
      }
    }

    /// <summary>
    /// l'objet Couleur
    /// </summary>
    [Ignore]
    public NomCle Couleur
    {
      get
      {
        if (!this.CouleurCle.HasValue || this.CouleurCle.Value <= 0)
        {
          return null;
        }
        else
        {
          return new NomCle() { Cle = this.CouleurCle.Value, Nom = this.CouleurNom };
        }
      }
    }

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

    /// <summary>
    /// La description de la couleur
    /// </summary>
    [Alias("coul_description")]
    public string CouleurDescription { get; set; }

    /// <summary>
    /// Nombre de couleurs disponibles pour cette pièce
    /// </summary>
    [Alias("nb_couleur")]
    public int CouleurNombre { get; set; }

    /// <summary>
    /// La liste des couleurs possibles pour cette pièce
    /// </summary>
    [Ignore]
    public List<NomCle> Couleurs { get; set; }

    /// <summary>
    /// l'objet Taille
    /// </summary>
    [Ignore]
    public NomCle Taille
    {
      get
      {
        if (!this.TailleCle.HasValue || this.TailleCle.Value <= 0)
        {
          return null;
        }
        else
        {
          return new NomCle() { Cle = this.TailleCle.Value, Nom = this.TailleNom };
        }
      }
    }

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
    /// Description de la taille
    /// </summary>
    [Alias("tail_description")]
    public string TailleDescription { get; set; }

    /// <summary>
    /// Nombre de tailles disponibles pour cette pièce
    /// </summary>
    [Alias("nb_taille")]
    public int TailleNombre { get; set; }

    /// <summary>
    /// La liste des tailles possibles pour cette pièce
    /// </summary>
    [Ignore]
    public List<NomCle> Tailles { get; set; }

    /// <summary>
    /// Les n° d'étiquette associés à la ligne
    /// Uniqument rempli lors de l'envoie des emails
    /// </summary>
    [Ignore]
    public List<string> Etiquettes { get; set; }
  }
}
