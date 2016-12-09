using System.Text;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe une couleur
  /// </summary>
  [Alias("couleur")]
  public class Couleur
  {
    #region Properties
    /// <summary>
    /// La clé de la couleur
    /// </summary>
    [AutoIncrement]
    [Alias("coul_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Le nom de la couleur
    /// </summary>
    [Alias("coul_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Le code de la couleur
    /// </summary>
    [Alias("coul_code")]
    public string Code { get; set; }

    /// <summary>
    /// La description de la couleur
    /// </summary>
    [Alias("coul_description")]
    public string Description { get; set; }

    /// <summary>
    /// renvoie le code couleur opposé pour les écritures à l'intérieur
    /// </summary>
    [Ignore]
    public string CodeeOppose
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.Code))
        {
          return this.Code;
        }

        string frm = "0123456789ABCDEF";
        string too = "FEDCBA9876543210";
        string txt = this.Code.ToUpper();
        StringBuilder res = new StringBuilder();
        int pos;
        foreach (char c in this.Code)
        {
          pos = frm.IndexOf(c);
          if (pos >= 0)
          { // trouvé
            res.Append(too[pos]);
          }
          else
          { // pas trouvé on ajoute le même 
            res.Append(c);
          }
        }

        return res.ToString();
      }
    }
    #endregion

    /// <summary>
    /// Indique si l'objet est correctement remplit pour se sauvegarder en bdd
    /// </summary>
    /// <returns>True si complet</returns>
    public bool IsComplet()
    {
      return !string.IsNullOrWhiteSpace(this.Nom) && !string.IsNullOrWhiteSpace(this.Code);
    }
  }
}
