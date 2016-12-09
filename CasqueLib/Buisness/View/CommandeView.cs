using System;
using System.Collections.Generic;
using System.Linq;
using CasqueLib.Common;
using CasqueLib.Services.Commande;
using ServiceStack.DataAnnotations;

namespace CasqueLib.Buisness.View
{
  /// <summary>
  /// Mappe la vue Commande
  /// </summary>
  [Alias("v_commande")]
  public class CommandeView : Commande
  {
    /// <summary>
    /// Nom du fournisseur
    /// </summary>
    [Alias("clfo_nom")]
    public string FournisseurNom { get; set; }

    /// <summary>
    /// Le type de poste (typé + nom)
    /// </summary>
    [Ignore]
    public NomCle FournisseurNomCle 
    {
      get
      {
        return new NomCle() { Nom = this.FournisseurNom, Cle = this.FournisseurCle };
      }

      set
      {
        this.FournisseurCle = value.Cle;
        this.FournisseurNom = value.Nom;
      }
    }
    
    /// <summary>
    /// email du client
    /// </summary>
    [Alias("clfo_email")]
    public string FournisseurEmail { get; set; }

    /// <summary>
    /// adresse de commande du fournisseur
    /// </summary>
    [Alias("clfo_adresse_commande")]
    public string FournisseurAdresseCommande { get; set; }

    /// <summary>
    /// L'adresse de livraison en HTML
    /// </summary>
    [Ignore]
    public string FournisseurAdresseCommandeHtml
    {
      get
      {
        if (!string.IsNullOrWhiteSpace(this.FournisseurAdresseCommande))
        {
          return this.FournisseurAdresseCommande.Replace("\n", "<br />\n");
        }
        else
        {
          return string.Empty;
        }
      }
    }

    /// <summary>
    /// sujet du email à envoyer au fournisseur
    /// </summary>
    [Alias("clfo_sujet_email")]
    public string FournisseurSujetEmail { get; set; }
    
    /// <summary>
    /// Utilisateur qui a fait la commande
    /// </summary>
    [Alias("util_prenom_nom")]
    public string UtilisateurNom { get; set; }

    /// <summary>
    /// Nombre total de pièces dans la commande
    /// </summary>
    [Alias("total")]
    public int PieceTotal { get; set; }

    /// <summary>
    /// Nombre total de pièces attendues dans la commande
    /// </summary>
    [Alias("attente")]
    public int PieceAttendues { get; set; }

    /// <summary>
    /// Nombre total de pièces reçues dans la commande
    /// </summary>
    [Alias("recu")]
    public int PieceRecues { get; set; }

    /// <summary>
    /// Nombre total de pièces assemblées dans la commande
    /// </summary>
    [Alias("assemble")]
    public int PieceAssemblees { get; set; }

    /// <summary>
    /// Nombre total de pièces Livrées dans la commande
    /// </summary>
    [Alias("livre")]
    public int PieceLivrees { get; set; }

    /// <summary>
    /// le statut d'avancement de la commande
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
    /// Le nom du statut
    /// </summary>
    [Ignore]
    public string StatutNom
    {
      get
      {
        return this.Statut.GetLibelle();
      }
    }

    /// <summary>
    /// La date d'échéance
    /// </summary>
    [Ignore]
    public DateTime Echeance
    {
      get
      {
        return this.Saisie.AddDays(this.DelaiSemaine * 7);
      }
    }

    /// <summary>
    /// Retard à l'échéance
    /// </summary>
    [Ignore]
    public int RetardEcheance
    {
      get
      {
        return this.Echeance.Subtract(DateTime.Now).Days;
      }
    }

    /// <summary>
    /// La liste des pièces commandes
    /// </summary>
    [Ignore]
    public List<CommandeLigneView> Pieces { get; set; }

    /// <summary>
    /// Renvoie le nombre de produits commandés
    /// </summary>
    [Ignore]
    public int NombreProduit
    {
      get
      {
        if (this.Pieces != null && this.Pieces.Any())
        {
          return this.Pieces.Where(x => x.Quantite > 0).Select(x => x.Quantite).Sum();
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// Renvoie le nombre de produits commandés qui ont une étiquette
    /// </summary>
    [Ignore]
    public int NombreProduitEtiquette
    {
      get
      {
        if (this.Pieces != null && this.Pieces.Any())
        {
          return this.Pieces.Where(x => x.Quantite > 0 && x.TypePieceAvecTag).Select(x => x.Quantite).Sum();
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// Montant total de la commande
    /// </summary>
    [Ignore]
    public decimal MontantCommande
    {
      get
      {
        if (this.Pieces != null && this.Pieces.Any())
        {
          return this.Pieces.Where(x => x.Quantite > 0).Select(x => (x.Quantite * (x.PrixUnitaire ?? 0)) + (x.Frais ?? 0)).Sum();
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    /// indique si la commande est complète pour être enregistrée
    /// </summary>
    /// <returns>true si complet</returns>
    public override bool IsComplet()
    {
      if (!base.IsComplet())
      {
        return false;
      }

      if (this.Pieces == null || !this.Pieces.Any())
      { // Faut des pièces dans la commande
        return false;
      }

      foreach (CommandeLigne l in this.Pieces)
      {
        if (l.Quantite > 0 && !l.IsComplet())
        { // il faut que toutes les pièces soient correctement remplie
          return false;
        }
      }

      return true;
    }
  }
}
