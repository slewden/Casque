using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasqueLib
{
  /// <summary>
  /// Classe pour extension liées aux enums
  /// </summary>
  public static class ENumHelper
  {
    /// <summary>
    /// Renvoie le nom d'une configuration
    /// </summary>
    /// <param name="cfg">La configuration</param>
    /// <returns>la chaine</returns>
    public static string GetNom(this EConfiguration cfg)
    {
      switch (cfg)
      {
        case EConfiguration.NombreMaxiQuantite:
          return "Qte max dans commande";
      }

      return string.Format("Configuration N°{0:G}", cfg);
    }

    /// <summary>
    /// Renvoie la valeur par défaut d'une configuration
    /// </summary>
    /// <param name="cfg">La configuration</param>
    /// <returns>la chaine</returns>
    public static string GetValeur(this EConfiguration cfg)
    {
      switch (cfg)
      {
        case EConfiguration.NombreMaxiQuantite:
          return "500";
      }

      return string.Empty;
    }

    /// <summary>
    /// Renvoie le type de donnée à editer
    /// </summary>
    /// <param name="cfg">la config</param>
    /// <returns>le type de donnée à editer</returns>
    public static string GetTypeName(this EConfiguration cfg)
    {
      switch (cfg)
      {
        case EConfiguration.NombreMaxiQuantite:
          return "int";
      }

      return string.Empty;
    }

    /// <summary>
    /// Renvoie le message de description
    /// </summary>
    /// <param name="cfg">la config</param>
    /// <returns>le type de donnée à editer</returns>
    public static string GetMessageDescription(this EConfiguration cfg)
    {
      switch (cfg)
      {
        case EConfiguration.NombreMaxiQuantite:
          return "Fixe la limite de la quantité maximale d'articles dans une ligne de commande";
      }

      return string.Empty;
    }

    /// <summary>
    /// Renvoie le message requis
    /// </summary>
    /// <param name="cfg">la config</param>
    /// <returns>le type de donnée à editer</returns>
    public static string GetMessageRequis(this EConfiguration cfg)
    {
      switch (cfg)
      {
        case EConfiguration.NombreMaxiQuantite:
          return "La quantité maximale d'articles dans une ligne de commande est requise";
      }

      return string.Empty;
    }

    /// <summary>
    /// Renvoie le message type
    /// </summary>
    /// <param name="cfg">la config</param>
    /// <returns>le type de donnée à editer</returns>
    public static string GetMessageFormat(this EConfiguration cfg)
    {
      switch (cfg)
      {
        case EConfiguration.NombreMaxiQuantite:
          return "La quantité maximale d'article dans une ligne de commande doit être un nombre";
      }

      return string.Empty;
    }
  }
}
