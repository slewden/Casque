namespace CasqueLib.Services.Livraison.Detail
{
  /// <summary>
  /// Classe d'info sur une étiquette
  /// </summary>
  public class EtiquetteInfo
  {
    /// <summary>
    /// L'index pour le type de pièce
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// L'index dans la commande
    /// </summary>
    public int IndexCommande { get; set; }

    /// <summary>
    /// Le numero de tag
    /// </summary>
    public string Numero { get; set; }
  }
}
