using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CasqueLib.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CasqueLib.Buisness.Encode
{
  /// <summary>
  /// Classe qui sert à démarrer le traitement d'une impression d'une commande
  /// </summary>
  public class EncodeCommande
  {
    /// <summary>
    /// Le nombre d'étiquettes à encoder
    /// </summary>
    [Alias("nombre")]
    public int NombreTag { get; set; }
    
    /// <summary>
    /// Lance le démarrage de l'impression d'une commande
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="commandeCle">La clé de la commande</param>
    /// <returns>l'objet remplit</returns>
    public static EncodeCommande Start(IDbConnection cnn, int commandeCle)
    {
      List<EncodeCommande> res = new List<EncodeCommande>();
      cnn.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.commande_encode_commence @comdId";
        cmd.Parameters.Add(new SqlParameter("comdId", commandeCle));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<EncodeCommande>();
          ////reader.NextResult();
        }
      });

      return res.FirstOrDefault();
    }

    /// <summary>
    /// Traite un encodage d'un tag d'une commande
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="commandeCle">La clé de la commande</param>
    /// <param name="numero">L'index de l'étiquette dans la commande</param>
    /// <returns>les infos pour encoder l'étiquette</returns>
    public static EncodeCommandeEtiquette Step(IDbConnection cnn, int commandeCle, int numero)
    {
      List<EncodeCommandeEtiquette> res = new List<EncodeCommandeEtiquette>();
      cnn.Exec(cmd =>
      {
        cmd.CommandText = "EXEC dbo.commande_encode_step @comdId, @num";
        cmd.Parameters.Add(new SqlParameter("comdId", commandeCle));
        cmd.Parameters.Add(new SqlParameter("num", numero));
        using (IDataReader reader = cmd.ExecuteReader())
        {
          res = reader.CustomConvertToList<EncodeCommandeEtiquette>();
          ////reader.NextResult();
        }
      });

      return res.FirstOrDefault();
    }

    /// <summary>
    /// Annule tout encodage d'une commande en cours
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="commandeCle">La clé de la commande</param>
    public static void Cancel(IDbConnection cnn, int commandeCle)
    {
      cnn.ExecuteNonQuery("EXEC dbo.commande_encode_annule @comdId", new { comdId = commandeCle });
    }

    /// <summary>
    /// déclare la fin de l'encodage d'une commande
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="commandeCle">La clé de la commande</param>
    public static void Stop(IDbConnection cnn, int commandeCle)
    {
      cnn.ExecuteNonQuery("EXEC dbo.commande_encode_finie @comdId", new { comdId = commandeCle });
    }

    /// <summary>
    /// Corrige les quantités des lignes d'une commande par rapport aux quantités d'étiquettes imprimées
    /// (quand y a eu un problème)
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="commandeCle">La clé de la commande</param>
    public static void AjusteQuantite(IDbConnection cnn, int commandeCle)
    {
      cnn.ExecuteNonQuery("EXEC dbo.commande_encode_ajuste_quantite @comdId", new { comdId = commandeCle });
    }

    /// <summary>
    /// Redémarre l'encodage d'une commande 
    /// (quand y a eu un problème)
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="commandeCle">La clé de la commande</param>
    public static void Restart(IDbConnection cnn, int commandeCle)
    {
      cnn.ExecuteNonQuery("EXEC dbo.commande_encode_restart @comdId", new { comdId = commandeCle });
    }
  }
}
