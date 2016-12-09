using System.Collections.Generic;

namespace CasqueLib.Matos.ServerOwin
{
  /// <summary>
  /// Mémorise les connexions IDs des lecteur sur le réseau
  /// </summary>
  public static class TypeConnexionList
  {
    /// <summary>
    /// Initialise les membres statiques de la classe <see cref="TypeConnexionList" />
    /// </summary>
    static TypeConnexionList()
    {
      TypeConnexionList.Lecteurs = new Dictionary<string, HubLecteurInfo>();
      TypeConnexionList.Encodeurs = new Dictionary<string, HubLecteurInfo>();
    }

    /// <summary>
    /// Les lecteurs
    /// </summary>
    public static Dictionary<string, HubLecteurInfo> Lecteurs { get; private set; }

    /// <summary>
    /// Les encodeurs
    /// </summary>
    public static Dictionary<string, HubLecteurInfo> Encodeurs { get; private set; }
  }
}
