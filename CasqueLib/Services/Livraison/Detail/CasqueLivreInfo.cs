using System.Collections.Generic;
using CasqueLib.Common;

namespace CasqueLib.Services.Livraison.Detail
{
  /// <summary>
  /// Détail d'un casque livré
  /// </summary>
  public class CasqueLivreInfo
  {
    /// <summary>
    /// La clé du casque
    /// </summary>
    public int CasqueCle { get; set; }

    /// <summary>
    /// Le nom du casque
    /// </summary>
    public string CasqueNom { get; set; }

    /// <summary>
    /// Le code du casque
    /// </summary>
    public string CasqueCode { get; set; }

    /// <summary>
    /// La photo du casque
    /// </summary>
    public string CasquePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    public string CasquePhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.Casque, this.CasquePhoto);
      }
    }

    /// <summary>
    /// Les étiquettes
    /// </summary>
    public List<EtiquetteInfo> Etiquettes { get; set; }

    /// <summary>
    /// Pour les comparaisons
    /// </summary>
    /// <param name="obj">l'objet à comparer</param>
    /// <returns>True si c'est les mêmes</returns>
    public override bool Equals(object obj)
    {
      CasqueLivreInfo c = obj as CasqueLivreInfo;
      if ((object)c == null)
      {
        return false;
      }
      else
      {
        return this.CasqueCle == c.CasqueCle;
      }
    }

    /// <summary>
    /// Pour les distinct dans le linq
    /// </summary>
    /// <returns>le code de hash</returns>
    public override int GetHashCode()
    {
      return this.CasqueCle.GetHashCode();
    }
  }
}
