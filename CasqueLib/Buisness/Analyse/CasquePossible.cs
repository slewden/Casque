using System.Collections.Generic;
using System.Linq;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Les infos sur un casque possible 
  /// Utilisé lors de l'assemblage
  /// </summary>
  public class CasquePossible : Casque
  {
    /// <summary>
    /// Nombre de tag lus et reconnus
    /// </summary>
    private int nombreCompare;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="CasquePossible"/>
    /// </summary>
    /// <param name="nombreTagOkLus">Nombre de tags lus</param>
    public CasquePossible(int nombreTagOkLus)
    {
      this.nombreCompare = nombreTagOkLus;
    }
    
    /// <summary>
    /// Les pièces du casque
    /// </summary>
    public List<CasquePossibleConstitue> Pieces { get; set; }

    /// <summary>
    /// Indique si le casque est complet et valide
    /// </summary>
    public bool Complet
    {
      get
      {
        if (this.Pieces != null && this.Pieces.Any())
        {
          int nb = this.Pieces.Sum(x => x.NombreEtiquette == 1 ? 1 : 0);

          return nb == this.nombreCompare && this.Pieces.Count == this.nombreCompare; 
        }

        return false;
      }
    }

    /// <summary>
    ///  Indique si le casque est encore éligible
    /// </summary>
    public bool Eligible
    {
      get
      {
        if (this.Pieces != null && this.Pieces.Any())
        {
          int nb = this.Pieces.Sum(x => x.NombreEtiquette == 1 ? 1 : 0);
          return nb >= this.nombreCompare && this.Pieces.Count >= this.nombreCompare;
        }

        return false;
      }
    }
  }
}
