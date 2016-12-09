using System.Collections.Generic;
using System.Data;
using System.Text;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Liste des étiquettes pour les programme de debug
  /// --
  /// --- Documentation des données renvoyées PAR v_etiquette_debug
  /// --- tag_operation                          colg_id                      Nom
  /// ---  1 : commandé, attente de reception    clé de la ligne de commande  Type de pièce
  /// ---  2 : reçu, attente d'assemblage        clé de la ligne de commande  Type de pièce
  /// ---  3 : assemblé, attente de livraison    clé de l'assemblage          Type de pièce
  /// ---  4 : livré                             clé de la livraison          Type de pièce
  /// ---  5 : tag_numéro d'assemblage           clé de l'assemblage          'Casque ' + Nom du casque
  /// ---  6 : les tags par N° d'assemblage      Index de la composition      Nom du casque + '(' + Nombre de pièces dans la compo + ')'
  /// ---  7 : tag_numéro d'assemblage           clé de la livraison          'Casque ' + Nom du casque
  /// </summary>
  [Alias("v_etiquette_debug")]
  public class EtiquetteDebug
  {
    /// <summary>
    /// Numéro du tag
    /// </summary>
    [Alias("etqu_numero")]
    public string Numero { get; set; }

    /// <summary>
    /// La clé de la ligne de commande (pour les tris)
    /// </summary>
    [Alias("colg_id")]
    public long LigneCommandeCle { get; set; }

    /// <summary>
    /// Nom du type de pièce
    /// </summary>
    [Alias("typi_nom")]
    public string TypePieceNom { get; set; }

    /// <summary>
    /// Nom de la couleur du type de pièce
    /// </summary>
    [Alias("coul_nom")]
    public string CouleurNom { get; set; }

    /// <summary>
    /// Nom de la taille du type de pièce
    /// </summary>
    [Alias("tail_nom")]
    public string TailleNom { get; set; }

    /// <summary>
    /// Le type d'état du tag et l'opération attendue :
    /// 1 : Tag commandé, attente de reception
    /// 2 : reçu, attente d'assemblage
    /// 3 : assemblé, attente de livraison
    /// 4 : livré
    /// </summary>
    [Alias("tag_operation")]
    public int OperationInt { get; set; }
    
    /// <summary>
    /// pour les tris dans les listes
    /// </summary>
    [Ignore]
    public string Tri
    {
      get
      {
        if (this.OperationInt < 7)
        {
          return string.Format("{0:0000}-{1}-{2}-{3}", this.LigneCommandeCle, this.TypePieceNom, this.CouleurNom, this.TailleNom);
        }
        else
        { // c'est des casques
          return string.Format("{0}-{1:0000}-{2}-{3}", this.TypePieceNom, this.LigneCommandeCle, this.CouleurNom, this.TailleNom);
        }
      }
    }

    /// <summary>
    /// Renvoie les étiquette debug
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <returns>La liste des étiquette debug</returns>
    public static List<EtiquetteDebug> LoadAll(IDbConnection db)
    {
      return db.Select<EtiquetteDebug>();
    }

    /// <summary>
    /// Pour affichage
    /// </summary>
    /// <returns>le texte à afficher</returns>
    public override string ToString()
    {
      StringBuilder res = new StringBuilder();
      res.AppendFormat("{0} : {1:0000} - {2}", this.Numero, this.LigneCommandeCle, this.TypePieceNom);
      if (!string.IsNullOrWhiteSpace(this.CouleurNom))
      {
        res.AppendFormat(" couleur : {0}", this.CouleurNom);
      }

      if (!string.IsNullOrWhiteSpace(this.TailleNom))
      {
        res.AppendFormat(" taille : {0}", this.TailleNom);
      }

      return res.ToString();
    }
  }
}
