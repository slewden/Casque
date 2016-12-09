using System;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe une commande
  /// </summary>
  [Alias("commande")]
  public class Commande
  {
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="Commande"/>
    /// </summary>
    public Commande()
    {
      this.Saisie = DateTime.Now;
      this.Numero = string.Format("CMD{0:yyyymmdd-hhMMssf}", this.Saisie);
    }

    /// <summary>
    /// Clé de la commande
    /// </summary>
    [AutoIncrement]
    [Alias("comd_id")]
    public int Cle { get; set; }

    /// <summary>
    /// Clé du fournisseur
    /// </summary>
    [Alias("clfo_id")]
    public int FournisseurCle { get; set; }

    /// <summary>
    /// Clé de l'utilisateur qui commande
    /// </summary>
    [Alias("util_id")]
    public int UtilisateurCle { get; set; }

    /// <summary>
    /// Date de la commande
    /// </summary>
    [Alias("comd_d_saisie")]
    public DateTime Saisie { get; set; }

    /// <summary>
    /// Numéro de la commande
    /// </summary>
    [Alias("comd_numero")]
    public string Numero { get; set; }
    
    /// <summary>
    /// Nombre de semaine pour la livraison
    /// </summary>
    [Alias("comd_delai_semaine")]
    public int DelaiSemaine { get; set; }

    /// <summary>
    /// Date de validation de la commande
    /// </summary>
    [Alias("comd_d_validation")]
    public DateTime? Validation { get; set; }

    /// <summary>
    /// Date de début de l'impression des étiquettes de la commande
    /// </summary>
    [Alias("comd_d_debut_impression")]
    public DateTime? DebutImpression { get; set; }
    
    /// <summary>
    /// L'impression des étiquettes est finie
    /// </summary>
    [Alias("comd_d_impression_fini")]
    public DateTime? ImpressionFinie { get; set; }

    /// <summary>
    /// Date à laquelle La demande a été envoyée (par email)
    /// </summary>
    [Alias("comd_d_envoie_email")]
    public DateTime? EnvoieEmail { get; set; }
    
    /// <summary>
    /// Date d'acquittement (utilisée quand les commandes sont reçues incomplètes)
    /// </summary>
    [Alias("comd_d_aquitte")]
    public DateTime? Acquittee { get; set; }

    /// <summary>
    /// Indique si la commande est complète pour être insérée en BDD
    /// </summary>
    /// <returns>True si complet</returns>
    public virtual bool IsComplet()
    {
      if (this.FournisseurCle <= 0)
      { // Faut un fournisseur
        return false;
      }

      if (this.UtilisateurCle <= 0)
      { // faut un opérateur de saisie
        return false;
      }

      if (string.IsNullOrWhiteSpace(this.Numero))
      { // Faut un numero
        return false;
      }

      return true;
    }
  }
}
