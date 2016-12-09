using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Classe statique pour les extensions des méthodes des enum du NameSpace
  /// </summary>
  public static class EnumHelper
  {
    /// <summary>
    /// Renvoie le nom associé à la valeur de l'enum
    /// </summary>
    /// <param name="val">La valeur</param>
    /// <returns>Le nom</returns>
    public static string GetName(this EActionLecteur val)
    {
      switch (val)
      {
        case EActionLecteur.Demarre:
          return "Démarrage lecteur";
        case EActionLecteur.QueryStatut:
          return "Etat du lecteur";
        case EActionLecteur.ResetLecture:
          return "Réinitialise les lectures";
        case EActionLecteur.Stoppe:
          return "Arrêt lecteur";
      }

      return string.Format("{0}", val);
    }

    /// <summary>
    /// Renvoie le nom associé à la valeur de l'enum
    /// </summary>
    /// <param name="val">La valeur</param>
    /// <returns>Le nom</returns>
    public static string GetName(this EActionEncode val)
    {
      switch (val)
      {
        case EActionEncode.CancelEncodage:
          return "Annuler un encodage";
        case EActionEncode.QueryStatut:
          return "Etat de l'encodeur";
        case EActionEncode.TraiteAssemblage:
          return "Encode un assemblage";
        case EActionEncode.TraiteCommande:
          return "Encode une commande";
      }

      return string.Format("{0}", val);
    }
  }
}
