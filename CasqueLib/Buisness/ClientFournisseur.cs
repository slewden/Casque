using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness
{
  /// <summary>
  /// Mappe la table de Bdd des clients ou fournisseurs
  /// </summary>
  [Alias("client_fournisseur")]
  public class ClientFournisseur
  {
    /// <summary>
    /// Clé du fournisseur ou client
    /// </summary>
    [AutoIncrement]
    [Alias("clfo_id")]
    public int Cle { get; set; }
    
    /// <summary>
    /// Est un client ou fournisseur
    /// </summary>
    [Alias("clfo_est_un_client")]
    public virtual bool EstUnClient { get; set; }

    /// <summary>
    /// Nom du fournisseur ou client
    /// </summary>
    [Alias("clfo_nom")]
    public string Nom { get; set; }

    /// <summary>
    /// Adresse de commande du fournisseur ou client 
    /// </summary>
    [Alias("clfo_adresse_commande")]
    public string AdresseCommande { get; set; }

    /// <summary>
    /// Adresse de livraison du fournisseur ou client 
    /// </summary>
    [Alias("clfo_adresse_livraison")]
    public string AdresseLivraison { get; set; }

    /// <summary>
    /// Adresse email du fournisseur ou client 
    /// </summary>
    [Alias("clfo_email")]
    public string Email { get; set; }

    /// <summary>
    /// Le sujet à utiliser lors d'envoie de email à ce client ou ce fournisseur
    /// </summary>
    [Alias("clfo_sujet_email")]
    public string SujetEmail { get; set; }

    /// <summary>
    /// Indique si l'objet est correctement remplit pour se sauvegarder en bdd
    /// </summary>
    /// <returns>True si complet</returns>
    public bool IsComplet()
    {
      return !string.IsNullOrWhiteSpace(this.Nom);
    }
  }
}
