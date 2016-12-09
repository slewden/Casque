using CasqueLib.Common;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Les infos dans un carton d'une livraison
  /// </summary>
  [Alias("v_carton_livre")]
  public class CartonLivreView : Carton
  {
    /// <summary>
    /// La clé de la livraison
    /// </summary>
    [Alias("livr_id")]
    public int LivraisonCle { get; set; }

    /// <summary>
    /// L'index du carton
    /// </summary>
    [Alias("asse_carton_index")]
    public int CartonIndex { get; set; }

    /// <summary>
    /// Le numero de l'étiquette d'assemblage
    /// </summary>
    [Alias("asse_tag")]
    public string Etiquette { get; set; }

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
    /// La photo du casque
    /// </summary>
    [Alias("casq_photo")]
    public string CasquePhoto { get; set; }

    /// <summary>
    /// L'url d'accès à la photo
    /// </summary>
    [Ignore]
    public string CasquePhotoUrl
    {
      get
      {
        return Folder.RelativeUrl(Folder.EFolder.Casque, this.CasquePhoto);
      }
    }

    /// <summary>
    /// Le nombre de casques dans le carton
    /// </summary>
    [Ignore]
    public int Nombre { get; set; }
  }
}
