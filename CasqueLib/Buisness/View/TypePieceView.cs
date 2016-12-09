using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe la vue TypePièce
  /// </summary>
  [Alias("v_type_piece")]
  public class TypePieceView : TypePiece
  {
    /// <summary>
    /// Nombre de taille disponibles
    /// </summary>
    [Alias("nb_taille")]
    public int NombreTaille { get; set; }

    /// <summary>
    /// Nombre de couleurs disponibles
    /// </summary>
    [Alias("nb_couleur")]
    public int NombreCouleur { get; set; }

    /// <summary>
    /// Nombre de ligne commande qui contienent cette pièces
    /// </summary>
    [Alias("nb_piece_commande")]
    public int NombreCommande { get; set; }

    /// <summary>
    /// Nombre de fournisseur pour cette pièce
    /// </summary>
    [Alias("nb_fournisseur")]
    public int NombreFournisseur { get; set; }

    /// <summary>
    /// Nombre de pièce recues
    /// </summary>
    [Alias("nb_piece_recue")]
    public int NombreStock { get; set; }

    /// <summary>
    /// Nombre de pièce dans un assemblage
    /// </summary>
    [Alias("nb_piece_utilisee")]
    public int NombreUtilisee { get; set; }

    /// <summary>
    /// Nombre de fois ou le type de pièce est utilisé
    /// </summary>
    [Ignore]
    public int NombreUtilisation
    {
      get
      {
        return this.NombreCommande + this.NombreStock + this.NombreUtilisee;
      }
    }

    /// <summary>
    /// La liste des clés des tailles associées au type de pièce
    /// </summary>
    [Ignore]
    public List<int> CleTailles { get; set; }

    /// <summary>
    /// La liste des clés de couleurs associées au type de pièce
    /// </summary>
    [Ignore]
    public List<int> CleCouleurs { get; set; }

    /// <summary>
    /// Renvoie l'objet bien typé
    /// </summary>
    /// <returns>Le bon objet</returns>
    public TypePiece ToTypePiece()
    {
      return new TypePiece()
      {
        Cle = this.Cle,
        Nom = this.Nom,
        Code = this.Code,
        AvecTag = this.AvecTag,
        Description = this.Description,
        Photo = this.Photo,
      };
    }
  }
}
