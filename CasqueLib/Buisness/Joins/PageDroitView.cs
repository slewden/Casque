using System.Collections.Generic;
using System.Linq;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Joins
{
  /// <summary>
  /// Mappe une vue simplifiée d'un droit d'un sur sur une page
  /// </summary>
  public class PageDroitView : Page
  {
    /// <summary>
    /// La clé de l'utilisateur
    /// </summary>
    [Alias("util_id")]
    public int? UtilisateurCle { get; set; }

    /// <summary>
    /// Niveau d'acces en Int
    /// </summary>
    [Alias("page_access")]
    public int? AccesInt { get; set; }

    /// <summary>
    /// Niveau d'accès (typé)
    /// </summary>
    [Ignore]
    public ETypeAccess Acces
    {
      get
      {
        if (this.AccesInt == null)
        {
          return 0;
        }
        else
        {
          return (ETypeAccess)this.AccesInt;
        }
      }
    }

    /// <summary>
    /// Renvoie les SQL pour synchroniser la liste
    /// </summary>
    /// <param name="utilisateurCle">Le clé de l'utilisateur</param>
    /// <param name="newCles">La liste des tailles choisies</param>
    /// <returns>Le Sql</returns>
    public static string GetSqlSynchronise(int utilisateurCle, List<PageDroitView> newCles)
    {
      if (utilisateurCle > 0)
      {
        string insert = string.Empty;
        if (newCles != null && newCles.Any())
        {
          var valid = newCles.Where(x => (x.AccesInt ?? 0) > 0);

          if (valid != null && valid.Any())
          {
            insert = string.Format(
              " INSERT INTO @ids (code, access) VALUES {0}; ",
              valid
                .Select(x => string.Format("({0}, {1})", SqlFormat.String(x.Cle), x.AccesInt))
                .Aggregate((x, y) => x + "," + y));
          }

          return string.Format(
    @"
  DECLARE @ids AS ListCodePageIntTable;
  {0}
  EXEC dbo.synchronise_page_droit {1}, @ids;
",
                     insert,
                     utilisateurCle);
        }
      }

      return null;
    }
  }
}
