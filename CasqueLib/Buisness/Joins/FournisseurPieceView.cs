using System.Collections.Generic;
using System.Linq;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.Joins
{
  /// <summary>
  /// Mappe une vue simplifiée d'une pièce fournie par un fournisseur
  /// </summary>
  public class FournisseurPieceView : TypePiece
  {
    /// <summary>
    /// La clé du fournisseur
    /// </summary>
    [Alias("clfo_id")]
    public int? FournisseurCle { get; set; }

    /// <summary>
    /// Prix unitaire de la pièce chez le fournisseur
    /// </summary>
    [Alias("fopi_prix_unitaire")]
    public decimal? PrixUnitaire { get; set; }

    /// <summary>
    /// Montant des frais à chaque commande
    /// </summary>
    [Alias("fopi_frais_commande")]
    public decimal? Frais { get; set; }

    /// <summary>
    /// Le nombre de couleur possible pour cette pièce
    /// </summary>
    [Alias("nb_couleur")]
    public int NombreCouleur { get; set; }

    /// <summary>
    /// Le nombre de taille possible pour cette pièce
    /// </summary>
    [Alias("nb_taille")]
    public int NombreTaille { get; set; }

    /// <summary>
    /// Renvoie le SQL pour synchroniser la liste
    /// </summary>
    /// <param name="fournisseurCle">Le clé de l'utilisateur</param>
    /// <param name="newCles">La liste des tailles choisies</param>
    /// <returns>Le Sql</returns>
    public static string GetSqlSynchronise(int fournisseurCle, List<FournisseurPieceView> newCles)
    {
      if (fournisseurCle > 0)
      {
        string insert = string.Empty;
        if (newCles != null && newCles.Any())
        {
          var valid = newCles.Where(x => (x.FournisseurCle ?? 0) > 0);

          if (valid != null && valid.Any())
          {
            insert = string.Format(
              " INSERT INTO @ids (typiId, prix, frais) VALUES {0}; ",
              valid
                .Select(x => string.Format("({0}, {1}, {2})", x.Cle, SqlFormat.Decimal(x.PrixUnitaire ?? 0), SqlFormat.Decimal(x.Frais ?? 0)))
                .Aggregate((x, y) => x + "," + y));
          }

          return string.Format(
    @"
  DECLARE @ids AS ListProduitPrixTable;
  {0}
  EXEC dbo.synchronise_produit_fournisseur {1}, @ids;
",
                     insert,
                     fournisseurCle);
        }
      }

      return null;
    }
  }
}
