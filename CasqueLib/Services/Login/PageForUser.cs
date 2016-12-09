using System.Collections.Generic;
using System.Data;
using System.Linq;
using CasqueLib.Buisness;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CasqueLib.Services.Login
{
  /// <summary>
  /// Classe qui mappe un accès à une page
  /// </summary>
  [Alias("v_page_for_user")]
  public class PageForUser : Page
  {
    /// <summary>
    /// La clé de l'utilisateur
    /// </summary>
    [Alias("util_id")]
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// Niveau d'acces en Int
    /// </summary>
    [Alias("page_access")]
    public int AccesInt { get; set; }

    /// <summary>
    /// Niveau d'accès (typé)
    /// </summary>
    [Ignore]
    public ETypeAccess Acces
    {
      get
      {
        return (ETypeAccess)this.AccesInt;
      }

      set
      {
        this.AccesInt = (int)value;
      }
    }

    /// <summary>
    /// Charge les menus d'un utilisateur
    /// </summary>
    /// <param name="db">La connexion à la base</param>
    /// <param name="utilisateurCle">La clé de l'utilisateur</param>
    /// <returns>La liste</returns>
    public static List<PageForUser> Load(IDbConnection db, int utilisateurCle)
    {
      return db.Select<PageForUser>(x => x.UtilisateurCle == utilisateurCle && x.AccesInt >= (int)ETypeAccess.Voir).OrderBy(x => x.Ordre).ToList();
    }
  }
}
