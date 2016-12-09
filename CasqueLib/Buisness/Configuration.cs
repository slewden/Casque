using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Classe pour mapper la table de configuration
  /// </summary>
  [Alias("configuration")]
  public class Configuration
  {
    /// <summary>
    /// Objet pour vérouillage lors du calcul de la liste
    /// </summary>
    private static object leLock = new object();
    
    /// <summary>
    /// Clé de la configuration
    /// </summary>
    [Alias("conf_id")]
    public int Cle { get; set; }
    
    /// <summary>
    /// Nom de la configuration
    /// </summary>
    [Alias("conf_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Valeur de la configuration
    /// </summary>
    [Alias("conf_valeur")]
    public string Valeur { get; set; }

    /// <summary>
    /// Renvoie le Type de config 
    /// </summary>
    [Ignore]
    public EConfiguration TypeConfig
    {
      get
      {
        return (EConfiguration)this.Cle;
      }

      set
      {
        this.Cle = (int)value;
      }
    }

    /// <summary>
    /// valeur non typée
    /// </summary>
    [Ignore]
    public int? TheIntValue
    {
      get
      {
        if (this.TypeDonnee == "int")
        {
          return Convert.ToInt32(this.Valeur);
        }

        return null;
      }

      set
      {
        this.Valeur = value.ToString();
      }
    }
    
    /// <summary>
    /// Le type à éditer
    /// </summary>
    [Ignore]
    public string TypeDonnee
    {
      get
      {
        return this.TypeConfig.GetTypeName();
      }
    }

    /// <summary>
    /// La description
    /// </summary>
    [Ignore]
    public string Description
    {
      get
      {
        return this.TypeConfig.GetMessageDescription();
      }
    }

    /// <summary>
    /// Le message pour le validator requis
    /// </summary>
    [Ignore]
    public string RequisMessage
    {
      get
      {
        return this.TypeConfig.GetMessageRequis();
      }
    }

    /// <summary>
    /// Le message pour le validator format
    /// </summary>
    [Ignore]
    public string FormatMessage
    {
      get
      {
        return this.TypeConfig.GetMessageFormat();
      }
    }

    /// <summary>
    /// Renvoie l'objet configuration demandé
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <param name="cle">La cle demandée</param>
    /// <returns>l'objet configuration chargé</returns>
    public static Configuration Get(IDbConnection db, EConfiguration cle)
    {
      lock (Configuration.leLock)
      {
        int n = (int)cle;
        Configuration c = db.Select<Configuration>(x => x.Cle == n).FirstOrDefault();

        if (c == null)
        { // elle manque ==> ajout
          c = new Configuration()
         {
           Cle = n,
           TypeConfig = cle,
           Nom = cle.GetNom(),
           Valeur = cle.GetValeur()
         };
          db.Insert(c);
        }

        return c;
      }
    }
 
    /// <summary>
    /// Renvoie les objets configurations chargés
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    /// <returns>la liste des configurations chargées</returns>
    public static List<Configuration> GetAll(IDbConnection db)
    {
      lock (Configuration.leLock)
      { // on s'assure que l'opération est atomique pour ne pas créer plusieur fois la même configuration
        List<Configuration> lst = db.Select<Configuration>();

        // check qu'il n'en manque pas
        foreach (EConfiguration cfg in Enum.GetValues(typeof(EConfiguration)))
        {
          if (!lst.Any(x => x.TypeConfig == cfg))
          { // elle manque ==> ajout
            Configuration c = new Configuration()
              {
                Cle = (int)cfg,
                TypeConfig = cfg,
                Nom = cfg.GetNom(),
                Valeur = cfg.GetValeur()
              };
            db.Insert(c);
            lst.Add(c);
          }
        }

        return lst;
      }
    }

    /// <summary>
    /// Mémorise la config
    /// </summary>
    /// <param name="db">La connexion à la base de données</param>
    public void Save(IDbConnection db)
    {
      db.UpdateOnly(this, x => new { x.Valeur }, u => u.Cle == this.Cle);
    }
  }
}
