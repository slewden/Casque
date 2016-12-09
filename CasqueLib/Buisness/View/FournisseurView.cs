using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe la vue fournisseur (sous ensemble de la table client_fournisseur)
  /// </summary>
  [Alias("v_fournisseur")]
  public class FournisseurView : ClientFournisseur
  {
    /// <summary>
    /// Nombre de type de pièce gérées par ce fournisseur
    /// </summary>
    [Alias("nb_piece")]
    public int NombreTypePiece { get; set; }

    /// <summary>
    /// Nombre de commandes
    /// </summary>
    [Alias("nb_commande")]
    public int NombreCommande { get; set; }

    /// <summary>
    /// Est un client ou fournisseur : Ici un fournisseur
    /// </summary>
    [Ignore]
    public override bool EstUnClient
    {
      get
      {
        return false;
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
        EstUnClient = false,
        Email = this.Email,
        AdresseCommande = this.AdresseCommande,
        AdresseLivraison = this.AdresseLivraison,
      };
    }
  }
}
