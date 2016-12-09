using System.Globalization;

namespace CasqueLib.Common
{
  /// <summary>
  /// Classe pour le formattage des infos SQL
  /// </summary>
  public static class SqlFormat
  {
    /// <summary>
    /// Met en forme une chaine
    /// </summary>
    /// <param name="text">le texte à formatté</param>
    /// <returns>le texte formatté</returns>
    public static string String(string text)
    {
      if (string.IsNullOrWhiteSpace(text))
      {
        return "NULL";
      }
      else
      {
        return string.Format("'{0}'", text.Replace("'", "''"));
      }
    }

    /// <summary>
    /// Met en forme un décimal
    /// </summary>
    /// <param name="mtx">le nombre à formatter</param>
    /// <returns>le texte formatté</returns>
    public static string Decimal(decimal mtx)
    {
      if (mtx == decimal.MinValue || mtx == decimal.MaxValue)
      {
        return "NULL";
      }
      else
      {
        return string.Format("{0:N2}", mtx).Replace(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator, ".");
      }
    }

    /// <summary>
    /// Met en forme un décimal
    /// </summary>
    /// <param name="mtx">le nombre à formatter</param>
    /// <returns>le texte formatté</returns>
    public static string Decimal(decimal? mtx)
    {
      return SqlFormat.Decimal(mtx ?? decimal.MinValue);
    }

    /// <summary>
    /// Renvoie une valeur de foreign key valide (null si inférieur ou égal à 0)
    /// </summary>
    /// <param name="cle">La clé</param>
    /// <returns>le texte formatté</returns>
    public static string ForeignKey(int cle)
    {
      if (cle <= 0)
      {
        return "NULL";
      }
      else
      {
        return cle.ToString();
      }
    }

    /// <summary>
    /// Renvoie une valeur de foreign key valide (null si inférieur ou égal à 0)
    /// </summary>
    /// <param name="cle">La clé</param>
    /// <returns>le texte formatté</returns>
    public static string ForeignKey(int? cle)
    {
      return SqlFormat.ForeignKey(cle ?? -1);
    }

    /// <summary>
    /// Renvoie une valeur de foreign key valide (null si inférieur ou égal à 0)
    /// </summary>
    /// <param name="cle">La clé</param>
    /// <returns>le texte formatté</returns>
    public static string ForeignKey(string cle)
    {
      if (!string.IsNullOrWhiteSpace(cle))
      {
        int n;
        if (int.TryParse(cle, out n))
        {
          return SqlFormat.ForeignKey(n);
        }
      }

      return SqlFormat.ForeignKey(-1);
    }
  }
}
