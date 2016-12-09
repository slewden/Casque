using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe la vue client (sous ensemble de la table client_fournisseur)
  /// </summary>
  [Alias("v_client")]
  public class ClientView : ClientFournisseur
  {
    /// <summary>
    /// Nombre de livaisons du client
    /// </summary>
    [Alias("nb_livraison")]
    public int NombreLivraison { get; set; }

    /// <summary>
    /// Est un client ou fournisseur : Ici un client
    /// </summary>
    [Ignore]
    public override bool EstUnClient
    {
      get
      {
        return true;
      }
    }

    /// <summary>
    /// L'adresse de commande en HTML
    /// </summary>
    [Ignore]
    public string AdresseCommandeHtml
    {
      get
      {
        if (!string.IsNullOrWhiteSpace(this.AdresseCommande))
        {
          return this.AdresseCommande.Replace("\n", "<br />\n");
        }
        else
        {
          return string.Empty;
        }
      }
    }

    /// <summary>
    /// L'adresse de livraison en HTML
    /// </summary>
    [Ignore]
    public string AdresseLivraisonHtml
    {
      get
      {
        if (!string.IsNullOrWhiteSpace(this.AdresseLivraison))
        {
          return this.AdresseLivraison.Replace("\n", "<br />\n");
        }
        else
        {
          return string.Empty;
        }
      }
    }

    /// <summary>
    /// Renvoie un vrai objet ClientFournisseur
    /// </summary>
    /// <returns>l'objet correctement instancié</returns>
    public ClientFournisseur ToClientFournisseur()
    {
      return new ClientFournisseur()
      {
        Cle = this.Cle,
        Nom = this.Nom,
        EstUnClient = true,
        Email = this.Email,
        AdresseCommande = this.AdresseCommande,
        AdresseLivraison = this.AdresseLivraison,
      };
    }
  }
}
