using ServiceStack.DataAnnotations;

namespace CasqueLib.Services.Commande.Resume
{
  /// <summary>
  /// Liste le nombre de commande par rapport à leur statut de traitement
  /// </summary>
  [Alias("v_commande_resume")]
  public class CommandeResumeData
  {
    /// <summary>
    /// Le statut en int
    /// </summary>
    [Alias("statut")]
    public int StatutInt { get; set; }

    /// <summary>
    /// Le statut (typé)
    /// </summary>
    [Ignore]
    public EStatutCommande Statut
    {
      get
      {
        return (EStatutCommande)this.StatutInt;
      }

      set
      {
        this.StatutInt = (int)value;
      }
    }

    /// <summary>
    /// Le nombre
    /// </summary>
    [Alias("nb")]
    public int Nombre { get; set; }

    /// <summary>
    /// Le nombre de pièces dans les commandes de ce statut
    /// </summary>
    [Alias("total_piece")]
    public int TotalPiece { get; set; }
  }
}
