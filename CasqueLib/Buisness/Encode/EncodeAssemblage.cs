using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CasqueLib.Buisness.Encode
{
  /// <summary>
  /// Classe qui sert à démarrer le traitement d'une impression d'un assemblage
  /// </summary>
  [Alias("v_assemblage")]
  public class EncodeAssemblage
  {
    /// <summary>
    /// Clé de l'assemblage concerné
    /// </summary>
    [Alias("asse_id")]
    public int Cle { get; set; }

    /// <summary>
    /// La clé du casque
    /// </summary>
    [Alias("casq_id")]
    public int CasqueCle { get; set; }

    /// <summary>
    /// Le nom du casque
    /// </summary>
    [Alias("casq_nom")]
    public string CasqueNom { get; set; }

    /// <summary>
    /// Le code du casque
    /// </summary>
    [Alias("casq_code")]
    public string CasqueCode { get; set; }

    /// <summary>
    /// Charge l'assemblage à imprimer
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="assemblageCle">La clé de l'assemblage</param>
    /// <returns>l'objet remplit</returns>
    public static EncodeAssemblage Start(System.Data.IDbConnection cnn, int assemblageCle)
    {
      return cnn.Select<EncodeAssemblage>(x => x.Cle == assemblageCle).FirstOrDefault();
    }

    /// <summary>
    /// Traite la fin de l'assemblage
    /// </summary>
    /// <param name="cnn">La connexion à la base de données</param>
    /// <param name="assemblageCle">La clé de l'assemblage</param>
    public static void Stop(System.Data.IDbConnection cnn, int assemblageCle)
    {
      cnn.ExecuteNonQuery("EXEC dbo.assemblage_encode @asseId", new { asseId = assemblageCle });
    }
  }
}
