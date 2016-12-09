using System.Collections.Generic;
using System.Linq;
using CasqueLib.Buisness.View;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Joins
{
  /// <summary>
  /// Mappe une pièce qui fait partie de ce qu'il doit y avoir dans un casque
  /// </summary>
  public class CasqueConstitueView : TypePieceView
  {
    /// <summary>
    /// Clé du casque
    /// </summary>
    [Alias("casq_id")]
    public int? CasqueCle { get; set; }

    /// <summary>
    /// Clé de la taille
    /// </summary>
    [Alias("tail_id")]
    public int? TailleCle { get; set; }

    /// <summary>
    /// Clé de la couleur
    /// </summary>
    [Alias("coul_id")]
    public int? CouleurCle { get; set; }

    /// <summary>
    /// La couleur choisie
    /// </summary>
    [Ignore]
    public CouleurTypePiece Couleur { get; set; }

    /// <summary>
    /// La taille choisie
    /// </summary>
    [Ignore]
    public TailleTypePiece Taille { get; set; }

    /// <summary>
    /// Les couleurs possible pour la pièce
    /// </summary>
    [Ignore]
    public List<CouleurTypePiece> Couleurs { get; set; }

    /// <summary>
    /// Les tailles possible pour la pièce
    /// </summary>
    [Ignore]
    public List<TailleTypePiece> Tailles { get; set; }

    /// <summary>
    /// Renvoie les SQL pour synchroniser la liste
    /// </summary>
    /// <param name="casqueCle">Le clé de l'utilisateur</param>
    /// <param name="newCles">La liste des tailles choisies</param>
    /// <returns>Le Sql</returns>
    public static string GetSqlSynchronise(int casqueCle, List<CasqueConstitueView> newCles)
    {
      if (casqueCle > 0)
      {
        string insert = string.Empty;
        if (newCles != null && newCles.Any())
        {
          var valid = newCles.Where(x => (x.CasqueCle ?? 0) > 0);

          if (valid != null && valid.Any())
          {
            insert = string.Format(
              " INSERT INTO @ids (typiId, tailId, coulId) VALUES {0}; ",
              valid
                .Select(x => string.Format(
                                      "({0}, {1}, {2})",
                                      x.Cle,
                                      SqlFormat.ForeignKey(x.Taille != null ? x.Taille.Cle : -1),
                                      SqlFormat.ForeignKey(x.Couleur != null ? x.Couleur.Cle : -1)))
                .Aggregate((x, y) => x + "," + y));
          }

          return string.Format(
    @"
  DECLARE @ids AS ListProduitCasqueTable;
  {0}
  EXEC dbo.synchronise_produit_casque {1}, @ids;
",
                     insert,
                     casqueCle);
        }
      }

      return null;
    }
  }
}
