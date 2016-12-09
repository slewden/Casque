using System.Collections.Generic;
using System.Linq;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Joins
{
  /// <summary>
  /// Table de jointure : Type_piece Taille
  /// </summary>
  [Alias("type_piece_taille")]
  public class TypePieceTaille
  {
    /// <summary>
    /// Clé du type de pièce
    /// </summary>
    [Alias("typi_id")]
    public int TypePieceCle { get; set; }
    
    /// <summary>
    /// Clé de la taille
    /// </summary>
    [Alias("tail_id")]
    public int TailleCle { get; set; }

    /// <summary>
    /// Renvoie les SQL pour synchroniser la liste
    /// </summary>
    /// <param name="typePieceCle">Le clé du type de pièce</param>
    /// <param name="newCles">La liste des tailles choisies</param>
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
  EXEC dbo.synchronise_type_piece_taille {1}, @ids;
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
