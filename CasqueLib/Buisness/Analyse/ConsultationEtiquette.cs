using System.Collections.Generic;
using System.Linq;
using CasqueLib.Buisness.View;

namespace CasqueLib.Buisness.Analyse
{
  /// <summary>
  /// Mémorise les informations pour une étiquette
  /// </summary>
  public class ConsultationEtiquette
  {
    /// <summary>
    /// Est-ce un tag ou une étiquette
    /// </summary>
    public bool EstUnTag
    {
      get
      {
        return this.Etiquette == null;
      }
    }

    /// <summary>
    /// Le numéro de l'etiquette ou du tag
    /// </summary>
    public string Numero { get; set; }

    /// <summary>
    /// Les infos de l'étiquette
    /// </summary>
    public EtiquetteView Etiquette { get; set; }

    /// <summary>
    /// Les infos de la commande
    /// </summary>
    public CommandeView Commande { get; set; }

    /// <summary>
    /// Les infos de l'assemblage
    /// </summary>
    public AssemblageView Assemblage { get; set; }

    /// <summary>
    /// Les infos de la livraison
    /// </summary>
    public LivraisonView Livraison { get; set; }

    /// <summary>
    /// Les evènements
    /// </summary>
    public List<ConsultationEvenement> Evenements { get; set; }

    /// <summary>
    /// Le nombre d'évent de commande
    /// </summary>
    public int NombreRowCommande { get; private set; }

    /// <summary>
    /// Le nombre d'évent de réception
    /// </summary>
    public int NombreRowReception { get; private set; }

    /// <summary>
    /// Le nombre d'évent d'assemblage
    /// </summary>
    public int NombreRowAssemblage { get; private set; }

    /// <summary>
    /// Le nombre d'event de livraison
    /// </summary>
    public int NombreRowLivraison { get; private set; }

    /// <summary>
    /// Calcule les évènements
    /// </summary>
    public void Compute()
    {
      this.Evenements = new List<ConsultationEvenement>();

      if (this.Etiquette != null)
      {
        this.Evenements.Add(new ConsultationEvenement(4, this.Commande.DebutImpression.Value, "Encodage de l'étiquette"));

        if (this.Etiquette.EntreeStock.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(7, this.Etiquette.EntreeStock.Value, "Réception de la pièce étiquettée"));
        }

        if (this.Etiquette.Assemblage.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(9, this.Etiquette.Assemblage.Value, "Inclusion de la pièce étiquettée dans l'assemblage"));
        }

        if (this.Etiquette.Livraison.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(12, this.Etiquette.Livraison.Value, "Mise de la pièce dans un carton de livraision"));
        }
      }

      if (this.Commande != null)
      {
        this.Evenements.Add(new ConsultationEvenement(1, this.Commande.Saisie, "Composition de la commande"));
        if (this.Commande.Validation.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(2, this.Commande.Validation.Value, "Validation de la commande"));
        }

        if (this.Commande.DebutImpression.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(3, this.Commande.DebutImpression.Value, "Début de l'encodage de la commande"));
        }

        if (this.Commande.ImpressionFinie.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(5, this.Commande.ImpressionFinie.Value, "Fin de l'encodage de la commande"));
        }

        if (this.Commande.EnvoieEmail.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(6, this.Commande.EnvoieEmail.Value, "Envoie de la commande au fournisseur"));
        }
      }

      if (this.Assemblage != null)
      {
        this.Evenements.Add(new ConsultationEvenement(8, this.Assemblage.Creation, "Début de l'assemblage de la pièce"));

        if (this.Assemblage.Validation.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(10, this.Assemblage.Validation.Value, "Fin de l'assemblage"));
        }
      }

      if (this.Livraison != null)
      {
        this.Evenements.Add(new ConsultationEvenement(11, this.Livraison.Creation, "Début de la préparation de la livraison"));
        if (this.Livraison.Validation.HasValue)
        {
          this.Evenements.Add(new ConsultationEvenement(13, this.Livraison.Validation.Value, "Fin de la livraison : expédition au client"));
        }
      }

      this.Evenements = this.Evenements.OrderBy(x => x.TypeEvenement).ToList();

      this.NombreRowCommande = this.Evenements.Where(x => x.TypeEvenement <= 6).Count();
      this.NombreRowReception = this.Evenements.Where(x => x.TypeEvenement == 7).Count();
      this.NombreRowAssemblage = this.Evenements.Where(x => x.TypeEvenement > 7 && x.TypeEvenement < 11).Count();
      this.NombreRowLivraison = this.Evenements.Where(x => x.TypeEvenement >= 11).Count();
    }
  }
}
