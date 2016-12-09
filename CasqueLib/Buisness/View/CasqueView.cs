using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe la vue casque
  /// </summary>
  [Alias("v_casque")]
  public class CasqueView : Casque
  {
    /// <summary>
    /// Nombre de pièces qui constituent le casque
    /// </summary>
    [Alias("nb_piece")]
    public int NombrePiece { get; set; }
    
    /// <summary>
    /// Nombre de tailles dans les pièces qui constituent le casque
    /// </summary>
    [Alias("nb_taille")]
    public int NombreTaille { get; set; }
    
    /// <summary>
    /// Nombre de couleur dans les pièces qui constituent le casque
    /// </summary>
    [Alias("nb_couleur")]
    public int NombreCouleur { get; set; }

    /// <summary>
    /// Nombre d'assemblages en stock
    /// </summary>
    [Alias("nb_assemblage_stock")]
    public int NombreAssemblageStock { get; set; }

    /// <summary>
    /// Nombre d'assemblages livrés
    /// </summary>
    [Alias("nb_assemblage_livre")]
    public int NombreAssemblageLivre { get; set; }

    /// <summary>
    /// Nombre d'assemblage total pour ce casque
    /// </summary>
    [Ignore]
    public int NombreAssemblage
    {
      get
      {
        return this.NombreAssemblageStock + this.NombreAssemblageLivre;
      }
    }

    /// <summary>
    /// Renvoie l'objet bien typé
    /// </summary>
    /// <returns>Le bon objet</returns>
    public Casque ToCasque()
    {
      return new Casque()
      {
        Cle = this.Cle,
        Nom = this.Nom,
        Code = this.Code,
        Description = this.Description,
        Photo = this.Photo,
      };
    }
  }
}
