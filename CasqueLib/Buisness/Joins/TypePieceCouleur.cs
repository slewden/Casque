using System.Collections.Generic;
using System.Linq;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Joins
{
  /// <summary>
  /// Table de jointure : Type_piece Couleur
  /// </summary>
  [Alias("type_piece_couleur")]
  public class TypePieceCouleur
  {
    /// <summary>
    /// Clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }

    /// <summary>
    /// Clé de la taille
    /// </summary>
    [Alias("coul_id")]
    public int CouleurCle { get; set; }

    /// <summary>
    /// Renvoie les SQL pour synchroniser la liste
    /// </summary>
    /// <param name="typePieceCle">Le clé du type de pièce</param>
    /// <param name="newCles">La liste des couleurs choisies</param>
    /// <returns>Le sql</returns>
    public static string GetSqlSynchronise(int typePieceCle, List<int> newCles)
    {
      if (typePieceCle > 0)
      {
        string insert = string.Empty;
        if (newCles != null && newCles.Any())
        {
          insert = string.Format(" INSERT INTO @ids (id) VALUES {0}; ", newCles.Select(x => string.Format("({0})", x)).Aggregate((x, y) => x + "," + y));
        }

        return string.Format(
  @"
  DECLARE @ids AS ListIntTable;
  {0}
  EXEC dbo.synchronise_type_piece_couleur {1}, @ids;
",
                   insert,
                   typePieceCle);
      }
      else
      {
        return null;
      }
    }
  }
}
