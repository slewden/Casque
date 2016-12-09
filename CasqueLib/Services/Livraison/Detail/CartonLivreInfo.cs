using System.Collections.Generic;
using CasqueLib.Buisness;

namespace CasqueLib.Services.Livraison.Detail
{
  /// <summary>
  /// Le détail d'un carton livré
  /// </summary>
  public class CartonLivreInfo : Carton
  {
    /// <summary>
    /// L'index du carton
    /// </summary>
    public int CartonIndex { get; set; }

    /// <summary>
    /// La liste des casques dans le carton
    /// </summary>
    public List<CasqueLivreInfo> Casques { get; set; }

    /// <summary>
    /// Pour les comparaisons
    /// </summary>
    /// <param name="obj">l'objet à comparer</param>
    /// <returns>True si c'est les mêmes</returns>
    public override bool Equals(object obj)
    {
      CartonLivreInfo c = obj as CartonLivreInfo;
      if ((object)c == null)
      {
        return false;
      }
      else
      {
        return this.Cle == c.Cle && this.CartonIndex == c.CartonIndex;
      }
    }

    /// <summary>
    /// Pour les distinct dans le linq
    /// </summary>
    /// <returns>le code de hash</returns>
    public override int GetHashCode()
    {
      return string.Format("{0}-{1}", this.Cle, this.CartonIndex).GetHashCode();
    }
  }
}
